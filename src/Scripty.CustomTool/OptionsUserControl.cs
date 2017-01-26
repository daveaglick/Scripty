namespace Scripty
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows.Forms;
    using Core;

    public partial class OptionsUserControl : UserControl
    {
        public OptionsUserControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            var outputBehavior = ScriptyOptions.OutputBehavior;
            ComboItem selectedItem = null;

            if (cbOutputBehavior.Items.Count == 0)
            {
                foreach (var enumValue in Enum.GetValues(typeof(OutputBehavior)))
                {
                    var obType = (OutputBehavior)enumValue;
                    var comboItem = new ComboItem
                    {
                        OB= obType,
                        DisplayName= obType.GetName(),
                        Description =  obType.GetDescription()
                    };
                    if (outputBehavior == obType)
                    {
                        selectedItem = comboItem;
                    }
                    cbOutputBehavior.Items.Add(comboItem);
                }
            }

            cbOutputBehavior.DisplayMember = "DisplayName";

            
            cbOutputBehavior.SelectedItem = selectedItem;
        }


        internal ScriptyOptions ScriptyOptions { get; set; }
        
        private void cbOutputBehavior_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedValue = cbOutputBehavior.SelectedItem as ComboItem;
            if (selectedValue == null)
            {
                return;
            }

            rtbDescription.Text = selectedValue.Description;
        }
      
        private class ComboItem
        {
            public OutputBehavior OB { get; set; }
            public string DisplayName { get; set; }
            public string Description { get; set; }
        }

    }
}