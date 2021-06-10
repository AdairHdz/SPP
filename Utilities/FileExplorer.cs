using System;
using System.Windows.Forms;

namespace Utilities
{
    public class FileExplorer
    {        
        public static string Show(string windowTitle)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word document|*.docx";
            saveFileDialog.Title = windowTitle;
            saveFileDialog.ShowDialog();
            string savingPath = saveFileDialog.FileName;
            if(savingPath.Length == 0)
            {
                throw new NullReferenceException();
            }
            return savingPath;
        }
    }
}
