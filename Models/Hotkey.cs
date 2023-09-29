using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace ExtendedClipboard.Models
{
    public class Hotkey
    {
        public enum HotkeyOptions
        {
            ToggleVisibility = 0,
            SwapMonitor = 1
                
        }

        private const int WM_HOTKEY = 0x312;
        private static int hotkeyID = 0;
        private Window _window;
        private HwndSource _source;
        private IntPtr _windowHandle;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public Hotkey(Window window)
        {
            _window = window;
            _windowHandle = new WindowInteropHelper(window).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);
            RegisterHotKey(_windowHandle, hotkeyID, 0, 0);

            hotkeyID++;
        }

        public Hotkey(Window window, int modifier, int key)
        {
            _window = window;
            _windowHandle = new WindowInteropHelper(window).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);
            RegisterHotKey(_windowHandle, hotkeyID, modifier, key);
            hotkeyID++;
        }

        public void ChangeBind(int id, int modifier, int key)
        {
            UnregisterHotKey(_windowHandle, id);

            RegisterHotKey(_windowHandle, id, modifier, key);
        }
                


        private IntPtr HwndHook(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(message == WM_HOTKEY)
            {
                Debug.Write("hotkey pressed");
                switch (wParam.ToInt32())
                {
                    case (int)HotkeyOptions.ToggleVisibility:
                        {
                            if (_window.Visibility == Visibility.Visible)
                            {
                                _window.Visibility = Visibility.Hidden;
                            }
                            else
                            {
                                _window.Visibility = Visibility.Visible;
                            }
                            break;
                        }
                    case (int)HotkeyOptions.SwapMonitor:
                        {
                            Debug.Write("SwapMonitor Hotkey pressed");

                            break;
                        }
                    
                }

                handled = true;
            }

            return IntPtr.Zero;
        }
    }
}
