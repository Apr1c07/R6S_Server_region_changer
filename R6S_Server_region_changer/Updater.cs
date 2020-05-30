using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Reflection;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace R6S_Server_region_changer
{
    class Updater
    {
        private readonly string _apiEndpoint = "https://api.github.com/repos/sir-wilhelm/SmartHunter/releases/latest";
        private readonly string _userAgent = "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        public bool CheckForUpdates()
        {
            try
            {
                var latestRelease = GetLatestRelease();
                var needsUpdates = new Version(latestRelease.tag_name) > Assembly.GetExecutingAssembly().GetName().Version;
                if (!needsUpdates)
                {
                    MessageBox.Show("No updates found.");
                }
                return needsUpdates;
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error has occured while searching for updates:{Environment.NewLine}{e}");
                MessageBox.Show("Resuming the normal flow of the application.");
                return false;
            }
        }

        private LatestRelease GetLatestRelease()
        {
            var request = WebRequest.CreateHttp(_apiEndpoint);
            request.ContentType = "application/json";
            request.UserAgent = _userAgent;
            var stream = request.GetResponse().GetResponseStream();
            var reader = new StreamReader(stream);
            var latestReleaseAsJson = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<LatestRelease>(latestReleaseAsJson);
        }

        public bool DownloadUpdates()
        {
            try
            {
                var latestRelease = GetLatestRelease();

                var client = new WebClient();
                var releaseZip = latestRelease.assets[0];
                MessageBox.Show("Deleting older update.");
                File.Delete(releaseZip.name);
                client.DownloadFile(releaseZip.browser_download_url, releaseZip.name);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error has occured while downloading update:{Environment.NewLine}{e}");
                return false;
            }
        }

        internal class LatestRelease
        {
            public string tag_name { get; set; }
            public Asset[] assets { get; set; }
        }
        internal class Asset
        {
            public string name { get; set; }
            public string browser_download_url { get; set; }
        }
    }
}
