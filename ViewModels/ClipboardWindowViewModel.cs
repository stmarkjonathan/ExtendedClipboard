using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Remote.Protocol.Viewport;
using ExtendedClipboardAvalonia.Models;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace ExtendedClipboardAvalonia.ViewModels;

public class ClipboardWindowViewModel : ViewModelBase
{

    private ObservableCollection<ClipboardItem> _clipboards;
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

    #region Commands
    public ReactiveCommand<Unit, Unit> AddCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearCommand { get; }

    public ReactiveCommand<ClipboardItem, Unit> SaveCommand { get; }
    public ReactiveCommand<ClipboardItem, Unit> RetrieveCommand { get; }

    public ReactiveCommand<ClipboardItem, Unit> DeleteCommand { get; }

    #endregion

    public ClipboardWindowViewModel()
    {
        _clipboards = new ObservableCollection<ClipboardItem>();
        AddCommand = ReactiveCommand.Create(AddClipboard);
        ClearCommand = ReactiveCommand.Create(ClearClipboards);
        SaveCommand = ReactiveCommand.CreateFromTask<ClipboardItem>(CopyFromSystemClipboard);
        RetrieveCommand = ReactiveCommand.CreateFromTask<ClipboardItem>(RetrieveFromClipboardItem);
        DeleteCommand = ReactiveCommand.Create<ClipboardItem>(DeleteClipboardItem);
    }

    private void AddClipboard()
    {
        ClipboardItem newClip = new ClipboardItem();
        Clipboards.Add(newClip);
        SelectedClipboard = newClip;
    }

    private async Task CopyFromSystemClipboard(ClipboardItem listItem)
    {

        Debug.Write("COPYING FROM CLIPBOARD...\n");

        //clear clipboard item description, in case we want to display something other than text, preventing overlap
        listItem.Desc = "";

        IClipboard clipboard = App.TopLevel.Clipboard;

        if (clipboard != null)
        {
            string[] formats = await App.TopLevel.Clipboard.GetFormatsAsync();


            if (formats.Contains("Text"))
            {
                var clipText = await clipboard.GetTextAsync();

                if (!clipText.Equals(listItem.ClipboardData.TextData))
                {
                    listItem.Desc = clipText;
                    listItem.ClipboardData.TextData = clipText;
                    listItem.ClipboardData.CurrentType = ClipboardData.ClipboardDataTypes.Text;
                }
            }


        }

        //TODO implement file copying

        //else if (Clipboard.ContainsFileDropList())
        //{

        //    var clipFiles = Clipboard.GetFileDropList();

        //    if (clipFiles != listItem.ClipboardData.FileData)
        //    {

        //        foreach (var file in clipFiles)
        //        {
        //            listItem.Desc += file.ToString() + "\n";
        //        }

        //        listItem.ClipboardData.FileData = clipFiles;
        //        listItem.ClipboardData.CurrentType = ClipboardData.ClipboardDataTypes.FileDrop;
        //    }
        //}

    }

    private async Task RetrieveFromClipboardItem(ClipboardItem listItem)
    {
        IClipboard clipboard = App.TopLevel.Clipboard;

        if (clipboard != null)
        {

            (object? clipboardData, string? format) = listItem.ClipboardData.GetData();

            if (clipboardData != null)
            {

                if (format == DataFormats.Text)
                    await clipboard.SetTextAsync(clipboardData.ToString());

                else if (format == DataFormats.Files)
                {
                    //TODO add file support
                }
                    

            }
        }

    }

    private void DeleteClipboardItem(ClipboardItem listItem)
    {

        var clipboardID = Clipboards[Clipboards.IndexOf(listItem)].ClipboardID;
        foreach (ClipboardItem clip in Clipboards)
        {
            if (clipboardID == clip.ClipboardID)
            {           
                Clipboards.Remove(clip);
               //SaveClipboardsToJson();
                return;
            }
        }
    }

    private void ClearClipboards()
    {
        Clipboards.Clear();
        //SaveClipboardsToJson();
    }


}




