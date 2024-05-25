using Avalonia.Input;
using Avalonia.Platform;
using Avalonia.Win32.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedClipboardAvalonia.Models
{
    public class HotkeyController
    {

        private nint _windowHandle;
        private static int _hotkeyCount;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        List<Hotkey> _hotkeys = new List<Hotkey>()
        {
            new Hotkey((int)KeyModifiers.Control, Key.F9)
        };

        List<Hotkey> Hotkeys
        {
            get { return _hotkeys; }
            set { _hotkeys = value; }
        }

        public HotkeyController(nint windowHandle)
        {
            _windowHandle = windowHandle;
            InitHotkeys();          
        }

        private void InitHotkeys()
        {
            foreach(var hotkey in _hotkeys)
            {
                RegisterHotKey(_windowHandle, _hotkeyCount, hotkey.KeyModifier, hotkey.VirtualKey);
            }
        }

        public void RebindHotkey(int hotkeyID, int keyModifier, Key key)
        {
            var hotkey = _hotkeys[hotkeyID];

            hotkey.KeyModifier = keyModifier;
            hotkey.VirtualKey = KeyInterop.VirtualKeyFromKey(key);

            UnregisterHotKey(_windowHandle, hotkeyID);

            RegisterHotKey(_windowHandle, hotkeyID, hotkey.KeyModifier, hotkey.VirtualKey);
        }

    }
}
