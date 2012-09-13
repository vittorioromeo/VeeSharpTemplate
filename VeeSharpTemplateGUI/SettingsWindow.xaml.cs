#region
using System.ComponentModel;
using System.Windows;
using VeeSharpTemplate;

#endregion

namespace VeeSharpTemplateGUI
{
    public partial class SettingsWindow
    {
        private readonly File _file;
        private bool _saveChoiceMade;

        public SettingsWindow(File mFile)
        {
            InitializeComponent();
            _file = mFile;
            SyncFromTemplateFile();
        }

        private void SyncFromTemplateFile()
        {
            textBoxPrefix.Text = _file.Prefix;
            textBoxTemplateName.Text = _file.TemplateName;
            textBoxFolder.Text = _file.Folder;
        }
        private void SyncToTemplateFile()
        {
            _file.Prefix = textBoxPrefix.Text;
            _file.TemplateName = textBoxTemplateName.Text;
            _file.Folder = textBoxFolder.Text;
        }

        private void ButtonApplyClick(object sender, RoutedEventArgs e)
        {
            _saveChoiceMade = true;
            SyncToTemplateFile();
            Close();
        }
        private void MetroWindowClosing(object sender, CancelEventArgs e)
        {
            if (_saveChoiceMade) return;

            e.Cancel = true;
            new ChoiceWindow("apply before closing?", "yes", () =>
                                                             {
                                                                 SyncToTemplateFile();
                                                                 Close();
                                                             }, "no", Close).Show();
            _saveChoiceMade = true;
        }
    }
}