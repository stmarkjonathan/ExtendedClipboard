using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;
using ExtendedClipboardAvalonia.ViewModels;
using System.Diagnostics;

namespace ExtendedClipboardAvalonia.Views;

public partial class ClipboardWindow : Window
{
    public ClipboardWindow()
    {
        InitializeComponent();
    }
    private void ClipboardDisplay_Opened(object? sender, System.EventArgs e)
    {
        var screenSize = Screens.Primary.WorkingArea.Size;
        var windowSize = PixelSize.FromSize(ClientSize, Screens.Primary.Scaling);

        Position = new PixelPoint(
            screenSize.Width - windowSize.Width,
            screenSize.Height - windowSize.Height);
    }

    protected void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var ViewModel = DataContext as ClipboardWindowViewModel;

            ViewModel.SaveClipboardsToJson();
        }
    }

}