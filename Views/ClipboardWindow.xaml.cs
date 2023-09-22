using ExtendedClipboard.Models;
using ExtendedClipboard.ViewModels;
using ExtendedClipboard.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Effects;

namespace ExtendedClipboard
{
    /// <summary>
    /// Interaction logic for ClipboardWindow.xaml
    /// </summary>
    public partial class ClipboardWindow : Window
    {

        ClipboardWindowViewModel ViewModel;
        public ClipboardWindow()
        {
            InitializeComponent();
            var screenArea = SystemParameters.WorkArea;
            this.Left = screenArea.Right - this.Width;
            this.Top = screenArea.Bottom - this.Height;
            DataContext = ViewModel = new ClipboardWindowViewModel();
        }

        private void clipboardList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            clipboardList.ScrollIntoView(clipboardList.SelectedItem);
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveToJson();
            WindowState = WindowState.Minimized;
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearConfirmationWindow newWindow = new ClearConfirmationWindow();
            newWindow.Owner = this;
            newWindow.DataContext = this.DataContext;
            BlurEffect blur = new BlurEffect();
            blur.Radius = 5;
            Effect = blur;

            newWindow.ShowDialog();
        }

        private void ClipboardDisplay_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.SaveToJson();
        }

        private void ClipboardDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            Hotkey.InstantiateHotKey(new WindowInteropHelper(this).Handle);
            Hotkey.window = this;
        }
    }
}
