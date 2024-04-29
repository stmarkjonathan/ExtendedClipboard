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
using static System.Windows.Forms.DataFormats;
using System.Windows.Media.Imaging;
using System.Collections.Specialized;

namespace ExtendedClipboard.ViewModels
{
    public class ClipboardWindowViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<ClipboardItem>? _clipboards;
        public ObservableCollection<ClipboardItem> Clipboards
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

        private ClipboardItem? _selectedClipboard;
        public ClipboardItem? SelectedClipboard
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

        #region Commands

        private JsonSerializeService<ClipboardItem> _serializeClipboard = new JsonSerializeService<ClipboardItem>();
        private JsonSerializeService<Hotkey> _serializeHotkey = new JsonSerializeService<Hotkey>();

        private JsonParserService _jsonParse = new JsonParserService();

        public RelayCommand AddCommand => new RelayCommand(execute => AddClipboard());

        public RelayCommand SaveToJsonCommand => new RelayCommand(execute => SaveClipboardsToJson());

        public RelayCommand ClearCommand => new RelayCommand(execute => ClearClipboards());

        public RelayCommand<ClipboardItem> CopyCommand => new RelayCommand<ClipboardItem>(execute => CopyFromClipboard(execute));

        public RelayCommand<ClipboardItem> RetrieveCommand => new RelayCommand<ClipboardItem>(execute => RetrieveClipboard(execute));
        public RelayCommand<ClipboardItem> DeleteCommand => new RelayCommand<ClipboardItem>(execute => DeleteClipboard(execute));

        #endregion
        private void AddClipboard()
        {
            ClipboardItem newClip = new ClipboardItem();
            Clipboards.Add(newClip);
            SelectedClipboard = newClip;
        }
        public ClipboardWindowViewModel()
        {
            //Clipboards = _jsonParse.ParseClipboardFile(@"C:\ExtendedClipboard\clipboards.txt");
            Clipboards = new ObservableCollection<ClipboardItem>();
            Hotkeys = _jsonParse.ParseHotkeyFile(@"C:\ExtendedClipboard\hotkeys.txt");
        }

        private void CopyFromClipboard(ClipboardItem listItem)
        {
            //clear clipboard item description, in case we want to display something other than text, preventing overlap
            listItem.Desc = "";

            if (Clipboard.ContainsText())
            {

                var clipText = Clipboard.GetText();

                if (!clipText.Equals(listItem.ClipboardData.TextData))
                {
                    

                    listItem.Desc = clipText;
                    listItem.ClipboardData.TextData = clipText;
                    listItem.ClipboardData.CurrentType = ClipboardData.ClipboardDataTypes.UnicodeText;
                }
            }
            else if (Clipboard.ContainsImage())
            {

                var clipImg = Clipboard.GetImage();

                if(clipImg != listItem.ClipboardData.ImageData)
                {
                    listItem.ClipboardData.ImageData = clipImg;
                    listItem.ClipboardData.CurrentType = ClipboardData.ClipboardDataTypes.Bitmap;
                }
          
            }
            else if (Clipboard.ContainsFileDropList())
            {

                var clipFiles = Clipboard.GetFileDropList();

                if (clipFiles != listItem.ClipboardData.FileData)
                {

                    foreach (var file in clipFiles)
                    {
                        listItem.Desc += file.ToString() + "\n";
                    }

                    listItem.ClipboardData.FileData = clipFiles;
                    listItem.ClipboardData.CurrentType = ClipboardData.ClipboardDataTypes.FileDrop;
                }
            }


            //string userData = Clipboard.GetText(TextDataFormat.UnicodeText);

            //if (!String.IsNullOrWhiteSpace(userData) && !userData.Equals(listItem.ClipboardData))
            //{
            //    listItem.setData(userData);

            //    if(Clipboards != null)
            //    {
            //        SaveClipboardsToJson();
            //    }
            //}    
        }

        private void ClearClipboards()
        {
            Clipboards.Clear();
            SaveClipboardsToJson();
        }

        private void RetrieveClipboard(ClipboardItem listItem)
        {

            (object? clipboardData, string? format) = listItem.ClipboardData.GetData();

            if (clipboardData != null)
            {

                if (format == DataFormats.UnicodeText)
                    Clipboard.SetText(clipboardData.ToString());

                else if (format == DataFormats.Bitmap)
                    Clipboard.SetImage((BitmapSource)clipboardData);

                else if (format == DataFormats.FileDrop)
                    Clipboard.SetFileDropList((StringCollection)clipboardData);

            }

        }

        private void DeleteClipboard(ClipboardItem listItem)
        {

            var clipboardID = Clipboards[Clipboards.IndexOf(listItem)].ClipboardID;
            foreach (ClipboardItem clip in Clipboards)
            {
                if (clipboardID == clip.ClipboardID)
                {
                    clip.ClipboardData.ImageData = null;
                    Clipboards.Remove(clip);
                    SaveClipboardsToJson();
                    return;
                }
            }
        }
        public void SaveClipboardsToJson()
        {
            /* _serializeClipboard.TargetList.Clear();

             foreach (var clipboard in Clipboards)
             {
                 if (!String.IsNullOrWhiteSpace(clipboard.ClipboardData))
                 _serializeClipboard.TargetList.Add(clipboard);
             }

         _serializeClipboard.SerializeData(@"C:\ExtendedClipboard\", "clipboards.txt");*/

        }

        public void SaveHotkeysToJson()
        {

            _serializeHotkey.TargetList.Clear();

            foreach (var hotkey in Hotkeys)
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

