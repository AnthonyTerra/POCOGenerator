using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Fclp;

namespace CommandLine
{
    // https://github.com/fclp/fluent-command-line-parser
    public static class FclpExtensions
    {
        public static ICommandLineParserResult Parse<TBuildType>(
            this FluentCommandLineParser<TBuildType> parser,
            string[] args,
            out TBuildType options,
            Func<PropertyInfo, IEnumerable<char>> ShortOptions = null,
            Func<PropertyInfo, IEnumerable<string>> LongOptions = null,
            Func<PropertyInfo, string> Description = null,
            Func<PropertyInfo, bool> Required = null,
            Func<PropertyInfo, object> Default = null
            ) where TBuildType : new()
        {
            parser.Setup<TBuildType>(ShortOptions, LongOptions, Description, Required, Default);
            var result = parser.Parse(args);
            options = (TBuildType)parser.Object;
            return result;
        }

        public static void Setup<TBuildType>(
            this FluentCommandLineParser<TBuildType> parser,
            Func<PropertyInfo, IEnumerable<char>> ShortOptions = null,
            Func<PropertyInfo, IEnumerable<string>> LongOptions = null,
            Func<PropertyInfo, string> Description = null,
            Func<PropertyInfo, bool> Required = null,
            Func<PropertyInfo, object> Default = null
            ) where TBuildType : new()
        {
            var properties = typeof(TBuildType).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .Where(p => p.GetGetMethod(true).IsPublic)
                .Where(p => p.GetIndexParameters().Length == 0);

            var method = typeof(FclpExtensions).GetMethod("SetupProperty", BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo property in properties)
                method.MakeGenericMethod(typeof(TBuildType), property.PropertyType).Invoke(null, new object[] { parser, property, ShortOptions, LongOptions, Description, Required, Default });
        }

        public static void SetupProperty<TBuildType, TProperty>(
            this FluentCommandLineParser<TBuildType> parser,
            PropertyInfo property,
            Func<PropertyInfo, IEnumerable<char>> ShortOptions = null,
            Func<PropertyInfo, IEnumerable<string>> LongOptions = null,
            Func<PropertyInfo, string> Description = null,
            Func<PropertyInfo, bool> Required = null,
            Func<PropertyInfo, object> Default = null
            ) where TBuildType : new()
        {
            string propertyName = property.Name;

            IEnumerable<char> shortOptions = null;
            if (ShortOptions != null)
                shortOptions = ShortOptions(property);
            if (shortOptions == null)
                shortOptions = new char[0];

            IEnumerable<string> longOptions = null;
            if (LongOptions != null)
                longOptions = LongOptions(property);
            if (longOptions == null)
                longOptions = new string[0];

            object defaultValue = null;
            if (Default != null)
                defaultValue = Default(property);

            bool required = false;
            if (Required != null)
                required = Required(property);

            string description = null;
            if (Description != null)
                description = Description(property);

            if (shortOptions.Count() < longOptions.Count())
            {
                var switches =
                    longOptions.Zip(shortOptions, (lo, so) => new { shortOption = (char?)so, longOption = lo })
                    .Concat(longOptions.Skip(shortOptions.Count()).Select(lo => new { shortOption = (char?)null, longOption = lo }));

                foreach (var s in switches)
                    parser.SetupSwitch<TBuildType, TProperty>(propertyName, s.shortOption, s.longOption, description, required, defaultValue);
            }
            else
            {
                var switches =
                    shortOptions.Zip(longOptions, (so, lo) => new { shortOption = so, longOption = lo })
                    .Concat(shortOptions.Skip(longOptions.Count()).Select(so => new { shortOption = so, longOption = (string)null }));

                foreach (var s in switches)
                    parser.SetupSwitch<TBuildType, TProperty>(propertyName, s.shortOption, s.longOption, description, required, defaultValue);
            }
        }

        public static void SetupSwitch<TBuildType, TProperty>(
            this FluentCommandLineParser<TBuildType> parser,
            string propertyName,
            char? shortOption = null,
            string longOption = null,
            string description = null,
            bool required = false,
            object defaultValue = null
            ) where TBuildType : new()
        {
            var arg = Expression.Parameter(typeof(TBuildType), "arg");
            var body = Expression.Property(arg, propertyName);
            var propertyPicker = Expression.Lambda<Func<TBuildType, TProperty>>(body, arg);
            var optionBuilder = parser.Setup(propertyPicker);

            ICommandLineOptionFluent<TProperty> option = null;
            if (shortOption != null && string.IsNullOrEmpty(longOption) == false)
                option = optionBuilder.As(shortOption.Value, longOption);
            else if (shortOption != null && string.IsNullOrEmpty(longOption))
                option = optionBuilder.As(shortOption.Value);
            else if (shortOption == null && string.IsNullOrEmpty(longOption) == false)
                option = optionBuilder.As(longOption);
            else
                option = optionBuilder.As(propertyName);

            if (string.IsNullOrEmpty(description) == false)
                option.WithDescription(description);

            if (required)
                option.Required();

            if (defaultValue != null && defaultValue is TProperty)
                option.SetDefault((TProperty)defaultValue);
        }
    }
}
