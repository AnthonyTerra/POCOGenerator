using System;
using System.Collections.Generic;
using Fclp;

namespace CommandLine
{
    public class CommandLineParsingResult<TBuildType> where TBuildType : new()
    {
        public TBuildType Options { get; set; }
        public bool EmptyArgs { get; set; }
        public bool HasErrors { get; set; }
        public string ErrorText { get; set; }
        public bool HelpCalled { get; set; }
        public bool HasMutuallyExclusiveSetErrors { get; set; }
        public IEnumerable<string> InvalidMutuallyExclusiveSets { get; set; }
        public IEnumerable<string> InvalidMutuallyExclusiveSetsByDefaultValues { get; set; }
        public ICommandLineParserResult ParserResult { get; set; }
    }
}
