#region
using System;
using System.Windows;
using VeeSharpTemplate;

#endregion

namespace VeeSharpTemplateGUI
{
    public partial class PreviewWindow
    {
        private readonly string _code;
        private readonly string _title;

        public PreviewWindow(bool mCanTest, string mTitle, string mCode, string mException = "")
        {
            InitializeComponent();
            _title = mTitle;
            _code = mCode;

            Title = _title + " - VeeSharpTemplate";
            textBoxCode.Text = mException + Environment.NewLine + mCode;

            if (mCanTest) return;
            buttonTest.IsEnabled = false;
            buttonTest.Content = "";
        }

        private void Test()
        {
            try
            {
                new PreviewWindow(false, _title + " - Test", Parser.Process(_code), "Test successful - results:" + Environment.NewLine).Show();
            }
            catch (Exception exception)
            {
                textBoxCode.Text = "Test failed - results:" + Environment.NewLine + exception.Message + Environment.NewLine + _code;
            }
        }

        private void ButtonTestClick(object sender, RoutedEventArgs e) { Test(); }
    }
}