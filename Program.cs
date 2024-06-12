using System.Diagnostics;
using System.Runtime.InteropServices;

[DllImport("shell32", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
static extern string SHGetKnownFolderPath(
       [MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags,
       nint hToken = 0);

string path = SHGetKnownFolderPath(new Guid("F1B32785-6FBA-4FCF-9D55-7B8E7F157091"), 0) + "\\Packages\\TheBrowserCompany.Arc_ttt1ap7aakyb4\\LocalCache\\Local\\firestore\\Arc\\bcny-arc-server\\main";

while (true) // 0% CPU and 8MB RAM
{
    try
    {
        Directory.Delete(path);
        var processes = Process.GetProcessesByName("Arc");

        Co
        while (processes.Count() == 0)
        {
            await Task.Delay(100);
            processes = Process.GetProcessesByName("Arc");
        }

        Task.WaitAll(processes.Select(p => p.WaitForExitAsync()).ToArray());
        Console.WriteLine("Arc for Windows closed, deleting the firestore folder");
    }
    catch (Exception e) { Console.WriteLine(e.Message); }
}
