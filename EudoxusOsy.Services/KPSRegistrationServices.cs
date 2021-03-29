using EudoxusOsy.BusinessModel;
using EudoxusOsy.Utils;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace EudoxusOsy.Services
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class KPSRegistrationServices : BaseService
    {
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "insert", Method = "PUT")]
        public ServiceResponse InsertRegistration(RegistrationRequest request)
        {
            try
            {
                bool? registrationInserted;
                registrationInserted = KpsRegistrationService.InsertRegistration(request);

                LogCall(true, enStatusCode.OK);

                if (registrationInserted == true)
                {
                    return new ServiceResponse(true, enStatusCode.KPSRegistrationInsertionSucceeded);
                }
                else
                {
                    return new ServiceResponse(true, enStatusCode.KPSRegistrationInsertionFailed);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse(true, enStatusCode.Errors, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "insertLibrary", Method = "PUT")]
        public ServiceResponse InsertLibraryRegistration(LibraryRegistrationRequest request)
        {
            try
            {
                bool? libraryRegistrationInserted;
                libraryRegistrationInserted = KpsRegistrationService.LibraryRegistration(request);

                LogCall(true, enStatusCode.OK);

                if (libraryRegistrationInserted == true)
                {
                    return new ServiceResponse(true, enStatusCode.KPSRegistrationInsertionSucceeded);
                }
                else
                {
                    return new ServiceResponse(true, enStatusCode.KPSRegistrationInsertionFailed);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse(true, enStatusCode.Errors, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [WebInvoke(UriTemplate = "putXML", Method = "PUT")]
        public ServiceResponse PostRegistrationXML(Stream finalXml)
        {
            try
            {
                string fileName = Path.Combine(Config.XmlFilesPath, string.Format("AuditReceiptsXmlData_{0}.xml", DateTime.Today.Ticks));

                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    finalXml.CopyTo(fileStream);
                }

                UpdateApplicationDataEntries(fileName);

                bool? registrationInserted;

                SQLXMLBulkLoad.doBulkLoad(fileName);
                registrationInserted = true;


                LogCall(true, enStatusCode.OK);

                if (registrationInserted == true)
                {
                    return new ServiceResponse(true, enStatusCode.KPSRegistrationInsertionSucceeded);
                }
                else
                {
                    return new ServiceResponse(true, enStatusCode.KPSRegistrationInsertionFailed);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse(true, enStatusCode.Errors, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        private void UpdateApplicationDataEntries(string fileName)
        {
            BusinessHelper.UpdateApplicationDataEntry(ApplicationDataNames.CurrentAuditReceiptXml, fileName);
            BusinessHelper.UpdateApplicationDataEntry(ApplicationDataNames.ShouldRunXmlComparisonAndSendReports, enYesNo.Yes.GetValue().ToString());
        }
    }

    public static class SQLXMLBulkLoad
    {
        [STAThread]
        public static void doBulkLoad(string fileName)
        {            
            try
            {
                SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                myConnection.Open();

                SqlCommand truncateCmd = myConnection.CreateCommand();
                truncateCmd.CommandText = "TRUNCATE TABLE [dbo].[AuditReceiptXml]";
                truncateCmd.ExecuteNonQuery();

                SQLXMLBULKLOADLib.SQLXMLBulkLoad4Class objBL = new SQLXMLBULKLOADLib.SQLXMLBulkLoad4Class();
                objBL.ConnectionString = EudoxusOsyConstants.SQLXML_BULK_CONNECTIONSTRING;
                objBL.ErrorLogFile = Path.Combine(Config.XmlFilesPath, "error.xml");
                objBL.KeepIdentity = false;
                string schemaPath = Path.Combine(Config.XmlFilesPath, "schema.xml");
                string dataPath = fileName;
                objBL.Execute(schemaPath, dataPath);
            }
            catch (Exception e)
            {
                LogHelper.LogError(e, typeof(KPSRegistrationServices), "SQLXMLBULK exception");
            }
        }
    }

    public class RawContentTypeMapper : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            try
            {
                if (contentType.Contains("text/xml") ||
                    contentType.Contains("application/xml"))
                {
                    return WebContentFormat.Raw;
                }
                else
                {
                    return WebContentFormat.Default;
                }
            }
            catch (Exception ex)
            { }
            return WebContentFormat.Default;
        }
    }
}
