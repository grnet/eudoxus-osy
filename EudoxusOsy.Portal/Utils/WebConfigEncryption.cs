using System.Configuration;
using System.Web;
using System.Web.Configuration;
namespace EudoxusOsy.Portal.Utils
{

    /// <summary>
    /// Utility class for encrypting and decrypting the web.config file
    /// </summary>
    public static class WebConfigEncryption
    {
        /// <summary>
        /// Encrypt a section of the web.config
        /// </summary>
        /// <param name="sectionToEncrypt">Name of the section to encrypt</param>
        /// <param name="encryptionProvider">One of the EncryptionProviders to use</param>
        public static void EncryptSection(string sectionToEncrypt)
        {
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("/");
            ConfigurationSection section = config.GetSection(sectionToEncrypt);

            if (!section.SectionInformation.IsProtected)
            {
                if (!section.ElementInformation.IsLocked)
                {
                    section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                    config.Save(ConfigurationSaveMode.Full);
                }
            }
        }

        ///// <summary>
        ///// Decrypt a section of the web.config
        ///// </summary>
        ///// <param name="sectionToEncrypt">Name of the section to decrypt</param>
        //public static void DecryptSection(string sectionToEncrypt) {
        //    Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
        //    ConfigurationSection section = config.GetSection(sectionToEncrypt);

        //    if (section.SectionInformation.IsProtected) {
        //        if (!section.ElementInformation.IsLocked) {
        //            section.SectionInformation.UnprotectSection();
        //            config.Save(ConfigurationSaveMode.Full);
        //        }
        //    }
        //}
    }
}
