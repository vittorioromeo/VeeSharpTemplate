#region
using Microsoft.VisualBasic;
using Microsoft.Win32;
using VeeSharpTemplate;

#endregion

namespace VeeSharpTemplateGUI
{
    public static class Utils
    {
        private static readonly string FilterSolution = string.Format("VeeSharpTemplate Solution (*{0})|*{0}", VeeSharpTemplate.Utils.ExtensionSolution);

        public static Solution SolutionFromSaveDialog()
        {
            var saveFileDialog = new SaveFileDialog {Filter = FilterSolution};
            if (!saveFileDialog.ShowDialog().Value) return null;

            var templateSolution = new Solution(saveFileDialog.FileName);
            templateSolution.SaveToFile();
            return templateSolution;
        }
        public static Solution SolutionFromOpenDialog()
        {
            var openFileDialog = new OpenFileDialog {Filter = FilterSolution};
            if (!openFileDialog.ShowDialog().Value) return null;

            var templateSolution = new Solution(openFileDialog.FileName);
            templateSolution.LoadFromFile();
            return templateSolution;
        }
        public static File FileCreateFromSaveDialog(Solution mSolution)
        {
            var input = Interaction.InputBox("Template file name without extension (will be placed in solution folder)",
                                             "Insert template file name");
            if (string.IsNullOrEmpty(input)) return null;

            var templateFile = new File(mSolution, input);
            templateFile.SaveToFile();
            return templateFile;
        }
        public static string CSProjectFromOpenDialog()
        {
            var openFileDialog = new OpenFileDialog {Filter = "C# Project (*.csproj)|*.csproj"};
            var dialogValid = openFileDialog.ShowDialog();
            return !dialogValid.Value ? null : openFileDialog.FileName;
        }
    }
}