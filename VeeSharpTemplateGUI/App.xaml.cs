#region
using System.Windows;

#endregion

namespace VeeSharpTemplateGUI
{
    /// <summary>
    ///   Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new StartWindow().Show();
        }
    }
}