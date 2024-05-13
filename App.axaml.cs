using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ExtendedClipboardAvalonia.ViewModels;
using ExtendedClipboardAvalonia.Views;
using System;
using System.IO;
using System.Threading;

namespace ExtendedClipboardAvalonia;

public partial class App : Application
{

    static FileStream _lockFile;

    public static TopLevel TopLevel { get; private set; }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new ClipboardWindow
            {
                DataContext = new ClipboardWindowViewModel(),
            };

            TopLevel = TopLevel.GetTopLevel(desktop.MainWindow);

        }

        LockInstance();

        base.OnFrameworkInitializationCompleted();
    }

    private void LockInstance()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Directory.CreateDirectory(dir);
            try
            {
                _lockFile = File.Open(Path.Combine(dir, ".lock"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                _lockFile.Lock(0, 0);
                return;
            }
            catch
            {
                desktop.Shutdown();
            }
        }
        
    }
}