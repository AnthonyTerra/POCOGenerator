using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CommandLine;
using POCOGenerator.CommandLine;

namespace POCOGenerator
{
    public partial class CommandLineForm : Form
    {
        private List<string> shortCommandParts;
        private List<string> longCommandParts;
        private bool isShortCommand;

        public CommandLineForm()
        {
            InitializeComponent();
            SetCommandLineHelp();
        }

        private static readonly Regex regexCamelCase = new Regex("(?<word>[A-Z]{2,}|[A-Z][^A-Z]*?|^[^A-Z]*?)(?=[A-Z]|$)", RegexOptions.Compiled);

        private void SetCommandLineHelp()
        {
            List<Tuple<string, string>> help = CommandLineParser<Options>.GetCommandLineHelp();

            List<Tuple<string, List<Tuple<string, string>>>> helpGroups = new List<Tuple<string, List<Tuple<string, string>>>>();
            SetHelpGroup(help, helpGroups, 1, string.Empty);
            SetHelpGroup(help, helpGroups, 14, "POCO");
            SetHelpGroup(help, helpGroups, 8, "Navigation Properties");
            SetHelpGroup(help, helpGroups, 16, "Class Name");
            SetHelpGroup(help, helpGroups, 11, "EF Code-First Annotations");
            SetHelpGroup(help, helpGroups, 3, "Export to Files");
            SetHelpGroup(help, helpGroups, 1, "All");
            SetHelpGroup(help, helpGroups, 4, "Tables");
            SetHelpGroup(help, helpGroups, 4, "Views");
            SetHelpGroup(help, helpGroups, 4, "Stored Procedures");
            SetHelpGroup(help, helpGroups, 4, "Functions");
            SetHelpGroup(help, helpGroups, 4, "TVPs");

            List<Tuple<string, string>> resultsHelpLines = new List<Tuple<string, string>>();
            var commandLineResults = (Enum.GetValues(typeof(CommandLineResult)) as CommandLineResult[]).Select(val => new { Name = Enum.GetName(typeof(CommandLineResult), val), Code = (int)val }).ToArray();
            foreach (var result in commandLineResults.Where(r => r.Code >= 0).OrderBy(r => r.Code))
            {
                List<string> camelCaseWords = new List<string>();
                foreach (Match match in regexCamelCase.Matches(result.Name))
                    camelCaseWords.Add(match.Groups["word"].Value);

                string name = string.Format("{0,2} {1}", result.Code, string.Join(" ", camelCaseWords));

                resultsHelpLines.Add(new Tuple<string, string>(name, string.Empty));
            }
            helpGroups.Add(new Tuple<string, List<Tuple<string, string>>>("Return Codes", resultsHelpLines));

            List<string> lines = new List<string>();
            foreach (var group in helpGroups)
            {
                int leftAlignment = group.Item2.Max(l => l.Item1.Length) + 2;
                if (string.IsNullOrEmpty(group.Item1) == false)
                    lines.Add(group.Item1 + ":");
                lines.AddRange(group.Item2.Select(l => string.Format("{0," + (-leftAlignment) + "}{1}", l.Item1, l.Item2)));
                lines.Add(string.Empty);
            }
            lines.RemoveAt(lines.Count - 1);

            txtHelpEditor.Lines = lines.ToArray();
        }

        private void SetHelpGroup(List<Tuple<string, string>> help, List<Tuple<string, List<Tuple<string, string>>>> helpGroups, int count, string groupName)
        {
            helpGroups.Add(new Tuple<string, List<Tuple<string, string>>>(groupName, help.Take(count).ToList()));
            help.RemoveRange(0, count);
        }

        public void SetCommandLineParts(List<string> shortCommandParts, List<string> longCommandParts)
        {
            this.shortCommandParts = shortCommandParts;
            this.longCommandParts = longCommandParts;
            string executable = string.Format("\"{0}\"", System.Reflection.Assembly.GetEntryAssembly().Location);
            this.shortCommandParts.Insert(0, executable);
            this.longCommandParts.Insert(0, executable);
            SetCommandLine();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(txtCommandLineEditor.Text);
            }
            catch { }
        }

        private void btnToggleSwitchesSize_Click(object sender, EventArgs e)
        {
            isShortCommand = !isShortCommand;
            btnToggleSwitchesSize.Text = (isShortCommand ? "Long Switches" : "Short Switches");
            SetCommandLine();
        }

        private void SetCommandLine()
        {
            txtCommandLineEditor.Text = string.Join(" ", (isShortCommand ? shortCommandParts : longCommandParts));
        }

        private void btnSaveBatFile_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText("POCOGenerator.bat", txtCommandLineEditor.Text);
                MessageBox.Show("POCOGenerator.bat was saved successfully", "File Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save POCOGenerator.bat" + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
