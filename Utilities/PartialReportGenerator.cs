using System;
using System.IO;
using System.Reflection;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using Shape = Microsoft.Office.Interop.Word.Shape;


namespace Utilities
{
    public class PartialReportGenerator
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

        public void CreatePartialReportDocument(object SaveAs, PartialReportTemplate partialReportTemplate)
        {
            object filename = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Views\\partialReport.docx"));
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

                FindAndReplace(wordApp, "<Career>", partialReportTemplate.Career);
                FindAndReplace(wordApp, "<Teacher>", partialReportTemplate.Techaer);
                FindAndReplace(wordApp, "<NRC>", partialReportTemplate.NRC);
                FindAndReplace(wordApp, "<Term>", partialReportTemplate.Term);

                FindAndReplace(wordApp, "<Practicioner>", partialReportTemplate.Practicioner);
                FindAndReplace(wordApp, "<Project>", partialReportTemplate.Project);
                FindAndReplace(wordApp, "<Date>", partialReportTemplate.Date);
                FindAndReplace(wordApp, "<LinkedOrganization>", partialReportTemplate.LinkedOrganization);
                FindAndReplace(wordApp, "<Hours>", partialReportTemplate.Hours);
                FindAndReplace(wordApp, "<Number>", partialReportTemplate.Number);

                FindAndReplace(wordApp, "<GeneralObjectives>", partialReportTemplate.GeneralObjective);
                FindAndReplace(wordApp, "<Methodology>", partialReportTemplate.Methodology);

                FindAndReplace(wordApp, "<ActivityOne>", partialReportTemplate.ActivityOne);
                FindAndReplace(wordApp, "<ActivityTwo>", partialReportTemplate.ActivityTwo);
                FindAndReplace(wordApp, "<ActivityThree>", partialReportTemplate.ActivityThree);
                FindAndReplace(wordApp, "<ActivityFour>", partialReportTemplate.ActivityFour);
                FindAndReplace(wordApp, "<ActivityFive>", partialReportTemplate.ActivityFive);

                foreach (CheckListItem item in partialReportTemplate.WeekPlan1)
                {
                    FindAndReplace(wordApp, item.TheName, item.TheValue);
                }
                foreach (CheckListItem item in partialReportTemplate.WeekPlan2)
                {
                    FindAndReplace(wordApp, item.TheName, item.TheValue);
                }
                foreach (CheckListItem item in partialReportTemplate.WeekPlan3)
                {
                    FindAndReplace(wordApp, item.TheName, item.TheValue);
                }
                foreach (CheckListItem item in partialReportTemplate.WeekPlan4)
                {
                    FindAndReplace(wordApp, item.TheName, item.TheValue);
                }
                foreach (CheckListItem item in partialReportTemplate.WeekPlan5)
                {
                    FindAndReplace(wordApp, item.TheName, item.TheValue);
                }

                foreach (CheckListItem item in partialReportTemplate.WeekReal1)
                {
                    FindAndReplace(wordApp, item.TheName, item.TheValue);
                }
                foreach (CheckListItem item in partialReportTemplate.WeekReal2)
                {
                    FindAndReplace(wordApp, item.TheName, item.TheValue);
                }
                foreach (CheckListItem item in partialReportTemplate.WeekReal3)
                {
                    FindAndReplace(wordApp, item.TheName, item.TheValue);
                }
                foreach (CheckListItem item in partialReportTemplate.WeekReal4)
                {
                    FindAndReplace(wordApp, item.TheName, item.TheValue);
                }
                foreach (CheckListItem item in partialReportTemplate.WeekReal5)
                {
                    FindAndReplace(wordApp, item.TheName, item.TheValue);
                }
                FindAndReplace(wordApp, "<Result>", partialReportTemplate.Result);
                FindAndReplace(wordApp, "<Observations>", partialReportTemplate.Observations);
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
