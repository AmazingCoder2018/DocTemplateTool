using System.Reflection;
using System.Text;
using DocTemplateTool.Pdf.Test.Model;
using DocTemplateTool.Pdf;
using static System.Net.Mime.MediaTypeNames;

namespace DocTemplateTool.Pdf.Test
{
    [TestClass]
    public class UnitTest1
    {


        private static readonly string templatePath_Doc = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets");

        private static readonly string outputPath_Doc = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

        //�����Զ���Ŀ���ļ���·��
        //private static readonly string outputPath_Doc = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);



    
        /// <summary>
        /// ���ԣ�������
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Generator begin");
            var docinfo = GetReportDocInfo();

            var result = Exporter.ExportDocxByObject(Path.Combine(templatePath_Doc, $"RecipeTemplate.pdf"), docinfo, (s) => s);
            result.Save(Path.Combine(outputPath_Doc, $"Recipe.pdf"));

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



    }
}