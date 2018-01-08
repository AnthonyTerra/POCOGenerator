using System;
using System.Text;
using Db;

namespace POCOGenerator.POCOWriter
{
    public class StringBuilderPOCOWriter : IPOCOWriter
    {
        private StringBuilder sb;

        public StringBuilderPOCOWriter(StringBuilder sb)
        {
            this.sb = sb;
        }

        public void Clear()
        {
            sb.Clear();
        }

        public void Write(string text)
        {
            sb.Append(text);
        }

        public void WriteKeyword(string text)
        {
            sb.Append(text);
        }

        public void WriteUserType(string text)
        {
            sb.Append(text);
        }

        public void WriteString(string text)
        {
            sb.Append(text);
        }

        public void WriteComment(string text)
        {
            sb.Append(text);
        }

        public void WriteError(string text)
        {
            sb.Append(text);
        }

        public void WriteLine()
        {
            sb.AppendLine();
        }

        public void WriteLine(string text)
        {
            sb.AppendLine(text);
        }

        public void WriteLineKeyword(string text)
        {
            sb.AppendLine(text);
        }

        public void WriteLineUserType(string text)
        {
            sb.AppendLine(text);
        }

        public void WriteLineString(string text)
        {
            sb.AppendLine(text);
        }

        public void WriteLineComment(string text)
        {
            sb.AppendLine(text);
        }

        public void WriteLineError(string text)
        {
            sb.AppendLine(text);
        }
    }
}
