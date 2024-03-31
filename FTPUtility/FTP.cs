using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using Renci.SshNet;
using System.IO;
using System.Threading.Tasks;

namespace FTPUtility
{

    public class FtpClient
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly HttpClient _httpClient;

        public FtpClient(string host, int port, string username, string password)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"sftp://{_username}@{_host}/");
        }

        //public async Task UploadFileAsync(string localFilePath, string remoteFilePath)
        //{
        //    using (var fileStream = File.OpenRead(localFilePath))
        //    using (var content = new StreamContent(fileStream))
        //    {
        //        var ftpUri = new Uri($"sftp://{_host}/{remoteFilePath}");
        //        var requestMessage = new HttpRequestMessage(HttpMethod.Put, ftpUri);
        //        requestMessage.Content = content;

        //        requestMessage.Headers.Add("Authorization", $"Basic {GetBase64Credentials()}");
        //        requestMessage.Headers.Add("Content-Type", "application/octet-stream");

        //        using (var response = await _httpClient.SendAsync(requestMessage))
        //        {
        //            response.EnsureSuccessStatusCode();
        //        }
        //    }
        //}

        public static void SFTPUploadFileAsync()
        {
            //string remoteDirectoryPath = @"\C:\Users\sftpuser"; //.DellArcturus\Pictures";
            string remoteDirectoryPath = @"/C:/Users/sftpuser";

            // Local file path
            string localFilePath = @"C:\Users\admin\source\scriptingSchoolNotes\PowerShellSQL.txt";

            try
            {
                using (var client = new SftpClient("localhost", 22, "sftpuser", "password"))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        Console.WriteLine("I AM CONNECTED");
                    }

                    //PrintDirectoryTree(client, remoteDirectoryPath, "");



                    client.ChangeDirectory(remoteDirectoryPath);
                    Console.WriteLine($"Changed remote directory to: {remoteDirectoryPath}");



                    using (var fileStream = File.OpenRead(localFilePath))
                    {
                        if (fileStream != null)
                        {
                            Console.WriteLine("YOU ARE NOT NULL");
                        }
                        client.UploadFile(fileStream, Path.GetFileName(localFilePath));
                    }
                    client.Disconnect();
                }
                Console.WriteLine("File transfer successful. End of Code-Block");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {localFilePath}");
                Console.WriteLine($"Error message: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during file upload: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }


        public async Task DownloadFileAsync(string remoteFilePath, string localFilePath)
        {
            var ftpUri = new Uri($"sftp://{_host}/{remoteFilePath}");
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

        static void PrintDirectoryTree(SftpClient client, string directory, string prefix)
        {
            var files = client.ListDirectory(directory);
            foreach (var file in files)
            {
                Console.WriteLine(prefix + file.Name);
                if (file.IsDirectory && file.Name != "." && file.Name != "..")
                {
                    PrintDirectoryTree(client, directory + "/" + file.Name, prefix + "   ");
                }
            }
        }
    }
}
