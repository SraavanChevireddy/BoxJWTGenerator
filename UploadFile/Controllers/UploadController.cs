using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UploadFile.Controllers
{
    public class UploadController : System.Web.Http.ApiController
    {
        [HttpGet]
        public string GetAccessToken()
        {
            ReturnUpload returnUpload = new ReturnUpload();
            string configFileName = System.Web.Hosting.HostingEnvironment.MapPath("~/Models/Clientconfiguration.json");
            StreamReader reader = new StreamReader(configFileName);
            string json = reader.ReadToEnd();
            Config config = JsonConvert.DeserializeObject<Config>(json);

            BoxConfig boxConfig = new BoxConfig(config.boxAppSettings.clientID, config.boxAppSettings.clientSecret, config.enterpriseID, config.boxAppSettings.appAuth.privateKey, config.boxAppSettings.appAuth.passphrase, config.boxAppSettings.appAuth.publicKeyID);

            try
            {
                BoxJWTAuth boxJWT = new BoxJWTAuth(boxConfig);
                string adminToken = boxJWT.AdminToken();

                return adminToken;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        internal class Config
        {
            public class BoxAppSettings
            {
                public class AppAuth
                {
                    public string privateKey { get; set; }
                    public string passphrase { get; set; }
                    public string publicKeyID { get; set; }
                }
                public string clientID { get; set; }
                public string clientSecret { get; set; }
                public AppAuth appAuth { get; set; }

            }
            public string enterpriseID { get; set; }
            public BoxAppSettings boxAppSettings { get; set; }
        }

        private class ReturnUpload
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public string message { get; set; }
        }
    }
}