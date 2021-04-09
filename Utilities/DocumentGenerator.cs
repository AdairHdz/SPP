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
        //private void FindAndReplace(Microsoft.Office.Interop.Word.Application doc, object findText, object replaceWithText)
        //{
        //    //options
        //    object matchCase = false;
        //    object matchWholeWord = true;
        //    object matchWildCards = false;
        //    object matchSoundsLike = false;
        //    object matchAllWordForms = false;
        //    object forward = true;
        //    object format = false;
        //    object matchKashida = false;
        //    object matchDiacritics = false;
        //    object matchAlefHamza = false;
        //    object matchControl = false;
        //    object read_only = false;
        //    object visible = true;
        //    object replace = 2;
        //    object wrap = 1;
        //    //execute find and replace
        //    doc.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
        //        ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace,
        //        ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
        //}

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

        //Creeate the Doc Method
        public void CreateWordDocument(object filename, object SaveAs)
        {
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
                this.FindAndReplace(wordApp, myWordDoc, "<officeNumber>", "09/04");
                this.FindAndReplace(wordApp, myWordDoc, "<day>", "9");
                this.FindAndReplace(wordApp, myWordDoc, "<month>", "abril");
                this.FindAndReplace(wordApp, myWordDoc, "<year>", "2021");
                this.FindAndReplace(wordApp, myWordDoc, "<responsibleProjectName>", "Eduardo Aldair Hernández Solís");
                this.FindAndReplace(wordApp, myWordDoc, "<responsibleProjectCharge>", "Técnico Ejecutivo");
                this.FindAndReplace(wordApp, myWordDoc, "<linkedOrganizationName>", "Oracle, Inc.");
                this.FindAndReplace(wordApp, myWordDoc, "<linkedOrganizationAddress>", "Obrero Campesino S/N");
                this.FindAndReplace(wordApp, myWordDoc, "<city>", "Xalapa");
                this.FindAndReplace(wordApp, myWordDoc, "<state>", "Veracruz");
                this.FindAndReplace(wordApp, myWordDoc, "<practicionerName>", "Adair Benjamín Hernández Ortiz");
                this.FindAndReplace(wordApp, myWordDoc, "<practicionerEnrollment>", "S18012122");
                this.FindAndReplace(wordApp, myWordDoc, "<projectName>", "Desarrollo de sistema web");
                this.FindAndReplace(wordApp, myWordDoc, "<startingDate>", "9 de abril");
                this.FindAndReplace(wordApp, myWordDoc, "<projectDuration>", "480");
                this.FindAndReplace(wordApp, myWordDoc, "<coordinatorName>", "Irving de Jesús Lozada Rodriguez");
                this.FindAndReplace(wordApp, myWordDoc, "<mondaySchedule>", "De 7AM a 2PM");
                this.FindAndReplace(wordApp, myWordDoc, "<tuesdaySchedule>", "De 7AM a 12PM");
                this.FindAndReplace(wordApp, myWordDoc, "<coordinatorEmail>", "coordinator@gmail.com");
                this.FindAndReplace(wordApp, myWordDoc, "<coordinatorPhoneNumber>", "2287009865");
                this.FindAndReplace(wordApp, myWordDoc, "<coordinatorCharge>", "Ser bien perrón");
                this.FindAndReplace(wordApp, myWordDoc, "<endingDate>", "12 de julio");

            }
            else
            {
                Console.WriteLine("File not Found!");
            }

            //Save as
            myWordDoc.SaveAs2(ref SaveAs, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing);

            myWordDoc.Close();
            wordApp.Quit();
            Console.WriteLine("File Created!");
        }
    }
}
