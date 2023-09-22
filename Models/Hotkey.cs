using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace ExtendedClipboard.Models
{
    public class Hotkey
    {
        public const int MOD_ALT = 0x1;
        public const int MOD_CTRL = 0x2;
        public const int MOD_SHIFT = 0x4;
        public const int MOD_WIN = 0x8;
        public static Window window;

        private const int WM_HOTKEY = 0x312;

        private static int hotkeyId = 0;
        private static HwndSource source;


        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static void InstantiateHotKey(IntPtr handle)
        {
            source = HwndSource.FromHwnd(handle);
            source.AddHook(HwndHook);

            //0x79 = F9 Key
            RegisterHotKey(handle, hotkeyId, MOD_CTRL, 0x78);
        }

        private static IntPtr HwndHook(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(message == WM_HOTKEY)
            {

                if (window.WindowState == WindowState.Normal)
                {
                    window.WindowState = WindowState.Minimized;
                }
                else
                {
                    window.WindowState = WindowState.Normal;
                }
                
                handled = true;
            }

            return IntPtr.Zero;
        }

    }
}
