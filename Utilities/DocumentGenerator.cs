using System;
using System.IO;
using System.Reflection;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using Shape = Microsoft.Office.Interop.Word.Shape;

namespace Utilities
{
    public class DocumentGenerator
    {

        private void FindAndReplace(Word.Application app, Word.Document doc, string findText, string replaceWithText)
        {
            Find findObject = app.Selection.Find;
            findObject.ClearFormatting();
            findObject.Text = findText;
            findObject.Replacement.ClearFormatting();
            findObject.Replacement.Text = replaceWithText;
            object missing = System.Reflection.Missing.Value;
            object replaceAll = WdReplace.wdReplaceAll;
            findObject.Execute(ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref replaceAll, ref missing, ref missing, ref missing, ref missing);
            var shapes = doc.Shapes;
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

        //Create the Doc Method
        public void CreateAcceptanceOfficeDocument(object SaveAs, AcceptanceOfficeTemplate acceptanceOfficeTemplate)
        {            
            object filename = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Views\\AcceptanceOfficeTemplate.docx"));
            Word.Application wordApp = new Word.Application();
            object missing = Missing.Value;
            Word.Document myWordDoc = null;

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

                //find and replace
                //this.FindAndReplace(wordApp, myWordDoc, "<officeNumber>", "09/04");
                this.FindAndReplace(wordApp, myWordDoc, "<day>", acceptanceOfficeTemplate.Day.ToString());
                this.FindAndReplace(wordApp, myWordDoc, "<month>", acceptanceOfficeTemplate.Month);
                this.FindAndReplace(wordApp, myWordDoc, "<year>", acceptanceOfficeTemplate.Year.ToString());
                this.FindAndReplace(wordApp, myWordDoc, "<coordinatorNameUpperCase>", acceptanceOfficeTemplate.CoordinatorName.ToUpper());
                this.FindAndReplace(wordApp, myWordDoc, "<coordinatorName>", acceptanceOfficeTemplate.CoordinatorName);
                this.FindAndReplace(wordApp, myWordDoc, "<practicionerName>", acceptanceOfficeTemplate.PracticionerName);
                this.FindAndReplace(wordApp, myWordDoc, "<practicionerEnrollment>", acceptanceOfficeTemplate.PracticionerEnrollment);
                this.FindAndReplace(wordApp, myWordDoc, "<linkedOrganizationName>", acceptanceOfficeTemplate.LinkedOrganizationName);
                this.FindAndReplace(wordApp, myWordDoc, "<startingDate>", acceptanceOfficeTemplate.StartingDate.ToString());
                this.FindAndReplace(wordApp, myWordDoc, "<projectDuration>", acceptanceOfficeTemplate.ProjectDuration.ToString());
                this.FindAndReplace(wordApp, myWordDoc, "<mondaySchedule>", acceptanceOfficeTemplate.MondaySchedule);
                this.FindAndReplace(wordApp, myWordDoc, "<tuesdaySchedule>", acceptanceOfficeTemplate.TuesdaySchedule);
                this.FindAndReplace(wordApp, myWordDoc, "<wednesdaySchedule>", acceptanceOfficeTemplate.WednesdaySchedule);
                this.FindAndReplace(wordApp, myWordDoc, "<thursdaySchedule>", acceptanceOfficeTemplate.ThursdaySchedule);
                this.FindAndReplace(wordApp, myWordDoc, "<fridaySchedule>", acceptanceOfficeTemplate.FridaySchedule);
                this.FindAndReplace(wordApp, myWordDoc, "<coordinatorEmail>", acceptanceOfficeTemplate.CoordinatorEmail);
                this.FindAndReplace(wordApp, myWordDoc, "<coordinatorPhoneNumber>", acceptanceOfficeTemplate.CoordinatorPhoneNumber);
                this.FindAndReplace(wordApp, myWordDoc, "<coordinatorCharge>", acceptanceOfficeTemplate.CoordinatorCharge);

            }

            //Save as
            myWordDoc.SaveAs2(ref SaveAs, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing);

            myWordDoc.Close();
            wordApp.Quit();
        }

        public void CreateAssignmentOfficeTemplate(object saveAs, AssignmentOfficeTemplate assignmentOfficeTemplate)
        {
            object filename = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Views\\AssignmentOfficeTemplate.docx"));
            Word.Application wordApp = new Word.Application();
            object missing = Missing.Value;
            Word.Document myWordDoc = null;

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

                //find and replace
                this.FindAndReplace(wordApp, myWordDoc, "<officeNumber>", assignmentOfficeTemplate.OfficeNumber);
                this.FindAndReplace(wordApp, myWordDoc, "<day>", assignmentOfficeTemplate.DateOfGeneration.Day.ToString());
                this.FindAndReplace(wordApp, myWordDoc, "<month>", assignmentOfficeTemplate.DateOfGeneration.Month.ToString());
                this.FindAndReplace(wordApp, myWordDoc, "<year>", assignmentOfficeTemplate.DateOfGeneration.Year.ToString());
                this.FindAndReplace(wordApp, myWordDoc, "<responsibleProjectName>", assignmentOfficeTemplate.ResponsibleProjectName);
                this.FindAndReplace(wordApp, myWordDoc, "<responsibleProjectCharge>", assignmentOfficeTemplate.ResponsibleProjectCharge);
                this.FindAndReplace(wordApp, myWordDoc, "<linkedOrganizationName>", assignmentOfficeTemplate.LinkedOrganizationName);
                this.FindAndReplace(wordApp, myWordDoc, "<linkedOrganizationAddress>", assignmentOfficeTemplate.LinkedOrganizationAddress);
                this.FindAndReplace(wordApp, myWordDoc, "<city>", assignmentOfficeTemplate.City);
                this.FindAndReplace(wordApp, myWordDoc, "<state>", assignmentOfficeTemplate.State);
                this.FindAndReplace(wordApp, myWordDoc, "<practicionerName>", assignmentOfficeTemplate.PracticionerName);
                this.FindAndReplace(wordApp, myWordDoc, "<practicionerEnrollment>", assignmentOfficeTemplate.PracticionerEnrollment);
                this.FindAndReplace(wordApp, myWordDoc, "<projectName>", assignmentOfficeTemplate.ProjectName);
                this.FindAndReplace(wordApp, myWordDoc, "<projectDuration>", assignmentOfficeTemplate.ProjectDuration.ToString());
                this.FindAndReplace(wordApp, myWordDoc, "<coordinatorName>", assignmentOfficeTemplate.CoordinatorName);

            }

            //Save as
            myWordDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing);

            myWordDoc.Close();
            wordApp.Quit();
        }
    }
}
