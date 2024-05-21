using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Win32.Input;
using ExtendedClipboardAvalonia.ViewModels;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using Tmds.DBus.Protocol;

namespace ExtendedClipboardAvalonia.Views;

public partial class ClipboardWindow : Window
{

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

    public ClipboardWindow()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
        {
            InitWndProc();
        }

       
    }
    private void ClipboardDisplay_Opened(object? sender, System.EventArgs e)
    {
        var screenSize = Screens.Primary.WorkingArea.Size;
        var windowSize = PixelSize.FromSize(ClientSize, Screens.Primary.Scaling);

        Position = new PixelPoint(
            screenSize.Width - windowSize.Width,
        screenSize.Height - windowSize.Height);  
    }

    private void Window_Closed(object? sender, System.EventArgs e)
    {
        var ViewModel = DataContext as ClipboardWindowViewModel;

        ViewModel.SaveClipboardsToJson();
    }

    protected void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {

            var ViewModel = DataContext as ClipboardWindowViewModel;

            ViewModel.SaveClipboardsToJson();
        }
    }

    private void InitWndProc()
    {
        Win32Properties.AddWndProcHookCallback(this, WndProcHook);

        RegisterHotKey(this.TryGetPlatformHandle().Handle, 1, (int)KeyModifiers.Control, KeyInterop.VirtualKeyFromKey(Key.F9));
    }

    private nint WndProcHook(nint hWnd, uint message, nint wParam, nint lParam, ref bool handled)
    {


        if (message == 0x312) // WMHOTKEY
        {
            if (IsVisible)
            {
                Hide();
            }
            else
            {
                Show();
            }

            handled = true;
        }

        return IntPtr.Zero;
    }


}