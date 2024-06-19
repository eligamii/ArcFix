using System.Diagnostics;
using System.Runtime.InteropServices;
using IWshRuntimeLibrary;

public static class Program
{
    [DllImport("shell32", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
    static extern string SHGetKnownFolderPath(
       [MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags,
       nint hToken = 0);

    private readonly static string _arcLocalAppDataPath = SHGetKnownFolderPath(new Guid("F1B32785-6FBA-4FCF-9D55-7B8E7F157091"), 0) + "\\Packages\\TheBrowserCompany.Arc_ttt1ap7aakyb4";
    private readonly static string _arcFixFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ArcFix");

    public static int Main(string[] args)
    {
        if (args.Count() == 0)
            FixFirestore();
        else if (args[0] == "startup")
        {
            if (args[1] == "on")
            {
                try
                {
                    RegisterStartupTask();
                    Console.WriteLine("Successfully registered ArcFix as startup task");
                }
                catch
                {
                    Console.WriteLine("Could not register ArcFix as startup task, maybe it is already registered?");
                }
            }
            else if(args[1] == "off")
            {
                try
                {
                    UnregisterStartupTask();
                    Console.WriteLine("Successfully unregistered ArcFix as startup task");
                }
                catch
                {
                    Console.WriteLine("Could not unregister ArcFix as startup task, maybe it is already unregistered?");
                }
            }
        }
        else if (args[0] == "reset")
        {
            try
            {
                ResetArc();
                Console.WriteLine("Successfully reset Arc. You can restore your data by using the 'restore' command");
            }
            catch
            {
                Console.WriteLine("Could not reset Arc");
            }
        }
        else if (args[0] == "restore")
        {
            RestoreArc();
            Console.WriteLine("Successfully restored Arc data");
        }
 

        return 0;
    }

    private static void FixFirestore()
    {
        string path = Path.Combine(_arcLocalAppDataPath, "LocalCache", "Local", "firestore", "Arc");

        while (true) // 0.5% or 0% CPU when using Arc (i5 1155g7) and 8.7MB RAM
        {
            try
            {
                try { if(Directory.Exists(path)) Directory.Delete(path, true); } catch { }
                var processes = Process.GetProcessesByName("Arc");


                while (processes.Count() == 0)
                {
                    Thread.Sleep(100);
                    processes = Process.GetProcessesByName("Arc");
                }

                Task.WaitAll(processes.Select(p => p.WaitForExitAsync()).ToArray());
                Console.WriteLine("Arc for Windows closed, deleting the firestore folder");
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }

    private static void RegisterStartupTask()
    {
        
        // Put the executable in a safe folder
        string execPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArcFix.exe");
        string permanantPath = Path.Combine(_arcFixFolderPath, "ArcFix.exe");
        try { Directory.CreateDirectory(_arcFixFolderPath); }  catch { }

        System.IO.File.Copy(execPath, permanantPath, true);

        // Create the shortcut for the executable to run at startup
        WshShell wshShell = new();

        IWshShortcut shortcut;
        string startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

        shortcut = (IWshShortcut)wshShell.CreateShortcut(Path.Combine(startUpFolderPath, "ArcFix.lnk"));

        shortcut.TargetPath = permanantPath;
        shortcut.Description = "Launch ArcFix at startup";

        shortcut.Save();
    }

    private static void UnregisterStartupTask()
    {
        System.IO.File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "ArcFix.lnk")); 
    }

    private static void ResetArc()
    {
        TerminateArc();
        
        // Try to backup Arc's data
        try
        {
            Directory.Move(Path.Combine(_arcLocalAppDataPath, "LocalCache", "Local", "Arc"), Path.Combine(_arcFixFolderPath, "ArcBackup"));
        }
        catch { }

        Directory.Delete(_arcLocalAppDataPath, true);
    }

    private static void RestoreArc()
    {
        TerminateArc();
        Directory.Delete(Path.Combine(_arcLocalAppDataPath, "LocalCache", "Local", "Arc"), true);
        Directory.Move(Path.Combine(_arcFixFolderPath, "ArcBackup"), Path.Combine(_arcLocalAppDataPath, "LocalCache", "Local", "Arc"));
    }

    private static void TerminateArc()
    {
        Process.Start(new ProcessStartInfo("taskkill.exe")
        {
            Arguments = $"/f /im Arc.exe",
            CreateNoWindow = true,
            UseShellExecute = false,
            Verb = "runas"
        });
    }

}
