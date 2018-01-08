using System;
using System.Windows.Forms;

namespace POCOGenerator
{
    public partial class FilterSettingsForm : Form
    {
        public FilterSettingsForm()
        {
            InitializeComponent();
            SetFilterSettingsForm();
        }

        private void SetFilterSettingsForm()
        {
            ddlFilterName.Items.Add(new FilterItem() { FilterType = FilterType.Equals });
            ddlFilterName.Items.Add(new FilterItem() { FilterType = FilterType.Contains });
            ddlFilterName.Items.Add(new FilterItem() { FilterType = FilterType.Does_Not_Contain });

            ddlFilterSchema.Items.Add(new FilterItem() { FilterType = FilterType.Equals });
            ddlFilterSchema.Items.Add(new FilterItem() { FilterType = FilterType.Contains });
            ddlFilterSchema.Items.Add(new FilterItem() { FilterType = FilterType.Does_Not_Contain });

            ClearFilter();
        }

        public void ClearFilter()
        {
            SetFilterName(FilterType.Contains, string.Empty);
            SetFilterSchema(FilterType.Contains, string.Empty);
        }

        public void SetFilter(FilterSettings filterSettings)
        {
            SetFilterName(filterSettings.FilterName.FilterType, filterSettings.FilterName.Filter);
            SetFilterSchema(filterSettings.FilterSchema.FilterType, filterSettings.FilterSchema.Filter);
        }

        public FilterSettings GetFilter()
        {
            return new FilterSettings()
            {
                FilterName = new FilterSetting() { FilterType = FilterTypeName, Filter = FilterName },
                FilterSchema = new FilterSetting() { FilterType = FilterTypeSchema, Filter = FilterSchema }
            };
        }

        public void SetFilterName(FilterType filterType, string value)
        {
            ddlFilterName.SelectedIndex = (int)filterType;
            txtFilterName.Text = value;
        }

        public void SetFilterSchema(FilterType filterType, string value)
        {
            ddlFilterSchema.SelectedIndex = (int)filterType;
            txtFilterSchema.Text = value;
        }

        public string FilterName
        {
            get { return txtFilterName.Text; }
        }

        public string FilterSchema
        {
            get { return txtFilterSchema.Text; }
        }

        public FilterType FilterTypeName
        {
            get { return ((FilterItem)ddlFilterName.SelectedItem).FilterType; }
        }

        public FilterType FilterTypeSchema
        {
            get { return ((FilterItem)ddlFilterSchema.SelectedItem).FilterType; }
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            ClearFilter();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FilterSettingsForm_Shown(object sender, EventArgs e)
        {
            txtFilterName.Focus();
        }
    }
}
