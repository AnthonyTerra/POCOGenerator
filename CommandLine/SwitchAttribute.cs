using System;

namespace CommandLine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SwitchAttribute : Attribute
    {
        public char? ShortOption { get; private set; }
        public string LongOption { get; private set; }

        public SwitchAttribute(char ShortOption)
        {
            this.ShortOption = ShortOption;
        }

        public SwitchAttribute(string LongOption)
        {
            this.LongOption = LongOption;
        }

        public SwitchAttribute(char ShortOption, string LongOption)
        {
            this.ShortOption = ShortOption;
            this.LongOption = LongOption;
        }
    }
}