using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace ServerBrowser.Services.Dialog
{
    public class DialogService : IDialogService
    {
        public string ShowOpenFileDialog(string filter)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var openFileDialog = new OpenFileDialog { Filter = filter, InitialDirectory = directory };
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return null;

        }
    }
}
