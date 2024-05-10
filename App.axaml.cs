using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ExtendedClipboardAvalonia.ViewModels;
using ExtendedClipboardAvalonia.Views;

namespace ExtendedClipboardAvalonia;

public partial class App : Application
{
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


        base.OnFrameworkInitializationCompleted();
    }
}