using System;
using System.Text;
using System.Windows.Forms;
using Db;

namespace POCOGenerator.POCOWriter
{
    public abstract class POCOWriterFactory
    {
        public abstract IPOCOWriter CreatePOCOWriter();
    }

    public class RichTextBoxWriterFactory : POCOWriterFactory
    {
        private RichTextBox rtb;

        public RichTextBoxWriterFactory(RichTextBox rtb)
        {
            this.rtb = rtb;
        }

        public override IPOCOWriter CreatePOCOWriter()
        {
            return new RichTextBoxPOCOWriter(rtb);
        }
    }

    public class StringBuilderWriterFactory : POCOWriterFactory
    {
        public StringBuilder sb;

        public StringBuilderWriterFactory(StringBuilder sb)
        {
            this.sb = sb;
        }

        public override IPOCOWriter CreatePOCOWriter()
        {
            return new StringBuilderPOCOWriter(sb);
        }
    }

    public class ConsoleWriterFactory : POCOWriterFactory
    {
        public override IPOCOWriter CreatePOCOWriter()
        {
            return new ConsolePOCOWriter();
        }
    }
}
