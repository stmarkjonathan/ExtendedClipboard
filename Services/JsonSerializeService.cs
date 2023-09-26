using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ExtendedClipboard.Models;
using ExtendedClipboard.ViewModels;

namespace ExtendedClipboard.Services
{
    public class JsonSerializeService
    {

        private List<ClipboardClass> _clipboardList;

        public List<ClipboardClass> ClipboardList
        {
            get 
            {   
                if(_clipboardList == null)
                {
                    _clipboardList = new List<ClipboardClass>();
                }
                return _clipboardList; 
            }
            set
            {
                _clipboardList = value;
            }
        }

        private JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
        public void SerializeData()
        {
            string directory = @"C:\ExtendedClipboard\";
            Directory.CreateDirectory(directory);
            string path = System.IO.Path.Combine(directory, "clipboards.txt");
            var result = JsonSerializer.SerializeToUtf8Bytes(ClipboardList, _options);

            if(File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.WriteAllBytes(path, result);
                File.SetAttributes(path, FileAttributes.ReadOnly);
            }
            else
            {
                File.WriteAllBytes(path, result);
                File.SetAttributes(path, FileAttributes.ReadOnly);
            }


        }

        

    }
}
