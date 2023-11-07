using System.Reflection;
using System.Text;
using DocTemplateTool.Word.Test.Model;
using NPOI.SS.Formula.Functions;
using static System.Net.Mime.MediaTypeNames;

namespace DocTemplateTool.Word.Test
{
    [TestClass]
    public class UnitTest1
    {


        private static readonly string templatePath_Doc = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets");

        private static readonly string outputPath_Doc = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

        //�����Զ���Ŀ���ļ���·��
        //private static readonly string outputPath_Doc = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);



        /// <summary>
        /// ���ԣ��ĵ籨��
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Generator begin");
            byte[] docFileContent;

            var docinfo = GetReportDocInfo();

            var result = Word.Exporter.ExportDocxByObject(Path.Combine(templatePath_Doc, $"ReportTemplate.docx"), docinfo, (s) => s);

            using (var memoryStream = new MemoryStream())
            {
                result.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                docFileContent = memoryStream.ToArray();
            }

            File.WriteAllBytes(Path.Combine(outputPath_Doc, $"Report.docx"), docFileContent);


        }

        /// <summary>
        /// ���ԣ�������
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Generator begin");
            byte[] docFileContent;

            var docinfo = GetReportDocInfo();

            var result = Word.Exporter.ExportDocxByObject(Path.Combine(templatePath_Doc, $"RecipeTemplate.docx"), docinfo, (s) => s);

            using (var memoryStream = new MemoryStream())
            {
                result.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                docFileContent = memoryStream.ToArray();
            }

            File.WriteAllBytes(Path.Combine(outputPath_Doc, $"Recipe.docx"), docFileContent);


        }

        /// <summary>
        /// ���ԣ�Ա��������Ϣ����
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Generator begin");
            byte[] docFileContent;

            var docinfo = GetHealthReportDocInfo();

            var result = Word.Exporter.ExportDocxByObject(Path.Combine(templatePath_Doc, $"ReportTemplate2.docx"), docinfo, (s) => s);

            using (var memoryStream = new MemoryStream())
            {
                result.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                docFileContent = memoryStream.ToArray();
            }


            File.WriteAllBytes(Path.Combine(outputPath_Doc, $"Report2.docx"), docFileContent);


        }


        /// <summary>
        /// ���ԣ����ʵǼǱ�
        /// </summary>

        [TestMethod]
        public void TestMethod4()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Generator begin");
            byte[] docFileContent;

            var docinfo = new
            {

                Dept = "�з���",
                Date = DateTime.Now,
                Details = new List<dynamic>() {

                  new
                  {
                      Number = "mk201406gz324",
                    Name = "����",
                    BaseAccount = 2000,
                    TechnicalAllowance = 1500,
                    SeniorityAllowance = 700,
                    DutyAllowance = 300,
                    JobAllowance = 200,
                    ItemSum = 4700
                }
,
new
{
                Number = "mk201406gz325",
                      Name = "����",
                      BaseAccount = 2500,
                      TechnicalAllowance = 2000,
                      SeniorityAllowance = 800,
                      DutyAllowance = 350,
                      JobAllowance = 250,
                      ItemSum = 5800
                  }
            ,
            new
{
                Number = "mk201406gz326",
                      Name = "����",
                      BaseAccount = 1800,
                      TechnicalAllowance = 1200,
                      SeniorityAllowance = 600,
                      DutyAllowance = 300,
                      JobAllowance = 200,
                      ItemSum = 4900
                  }
            ,
            new
{
                Number = "mk201406gz327",
                      Name = "����",
                      BaseAccount = 2200,
                      TechnicalAllowance = 1600,
                      SeniorityAllowance = 750,
                      DutyAllowance = 350,
                      JobAllowance = 250,
                      ItemSum = 5650
                  }
            ,
            new
{
                Number = "mk201406gz328",
                      Name = "���",
                      BaseAccount = 1950,
                      TechnicalAllowance = 1350,
                      SeniorityAllowance = 650,
                      DutyAllowance = 325,
                      JobAllowance = 225,
                      ItemSum = 5550
                  }
            ,
            new
{
                Number = "mk201406gz329",
                      Name = "���",
                      BaseAccount = 2350,
                      TechnicalAllowance = 1750,
                      SeniorityAllowance = 850,
                      DutyAllowance = 375,
                      JobAllowance = 275,
                      ItemSum = 6450
                  }

        },
                DeptorSum = 50000,
                LenderSum = 50000,
                //12800   9400    4350    2000    1400    33050

                Sum1 = 12800,
                Sum2 = 9400,
                Sum3 = 4350,
                Sum4 = 2000,
                Sum5 = 1400,
                Sum = 33050,
                Auditor = "����",
                Register = "����",

            };

            var result = Word.Exporter.ExportDocxByObject(Path.Combine(templatePath_Doc, $"SalaryTemplate.docx"), docinfo, (s) => s);

            using (var memoryStream = new MemoryStream())
            {
                result.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                docFileContent = memoryStream.ToArray();
            }


            File.WriteAllBytes(Path.Combine(outputPath_Doc, $"Salary.docx"), docFileContent);


        }




        protected virtual ReportDocInfo GetReportDocInfo()
        {
            var docinfo = new ReportDocInfo
            {
                Id = 202247589,
                HospitalName = "�㶫ʡĳ������ҽԺ",
                ReportTime = new DateTime(2023, 11, 1),
                DepartmentName = "��Ѫ���ڿ�",
                AuditEmployeeName = "����",
                AuditEmployeeSignature = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets", $"TestPic.jpg"),
                DraftEmployeeName = "����",
                DraftEmployeeSignature = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets", $"TestPic.jpg"),
                ClientName = "����",
                ClientAge = "35",
                Name = "���Ը�Ѫѹ�Ĵ���",
                Diagnostic = "���Ը�Ѫѹ",
                ClientSex = "��",
                Price = 12.0M,
                Graphic = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets", $"ECGGraphic.png"),
                RpList = new List<string>()
                {
                    "1.�����ᰱ�ȵ�ƽƬ(����)(ʡ��)\n ���2mg*28Ƭ\t��10Ƭ\n�÷�������2Ƭ/�Σ�ÿ�����Σ��ڷ���",
                    "2.�����ᰱ�ȵ�ƽƬ(���ϲ)(��ʡ��)\n ���5mg*28Ƭ\t��20Ƭ\n�÷�������1Ƭ/�Σ�ÿ�����Σ��ڷ���",
                }
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