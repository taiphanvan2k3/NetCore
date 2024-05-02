using System.Text.RegularExpressions;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace StringExample
{
    public class DocxExample
    {
        private static readonly Dictionary<string, string> _replacePatterns = new()
        {
            { "StudentName", "Phan Văn Tài" },
            { "StudentClass", "21TCLC_DT3" }
            // Thêm các cặp key-value khác nếu cần thiết
        };

        public static void Example1()
        {
            // Create a new document
            DocX document = DocX.Create("SampleDocument.docx");

            // Add a title to the document
            document.InsertParagraph("Sample Document Title").FontSize(18).Bold().Alignment = Alignment.center;

            // Add a paragraph with some text
            document.InsertParagraph("This is a sample paragraph.").FontSize(12).Alignment = Alignment.left;

            // Add a table with some data
            Table table = document.AddTable(3, 2);
            table.Design = TableDesign.ColorfulList;
            table.Alignment = Alignment.center;
            table.AutoFit = AutoFit.Contents;

            // Add headers to the table
            table.Rows[0].Cells[0].Paragraphs[0].Append("Name").Bold();
            table.Rows[0].Cells[1].Paragraphs[0].Append("Age").Bold();

            // Add data to the table
            table.Rows[1].Cells[0].Paragraphs[0].Append("John Doe");
            table.Rows[1].Cells[1].Paragraphs[0].Append("30");
            table.Rows[2].Cells[0].Paragraphs[0].Append("Jane Smith");
            table.Rows[2].Cells[1].Paragraphs[0].Append("25");

            document.InsertTable(table);

            // Save the document
            document.Save();
        }

        public static void Example2()
        {
            // Load the document
            DocX document = DocX.Load("SampleDocument.docx");

            if (document.FindUniqueByPattern(@"«[\w \=]{4,}»", RegexOptions.IgnoreCase).Count > 0)
            {
                var replaceTextOptions = new FunctionReplaceTextOptions()
                {
                    FindPattern = @"«(.*?)»",
                    RegexMatchHandler = ReplaceFunc,
                    RegExOptions = RegexOptions.IgnoreCase,
                    NewFormatting = new Formatting()
                    {
                        Bold = false,
                        FontColor = System.Drawing.Color.Black,
                        Italic = true,
                        Size = 12,
                        FontFamily = new Font("Arial"),
                    }
                };

                document.ReplaceText(replaceTextOptions);
                document.SaveAs("Output.docx");
            }
            document.Save();
        }

        private static string ReplaceFunc(string findStr)
        {
            // Kiểm tra xem findStr có trong dictionary thay thế không
            if (_replacePatterns.TryGetValue(findStr, out string value))
            {
                return value;
            }
            return findStr;
        }
    }
}