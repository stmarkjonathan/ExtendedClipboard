using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

namespace ExtendedClipboard.Models
{
    public class Hotkey : INotifyPropertyChanged
    {
        public enum HotkeyOptions
        {
            ToggleVisibility = 0,
            SwapMonitor = 1

        }

        private const int WM_HOTKEY = 0x312;

        private int _option;

        public int Option
        {
            get { return _option; }
            set 
            { 
                _option = value;
                OnPropertyChanged("Option");
            }
        }

        private int _modifier;

        public int Modifier
        {
            get { return _modifier; }

            set 
            { 
                _modifier = value;
                OnPropertyChanged("Modifier");
            }
        }

        private int _virtualKey;

        public Key PressedKey
        {
            get { return KeyInterop.KeyFromVirtualKey(_virtualKey); }
            
            set 
            {
                _virtualKey = KeyInterop.VirtualKeyFromKey(value);
                OnPropertyChanged("PressedKey");
            }
        }

        private static Window _window;
        private static int hotkeyID = 0;
        
        private HwndSource _source;
        private IntPtr _windowHandle;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static void InitializeWindow(Window window)
        {
            _window = window;
        }

        public Hotkey(int modifier, Key pressedKey, int option)
        {
            Modifier = modifier;
            PressedKey = pressedKey;
            Option = option;

            _windowHandle = new WindowInteropHelper(_window).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);
            RegisterHotKey(_windowHandle, hotkeyID, modifier, _virtualKey);
            hotkeyID++;
        }

        public void ChangeBind(int id, int modifier, Key key)
        {
            UnregisterHotKey(_windowHandle, id);

            Modifier = modifier;
            PressedKey = key;

            RegisterHotKey(_windowHandle, id, modifier, _virtualKey);
        }



        private IntPtr HwndHook(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (message == WM_HOTKEY)
            {
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
                }

                handled = true;
            }

            return IntPtr.Zero;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
