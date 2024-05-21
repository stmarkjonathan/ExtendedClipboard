﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedClipboardAvalonia.Models
{
    public class ClipboardItem : INotifyPropertyChanged
    {

        static private int _clipboardCount;
        private int _clipboardID;
        private string _name;
        private string _desc;
        private ClipboardData _clipboardData = new ClipboardData();

        public int ClipboardID
        {
            get { return _clipboardID; }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
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

        public ClipboardData ClipboardData
        {

            get
            {
                return _clipboardData;
            }
            set
            {
                _clipboardData = value;
            }
        }

        public ClipboardItem()
        {
            _clipboardID = _clipboardCount++;
            _name = "New Clipboard " + _clipboardCount;
            _desc = "Click 'Save' to save data from your clipboard";
            _clipboardData.TextData = "";
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
