using System;
using System.Drawing;
using System.Windows.Forms;
using POCOGenerator.POCOWriter;

namespace POCOGenerator
{
    public partial class TypeMappingForm : Form
    {
        public TypeMappingForm()
        {
            InitializeComponent();
            LoadTypeMappingForm();
        }

        private void AppendText(string text, Color color)
        {
            txtTypeMappingEditor.Select(txtTypeMappingEditor.TextLength, 0);
            txtTypeMappingEditor.SelectionColor = color;
            txtTypeMappingEditor.SelectedText = text;
            txtTypeMappingEditor.SelectionColor = txtTypeMappingEditor.ForeColor;
        }

        private void LoadTypeMappingForm()
        {
            txtTypeMappingEditor.AppendText("┌──────────────────┬──────────────────────────────────────────┐");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│    SQL Server    │                   .NET                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("├──────────────────┼──────────────────────────────────────────┤");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("bigint", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("           │ ");
            AppendText("long", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                     │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("binary", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("           │ ");
            AppendText("byte", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("[]");
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("bit", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("              │ ");
            AppendText("bool", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                     │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("char", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("             │ ");
            AppendText("string", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("date", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("             │ ");
            AppendText("DateTime", SyntaxColors.UserTypeColor);
            txtTypeMappingEditor.AppendText("                                 │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("datetime", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("         │ ");
            AppendText("DateTime", SyntaxColors.UserTypeColor);
            txtTypeMappingEditor.AppendText("                                 │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("datetime2", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("        │ ");
            AppendText("DateTime", SyntaxColors.UserTypeColor);
            txtTypeMappingEditor.AppendText("                                 │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("datetimeoffset", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("   │ ");
            AppendText("DateTimeOffset", SyntaxColors.UserTypeColor);
            txtTypeMappingEditor.AppendText("                           │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("decimal", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("          │ ");
            AppendText("decimal", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                  │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("filestream", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("       │ ");
            AppendText("byte", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("[]");
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("float", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("            │ ");
            AppendText("double", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("geography", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("        │ ");
            txtTypeMappingEditor.AppendText("Microsoft.SqlServer.Types.");
            AppendText("SqlGeography", SyntaxColors.UserTypeColor);
            txtTypeMappingEditor.AppendText("   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("geometry", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("         │ ");
            txtTypeMappingEditor.AppendText("Microsoft.SqlServer.Types.");
            AppendText("SqlGeometry", SyntaxColors.UserTypeColor);
            txtTypeMappingEditor.AppendText("    │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("hierarchyid", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("      │ ");
            txtTypeMappingEditor.AppendText("Microsoft.SqlServer.Types.");
            AppendText("SqlHierarchyId", SyntaxColors.UserTypeColor);
            txtTypeMappingEditor.AppendText(" │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("image", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("            │ ");
            AppendText("byte", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("[]");
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("int", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("              │ ");
            AppendText("int", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                      │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("money", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("            │ ");
            AppendText("decimal", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                  │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("nchar", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("            │ ");
            AppendText("string", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("ntext", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("            │ ");
            AppendText("string", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("numeric", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("          │ ");
            AppendText("decimal", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                  │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("nvarchar", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("         │ ");
            AppendText("string", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("real", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("             │ ");
            AppendText("Single", SyntaxColors.UserTypeColor);
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("rowversion", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("       │ ");
            AppendText("byte", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("[]");
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("smalldatetime", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("    │ ");
            AppendText("DateTime", SyntaxColors.UserTypeColor);
            txtTypeMappingEditor.AppendText("                                 │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("smallint", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("         │ ");
            AppendText("short", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                    │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("smallmoney", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("       │ ");
            AppendText("decimal", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                  │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("sql_variant", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("      │ ");
            AppendText("object", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("text", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("             │ ");
            AppendText("string", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("time", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("             │ ");
            AppendText("TimeSpan", SyntaxColors.UserTypeColor);
            txtTypeMappingEditor.AppendText("                                 │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("timestamp", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("        │ ");
            AppendText("byte", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("[]");
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("tinyint", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("          │ ");
            AppendText("byte", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                     │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("uniqueidentifier", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText(" │ ");
            AppendText("Guid", SyntaxColors.UserTypeColor);
            txtTypeMappingEditor.AppendText("                                     │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("varbinary", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("        │ ");
            AppendText("byte", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("[]");
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("varchar", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("          │ ");
            AppendText("string", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendText("xml", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("              │ ");
            AppendText("string", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            txtTypeMappingEditor.AppendText("else");
            txtTypeMappingEditor.AppendText("             │ ");
            AppendText("object", SyntaxColors.KeywordColor);
            txtTypeMappingEditor.AppendText("                                   │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("└──────────────────┴──────────────────────────────────────────┘");
        }
    }
}