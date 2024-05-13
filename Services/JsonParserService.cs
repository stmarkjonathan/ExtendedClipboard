using ExtendedClipboardAvalonia.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExtendedClipboardAvalonia.Services
{
    public class JsonParserService
    {

        private static JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public static ObservableCollection<ClipboardItem> ParseClipboardFile(string clipboardFile)
        {
            List<ClipboardItem> clipboards = new List<ClipboardItem>();
            ObservableCollection<ClipboardItem> returnedList = new ObservableCollection<ClipboardItem>();


            if (File.Exists(clipboardFile))
            {
                File.SetAttributes(clipboardFile, FileAttributes.Normal);

                string jsonString = File.ReadAllText(clipboardFile);

                try
                {
                    clipboards = JsonSerializer.Deserialize<List<ClipboardItem>>(jsonString, _options);

                    if (clipboards != null)
                    {
                        foreach (var clipboard in clipboards)
                        {
                            returnedList.Add(clipboard);
                        }
                    }
                }
                catch (Exception)
                {
                    File.WriteAllText(clipboardFile, "[]");
                }

                File.SetAttributes(clipboardFile, FileAttributes.ReadOnly);

            }

            return returnedList;
        }

        //public List<Hotkey> ParseHotkeyFile(string filePath)
        //{
        //    List<Hotkey> hotkeyList;

        //    List<Hotkey> returnedList = new List<Hotkey>();

        //    if (File.Exists(filePath))
        //    {
        //        File.SetAttributes(filePath, FileAttributes.Normal);

        //        string jsonString = File.ReadAllText(filePath);

        //        try
        //        {
        //            hotkeyList = JsonSerializer.Deserialize<List<Hotkey>>(jsonString, _options);

        //            if (hotkeyList != null)
        //            {
        //                foreach (var hotkey in hotkeyList)
        //                {
        //                    returnedList.Add(new Hotkey(hotkey.Modifier, hotkey.PressedKey, hotkey.Option));
        //                }
        //            }

        //            if (!returnedList.Any())
        //            {
        //                returnedList = InstantiateHotkeys();
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            returnedList = InstantiateHotkeys();
        //        }

        //        File.SetAttributes(filePath, FileAttributes.ReadOnly);

        //    }
        //    else
        //    {
        //        returnedList = InstantiateHotkeys();
        //    }
        //    return returnedList;
        //}

        //private List<Hotkey> InstantiateHotkeys()
        //{
        //    return new List<Hotkey>
        //            {
        //                new Hotkey((int)ModifierKeys.Control, Key.F9, 0),
        //            };
        //}

    }
}