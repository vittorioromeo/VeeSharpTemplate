#region
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using VeeSharpTemplate;

#endregion

namespace VeeSharpTemplateGUI
{
    public partial class SolutionWindow
    {
        private readonly Solution _solution;
        private bool _saveChoiceMade;

        public SolutionWindow(Solution mSolution)
        {
            InitializeComponent();

            _solution = mSolution;
            SyncFromSolution();
        }

        private void CreateFile()
        {
            try
            {
                var templateFile = Utils.FileCreateFromSaveDialog(_solution);
                if (templateFile == null) return;
                _solution.AddNewFile(templateFile);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            SyncFromSolution();
        }
        private void SaveSolution()
        {
            try
            {
                _solution.SaveToFile();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            SyncFromSolution();
        }
        private void SetProject()
        {
            var projectPath = Utils.CSProjectFromOpenDialog();
            if (projectPath == null) return;
            _solution.SetProject(projectPath);
            SyncFromSolution();
        }
        private void GenerateSolution()
        {
            var previews = new List<string>();

            try
            {
                _solution.Generate(previews);
            }
            catch (Exception exception)
            {
                foreach (var preview in previews)
                    new PreviewWindow(false, "Generation Error", exception.Message + Environment.NewLine + preview).Show();
            }

            SyncFromSolution();
        }

        private void SyncFromSolution()
        {
            try
            {
                while (listBoxFiles.Items.Count > 5) listBoxFiles.Items.RemoveAt(5);

                foreach (var templateFile in _solution.Files)
                {
                    var item = new ListBoxItem
                               {
                                   Content = templateFile.TemplateName,
                                   FontSize = 18,
                                   FontStyle = FontStyles.Italic
                               };

                    listBoxFiles.Items.Add(item);

                    var file = templateFile;
                    item.MouseDoubleClick += (sender, args) =>
                                             {
                                                 var editorWindow = new EditorWindow(file);
                                                 editorWindow.Show();
                                                 SyncFromSolution();
                                             };
                }

                var solutionName = Path.GetFileNameWithoutExtension(_solution.Path);

                textBlockTitle.Text = solutionName;

                if (string.IsNullOrEmpty(_solution.ProjectFileName)) textBlockProject.Text = "no project set";
                else textBlockProject.Text = "current project: " + _solution.ProjectFileName;

                Title = string.Format("{0} - VeeSharpTemplate", solutionName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void ButtonCreateFileClick(object sender, RoutedEventArgs e) { CreateFile(); }
        private void ButtonSetProject(object sender, RoutedEventArgs e) { SetProject(); }
        private void ButtonSaveClick(object sender, RoutedEventArgs e) { SaveSolution(); }
        private void ButtonGenerateClick(object sender, RoutedEventArgs e) { GenerateSolution(); }
        private void MetroWindowClosing(object sender, CancelEventArgs e)
        {
            if (_saveChoiceMade) return;

            e.Cancel = true;
            new ChoiceWindow("save before closing?", "yes", () =>
                                                            {
                                                                SaveSolution();
                                                                Close();
                                                            }, "no", Close).Show();
            _saveChoiceMade = true;
        }
    }
}