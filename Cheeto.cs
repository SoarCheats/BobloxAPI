using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Text;



// This Public API was Created By SoarCheats#2848
// If You Copy This Ill Chop your dick off
// hf


namespace SoarCheetos
{
    #region main
    class Bridge
    {
        public static void AttachDLL(string file)
        {
            if (NamedPipes.NamedPipeExist(NamedPipes.luapipename))
            {
                MessageBox.Show("Already Injected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            else if (!NamedPipes.NamedPipeExist(NamedPipes.luapipename))
            {
                string dir = Directory.GetCurrentDirectory();
                WebClient wb = new WebClient();
                switch (Injector.DllInjector.GetInstance.Inject("RobloxPlayerBeta", dir + @"\" + file))
                {
                    case Injector.DllInjectionResult.Success:
                        MessageBox.Show("Attachment Completed!", "Injection Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    case Injector.DllInjectionResult.DllNotFound:
                        MessageBox.Show("Failed to Locate Module", "DLL Provider", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    case Injector.DllInjectionResult.GameProcessNotFound:
                        MessageBox.Show("Couldn't Find RobloxPlayerBeta.exe ?", "Roblox Was Not Found ?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    case Injector.DllInjectionResult.InjectionFailed:
                        MessageBox.Show("Injection Failed, Make Sure All Anti Virus Softwares Are Off If Continues Download VC++", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }
                Thread.Sleep(2000);
                if (!NamedPipes.NamedPipeExist(NamedPipes.luapipename))
                {
                    MessageBox.Show("The Main Exploit Module Is Probably Not Updated", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
    class updater
    {
        public static void Check(string versioncheck, string currentversion)
        {
            WebClient wb = new WebClient();
            string ver = wb.DownloadString(versioncheck);
            if (!ver.Contains(currentversion))
            {
                bool ov = true;
            }
            else
            {
                return;
            }
        }
    }


    class Bootstrapper
    {
        public static void Start(string zipdownload, string zipname)
        {
            if (ov == true)
            {
                dialogresult msg = MessageBox.Show("Your Currently Running An Old Version Of The Exploit\n Would You Like To Download The Latest Version?","Database Connection Error",MessageBoxButtons.YesNo,MessageBoxIcon.Error);
                if (msg == dialogresult.Yes)
                {
                    wb.DownloadFile(zipdownload,path);
                    string dir = Directory.GetCurrentDirectory();
                    Directory.CreateDirectory(dir + "\\Output");
                    ZipFile.ExtractToDirectory(dir + "\\" + zipname, dir + "\\output");
                    File.Delete(dir + zipname);
                    MessageBox.Show("Download Complete, New Version Located In Executable / Output","Database Connection",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                }
                else
                {
                    Application.Exit();
                }
            }
            else
            {
                return;
            }
        }
    }

    class API
    {
        public static void Execute(string script)
        {
            NamedPipes.LuaPipe(script);
        }

    }
    #endregion
    #region dllinjection

    class Injector
    {
        public enum DllInjectionResult
        {
            DllNotFound,
            GameProcessNotFound,
            InjectionFailed,
            Success
        }

        public sealed class DllInjector
        {
            static readonly IntPtr INTPTR_ZERO = (IntPtr)0;

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern int CloseHandle(IntPtr hObject);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr GetModuleHandle(string lpModuleName);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress,
                IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

            static DllInjector _instance;

            public static DllInjector GetInstance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new DllInjector();
                    }
                    return _instance;
                }
            }

            DllInjector() { }

            public DllInjectionResult Inject(string sProcName, string sDllPath)
            {
                if (!File.Exists(sDllPath))
                {
                    return DllInjectionResult.DllNotFound;
                }

                uint _procId = 0;

                Process[] _procs = Process.GetProcesses();
                for (int i = 0; i < _procs.Length; i++)
                {
                    if (_procs[i].ProcessName == sProcName)
                    {
                        _procId = (uint)_procs[i].Id;
                        break;
                    }
                }

                if (_procId == 0)
                {
                    return DllInjectionResult.GameProcessNotFound;
                }

                if (!bInject(_procId, sDllPath))
                {
                    return DllInjectionResult.InjectionFailed;
                }

                return DllInjectionResult.Success;
            }

            bool bInject(uint pToBeInjected, string sDllPath)
            {
                IntPtr hndProc = OpenProcess((0x2 | 0x8 | 0x10 | 0x20 | 0x400), 1, pToBeInjected);

                if (hndProc == INTPTR_ZERO)
                {
                    return false;
                }

                IntPtr lpLLAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

                if (lpLLAddress == INTPTR_ZERO)
                {
                    return false;
                }

                IntPtr lpAddress = VirtualAllocEx(hndProc, (IntPtr)null, (IntPtr)sDllPath.Length, (0x1000 | 0x2000), 0X40);

                if (lpAddress == INTPTR_ZERO)
                {
                    return false;
                }

                byte[] bytes = Encoding.ASCII.GetBytes(sDllPath);

                if (WriteProcessMemory(hndProc, lpAddress, bytes, (uint)bytes.Length, 0) == 0)
                {
                    return false;
                }

                if (CreateRemoteThread(hndProc, (IntPtr)null, INTPTR_ZERO, lpLLAddress, lpAddress, 0, (IntPtr)null) == INTPTR_ZERO)
                {
                    return false;
                }

                CloseHandle(hndProc);

                return true;
            }
        }
    }
    #endregion

    #region LuaPipeLines
    class NamedPipes
    {
        public static string luapipename = "soarcheatsiscute";

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WaitNamedPipe(string name, int timeout);
        public static bool NamedPipeExist(string pipeName)
        {
            bool result;
            try
            {
                int timeout = 0;
                if (!WaitNamedPipe(Path.GetFullPath(string.Format("\\\\\\\\.\\\\pipe\\\\{0}", pipeName)), timeout))
                {
                    int lastWin32Error = Marshal.GetLastWin32Error();
                    if (lastWin32Error == 0)
                    {
                        result = false;
                        return result;
                    }
                    if (lastWin32Error == 2)
                    {
                        result = false;
                        return result;
                    }
                }
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public static void LuaPipe(string script)
        {
            if (NamedPipeExist(luapipename))
            {
                new Thread(() =>
                {
                    try
                    {
                        using (NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", luapipename, PipeDirection.Out))
                        {
                            namedPipeClientStream.Connect();
                            using (StreamWriter streamWriter = new StreamWriter(namedPipeClientStream, System.Text.Encoding.Default, 999999))
                            {
                                streamWriter.Write(script);
                                streamWriter.Dispose();
                            }
                            namedPipeClientStream.Dispose();
                        }
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Couldnt Get Into Contact With The Modules Lua Pipe..", "Failed ;(", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }).Start();
            }
            else
            {
                MessageBox.Show("Inject Module First...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
    }
    #endregion
}