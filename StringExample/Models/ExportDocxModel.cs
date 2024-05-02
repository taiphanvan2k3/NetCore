using System.Text.RegularExpressions;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace StringExample.Models
{
    enum BoldIndexes
    {
        FACULTY = 3,
        SCHOOL_YEAR = 7
    }

    public class ExportDocxModel
    {
        private readonly string _templateFile = "KPI_Template.docx";
        private readonly string _exportFile = "KPI_Export.docx";

        private readonly Dictionary<string, string> _replacePatterns;

        public ExportDocxModel()
        {
            _replacePatterns = new Dictionary<string, string>
            {
                { "CadreName", "Nguyễn Văn A" },
                { "SchoolYear", "2023-2024" },
                { "Faculty", "Công nghệ thông tin" },
                { "Position", "Giảng viên" },
                { "CadreFaculty", "Công nghệ thông tin" },
                { "Evaluation_I1", "Hoàn thành" },
                { "Evaluation_I2", "Hoàn thành" },
                { "Evaluation_I3", "Hoàn thành" },
                { "Evaluation_I4", "Tương đối hoàn thành" },
                { "Evaluation_I5", "Hoàn thành" },
                { "Evaluation_I6", "Hoàn thành" },
                { "Evaluation_I7", "Hoàn thành" },
                { "Evaluation_I8", "Chưa hoàn thành" },
                { "Evaluation_I9", "Hoàn thành" },
                { "Evaluation_II1", "Hoàn thành nhiệm vụ xuất sắc"},
                { "Evaluation_II2", "Tốt" },
                { "Evaluation_III", "Ý kiến gì đây, tôi chẳng biết ý kiến gì cả"},
                { "Evaluation_IV1", "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Animi suscipit autem quas, assumenda rerum obcaecati. Deleniti porro unde corrupti voluptas asperiores, consequatur quidem veritatis ut aspernatur labore, reprehenderit, autem nostrum? Beatae recusandae iste provident molestias ea totam at facilis cum ad repudiandae? Praesentium, consequuntur quis quisquam quae excepturi quod tempora consectetur nobis sequi. Sint vel tempora minus expedita nostrum deleniti." },
                { "Evaluation_IV2", "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Animi suscipit autem quas, assumenda rerum obcaecati. Deleniti porro unde corrupti voluptas asperiores, consequatur quidem veritatis ut aspernatur labore, reprehenderit, autem nostrum? Beatae recusandae iste provident molestias ea totam at facilis cum ad repudiandae? Praesentium, consequuntur quis quisquam quae excepturi quod tempora consectetur nobis sequi. Sint vel tempora minus expedita nostrum deleniti." },
            };
        }

        private void ConvertClassToDictionary(Data data)
        {
            _replacePatterns.Add("Faculty", data.Faculty);
            _replacePatterns.Add("CadreName", data.CadreName);
            _replacePatterns.Add("Position", data.Position);
            _replacePatterns.Add("CadreFaculty", data.CadreFaculty);

            foreach (var item in data.SelfEvaluation)
            {
                _replacePatterns.Add(item.Name, item.Value);
            }

            foreach (var item in data.LeaderEvaluation)
            {
                _replacePatterns.Add(item.Name, item.Value);
            }
        }

        public void ExportToDocx()
        {
            // Load the document
            DocX document = DocX.Load(_templateFile);

            var replaceTextOptions = new FunctionReplaceTextOptions()
            {
                FindPattern = @"{(.*?)}",
                RegexMatchHandler = ReplaceFunc,
                RegExOptions = RegexOptions.IgnoreCase,
                NewFormatting = new Formatting()
                {
                    Bold = false,
                    FontColor = System.Drawing.Color.Black,
                    Size = 13,
                    FontFamily = new Font("Times New Roman"),
                }
            };

            document.ReplaceText(replaceTextOptions);
            document.Paragraphs[(int)BoldIndexes.FACULTY].Bold();
            document.Paragraphs[(int)BoldIndexes.SCHOOL_YEAR].Bold();

            document.SaveAs(_exportFile);
        }

        private string ReplaceFunc(string findStr)
        {
            // Kiểm tra xem findStr có trong dictionary thay thế không
            if (_replacePatterns.TryGetValue(findStr, out string value))
            {
                return value;
            }
            return "";
        }
    }
}