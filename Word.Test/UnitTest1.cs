using System.Reflection;
using System.Text;
using GDMK.CAH.Client.Report.Model;
using static System.Net.Mime.MediaTypeNames;

namespace Word.Test
{
    [TestClass]
    public class UnitTest1
    {


        private static readonly string templatePath_Doc = System.IO.Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets", $"RecipeTemplate.docx");

        private static readonly string outputPath_Doc = System.IO.Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, $"Recipe.docx");

        [TestMethod]
        public void TestMethod1()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Generator begin");
            byte[] docFileContent;

            var docinfo = GetReportDocInfo();
            var ext = ".docx";
            var result = Word.Exporter.ExportDocxByObject(templatePath_Doc, docinfo, (s) => s);

            using (var memoryStream = new MemoryStream())
            {
                result.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                docFileContent = memoryStream.ToArray();
            }

            File.WriteAllBytes(outputPath_Doc, docFileContent);


        }



        protected virtual ReportDocInfo GetReportDocInfo()
        {
            var docinfo = new ReportDocInfo();

            docinfo.Id = 202247589;
            docinfo.HospitalName = "�㶫ʡĳ������ҽԺ";
            docinfo.ReportTime = new DateTime(2023, 11, 1);
            docinfo.DepartmentName = "��Ѫ���ڿ�";
            docinfo.AuditEmployeeName = "����";
            docinfo.AuditEmployeeSignature = System.IO.Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets", $"TestPic.jpg");
            docinfo.DraftEmployeeName = "����";
            docinfo.DraftEmployeeSignature = System.IO.Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets", $"TestPic.jpg");
            docinfo.ClientName = "����";
            docinfo.ClientAge = "35";
            docinfo.Name = "���Ը�Ѫѹ�Ĵ���";
            docinfo.Diagnostic = "���Ը�Ѫѹ";
            docinfo.ClientSex = "��";
            docinfo.Price = 12.0M;
            docinfo.Graphic = System.IO.Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets", $"ECGGraphic.png");
            docinfo.RpList = new List<string>()
            {
                "1.�����ᰱ�ȵ�ƽƬ(����)(ʡ��)\n ���2mg*28Ƭ\t��10Ƭ\n�÷�������2Ƭ/�Σ�ÿ�����Σ��ڷ���",
                "2.�����ᰱ�ȵ�ƽƬ(���ϲ)(��ʡ��)\n ���5mg*28Ƭ\t��20Ƭ\n�÷�������1Ƭ/�Σ�ÿ�����Σ��ڷ���",
            };
            docinfo.Rps = string.Join('\n', docinfo.RpList);
            return docinfo;
        }

    }
}