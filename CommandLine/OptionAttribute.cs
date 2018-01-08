using System;

namespace CommandLine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OptionAttribute : Attribute
    {
        public char? ShortOption { get; private set; }
        public string LongOption { get; private set; }
        public string Description { get; private set; }
        public bool Required { get; private set; }
        public object Default { get; private set; }
        public string MutuallyExclusiveSet { get; private set; }

        public OptionAttribute(string Description = null, bool Required = false, object Default = null, string MutuallyExclusiveSet = null)
        {
            this.Description = Description;
            this.Required = Required;
            this.Default = Default;
            this.MutuallyExclusiveSet = MutuallyExclusiveSet;
        }

        public OptionAttribute(char ShortOption, string Description = null, bool Required = false, object Default = null, string MutuallyExclusiveSet = null)
            : this(Description, Required, Default, MutuallyExclusiveSet)
        {
            this.ShortOption = ShortOption;
        }

        public OptionAttribute(string LongOption, string Description = null, bool Required = false, object Default = null, string MutuallyExclusiveSet = null)
            : this(Description, Required, Default, MutuallyExclusiveSet)
        {
            this.LongOption = LongOption;
        }

        public OptionAttribute(char ShortOption, string LongOption, string Description = null, bool Required = false, object Default = null, string MutuallyExclusiveSet = null)
            : this(Description, Required, Default, MutuallyExclusiveSet)
        {
            this.ShortOption = ShortOption;
            this.LongOption = LongOption;
        }
    }
}
