using System.Text.RegularExpressions;

namespace Utilities
{
    public class DocumentSaver
    {
        private string _savingPath;        

        private void OpenPathExplorer() { }

        private bool IsValidPath()
        {
            Regex regularExpression = new Regex("^[A-Z]{1}:\\([A-z0-9-_ +]+\\)*([A-z0-9]+.(docx))$");
            return regularExpression.IsMatch(_savingPath);
        }

        public void SaveFile(AcceptanceOfficeTemplate acceptanceOfficeTemplate)
        {
            OpenPathExplorer();
            DocumentGenerator documentGenerator = new DocumentGenerator();
            documentGenerator.CreateAcceptanceOfficeDocument($"{_savingPath}", acceptanceOfficeTemplate);
            throw new InvalidPathException();
        }

    }
}
