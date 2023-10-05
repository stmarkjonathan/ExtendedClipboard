using ExtendedClipboard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ExtendedClipboard.Services.Commands;
using ExtendedClipboard.Services;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Reflection.PortableExecutable;

namespace ExtendedClipboard.ViewModels
{
    public class ClipboardWindowViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<ClipboardClass>? _clipboards;
        public ObservableCollection<ClipboardClass> Clipboards
        {
            get
            {
                return _clipboards;
            }
            set
            {
                _clipboards = value;
                OnPropertyChanged("Clipboards");
            }
        }

        private ClipboardClass? _selectedClipboard;
        public ClipboardClass? SelectedClipboard
        {
            get
            {
                    return _selectedClipboard;     
            }
            set
            {
                _selectedClipboard = value;
                OnPropertyChanged("SelectedClipboard");
            }
        }

        private List<Hotkey> _hotkeys;

        public List<Hotkey> Hotkeys
        {
            get
            {
                return _hotkeys;
            }
            set
            {
                _hotkeys = value;
                OnPropertyChanged("Hotkeys");
            }
        }

        public enum HotkeyOptions
        {
            ToggleVisibility = 0,
            Test = 1
        }

        private JsonSerializeService<ClipboardClass> _serializeClipboard = new JsonSerializeService<ClipboardClass>();
        private JsonSerializeService<Hotkey> _serializeHotkey = new JsonSerializeService<Hotkey>();

        private JsonParserService _jsonParse = new JsonParserService();

        public RelayCommand AddCommand => new RelayCommand(execute => AddClipboard());

        public RelayCommand SaveToJsonCommand => new RelayCommand(execute => SaveClipboardsToJson());

        public RelayCommand ClearCommand => new RelayCommand(execute => ClearClipboards());

        public RelayCommand<ClipboardClass> CopyCommand => new RelayCommand<ClipboardClass>(execute => CopyFromClipboard(execute));

        public RelayCommand<ClipboardClass> RetrieveCommand => new RelayCommand<ClipboardClass>(execute => RetrieveClipboard(execute));
        public RelayCommand<ClipboardClass> DeleteCommand => new RelayCommand<ClipboardClass>(execute => DeleteClipboard(execute));
        private void AddClipboard()
        {
            ClipboardClass newClip = new ClipboardClass();
            Clipboards.Add(newClip);
            SelectedClipboard = newClip;
        }
        public ClipboardWindowViewModel()
        {
            Clipboards = _jsonParse.ParseClipboardFile(@"C:\ExtendedClipboard\clipboards.txt");
            Hotkeys = _jsonParse.ParseHotkeyFile(@"C:\ExtendedClipboard\hotkeys.txt");
        }

        private void CopyFromClipboard(ClipboardClass listItem)
        {
            string userData = Clipboard.GetText(TextDataFormat.UnicodeText);

            if (!String.IsNullOrWhiteSpace(userData) && !userData.Equals(listItem.ClipboardData))
            {
                listItem.setData(userData);

                if(Clipboards != null)
                {
                    SaveClipboardsToJson();
                }
            }    
        }

        private void ClearClipboards()
        {
            Clipboards.Clear();
            SaveClipboardsToJson();
        }

        private void RetrieveClipboard(ClipboardClass listItem)
        {
            if (listItem.ClipboardData != null)
            {
                Clipboard.SetText(listItem.ClipboardData);
            }

        }

        private void DeleteClipboard(ClipboardClass listItem)
        {

            var clipboardID = Clipboards[Clipboards.IndexOf(listItem)].ClipboardID;
            foreach (ClipboardClass clip in Clipboards)
            {
                if (clipboardID == clip.ClipboardID)
                {
                    Clipboards.Remove(clip);
                    SaveClipboardsToJson();
                    return;
                }
            }
        }
        public void SaveClipboardsToJson()
        {
                _serializeClipboard.TargetList.Clear();

                foreach (var clipboard in Clipboards)
                {
                    if (!String.IsNullOrWhiteSpace(clipboard.ClipboardData))
                    _serializeClipboard.TargetList.Add(clipboard);
                }

            _serializeClipboard.SerializeData(@"C:\ExtendedClipboard\", "clipboards.txt");
            
        }

        public void SaveHotkeysToJson()
        {

            _serializeHotkey.TargetList.Clear();

            foreach(var hotkey in Hotkeys)
            {
                _serializeHotkey.TargetList.Add(hotkey);
            }

            _serializeHotkey.SerializeData(@"C:\ExtendedClipboard\", "hotkeys.txt");

        }


        public void ChangeHotkey(int modifier, Key pressedKey, string action)
        {


            switch (Enum.Parse(typeof(HotkeyOptions), action))
            {
                case HotkeyOptions.ToggleVisibility:
                    {
                        Hotkeys[0].ChangeBind((int)HotkeyOptions.ToggleVisibility, modifier, pressedKey);
                        Debug.Write(pressedKey);
                        break;
                    }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        

    }

}

