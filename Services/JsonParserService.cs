using ExtendedClipboard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ExtendedClipboard.Services
{
    public class JsonParserService
    {

        private JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
        };
        public ObservableCollection<ClipboardClass> ParseJson(ObservableCollection<ClipboardClass> clipList)
        {
            List<ClipboardClass> clipboards = new List<ClipboardClass>();
            string clipboardFile = @"C:\ExtendedClipboard\clipboards.txt";
            string jsonString = File.ReadAllText(clipboardFile);

            try
            {
                clipboards = JsonSerializer.Deserialize<List<ClipboardClass>>(jsonString, _options);
            }
            catch (Exception)
            {
                //error with file occured, clear file
                File.WriteAllText(clipboardFile, "[]");
            }

            
            if (clipboards != null)
            {
                foreach(var clipboard in clipboards)
                {
                    clipList.Add(clipboard);
                }
            }

            return clipList;
            
            
        }

    }
} 