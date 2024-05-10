using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedClipboardAvalonia.Models
{
    public class ClipboardData
    {
        public enum ClipboardDataTypes
        {
            Text,
            Files,
        }

        private ClipboardDataTypes? _currentType;

        public ClipboardDataTypes? CurrentType
        {
            get { return _currentType; }
            set { _currentType = value; }

        }

        private string? _textData;

        public string? TextData
        {
            get { return _textData; }
            set { _textData = value; }

        }

        //private StringCollection? _fileData;

        //public StringCollection? FileData
        //{
        //    get { return _fileData; }
        //    set { _fileData = value; }
        //}

        public (object? data, string? format) GetData()
        {

            switch (CurrentType)
            {

                case ClipboardDataTypes.Text:

                    return (_textData, ClipboardDataTypes.Text.ToString());

                //case ClipboardDataTypes.Files:

                //    return (_fileData, ClipboardDataTypes.Files.ToString());

                default:

                    return (null, null);

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
