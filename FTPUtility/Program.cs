using FTPUtility;

Console.WriteLine("Hello, World!");
var ftpClient = new FtpClient("ftp.example.com", "username", "password");
await ftpClient.UploadFileAsync(@"C:\local\file.txt", "remote/file.txt");
await ftpClient.DownloadFileAsync("remote/file.txt", @"C:\local\file.txt");
