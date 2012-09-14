#region
using System;
using System.ComponentModel;
using System.Windows;
using VeeSharpTemplate;

#endregion

namespace VeeSharpTemplateGUI
{
    public partial class EditorWindow
    {
        private readonly File _file;
        private string _originalCode;
        private bool _saveChoiceMade;
        private SettingsWindow _settingsWindow;

        public EditorWindow(File mFile)
        {
            InitializeComponent();
            _file = mFile;

            SyncFromTemplateFile();
        }

        private void ShowPreview() { new PreviewWindow(true, string.Format("{0} - Preview", _file.TemplateName), Parser.ParseSymbols(textBoxCode.Text)).Show(); }
        private void SyncFromTemplateFile()
        {
            if (_originalCode == null) _originalCode = _file.Code;
            textBoxCode.Text = _file.Code;
            Title = string.Format("{0} - VeeSharpTemplate", _file.TemplateName);
        }
        private void SyncToTemplateFile()
        {
            try
            {
                _file.Code = textBoxCode.Text;
                _file.SaveToFile();
                _originalCode = _file.Code;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private void ShowSettings()
        {
            _settingsWindow = new SettingsWindow(_file);
            _settingsWindow.Show();
        }

        private void ButtonPreviewClick(object sender, RoutedEventArgs e) { ShowPreview(); }
        private void ButtonSaveClick(object sender, RoutedEventArgs e) { SyncToTemplateFile(); }
        private void ButtonSettingsClick(object sender, RoutedEventArgs e) { ShowSettings(); }
        private void MetroWindowClosing(object sender, CancelEventArgs e)
        {
            if (_originalCode == textBoxCode.Text) return;
            if (_saveChoiceMade) return;

            e.Cancel = true;
            new ChoiceWindow("save before closing?", "yes", () =>
                                                            {
                                                                SyncToTemplateFile();
                                                                Close();
                                                            }, "no", Close).Show();
            _saveChoiceMade = true;
        }
    }
}