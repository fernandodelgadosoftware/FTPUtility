using FTPUtility;


Console.WriteLine("Hello, World!");
string filePath = @"C:\Users\admin\source\scriptingSchoolNotes\PowerShellSQL.txt";

// Convert the file path to a URI-like representation
Uri fileUri = new Uri(filePath);

// Print out the URI
Console.WriteLine("File URL: " + fileUri.ToString());
string currentUser = Environment.UserName;
Console.WriteLine($"Current user: {currentUser}");
//var ftpClient = new FtpClient("localhost", 22, "sftpuser", "password");

FtpClient.SFTPUploadFileAsync();
//@"C:\Users\admin\source\scriptingSchoolNotes\PowerShellSQL.txt", @"\C:\Users\sftpuser\PowerShellSQL.txt"
//sftp://sftpuser@localhost/C%3A/Users/sftpuser/
//await ftpClient.DownloadFileAsync("remote/file.txt", @"C:\local\file.txt");
Console.ReadLine();

