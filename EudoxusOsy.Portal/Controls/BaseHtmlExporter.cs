using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using EudoxusOsy.BusinessModel;


namespace EudoxusOsy.Portal.Controls
{
    public enum ExcelFormat
    {
        Number,
        Text,
        Datetime,
        Currency
    }

    public class BaseHtmlExporter : UserControl
    {

        #region [ Export ]

        public void Export(DataTable dt, string fileName)
        {
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xls", fileName));

            ExportHtmlTable(dt, Response.OutputStream);

            Response.Flush();
        }

        public void Export(IDataReader dataReader, string fileName)
        {
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xls", fileName));

            ExportHtmlTableFromReader(dataReader, Response.OutputStream);

            Response.Flush();
        }

        public void ExportToFile(DataTable dt, string filePath)
        {
            using (var stream = System.IO.File.Create(filePath))
            {
                ExportHtmlTable(dt, stream);
            }
        }

        public void ExportToFile(IDataReader dataReader, string filePath)
        {
            using (var stream = System.IO.File.Create(filePath))
            {
                ExportHtmlTableFromReader(dataReader, stream);
            }
        }

        #endregion

        protected void ExportHtmlTable(DataTable dt, Stream str)
        {
            using (var writer = new StreamWriter(str))
            {
                WriteHeader(writer);

                writer.WriteLine("<tr>");
                foreach (DataColumn column in dt.Columns)
                    writer.Write("<th>{0}</th>", GetCaption(column.ColumnName));
                writer.WriteLine("</tr>");

                foreach (DataRow row in dt.Rows)
                {
                    writer.WriteLine("<tr>");
                    foreach (DataColumn column in dt.Columns)
                        WriteRow(writer, GetValue(row, column), column.ColumnName);
                    writer.WriteLine("</tr>");
                }

                WriteFooter(writer);
            }
        }

        protected void ExportHtmlTableFromReader(IDataReader dataReader, Stream str)
        {
            using (var writer = new StreamWriter(str))
            {
                WriteHeader(writer);

                if (dataReader.Read())
                {
                    var columnNames = new List<string>();

                    writer.WriteLine("<tr>");
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        var columnName = dataReader.GetName(i);
                        columnNames.Add(columnName);
                        writer.Write("<th>{0}</th>", GetCaption(columnName));
                    }
                    writer.WriteLine("</tr>");

                    writer.WriteLine("<tr>");
                    for (int i = 0; i < columnNames.Count; i++)
                        WriteRow(writer, GetValue(dataReader, i), columnNames[i]);
                    writer.WriteLine("</tr>");

                    while (dataReader.Read())
                    {
                        writer.WriteLine("<tr>");
                        for (int i = 0; i < columnNames.Count; i++)
                            WriteRow(writer, GetValue(dataReader, i), columnNames[i]);
                        writer.WriteLine("</tr>");
                    }
                }

                WriteFooter(writer);
            }
        }

        #region [ Virtual ]

        protected virtual Tuple<object, ExcelFormat?> GetRowValue(object value, string columnName)
        {
            var format = (ExcelFormat?)null;
            return Tuple.Create(value, format);
        }

        protected virtual string GetCaption(string columnName)
        {
            return columnName;
        }

        #endregion

        #region [ Helpers ]

        private string GetClassName(ExcelFormat format)
        {
            return format.ToString().ToLower().Substring(0, 1);
        }

        private object GetValue(IDataReader reader, int columnIndex)
        {
            return reader.IsDBNull(columnIndex) ? null : reader.GetValue(columnIndex);
        }

        private object GetValue(DataRow row, DataColumn column)
        {
            var value = row[column];
            return value == DBNull.Value ? null : value;
        }

        private void WriteHeader(StreamWriter writer)
        {
            writer.WriteLine(@"<html xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:x=""urn:schemas-microsoft-com:office:excel"" xmlns=""http://www.w3.org/TR/REC-html40"">");
            writer.WriteLine(@"<head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>Excel Document Name</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]-->");
            writer.WriteLine(@"<style type=""text/css"">
                    .t { mso-number-format:""\@""; }
                    .n { mso-number-format:General; }
                    .d { mso-number-format:""d\-m\-yyyy HH\:mm""; }
                    .c { mso-number-format:""\0022€\0022\#\,\#\#0\.00""; }
                </style>");
            writer.WriteLine("</head>");
            writer.WriteLine("<body><table>");
        }

        private void WriteFooter(StreamWriter writer)
        {
            writer.WriteLine("</table></body>");
            writer.WriteLine("</html>");
        }

        private void WriteRow(StreamWriter writer, object originalValue, string columnName)
        {
            var tuple = GetRowValue(originalValue, columnName);
            var value = tuple.Item1;
            var format = tuple.Item2;

            writer.Write("<td");
            if (format.HasValue)
                writer.Write(" class=\"{0}\"", GetClassName(format.Value));
            writer.Write(">{0}</td>", value);
        }

        #endregion
    }
}