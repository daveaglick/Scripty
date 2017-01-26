namespace Scripty
{
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Core;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    ///     Its options.
    /// </summary>
    [Guid(SCRIPTY_OPTIONS_GUID_STRING)]
    public sealed class ScriptyOptions : DialogPage
    {
        #region "const"

        /// <summary>
        ///     I SCREAM IN ALL CAPS, FOR I AM CONSTANT
        /// </summary>
        public const string SCRIPTY_OPTIONS_GUID_STRING = "1fd5d182-c25d-47a2-b4f3-e1471556b246";
        public const string CATEGORY = "Scripty Options";
        public const string PAGE = "General";
        public const string ITEM_OUTPUT_BEHAVIOR = nameof(OutputBehavior);

        #endregion //#region "const"

        #region "Properties"

        /// <summary>
        ///     Determine scripty output handling
        /// </summary>
        /// <remarks>This value is shown in the options page.</remarks>
        [Category("Behaviors")]
        [DisplayName("Output Behavior")]
        [Description("What should scripty do in regards to output handling?")]
        public OutputBehavior OutputBehavior { get; set; }

        #endregion //#region "Properties"
   
        protected override IWin32Window Window
        {
            get
            {
                var optionsPage = new OptionsUserControl();
                optionsPage.ScriptyOptions = this;
                optionsPage.Initialize();
                return optionsPage;
            }
        }
    }
}

