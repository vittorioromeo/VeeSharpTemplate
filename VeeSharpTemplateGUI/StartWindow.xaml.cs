#region
using System;
using System.Windows;

#endregion

namespace VeeSharpTemplateGUI
{
    public partial class StartWindow
    {
        public StartWindow() { InitializeComponent(); }

        private static void NewSolution()
        {
            try
            {
                var templateSolution = Utils.SolutionFromSaveDialog();
                if (templateSolution == null) return;
                new SolutionWindow(templateSolution).Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private static void OpenSolution()
        {
            try
            {
                var templateSolution = Utils.SolutionFromOpenDialog();
                if (templateSolution == null) return;
                new SolutionWindow(templateSolution).Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void ButtonNewClick(object sender, RoutedEventArgs e) { NewSolution(); }
        private void ButtonOpenClick(object sender, RoutedEventArgs e) { OpenSolution(); }
        private void MetroWindowClosed(object sender, EventArgs e) { Environment.Exit(0); }
    }
}