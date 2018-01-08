using System;
using System.Drawing;

namespace POCOGenerator.POCOWriter
{
    public static class SyntaxColors
    {
        public static readonly Color KeywordColor = Color.FromArgb(0, 0, 255);
        public static readonly Color UserTypeColor = Color.FromArgb(43, 145, 175);
        public static readonly Color StringColor = Color.FromArgb(163, 21, 21);
        public static readonly Color CommentColor = Color.FromArgb(0, 128, 0);
        public static readonly Color ErrorColor = Color.FromArgb(255, 0, 0);

        public static readonly ConsoleColor KeywordConsoleColor = ConsoleColor.Blue;
        public static readonly ConsoleColor UserTypeConsoleColor = ConsoleColor.DarkCyan;
        public static readonly ConsoleColor StringConsoleColor = ConsoleColor.DarkRed;
        public static readonly ConsoleColor CommentConsoleColor = ConsoleColor.Green;
        public static readonly ConsoleColor ErrorConsoleColor = ConsoleColor.Red;
    }
}
