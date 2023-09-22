using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace _02_block_key
{

    class BlockWindows
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WH_MOUSE_LL = 14;

        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_KEYDOWN = 0x0100;

        private static IntPtr keyHook = IntPtr.Zero;
        private static IntPtr mouseHook = IntPtr.Zero;

        private static Keyboard keyboard = new Keyboard();

        public static void Main()
        {
            Console.WriteLine("S - block\n" +
                              "Y - change to 'HELLO'\n" +
                              "Win - block\n");
            keyHook = SetHook(KeyboardHookCallback, WH_KEYBOARD_LL);

            //Console.WriteLine("Mouse move: X < 400 - block\n");
            //mouseHook = SetHook(MouseHookCallback, WH_MOUSE_LL);

            Application.Run();

            //UnhookWindowsHookEx(mouseHook);ttutiufvc
            UnhookWindowsHookEx(keyHook);
        }

        private static IntPtr SetHook(HookProc proc, int hookId)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(hookId, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return CallNextHookEx(keyHook, nCode, wParam, lParam);

            if (wParam == (IntPtr)WM_MOUSEMOVE)
            {
                int X = Marshal.ReadInt32(lParam);
                if (X < 400) return (IntPtr)1;
            }
            return CallNextHookEx(mouseHook, nCode, wParam, lParam);
        }
        private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return CallNextHookEx(keyHook, nCode, wParam, lParam);

            if (wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                Logger.LogKeyDown((Keys)vkCode);

                if ((Keys)vkCode == Keys.Y)
                {
                    Console.WriteLine("{0} change to HELLO!");
                    keyboard.Send(Keyboard.ScanCodeShort.KEY_H);
                    keyboard.Send(Keyboard.ScanCodeShort.KEY_E);
                    keyboard.Send(Keyboard.ScanCodeShort.KEY_L);
                    //Thread.Sleep(50);
                    keyboard.Send(Keyboard.ScanCodeShort.KEY_L);
                    keyboard.Send(Keyboard.ScanCodeShort.KEY_O);
                    return (IntPtr)1;
                }

                if ((Keys)vkCode == Keys.S)
                {
                    Console.WriteLine("{0} blocked!", (Keys)vkCode);
                    return (IntPtr)1;
                }

                if (((Keys)vkCode == Keys.LWin) || ((Keys)vkCode == Keys.RWin))
                {
                    Console.WriteLine("{0} blocked!", (Keys)vkCode);
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(keyHook, nCode, wParam, lParam);
        }


        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }

    class Logger
    {
        private static string logFilePath = "log.txt";
        public static void LogKeyDown(Keys key)
        {
            using (var writer = new StreamWriter(new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read, 64)))
            {
                writer.WriteLine($"{key} key down at {DateTime.Now}");
            }
        }
    }
}