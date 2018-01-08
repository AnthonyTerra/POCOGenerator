using System;
using System.Drawing;
using System.Windows.Forms;
using Db;

namespace POCOGenerator.POCOWriter
{
    public class RichTextBoxPOCOWriter : IPOCOWriter
    {
        private RichTextBox rtb;

        public RichTextBoxPOCOWriter(RichTextBox rtb)
        {
            this.rtb = rtb;
        }

        public void Clear()
        {
            rtb.Clear();
        }

        private void AppendText(string text, Color color)
        {
            if (string.IsNullOrEmpty(text) == false)
            {
                rtb.Select(rtb.TextLength, 0);
                rtb.SelectionColor = color;
                rtb.SelectedText = text;
                rtb.SelectionColor = rtb.ForeColor;
            }
        }

        public void Write(string text)
        {
            rtb.AppendText(text);
        }

        public void WriteKeyword(string text)
        {
            AppendText(text, SyntaxColors.KeywordColor);
        }

        public void WriteUserType(string text)
        {
            AppendText(text, SyntaxColors.UserTypeColor);
        }

        public void WriteString(string text)
        {
            AppendText(text, SyntaxColors.StringColor);
        }

        public void WriteComment(string text)
        {
            AppendText(text, SyntaxColors.CommentColor);
        }

        public void WriteError(string text)
        {
            AppendText(text, SyntaxColors.ErrorColor);
        }

        public void WriteLine()
        {
            rtb.AppendText(Environment.NewLine);
        }

        public void WriteLine(string text)
        {
            rtb.AppendText(text);
            rtb.AppendText(Environment.NewLine);
        }

        public void WriteLineKeyword(string text)
        {
            AppendText(text, SyntaxColors.KeywordColor);
            rtb.AppendText(Environment.NewLine);
        }

        public void WriteLineUserType(string text)
        {
            AppendText(text, SyntaxColors.UserTypeColor);
            rtb.AppendText(Environment.NewLine);
        }

        public void WriteLineString(string text)
        {
            AppendText(text, SyntaxColors.StringColor);
            rtb.AppendText(Environment.NewLine);
        }

        public void WriteLineComment(string text)
        {
            AppendText(text, SyntaxColors.CommentColor);
            rtb.AppendText(Environment.NewLine);
        }

        public void WriteLineError(string text)
        {
            AppendText(text, SyntaxColors.ErrorColor);
            rtb.AppendText(Environment.NewLine);
        }
    }
}
