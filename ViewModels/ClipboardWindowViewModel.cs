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

namespace ExtendedClipboard.ViewModels
{
    public class ClipboardWindowViewModel : INotifyPropertyChanged
    {
        private Hotkey hotkey;

        private ObservableCollection<ClipboardClass>? _clipboards;
        public ObservableCollection<ClipboardClass> Clipboards
        {
            get
            {
                if (_clipboards == null)
                {
                    _clipboards = new ObservableCollection<ClipboardClass>();
                }
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

        private JsonSerializeService _jsonSerialize = new JsonSerializeService();
        private JsonParserService _jsonParse = new JsonParserService();
        public RelayCommand AddCommand => new RelayCommand(execute => AddClipboard());

        public RelayCommand SaveToJsonCommand => new RelayCommand(execute => SaveToJson());

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
           Clipboards = _jsonParse.ParseJson(Clipboards);
        }

        private void CopyFromClipboard(ClipboardClass listItem)
        {
            string userData = Clipboard.GetText(TextDataFormat.UnicodeText);

            if (!String.IsNullOrWhiteSpace(userData) && !userData.Equals(listItem.ClipboardData))
            {
                listItem.setData(userData);

                if(Clipboards != null)
                {
                    SaveToJson();
                }
            }    
        }

        private void ClearClipboards()
        {
            Clipboards.Clear();
            SaveToJson();
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
                    SaveToJson();
                    return;
                }
            }
        }
        public void SaveToJson()
        {
                _jsonSerialize.ClipboardList.Clear();

                foreach (var clipboard in Clipboards)
                {
                    if (!String.IsNullOrWhiteSpace(clipboard.ClipboardData))
                        _jsonSerialize.ClipboardList.Add(clipboard);
                }

                _jsonSerialize.SerializeData();
            
        }
        

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        

    }

}

