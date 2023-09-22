using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExtendedClipboard.Models
{
    public class ClipboardClass : INotifyPropertyChanged
    {
        static private int _clipboardCount;
        private int _clipboardID;
        private string _name;
        private string _desc;
        private string _ClipboardData;

        public int ClipboardID
        {
            get { return _clipboardID; }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (!String.IsNullOrWhiteSpace(value)){
                    _name = value;
                    OnPropertyChanged("Name");
                }
                

            }
        }
        public string Desc
        {
            get { return _desc; }
            set
            {
                _desc = value;
                OnPropertyChanged("Desc");
            }
        }
        public string ClipboardData
        {
            get { return _ClipboardData; }
            set
            {
                _ClipboardData = value;
                OnPropertyChanged("ClipboardData");
            }
        }

        public ClipboardClass()
        {
            _clipboardID = ++_clipboardCount;
            _name = "New Clipboard";
            _desc = "Click 'Save' to save data from your clipboard";
            _ClipboardData = "";
        }

        public void setData(string clipboardData)
        {
            if (!String.IsNullOrWhiteSpace(clipboardData))
            {
                _ClipboardData = clipboardData;
                Desc = clipboardData.Trim();
            }
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
