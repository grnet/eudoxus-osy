using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;

namespace EudoxusOsy.Portal
{
    public static class GridViewExporterExtensions
    {
        public static void ExportWithDefaults(this ASPxGridViewExporter exporter)
        {
            exporter.WriteXlsxToResponse(true, new XlsxExportOptionsEx()
            {
                ExportType = ExportType.WYSIWYG
            });
        }
    }
}