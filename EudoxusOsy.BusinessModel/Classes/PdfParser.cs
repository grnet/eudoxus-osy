
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EudoxusOsy.BusinessModel
{
    public static class Parser
    {
        public static string ExtractNotesFromPdf(string path)
        {
            const string SearchString = "ΠΑΡΑΤΗΡΗΣΗ: ";

            using (PdfReader reader = new PdfReader(path))
            {
                StringBuilder text = new StringBuilder();

                text.Append(PdfTextExtractor.GetTextFromPage(reader, 1));
                var pdfText = text.ToString();
                var myIndex = -1;
                var notes = string.Empty;

                myIndex = pdfText.IndexOf(SearchString);
                if (myIndex > -1)
                {
                    var shortPdfText = pdfText.Substring(myIndex);
                    using (var myreader = new StringReader(shortPdfText))
                    {
                        notes = myreader.ReadLine().Substring(12);
                        return notes;
                    }
                }
                return null;
            }
        }

        public static void ExtractNotesFromDirectory(string path)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                var existingFiles = new FileRepository(uow).GetMatchingFilesByFilename("catalog_");
                //var existingFilesDictionary = existingFiles.Select(x => new KeyValuePair<int, string>(x.ID, x.FileName)); //Or PathName, depends on the actual name on disk
                foreach (var file in existingFiles)
                {
                    var filePath = System.IO.Path.Combine(new string[] { path, file.FileName });
                    if (System.IO.File.Exists(filePath))
                    {
                        var notes = ExtractNotesFromPdf(System.IO.Path.Combine(new string[] { path, file.FileName }));
                        file.PdfNotes = notes;
                    }
                }
                uow.Commit();
            }
        }

        public static void TestExtractNotesFromDirectory(string path)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                //var existingFiles = new FileRepository(uow).GetMatchingFilesByFilename(".pdf");
                List<string> notesList = new List<string>();
                var files = Directory.GetFiles(path);

                foreach (var file in files)
                {
                    var filePath = System.IO.Path.Combine(new string[] { path, file });
                    var notes = ExtractNotesFromPdf(filePath);
                    //file.PdfNotes = notes;
                    if (!string.IsNullOrEmpty(notes))
                        notesList.Add(notes);
                }
                //uow.Commit();
            }
        }
    }
}
