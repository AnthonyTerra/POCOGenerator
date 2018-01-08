using System;
using Db;

namespace POCOGenerator.POCOWriter
{
    public class ConsolePOCOWriter : IPOCOWriter
    {
        public void Clear()
        {
            Console.Clear();
        }

        private void Write(string text, ConsoleColor color)
        {
            if (string.IsNullOrEmpty(text) == false)
            {
                Console.ForegroundColor = color;
                Console.Write(text);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private void WriteLine(string text, ConsoleColor color)
        {
            if (string.IsNullOrEmpty(text) == false)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine();
            }
        }
        public void Write(string text)
        {
            Console.Write(text);
        }

        public void WriteKeyword(string text)
        {
            Write(text, SyntaxColors.KeywordConsoleColor);
        }

        public void WriteUserType(string text)
        {
            Write(text, SyntaxColors.UserTypeConsoleColor);
        }

        public void WriteString(string text)
        {
            Write(text, SyntaxColors.StringConsoleColor);
        }

        public void WriteComment(string text)
        {
            Write(text, SyntaxColors.CommentConsoleColor);
        }

        public void WriteError(string text)
        {
            Write(text, SyntaxColors.ErrorConsoleColor);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void WriteLineKeyword(string text)
        {
            WriteLine(text, SyntaxColors.KeywordConsoleColor);
        }

        public void WriteLineUserType(string text)
        {
            WriteLine(text, SyntaxColors.UserTypeConsoleColor);
        }

        public void WriteLineString(string text)
        {
            WriteLine(text, SyntaxColors.StringConsoleColor);
        }

        public void WriteLineComment(string text)
        {
            WriteLine(text, SyntaxColors.CommentConsoleColor);
        }

        public void WriteLineError(string text)
        {
            WriteLine(text, SyntaxColors.ErrorConsoleColor);
        }
    }
}
