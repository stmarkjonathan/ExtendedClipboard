using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Win32.Input;
using ExtendedClipboardAvalonia.Models;
using ExtendedClipboardAvalonia.ViewModels;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using Tmds.DBus.Protocol;

namespace ExtendedClipboardAvalonia.Views;

public partial class ClipboardWindow : Window
{

    private HotkeyController _hotkeyController;

    public ClipboardWindow()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
        {
            InitWndProc();

            if(TryGetPlatformHandle() != null)
            {
                _hotkeyController = new HotkeyController(TryGetPlatformHandle().Handle);
            }
            else
            {
                Debug.Write("Could not receive window handle");
            }           
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

    private void ClipboardWindow_Closed(object? sender, System.EventArgs e)
    {
        SaveClipboards();
    }

    protected void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            SaveClipboards();
        }
    }

    private void InitWndProc()
    {
        Win32Properties.AddWndProcHookCallback(this, WndProcHook);
    }

    private nint WndProcHook(nint hWnd, uint message, nint wParam, nint lParam, ref bool handled)
    {
        if (message == 0x312) // WM_HOTKEY
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

    private void SaveClipboards()
    {
        var ViewModel = DataContext as ClipboardWindowViewModel;

        if (ViewModel != null)
        {
            ViewModel.SaveClipboardsToJson();
        }
    }

    private void SettingsButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _hotkeyController.RebindHotkey(0, (int)KeyModifiers.Control, Key.F8); //hardcoded rebinding of hotkey
    }
}