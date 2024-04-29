using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ExtendedClipboard.Models
{
    public class ClipboardData : INotifyPropertyChanged
    {

        public enum ClipboardDataTypes
        {
            UnicodeText,
            Bitmap,
            FileDrop,
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
            set {  _textData = value; }

        }

        private StringCollection? _fileData;

        public StringCollection? FileData
        {
            get { return _fileData; }
            set { _fileData = value; }
        }

        private BitmapSource? _imageData;

        public BitmapSource? ImageData
        {
            get { return _imageData; }
            set 
            {
                _imageData = value;
                OnPropertyChanged("ImageData");
            }
        }

        //use method to get data so that we can inform view model of data type
        public (object? data, string? format) GetData()
        {

            switch(CurrentType)
            {

                case ClipboardDataTypes.UnicodeText:

                    return (_textData, ClipboardDataTypes.UnicodeText.ToString());

                case ClipboardDataTypes.Bitmap:

                    return (_imageData, ClipboardDataTypes.Bitmap.ToString());

                case ClipboardDataTypes.FileDrop:

                    return (_fileData, ClipboardDataTypes.FileDrop.ToString());

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
