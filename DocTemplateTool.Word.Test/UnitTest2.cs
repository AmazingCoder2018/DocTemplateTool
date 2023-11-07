using System.Reflection;
using System.Text;
using DocTemplateTool.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DocTemplateTool.Word.Test
{
    [TestClass]
    public class UnitTest2
    {


        private static readonly string templatePath_Doc = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets");

        private static readonly string outputPath_Doc = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

        /// <summary>
        /// ���ԣ��ӹ�ϣ������
        /// </summary>

        [TestMethod]
        public void TestFromHashTable()
        {
            byte[] docFileContent;

            var docinfo = new Dictionary<string, object>()
            {
                {"Dept", "XX�Ƽ��ɷ����޹�˾" },
                {"Date",  DateTime.Now     },
                {"Number",  "ƾ - 202301111"     },
                {"Details",  new List<Dictionary<string, object>>(){

                    new Dictionary<string, object>(){
                        { "Type","�����տ�"},
                        { "Name","Ӧ�տ�"},
                        { "DeptorAmount",0},
                        { "LenderAmount",50000}
                    },
                     new Dictionary<string, object>(){
                        { "Type","�����տ�"},
                        { "Name","Ԥ�տ�"},
                        { "DeptorAmount",30000},
                        { "LenderAmount",0}
                    },
                    new Dictionary<string, object>(){
                        { "Type","�����տ�"},
                        { "Name","�ֽ�"},
                        { "DeptorAmount",20000},
                        { "LenderAmount",0}
                    },

                }},
                { "DeptorSum",  50000     },
                { "LenderSum",  50000     },
                { "ClientName",  "XX�Ƽ��ɷ����޹�˾"     },
                { "Teller",  "����"     },
                { "Maker",  "����"     },
                { "Auditor",  "����"     },
                { "Register",  "����"     },
            };

            var result = Word.Exporter.ExportDocxByDictionary(Path.Combine(templatePath_Doc, $"AccountingTemplate.docx"), docinfo, (s) => s);

            using (var memoryStream = new MemoryStream())
            {
                result.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                docFileContent = memoryStream.ToArray();
            }
            File.WriteAllBytes(Path.Combine(outputPath_Doc, $"Accounting_FromHashTable.docx"), docFileContent);

        }

        /// <summary>
        /// ���ԣ���Json�ַ�������
        /// </summary>

        [TestMethod]
        public void TestFromJsonString()
        {
            byte[] docFileContent;

            var docinfoJson = "{\"Dept\":\"XX�Ƽ��ɷ����޹�˾\",\"Date\":\"2023-11-07T10:31:04.5331908+08:00\",\"Number\":\"ƾ - 202301111\",\"Details\":[{\"Type\":\"�����տ�\",\"Name\":\"Ӧ�տ�\",\"DeptorAmount\":0,\"LenderAmount\":50000},{\"Type\":\"�����տ�\",\"Name\":\"Ԥ�տ�\",\"DeptorAmount\":30000,\"LenderAmount\":0},{\"Type\":\"�����տ�\",\"Name\":\"�ֽ�\",\"DeptorAmount\":20000,\"LenderAmount\":0}],\"DeptorSum\":50000,\"LenderSum\":50000,\"ClientName\":\"XX�Ƽ��ɷ����޹�˾\",\"Teller\":\"����\",\"Maker\":\"����\",\"Auditor\":\"����\",\"Register\":\"����\"}";
            var docinfo = CommonHelper.ToCollections(JObject.Parse(docinfoJson)) as IDictionary<string, object>;


            var result = Word.Exporter.ExportDocxByDictionary(Path.Combine(templatePath_Doc, $"AccountingTemplate.docx"), docinfo, (s) => s);

            using (var memoryStream = new MemoryStream())
            {
                result.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                docFileContent = memoryStream.ToArray();
            }
            File.WriteAllBytes(Path.Combine(outputPath_Doc, $"Accounting_FromJson.docx"), docFileContent);

        }


        /// <summary>
        /// ���ԣ����������Ͷ�������
        /// </summary>
        [TestMethod]
        public void TestFromDynamicObject()
        {
            byte[] docFileContent;

            var docinfo = new
            {

                Dept = "XX�Ƽ��ɷ����޹�˾",
                Date = DateTime.Now,
                Number = "ƾ - 202301111",
                Details = new List<dynamic>() {

                  new
                  {
                      Type = "�����տ�",
                      Name = "Ӧ�տ�",
                      DeptorAmount = 0,
                      LenderAmount = 50000
                  },
                    new
                    {
                        Type = "�����տ�",
                        Name = "Ԥ�տ�",
                        DeptorAmount = 30000,
                        LenderAmount = 0
                    },
                   new
                   {
                       Type = "�����տ�",
                       Name = "�ֽ�",
                       DeptorAmount = 20000,
                       LenderAmount = 0
                   },
                },
                DeptorSum = 50000,
                LenderSum = 50000,
                ClientName = "XX�Ƽ��ɷ����޹�˾",
                Teller = "����",
                Maker = "����",
                Auditor = "����",
                Register = "����",
            };

            var result = Word.Exporter.ExportDocxByObject(Path.Combine(templatePath_Doc, $"AccountingTemplate.docx"), docinfo, (s) => s);

            using (var memoryStream = new MemoryStream())
            {
                result.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                docFileContent = memoryStream.ToArray();
            }
            File.WriteAllBytes(Path.Combine(outputPath_Doc, $"Accounting_FromDynamicObject.docx"), docFileContent);

        }



        /// <summary>
        /// ���ԣ��Ӿ������Ͷ�������
        /// </summary>

        [TestMethod]
        public void TestFromObject()
        {
            byte[] docFileContent;

            var docinfo = new DeptInfo(
                    "XX�Ƽ��ɷ����޹�˾",
                    DateTime.Now,
                    "ƾ - 202301111",
                    new List<DeptItem>() { new DeptItem(
                                            "�����տ�",
                                            "Ӧ�տ�",
                                            0,
                                            50000
                                                              ),
                                            new DeptItem(
                                            "�����տ�",
                                            "Ԥ�տ�",
                                            30000,
                                            0
                                                                ),
                                            new DeptItem(
                                            "�����տ�",
                                            "�ֽ�",
                                            20000,
                                            0
                                                               ),
                },
                50000,
                50000,
                "XX�Ƽ��ɷ����޹�˾",
                "����",
                "����",
                "����",
                "����"
            );

            var result = Word.Exporter.ExportDocxByObject(Path.Combine(templatePath_Doc, $"AccountingTemplate.docx"), docinfo, (s) => s);

            using (var memoryStream = new MemoryStream())
            {
                result.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                docFileContent = memoryStream.ToArray();
            }
            File.WriteAllBytes(Path.Combine(outputPath_Doc, $"Accounting_FromObject.docx"), docFileContent);

        }

        internal class DeptInfo
        {
            public string Dept { get; }
            public DateTime Date { get; }
            public string Number { get; }
            public List<DeptItem> Details { get; }
            public int DeptorSum { get; }
            public int LenderSum { get; }
            public string ClientName { get; }
            public string Teller { get; }
            public string Maker { get; }
            public string Auditor { get; }
            public string Register { get; }

            public DeptInfo(string dept, DateTime date, string number, List<DeptItem> details, int deptorSum, int lenderSum, string clientName, string teller, string maker, string auditor, string register)
            {
                Dept = dept;
                Date = date;
                Number = number;
                Details = details;
                DeptorSum = deptorSum;
                LenderSum = lenderSum;
                ClientName = clientName;
                Teller = teller;
                Maker = maker;
                Auditor = auditor;
                Register = register;
            }

        }

        internal class DeptItem
        {
            public string Type { get; }
            public string Name { get; }
            public int DeptorAmount { get; }
            public int LenderAmount { get; }

            public DeptItem(string type, string name, int deptorAmount, int lenderAmount)
            {
                Type = type;
                Name = name;
                DeptorAmount = deptorAmount;
                LenderAmount = lenderAmount;
            }
        }


    }



}