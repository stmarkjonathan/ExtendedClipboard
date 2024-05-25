using Avalonia.Input;
using Avalonia.Win32.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedClipboardAvalonia.Models
{
    public class Hotkey:INotifyPropertyChanged
    {

        private const int WM_HOTKEY = 0x312;

        private int _action;

        public int Action
        {
            get { return _action; }
            set
            {
                _action = value;
                OnPropertyChanged("Option");
            }
        }

        private int _keyModifier;

        public int KeyModifier
        {
            get { return _keyModifier; }

            set
            {
                _keyModifier = value;
                OnPropertyChanged("Modifier");
            }
        }

        private int _virtualKey;

        public int VirtualKey
        {
            get 
            {              
                return _virtualKey; ; 
            }

            set
            {
                _virtualKey = value;
                OnPropertyChanged("PressedKey");
            }
        }

        public Hotkey(int keyModifier, Key pressedKey)
        {
            _keyModifier = keyModifier;
            _virtualKey = KeyInterop.VirtualKeyFromKey(pressedKey);

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
