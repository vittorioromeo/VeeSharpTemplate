#region
using System;

#endregion

namespace VeeSharpTemplateGUI
{
    public partial class ChoiceWindow
    {
        public ChoiceWindow(string mQuestion, string mChoice1, Action mAction1, string mChoice2 = "Close", Action mAction2 = null)
        {
            InitializeComponent();
            textBlockQuestion.Text = mQuestion;

            buttonChoice1.Content = mChoice1;
            buttonChoice2.Content = mChoice2;

            buttonChoice1.Click += (sender, e) => mAction1();
            if (mAction2 != null) buttonChoice2.Click += (sender, e) => mAction2();
            else buttonChoice2.Click += (sender, e) => Close();

            buttonChoice1.Click += (sender, e) => Close();
            buttonChoice2.Click += (sender, e) => Close();
        }
    }
}