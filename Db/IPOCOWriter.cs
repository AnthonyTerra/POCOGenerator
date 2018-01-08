using System;

namespace Db
{
    public interface IPOCOWriter
    {
        void Clear();
        void Write(string text);
        void WriteKeyword(string text);
        void WriteUserType(string text);
        void WriteString(string text);
        void WriteComment(string text);
        void WriteError(string text);
        void WriteLine();
        void WriteLine(string text);
        void WriteLineKeyword(string text);
        void WriteLineUserType(string text);
        void WriteLineString(string text);
        void WriteLineComment(string text);
        void WriteLineError(string text);
    }
}
