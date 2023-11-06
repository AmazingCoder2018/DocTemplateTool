using System.Reflection;
using System.Text;
using NPOI.SS.Formula.Functions;
using Word.Test.Model;
using static System.Net.Mime.MediaTypeNames;

namespace Word.Test
{
    [TestClass]
    public class UnitTest1
    {


        private static readonly string templatePath_Doc = System.IO.Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets");

        private static readonly string outputPath_Doc = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

        [TestMethod]
        public void TestMethod1()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Generator begin");
            byte[] docFileContent;

            var docinfo = GetReportDocInfo();
            var ext = ".docx";
            var result = Word.Exporter.ExportDocxByObject(Path.Combine(templatePath_Doc, $"ReportTemplate.docx"), docinfo, (s) => s);

            using (var memoryStream = new MemoryStream())
            {
                result.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                docFileContent = memoryStream.ToArray();
            }

            File.WriteAllBytes(Path.Combine(outputPath_Doc, $"Report.docx"), docFileContent);


        }


        [TestMethod]
        public void TestMethod2()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Generator begin");
            byte[] docFileContent;

            var docinfo = GetReportDocInfo();
            var ext = ".docx";
            var result = Word.Exporter.ExportDocxByObject(Path.Combine(templatePath_Doc, $"RecipeTemplate.docx"), docinfo, (s) => s);

            using (var memoryStream = new MemoryStream())
            {
                result.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                docFileContent = memoryStream.ToArray();
            }

            File.WriteAllBytes(Path.Combine(outputPath_Doc, $"Recipe.docx"), docFileContent);


        }

        [TestMethod]
        public void TestMethod3()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Generator begin");
            byte[] docFileContent;

            var docinfo = GetHealthReportDocInfo();
            var ext = ".docx";
            var result = Word.Exporter.ExportDocxByObject(Path.Combine(templatePath_Doc, $"ReportTemplate2.docx"), docinfo, (s) => s);

            using (var memoryStream = new MemoryStream())
            {
                result.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                docFileContent = memoryStream.ToArray();
            }


            File.WriteAllBytes(Path.Combine(outputPath_Doc, $"Report2.docx"), docFileContent);


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


        protected virtual HealthReportDocInfo GetHealthReportDocInfo()
        {
            var docinfo = new HealthReportDocInfo();

            docinfo.ClientName = "XX�Ƽ��ɷ����޹�˾";
            docinfo.Title = "������ҵԱ�����������ܱ�";
            docinfo.TimeSpan = "2023��10��19��-10��29��";
            docinfo.Count1 = 2; docinfo.Count2 = 3;
            docinfo.TotalMemberCount = 60;
            docinfo.BloodPressureTestMemberCount = 42;
            docinfo.BloodGlucoseTestMemberCount = 43;
            docinfo.ECGTestMemberCount = 30;
            docinfo.BloodPressureAnalysis = new List<AnalysisList>()
            {
                 new AnalysisList()
                 {

                      Dept="������", Count=8
                 },
                  new AnalysisList()
                 {

                      Dept="�ܾ���", Count=1
                 },
                   new AnalysisList()
                 {

                      Dept="�ͻ���", Count=2
                 }


            };

            docinfo.BloodGlucoseAnalysis = new List<AnalysisList>()
            {
                 new AnalysisList()
                 {

                      Dept="������", Count=4
                 },
                  new AnalysisList()
                 {

                      Dept="�ܾ���", Count=1
                 },
                   new AnalysisList()
                 {

                      Dept="�ͻ���", Count=1
                 }


            };

            docinfo.ECGAnalysis = new List<AnalysisList>()
            {
                 new AnalysisList()
                 {

                      Dept="������", Count=4
                 },
                  new AnalysisList()
                 {

                      Dept="�ܾ���", Count=1
                 },
                   new AnalysisList()
                 {

                      Dept="�ͻ���", Count=1
                 }


            };

            docinfo.BloodPressureList = new List<DetailList> { new DetailList() {
            Dept="������",
             Name="����",
             Value="144/66",
             Result="��"
            },
            new DetailList() {
            Dept="������",
             Name="����",
             Value="150/86",
             Result="��"
            },
             new DetailList() {
            Dept="������",
             Name="��ΰ",
             Value="149/86",
             Result="��"
            },
            new DetailList() {
            Dept="������",
             Name="����",
             Value="128/92",
             Result="��"
            }
            };
            docinfo.BloodGlucoseList = new List<DetailList> { new DetailList() {
            Dept="������",
             Name="��ΰ",
             Value="6.3",
             Result="��"
            },
            new DetailList() {
            Dept="������",
             Name="����",
             Value="6.5",
             Result="��"
            }
            };
            docinfo.ECGList = new List<DetailList> { new DetailList() {
            Dept="������",
             Name="����",
             Value="122",
             Result="��"
            },
            new DetailList() {
            Dept="������",
             Name="����",
             Value="55",
             Result="��"
            }
            };
            return docinfo;
        }

    }
}