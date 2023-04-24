using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;

namespace FTPUtility
{

    public class FtpClient
    {
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private readonly HttpClient _httpClient;

        public FtpClient(string host, string username, string password)
        {
            _host = host;
            _username = username;
            _password = password;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"ftp://{_host}/");
        }

        public async Task UploadFileAsync(string localFilePath, string remoteFilePath)
        {
            using (var fileStream = File.OpenRead(localFilePath))
            using (var content = new StreamContent(fileStream))
            {
                var ftpUri = new Uri($"ftp://{_host}/{remoteFilePath}");
                var requestMessage = new HttpRequestMessage(HttpMethod.Put, ftpUri);
                requestMessage.Content = content;

                requestMessage.Headers.Add("Authorization", $"Basic {GetBase64Credentials()}");
                requestMessage.Headers.Add("Content-Type", "application/octet-stream");

                using (var response = await _httpClient.SendAsync(requestMessage))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        public async Task DownloadFileAsync(string remoteFilePath, string localFilePath)
        {
            var ftpUri = new Uri($"ftp://{_host}/{remoteFilePath}");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, ftpUri);
            requestMessage.Headers.Add("Authorization", $"Basic {GetBase64Credentials()}");

            using (var response = await _httpClient.SendAsync(requestMessage))
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var fileStream = File.Create(localFilePath))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        private string GetBase64Credentials()
        {
            var credentials = $"{_username}:{_password}";
            var bytes = System.Text.Encoding.UTF8.GetBytes(credentials);
            return Convert.ToBase64String(bytes);
        }
    }
}
