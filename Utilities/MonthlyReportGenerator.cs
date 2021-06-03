using System;
using System.IO;
using System.Reflection;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using Shape = Microsoft.Office.Interop.Word.Shape;


namespace Utilities
{
    public class MonthlyReportGenerator
    {
        private Document myWordDoc = null;
        private void FindAndReplace(Word.Application app, string findText, string replaceWithText)
        {
            Find findObject = app.Selection.Find;
            findObject.ClearFormatting();
            findObject.Text = findText;
            findObject.Replacement.ClearFormatting();
            findObject.Replacement.Text = replaceWithText;
            object missing = Missing.Value;
            object replaceAll = WdReplace.wdReplaceAll;
            findObject.Execute(ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref replaceAll, ref missing, ref missing, ref missing, ref missing);
            var shapes = myWordDoc.Shapes;
            foreach (Shape shape in shapes)
            {
                if (shape.TextFrame.HasText != 0)
                {
                    var initialText = shape.TextFrame.TextRange.Text;
                    var resultingText = initialText.Replace(findText, replaceWithText);
                    if (initialText != resultingText)
                    {
                        shape.TextFrame.TextRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
                        shape.TextFrame.TextRange.Text = resultingText;
                    }
                }
            }
        }

        public void CreateMonthlyReportDocument(object SaveAs, MonthlyReportTemplate monthlyReportTemplate)
        {
            object filename = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Views\\monthlyReport.docx"));
            Word.Application wordApp = new Word.Application();
            object missing = Missing.Value;

            if (File.Exists((string)filename))
            {
                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                myWordDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly,
                                        ref missing, ref missing, ref missing,
                                        ref missing, ref missing, ref missing,
                                        ref missing, ref missing, ref missing,
                                        ref missing, ref missing, ref missing, ref missing);
                myWordDoc.Activate();

                FindAndReplace(wordApp, "<Practicioner>", monthlyReportTemplate.Practicioner);
                FindAndReplace(wordApp, "<LinkedOrganization>", monthlyReportTemplate.LinkedOrganization);
                FindAndReplace(wordApp, "<Project>", monthlyReportTemplate.Project);
                FindAndReplace(wordApp, "<MonthYear>", monthlyReportTemplate.MonthYear);

                FindAndReplace(wordApp, "<Activities>", monthlyReportTemplate.Activities);
                FindAndReplace(wordApp, "<Results>", monthlyReportTemplate.Results);

                FindAndReplace(wordApp, "<ReasonOne>", monthlyReportTemplate.ReasonOne);
                FindAndReplace(wordApp, "<ReasonTwo>", monthlyReportTemplate.ReasonTwo);
                FindAndReplace(wordApp, "<ReasonThree>", monthlyReportTemplate.ReasonThree);
                FindAndReplace(wordApp, "<YesOne>", monthlyReportTemplate.YesOne);
                FindAndReplace(wordApp, "<YesTwo>", monthlyReportTemplate.YesTwo);
                FindAndReplace(wordApp, "<YesThree>", monthlyReportTemplate.YesThree);
                FindAndReplace(wordApp, "<NoOne>", monthlyReportTemplate.NoOne);
                FindAndReplace(wordApp, "<NoTwo>", monthlyReportTemplate.NoTwo);
                FindAndReplace(wordApp, "<NoThree>", monthlyReportTemplate.NoThree);

                FindAndReplace(wordApp, "<DateTime>", monthlyReportTemplate.DateTime);

            }

            int index = 1;
            bool isDownload = true;
            string routeDestination = (string)SaveAs;
            while (isDownload)
            {
                if (File.Exists(routeDestination))
                {
                    int indexPrevious = index - 1;
                    if (index == 1)
                    {
                        routeDestination = routeDestination.Replace(".docx", "(" + index.ToString() + ")" + ".docx");
                    }
                    else
                    {
                        routeDestination = routeDestination.Replace("(" + indexPrevious.ToString() + ")" + ".docx", "(" + index.ToString() + ")" + ".docx");
                    }
                    index++;
                }
                else
                {
                    object saveNewAs = routeDestination;
                    myWordDoc.SaveAs2(ref saveNewAs, ref missing, ref missing, ref missing,
                           ref missing, ref missing, ref missing,
                           ref missing, ref missing, ref missing,
                           ref missing, ref missing, ref missing,
                           ref missing, ref missing, ref missing);

                    myWordDoc.Close();
                    wordApp.Quit();
                    isDownload = false;
                }
            }
        }
    }
}
