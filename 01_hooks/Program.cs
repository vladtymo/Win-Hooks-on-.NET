using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_hooks
{
    /*
        LRESULT CALLBACK HookProc(int nCode, WPARAM wParam, LPARAM lParam)
        {
            // check whether to process the message
            if (code < 0)
                return CallNextHookEx(hookDeleg, code, wParam, lParam);
                
            // some code here...

            // Pass the message to the next filtering
            // function
            return CallNextHookEx(hookDeleg, code, wParam, lParam);
        }
    */

    // private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    /*
    IntPtr SetWindowsHookEx(
        int idHook,         - type of hook procedure (WH_MOUSE, WH_KEYBOARD, …)
        HookProc lpfn,      - hook procedure, callback-функция
        IntPtr hMod,        - handle для DLL or null
        uint dwThreadId);   - thread id, 0 - for all
    */
    // bool UnhookWindowsHookEx(IntPtr hhk);

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
