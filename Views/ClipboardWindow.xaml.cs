using ExtendedClipboard.Models;
using ExtendedClipboard.ViewModels;
using ExtendedClipboard.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        }


        private void clipboardList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            clipboardList.ScrollIntoView(clipboardList.SelectedItem);
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveClipboardsToJson();
            Visibility = Visibility.Hidden;
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearConfirmationWindow newWindow = new ClearConfirmationWindow();
            BlurEffect blur = new BlurEffect();

            newWindow.Owner = this;
            newWindow.DataContext = this.DataContext;

            blur.Radius = 5;
            Effect = blur;

            newWindow.ShowDialog();
        }

        private void ClipboardDisplay_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.SaveClipboardsToJson();
            ViewModel.SaveHotkeysToJson();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void toggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Visibility = Visibility.Hidden;
            }
            else
            {
                Visibility = Visibility.Visible;
            }
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsMenu newWindow = new SettingsMenu(ViewModel);
            newWindow.Owner = this;
            
            newWindow.ShowDialog();

        }

        private void ClipboardDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            Hotkey.InitializeWindow(this);
            DataContext = ViewModel = new ClipboardWindowViewModel();
        }

        private void titleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ViewModel.SaveClipboardsToJson();
            }
        }
    }
}
