using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fclp;
using Fclp.Internals;

namespace CommandLine
{
    public static class CommandLineParser<TBuildType> where TBuildType : new()
    {
        public static CommandLineParsingResult<TBuildType> Parse(string[] args, bool isCaseSensitive = false)
        {
            var parser = new FluentCommandLineParser<TBuildType>();
            parser.IsCaseSensitive = isCaseSensitive;

            TBuildType options;
            ICommandLineParserResult result = parser.Parse<TBuildType>(args, out options, ShortOptions, LongOptions, Description, Required, Default);

            IEnumerable<string> invalidMutuallyExclusiveSets;
            IEnumerable<string> invalidMutuallyExclusiveSetsByDefaultValues;
            bool isValidMutuallyExclusiveSets = ValidateMutuallyExclusiveSets(options, result.UnMatchedOptions, out invalidMutuallyExclusiveSets, out invalidMutuallyExclusiveSetsByDefaultValues);

            return new CommandLineParsingResult<TBuildType>()
            {
                Options = options,
                EmptyArgs = result.EmptyArgs,
                HasErrors = result.HasErrors,
                ErrorText = result.ErrorText,
                HelpCalled = result.HelpCalled,
                HasMutuallyExclusiveSetErrors = !isValidMutuallyExclusiveSets,
                InvalidMutuallyExclusiveSets = invalidMutuallyExclusiveSets,
                InvalidMutuallyExclusiveSetsByDefaultValues = invalidMutuallyExclusiveSetsByDefaultValues,
                ParserResult = result
            };
        }

        private static Func<PropertyInfo, IEnumerable<char>> ShortOptions = (property) =>
        {
            List<char> options = null;

            OptionAttribute optionAttr = Attribute.GetCustomAttribute(property, typeof(OptionAttribute)) as OptionAttribute;
            if (optionAttr != null && optionAttr.ShortOption != null)
                options = new List<char>() { optionAttr.ShortOption.Value };

            SwitchAttribute[] switchAttrs = Attribute.GetCustomAttributes(property, typeof(SwitchAttribute)) as SwitchAttribute[];
            if (switchAttrs != null && switchAttrs.Length > 0)
            {
                if (options == null)
                    options = switchAttrs.Where(a => a.ShortOption != null).Select(a => a.ShortOption.Value).ToList();
                else
                    options.AddRange(switchAttrs.Where(a => a.ShortOption != null).Select(a => a.ShortOption.Value));
            }

            return options;
        };

        private static Func<PropertyInfo, IEnumerable<string>> LongOptions = (property) =>
        {
            List<string> options = null;

            OptionAttribute optionAttr = Attribute.GetCustomAttribute(property, typeof(OptionAttribute)) as OptionAttribute;
            if (optionAttr != null && string.IsNullOrEmpty(optionAttr.LongOption) == false)
                options = new List<string>() { optionAttr.LongOption };

            SwitchAttribute[] switchAttrs = Attribute.GetCustomAttributes(property, typeof(SwitchAttribute)) as SwitchAttribute[];
            if (switchAttrs != null && switchAttrs.Length > 0)
            {
                if (options == null)
                    options = switchAttrs.Where(a => string.IsNullOrEmpty(a.LongOption) == false).Select(a => a.LongOption).ToList();
                else
                    options.AddRange(switchAttrs.Where(a => string.IsNullOrEmpty(a.LongOption) == false).Select(a => a.LongOption));
            }

            return options;
        };

        private static Func<PropertyInfo, string> Description = (property) =>
        {
            OptionAttribute optionAttr = Attribute.GetCustomAttribute(property, typeof(OptionAttribute)) as OptionAttribute;
            return (optionAttr != null ? optionAttr.Description : null);
        };

        private static Func<PropertyInfo, bool> Required = (property) =>
        {
            OptionAttribute optionAttr = Attribute.GetCustomAttribute(property, typeof(OptionAttribute)) as OptionAttribute;
            return (optionAttr != null ? optionAttr.Required : false);
        };

        private static Func<PropertyInfo, object> Default = (property) =>
        {
            OptionAttribute optionAttr = Attribute.GetCustomAttribute(property, typeof(OptionAttribute)) as OptionAttribute;
            return (optionAttr != null ? optionAttr.Default : null);
        };

        private static bool ValidateMutuallyExclusiveSets(TBuildType options, IEnumerable<ICommandLineOption> unMatchedOptions, out IEnumerable<string> invalidMutuallyExclusiveSets, out IEnumerable<string> invalidMutuallyExclusiveSetsByDefaultValues)
        {
            IEnumerable<string> unmatchedSwitches = new string[0];
            if (unMatchedOptions != null && unMatchedOptions.Count() > 0)
            {
                unmatchedSwitches =
                    unMatchedOptions.Where(o => o.HasShortName && string.IsNullOrEmpty(o.ShortName) == false).Select(o => o.ShortName).Union(
                    unMatchedOptions.Where(o => o.HasLongName && string.IsNullOrEmpty(o.LongName) == false).Select(o => o.LongName)).Distinct();
            }

            var invalidSets = typeof(TBuildType).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .Where(p => p.GetGetMethod(true).IsPublic)
                .Where(p => p.GetIndexParameters().Length == 0)
                .Where(p =>
                {
                    OptionAttribute optionAttr = Attribute.GetCustomAttribute(p, typeof(OptionAttribute)) as OptionAttribute;
                    return (optionAttr != null && string.IsNullOrEmpty(optionAttr.MutuallyExclusiveSet) == false);
                })
                .Select(p =>
                {
                    OptionAttribute optionAttr = Attribute.GetCustomAttribute(p, typeof(OptionAttribute)) as OptionAttribute;
                    return new
                    {
                        optionAttr.MutuallyExclusiveSet,
                        p.PropertyType,
                        PropertyValue = p.GetValue(options, null),
                        HasDefaultValue = (optionAttr.Default != null),
                        Switches = (ShortOptions(p) ?? new char[0]).Select(c => c.ToString()).Union(LongOptions(p) ?? new string[0])
                    };
                })
                .GroupBy(p => new
                {
                    p.MutuallyExclusiveSet,
                    SetType = p.PropertyType,
                    SetDefaultValue = (p.PropertyType.IsValueType ? Activator.CreateInstance(p.PropertyType) : null)
                }, p => new
                {
                    p.PropertyValue,
                    p.HasDefaultValue,
                    IsCalledExplicitly = p.Switches.Except(unmatchedSwitches).Count() > 0
                })
                .Where(g => g.Count() > 1)
                .Where(g => g.Count(p => !(
                        (p.PropertyValue == null && g.Key.SetDefaultValue == null) ||
                        (p.PropertyValue != null && g.Key.SetDefaultValue != null && p.PropertyValue.Equals(g.Key.SetDefaultValue))
                    )) != 1
                );

            invalidMutuallyExclusiveSets = null;
            if (invalidSets.Count() > 0)
                invalidMutuallyExclusiveSets = invalidSets.Select(g => g.Key.MutuallyExclusiveSet);

            // invalid set might due to default values of the set members
            // bool property1 = true // default value and not explicit from command line
            // bool property2 = true // explicit from command line
            // bool property3 = false // no default value and not explicit from command line
            var invalidSetsByDefaultValues = invalidSets.Where(g => g.Select(p => (p.IsCalledExplicitly || p.HasDefaultValue == false ? p.PropertyValue : g.Key.SetDefaultValue /* ignore default value */)).Count(value => !(
                    (value == null && g.Key.SetDefaultValue == null) ||
                    (value != null && g.Key.SetDefaultValue != null && value.Equals(g.Key.SetDefaultValue))
                )) == 1
            );

            invalidMutuallyExclusiveSetsByDefaultValues = null;
            if (invalidSetsByDefaultValues.Count() > 0)
                invalidMutuallyExclusiveSetsByDefaultValues = invalidSetsByDefaultValues.Select(g => g.Key.MutuallyExclusiveSet);

            return (invalidMutuallyExclusiveSets == null);
        }

        public static string GetCommandLine(TBuildType options, bool isShortCommand = true)
        {
            List<string> parts = GetCommandLineParts(options, isShortCommand);
            return string.Join(" ", parts);
        }

        public static List<string> GetCommandLineParts(TBuildType options, bool isShortCommand = true)
        {
            List<string> parts = new List<string>();

            var properties = typeof(TBuildType).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .Where(p => p.GetGetMethod(true).IsPublic)
                .Where(p => p.GetIndexParameters().Length == 0);

            foreach (var property in properties)
            {
                object value = property.GetValue(options, null);
                if (value != null)
                {
                    if (value is bool)
                    {
                        if ((bool)value == false)
                            value = null;
                    }
                    else if (value is string)
                    {
                        if (string.IsNullOrEmpty((string)value))
                            value = null;
                    }
                    else if (value is List<string>)
                    {
                        if (((List<string>)value).Count == 0)
                            value = null;
                    }
                }

                string command = null;
                if (value != null)
                {
                    OptionAttribute optionAttr = Attribute.GetCustomAttribute(property, typeof(OptionAttribute)) as OptionAttribute;
                    command = GetCommand(command, optionAttr.ShortOption, optionAttr.LongOption, isShortCommand);

                    SwitchAttribute[] switchAttrs = Attribute.GetCustomAttributes(property, typeof(SwitchAttribute)) as SwitchAttribute[];
                    if (switchAttrs != null && switchAttrs.Length > 0)
                    {
                        foreach (var switchAttr in switchAttrs)
                            command = GetCommand(command, switchAttr.ShortOption, switchAttr.LongOption, isShortCommand);
                    }
                }

                if (string.IsNullOrEmpty(command) == false && value != null)
                {
                    if (value is bool && (bool)value)
                        parts.Add(string.Format("/{0}", command));
                    else if (value is string && string.IsNullOrEmpty((string)value) == false)
                        parts.Add(string.Format("/{0} \"{1}\"", command, value));
                    else if (value is List<string> && ((List<string>)value).Count > 0)
                        parts.Add(string.Format("/{0} \"{1}\"", command, string.Join("\" \"", (List<string>)value)));
                }
            }

            return parts;
        }

        private static string GetCommand(string command, char? shortOption, string longOption, bool isShortCommand)
        {
            if (string.IsNullOrEmpty(command))
            {
                if (isShortCommand)
                {
                    if (shortOption != null)
                        return shortOption.Value.ToString();
                    else if (string.IsNullOrEmpty(longOption) == false)
                        return longOption;
                }
                else
                {
                    if (string.IsNullOrEmpty(longOption) == false)
                        return longOption;
                    else if (shortOption != null)
                        return shortOption.Value.ToString();
                }
            }
            else
            {
                if (isShortCommand)
                {
                    if (command.Length > 1)
                    {
                        if (shortOption != null)
                            return shortOption.Value.ToString();
                        else if (string.IsNullOrEmpty(longOption) == false && longOption.Length < command.Length)
                            return longOption;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(longOption) == false && longOption.Length > command.Length)
                        return longOption;
                }
            }

            return command;
        }

        public static List<Tuple<string, string>> GetCommandLineHelp()
        {
            List<Tuple<string, string>> help = new List<Tuple<string, string>>();

            var properties = typeof(TBuildType).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .Where(p => p.GetGetMethod(true).IsPublic)
                .Where(p => p.GetIndexParameters().Length == 0);

            foreach (var property in properties)
            {
                string description = string.Empty;
                List<string> commands = new List<string>();

                OptionAttribute optionAttr = Attribute.GetCustomAttribute(property, typeof(OptionAttribute)) as OptionAttribute;

                if (optionAttr != null && string.IsNullOrEmpty(optionAttr.Description) == false)
                    description = optionAttr.Description;

                if (optionAttr != null && optionAttr.Default != null && string.IsNullOrEmpty(optionAttr.Default.ToString()) == false)
                {
                    if (optionAttr.Default is string)
                        description += string.Format(" (default: \"{0}\")", optionAttr.Default);
                    else
                        description += string.Format(" (default: {0})", optionAttr.Default);
                }

                if (optionAttr != null && optionAttr.ShortOption != null)
                    commands.Add(optionAttr.ShortOption.Value.ToString());
                if (optionAttr != null && string.IsNullOrEmpty(optionAttr.LongOption) == false)
                    commands.Add(optionAttr.LongOption);

                SwitchAttribute[] switchAttrs = Attribute.GetCustomAttributes(property, typeof(SwitchAttribute)) as SwitchAttribute[];
                if (switchAttrs != null && switchAttrs.Length > 0)
                {
                    foreach (var switchAttr in switchAttrs)
                    {
                        if (switchAttr.ShortOption != null)
                            commands.Add(switchAttr.ShortOption.Value.ToString());
                        if (string.IsNullOrEmpty(switchAttr.LongOption) == false)
                            commands.Add(switchAttr.LongOption);
                    }
                }

                help.Add(new Tuple<string, string>(description, string.Join(" ", commands.OrderBy(c => c.Length).Select(c => "/" + c))));
            }

            return help;
        }
    }
}
