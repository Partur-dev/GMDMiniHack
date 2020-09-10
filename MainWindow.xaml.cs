using System;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Input;
using System.IO;
using System.Text;
namespace MegaHackMini
{
    public partial class MainWindow : Window
    {
        public static Process proc;
        public static IntPtr startOffset;
        public static IntPtr hProc;
        public static int wtf;
        public static byte[] Noclip_on = { 233, 121, 6, 0, 0 };
        public static byte[] Noclip_off = { 106, 20, 139, 203, 255 };
        public static byte[] Icon_on1 = { 176, 1, 144, 144, 144 };
        public static byte[] Icon_on2 = { 176, 1, 144, 144, 144 };
        public static byte[] Icon_off1 = { 232, 122, 205, 25, 0 };
        public static byte[] Icon_off2 = { 232, 104, 201, 25, 0 };
        public static byte[] PMH_on1 = { 144, 144, 144, 144, 144, 144 };
        public static byte[] PMH_on2 = { 144, 144 };
        public static byte[] PMH_on3 = { 144, 144 };
        public static byte[] PMH_on4 = { 144, 144 };
        public static byte[] PMH_off1 = { 15, 133, 247, 0, 0, 0 };
        public static byte[] PMH_off2 = { 117, 65 };
        public static byte[] PMH_off3 = { 117, 62 };
        public static byte[] PMH_off4 = { 117, 12 };
        private static string ProcName = "";
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Noclip_Checked(object sender, RoutedEventArgs e)
        {
            WriteProcessMemory(hProc, startOffset + 0x20A23C, Noclip_on, (uint)Noclip_on.Length, out wtf);
        }
        private void Noclip_UnChecked(object sender, RoutedEventArgs e)
        {
            WriteProcessMemory(hProc, startOffset + 0x20A23C, Noclip_off, (uint)Noclip_off.Length, out wtf);
        }
        private void PMH_Checked(object sender, RoutedEventArgs e)
        {
            WriteProcessMemory(hProc, startOffset + 0x20C925, PMH_on1, (uint)PMH_on1.Length, out wtf);
            WriteProcessMemory(hProc, startOffset + 0x20D143, PMH_on2, (uint)PMH_on2.Length, out wtf);
            WriteProcessMemory(hProc, startOffset + 0x20A563, PMH_on3, (uint)PMH_on3.Length, out wtf);
            WriteProcessMemory(hProc, startOffset + 0x20A595, PMH_on4, (uint)PMH_on4.Length, out wtf);
        }
        private void PMH_UnChecked(object sender, RoutedEventArgs e)
        {
            WriteProcessMemory(hProc, startOffset + 0x20C925, PMH_off1, (uint)PMH_off1.Length, out wtf);
            WriteProcessMemory(hProc, startOffset + 0x20D143, PMH_off2, (uint)PMH_off2.Length, out wtf);
            WriteProcessMemory(hProc, startOffset + 0x20A563, PMH_off3, (uint)PMH_off3.Length, out wtf);
            WriteProcessMemory(hProc, startOffset + 0x20A595, PMH_off4, (uint)PMH_off4.Length, out wtf);
        }
        private void Icon_Checked(object sender, RoutedEventArgs e)
        {
            WriteProcessMemory(hProc, startOffset + 0xC50A8, Icon_on1, (uint)Icon_on1.Length, out wtf);
            WriteProcessMemory(hProc, startOffset + 0xC54BA, Icon_on2, (uint)Icon_on2.Length, out wtf);
        }
        private void Icon_UnChecked(object sender, RoutedEventArgs e)
        {
            WriteProcessMemory(hProc, startOffset + 0xC50A8, Icon_off1, (uint)Icon_off1.Length, out wtf);
            WriteProcessMemory(hProc, startOffset + 0xC54BA, Icon_off2, (uint)Icon_off2.Length, out wtf);
        }
        private static void ConnectToGMD()
        {
            MainWindow.proc = Process.GetProcessesByName(ProcName).FirstOrDefault<Process>();
            MainWindow.startOffset = MainWindow.proc.MainModule.BaseAddress;
            MainWindow.hProc = MainWindow.OpenProcess(MainWindow.ProcessAccessFlags.All, false, MainWindow.proc.Id);
            MainWindow.wtf = 0;
        }
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (Process.GetProcessesByName(ProcessName.Text).Length > 0)
            {
                Connected.Content = "Connected";
                ProcName = ProcessName.Text;
                Noclip.IsEnabled = true;
                PMH.IsEnabled = true;
                Icon.IsEnabled = true;
                ConnectToGMD();
            }
            else
            {
                MessageBox.Show("Failed");
            }
        }
        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern IntPtr OpenProcess(MainWindow.ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            Terminate = 1,
            CreateThread = 2,
            VMOperation = 8,
            VMRead = 16,
            VMWrite = 32,
            DupHandle = 64,
            SetInformation = 512,
            QueryInformation = 1024,
            Synchronize = 1048576,
            All = 2035711
        }
    }
}
