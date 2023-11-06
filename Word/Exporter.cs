﻿using Flurl.Http;
using NPOI.POIFS.Crypt;
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;
using System.Collections;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Word.Helper;

namespace Word
{
    public class Exporter
    {
        const string Dot = ".";

        public static XWPFDocument ExportDocxByObject(Stream templateStream, object data, Func<string, string> func = null)
        {
            var doc = new XWPFDocument(templateStream);
            foreach (var para in doc.Paragraphs)
            {
                ReplaceKeyObjetAsync(para, data, func);
            }

            foreach (var table in doc.Tables)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.GetTableCells())
                    {
                        foreach (var para in cell.Paragraphs)
                        {
                            ReplaceKeyObjetAsync(para, data, func, cell);
                        }
                    }
                }
            }

            return doc;
        }


        public static XWPFDocument ExportDocxByObject(string templateFilePath, object data, Func<string, string> func = null)
        {
            using (var fileStream = new FileStream(templateFilePath, FileMode.Open))
            {
                return ExportDocxByObject(fileStream, data, func);
            }
        }


        public static XWPFDocument ExportDocxByDictionary(Stream templateStream, Dictionary<string, object> data, Func<string, string> func = null)
        {
            var doc = new XWPFDocument(templateStream);
            foreach (var para in doc.Paragraphs)
            {
                ReplaceKeyDictionaryAsync(para, data, func);
            }

            foreach (var table in doc.Tables)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.GetTableCells())
                    {
                        foreach (var para in cell.Paragraphs)
                        {
                            ReplaceKeyDictionaryAsync(para, data, func);
                        }
                    }
                }
            }

            return doc;
        }


        public static XWPFDocument ExportDocxByDictionary(string templateFilePath, Dictionary<string, object> data, Func<string, string> func = null)
        {
            using (var fileStream = new FileStream(templateFilePath, FileMode.Open))
            {
                return ExportDocxByDictionary(fileStream, data, func);
            }
        }



        private static async Task ReplaceKeyDictionaryAsync(XWPFParagraph para, Dictionary<string, object> data, Func<string, string> func, XWPFTableCell cell = null)
        {
            string text = "";

            foreach (var run in para.Runs)
            {
                text = run.ToString();
                foreach (var p in data.Keys)
                {
                    //string key = $"${p.Name}$";
                    var textReg = new Regex(@"^\$" + p + @"(\[\w+\])?\$$");
                    var assetReg = new Regex(@"^#" + p + @"(\[\d+,\d+\])?#$");
                    var sizeReg = new Regex(@"\[\d+,\d+\]");

                    if (textReg.IsMatch(text))
                    {
                        try
                        {
                            var value = data[p];
                            if (value is ICollection)
                            {
                                if (cell == null)
                                {
                                    var document = run.Document;
                                    var position = document.GetPosOfParagraph(para);
                                    foreach (var firstXwpfTable in document.Tables)
                                    {
                                        var currentTablePos = document.GetPosOfTable(firstXwpfTable);
                                        if (currentTablePos == position + 1)
                                        {
                                            foreach (var valueItem in value as ICollection)
                                            {
                                                if (valueItem != null)
                                                {

                                                    var currentRow = firstXwpfTable.CreateRow();
                                                    Type tv = valueItem.GetType();
                                                    PropertyInfo[] piv = tv.GetProperties();
                                                    var headers = firstXwpfTable.GetRow(1).GetTableCells();
                                                    int currenthIndex = -1;
                                                    foreach (PropertyInfo pv in piv)
                                                    {
                                                        var headerReg = new Regex(@"^\$" + pv.Name + @"(\[\w+\])?\$$");
                                                        var currenth = headers.FirstOrDefault(c => headerReg.IsMatch(c.GetText().Trim()));
                                                        if (currenth != null)
                                                        {
                                                            currenthIndex = headers.IndexOf(currenth);
                                                            var valuep = pv.GetValue(valueItem, null);

                                                            currentRow.GetCell(currenthIndex).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, GetStringValue(valuep), ParagraphAlignment.CENTER, 22, true));


                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {

                                text = textReg.Replace(text, GetStringValue(value));

                            }
                        }
                        catch (Exception ex)
                        {
                            text = "";
                        }
                    }
                    else if (assetReg.IsMatch(text))
                    {
                        var width = 5300000;
                        var height = 2500000;
                        var sizeMatch = sizeReg.Match(text);
                        if (sizeMatch.Success)
                        {
                            try
                            {
                                var w = sizeMatch.Value.Split(',')[0].TrimStart('[');
                                var h = sizeMatch.Value.Split(',')[1].TrimEnd(']');
                                width = int.Parse(w) * 9525;
                                height = int.Parse(h) * 9525;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("解析图片尺寸错误：" + ex);
                            }

                        }
                        try
                        {
                            var filePath = data[p] as string;
                            filePath = func?.Invoke(filePath);
                            if (string.IsNullOrEmpty(filePath))
                            {
                                continue;
                            }
                            if (File.Exists(filePath))
                            {
                                using (var fileStream = new FileStream(filePath.ToString(), FileMode.Open, FileAccess.Read))
                                {
                                    text = "";
                                    run.AddPicture(fileStream, (int)GetPictureType(filePath), $@"{p}", width, height);
                                    NPOI.OpenXmlFormats.Dml.WordProcessing.CT_Inline inline = run.GetCTR().GetDrawingList()[0].inline[0];
                                    var id = (uint)para.Runs.IndexOf(run);
                                    inline.docPr.id = id;
                                }
                            }
                            else
                            {

                                if (CommonHelper.IsBase64(filePath.ToString()))
                                {
                                    var fileContent = Convert.FromBase64String(filePath.ToString());
                                    using (var fileStream = new MemoryStream(fileContent))
                                    {
                                        text = "";
                                        run.AddPicture(fileStream, (int)GetPictureType(filePath), $@"{p}", width, height);
                                        NPOI.OpenXmlFormats.Dml.WordProcessing.CT_Inline inline = run.GetCTR().GetDrawingList()[0].inline[0];
                                        var id = (uint)para.Runs.IndexOf(run);
                                        inline.docPr.id = id;
                                    }
                                }
                                else
                                {
                                    using (var fileStream = await filePath.ToString().GetStreamAsync())
                                    {
                                        text = "";
                                        run.AddPicture(fileStream, (int)GetPictureType(filePath), $@"{p}", width, height);
                                        NPOI.OpenXmlFormats.Dml.WordProcessing.CT_Inline inline = run.GetCTR().GetDrawingList()[0].inline[0];
                                        var id = (uint)para.Runs.IndexOf(run);
                                        inline.docPr.id = id;
                                    }
                                }


                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    }
                }
                if (text.Contains('\n'))
                {
                    run.SetText(string.Empty, 0);

                    var p = text.Split('\n');
                    foreach (var item in p)
                    {
                        run.AppendText(item);
                        run.AddCarriageReturn();
                    }
                }
                else
                {
                    run.SetText(text, 0);

                }
            }
        }


        private static async Task ReplaceKeyObjetAsync(XWPFParagraph para, object model, Func<string, string> func, XWPFTableCell cell = null)
        {
            string text = "";
            Type t = model.GetType();
            PropertyInfo[] pi = t.GetProperties();

            foreach (var run in para.Runs)
            {
                text = run.ToString();
                foreach (PropertyInfo p in pi)
                {
                    //string key = $"${p.Name}$";
                    var textReg = new Regex(@"^\$" + p.Name + @"(\[\w+\])?\$$");
                    var assetReg = new Regex(@"^#" + p.Name + @"(\[\d+,\d+\])?#$");
                    var sizeReg = new Regex(@"\[\d+,\d+\]");

                    if (textReg.IsMatch(text))
                    {
                        try
                        {
                            var value = p.GetValue(model, null);
                            if (value is ICollection)
                            {
                                if (cell == null)
                                {
                                    var document = run.Document;
                                    var position = document.GetPosOfParagraph(para);
                                    foreach (var firstXwpfTable in document.Tables)
                                    {
                                        var currentTablePos = document.GetPosOfTable(firstXwpfTable);
                                        if (currentTablePos == position + 1)
                                        {
                                            foreach (var valueItem in value as ICollection)
                                            {
                                                if (valueItem != null)
                                                {

                                                    var currentRow = firstXwpfTable.CreateRow();
                                                    Type tv = valueItem.GetType();
                                                    PropertyInfo[] piv = tv.GetProperties();
                                                    var headers = firstXwpfTable.GetRow(1).GetTableCells();
                                                    int currenthIndex = -1;
                                                    foreach (PropertyInfo pv in piv)
                                                    {
                                                        var headerReg = new Regex(@"^\$" + pv.Name + @"(\[\w+\])?\$$");
                                                        var currenth = headers.FirstOrDefault(c => headerReg.IsMatch(c.GetText().Trim()));
                                                        if (currenth != null)
                                                        {
                                                            currenthIndex = headers.IndexOf(currenth);
                                                            var valuep = pv.GetValue(valueItem, null);

                                                            currentRow.GetCell(currenthIndex).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, GetStringValue(valuep), ParagraphAlignment.CENTER, 22, true));


                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                text = textReg.Replace(text, GetStringValue(value));

                            }
                        }
                        catch (Exception ex)
                        {
                            text = "";
                        }
                    }
                    else if (assetReg.IsMatch(text))
                    {
                        var width = 5300000;
                        var height = 2500000;
                        var sizeMatch = sizeReg.Match(text);
                        if (sizeMatch.Success)
                        {
                            try
                            {
                                var w = sizeMatch.Value.Split(',')[0].TrimStart('[');
                                var h = sizeMatch.Value.Split(',')[1].TrimEnd(']');
                                width = int.Parse(w) * 9525;
                                height = int.Parse(h) * 9525;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("解析图片尺寸错误：" + ex);
                            }

                        }
                        try
                        {
                            var filePath = p.GetValue(model, null) as string;
                            filePath = func?.Invoke(filePath);
                            if (string.IsNullOrEmpty(filePath))
                            {
                                continue;
                            }
                            if (File.Exists(filePath))
                            {
                                using (var fileStream = new FileStream(filePath.ToString(), FileMode.Open, FileAccess.Read))
                                {
                                    text = "";
                                    run.AddPicture(fileStream, (int)GetPictureType(filePath), $@"{p.Name}", width, height);
                                    NPOI.OpenXmlFormats.Dml.WordProcessing.CT_Inline inline = run.GetCTR().GetDrawingList()[0].inline[0];
                                    var id = (uint)para.Runs.IndexOf(run);
                                    inline.docPr.id = id;
                                }
                            }
                            else
                            {

                                if (CommonHelper.IsBase64(filePath.ToString()))
                                {
                                    var fileContent = Convert.FromBase64String(filePath.ToString());
                                    using (var fileStream = new MemoryStream(fileContent))
                                    {
                                        text = "";
                                        run.AddPicture(fileStream, (int)GetPictureType(filePath), $@"{p}", width, height);
                                        NPOI.OpenXmlFormats.Dml.WordProcessing.CT_Inline inline = run.GetCTR().GetDrawingList()[0].inline[0];
                                        var id = (uint)para.Runs.IndexOf(run);
                                        inline.docPr.id = id;
                                    }
                                }
                                else
                                {
                                    using (var fileStream = await filePath.ToString().GetStreamAsync())
                                    {
                                        text = "";
                                        run.AddPicture(fileStream, (int)GetPictureType(filePath), $@"{p.Name}", width, height);
                                        NPOI.OpenXmlFormats.Dml.WordProcessing.CT_Inline inline = run.GetCTR().GetDrawingList()[0].inline[0];
                                        var id = (uint)para.Runs.IndexOf(run);
                                        inline.docPr.id = id;
                                    }
                                }


                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    }
                }
                if (text.Contains('\n'))
                {
                    run.SetText(string.Empty, 0);

                    var p = text.Split('\n');
                    foreach (var item in p)
                    {
                        run.AppendText(item);
                        run.AddCarriageReturn();
                    }
                }
                else
                {
                    run.SetText(text, 0);

                }
            }
        }


        private static PictureType GetPictureType(string str)
        {
            object? result;
            if (!str.StartsWith(Dot))
            {
                var index = str.LastIndexOf(Dot);
                if (index != -1 && str.Length > index + 1)
                {
                    str = str.Substring(index + 1);
                }


            }
            PictureType.TryParse(typeof(PictureType), str.ToUpper(), out result);
            if (result != null)
            {
                return (PictureType)result;
            }
            else
            {
                switch (str.ToUpper())
                {

                    case "JPG":
                        result = PictureType.JPEG;
                        break;

                    default:
                        result = null;
                        break;
                }
                return (PictureType)result;
            }
        }

        private static string? GetStringValue(object value)
        {
            if (value is DateTime)
            {
                return ((DateTime)value).ToString("yyyy-MM-dd hh:mm");

            }
            else if (value is double)
            {
                return ((double)value).ToString("0.00");
            }
            else if (value is float)
            {
                return ((float)value).ToString("0.00");
            }

            return value.ToString();

        }

    }
}