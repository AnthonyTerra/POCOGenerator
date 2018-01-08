using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POCOGenerator
{
    public enum FilterType
    {
        Equals,
        Contains,
        Does_Not_Contain
    }

    public class FilterItem
    {
        public FilterType FilterType { get; set; }

        public override string ToString()
        {
            return FilterType.ToString().Replace('_', ' ');
        }
    }

    public class FilterSetting
    {
        public FilterType FilterType { get; set; }
        public string Filter { get; set; }
    }

    public class FilterSettings
    {
        public FilterSetting FilterName { get; set; }
        public FilterSetting FilterSchema { get; set; }
        public List<TreeNode> Nodes { get; set; }

        public FilterSettings()
        {
            Nodes = new List<TreeNode>();
        }
    }
}
