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
    public class JsonSerializeService<T>
    {

        private List<T> _targetList;

        public List<T> TargetList
        {
            get
            {
                if (_targetList == null)
                {
                    _targetList = new List<T>();
                }
                return _targetList;
            }
            set
            {
                _targetList = value;
            }
        }

        private JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
        public void SerializeData(string directory, string filePath)
        {
            Directory.CreateDirectory(directory);
            string path = System.IO.Path.Combine(directory, filePath);
            var result = JsonSerializer.SerializeToUtf8Bytes(_targetList, _options);

            if (File.Exists(path))
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
