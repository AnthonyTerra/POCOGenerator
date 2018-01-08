using System;
using System.Windows.Forms;
using CommandLine;
using POCOGenerator.CommandLine;

namespace POCOGenerator
{
    static class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            CommandLineParsingResult<Options> parsingResult = CommandLineParser<Options>.Parse(args);
            CommandLineResult resultCode = CommandLineParser.ValidateOptions(parsingResult);

            if (resultCode > CommandLineResult.NoErrors)
                return (int)resultCode;

            if (resultCode == CommandLineResult.EmptyArgs)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new POCOGeneratorForm());
            }
            else if (resultCode == CommandLineResult.NoErrors)
            {
                ComandLineWriter writer = new ComandLineWriter(parsingResult.Options);
                CommandLineResult result = writer.Export();
                return (int)result;
            }

            return 0;
        }
    }
}
