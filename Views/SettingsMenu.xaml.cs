using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ExtendedClipboard.ViewModels;

namespace ExtendedClipboard.Views
{
    /// <summary>
    /// Interaction logic for SettingsMenu.xaml
    /// </summary>
    public partial class SettingsMenu : Window
    {
        ClipboardWindowViewModel ViewModel;
        private enum _modifierKeys
        {
            //int values for respective keyboard keys
            LeftShift = 116,
            RightShift = 117,
            LeftCtrl = 118, 
            RightCtrl = 119,
            LeftAlt = 120,
            RightAlt = 121
        }

        public SettingsMenu(ClipboardWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = ViewModel = viewModel;
        }

        private void BindButton_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var targetButton = (ToggleButton)sender;
            string buttonAction = targetButton.Tag.ToString();

            Key shortcutKey = (e.Key == Key.System ? e.SystemKey : e.Key);

            //ignore all modifier keys
            if(Enum.IsDefined(typeof(_modifierKeys), (int)shortcutKey)){
                return;
            }

            if (targetButton.IsChecked == true)
            {
                switch (Keyboard.Modifiers)
                {
                    case ModifierKeys.Shift:
                        {
                            ViewModel.ChangeHotkey((int)ModifierKeys.Shift, shortcutKey, buttonAction);
                            break;
                        }
                    case ModifierKeys.Control:
                        {
                            ViewModel.ChangeHotkey((int)ModifierKeys.Control, shortcutKey, buttonAction);
                            break;
                        }
                    case ModifierKeys.Alt:
                        {
                            ViewModel.ChangeHotkey((int)ModifierKeys.Alt, shortcutKey, buttonAction);
                            break;
                        }

                }
                targetButton.IsChecked = false;
                ViewModel.SaveHotkeysToJson();
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveHotkeysToJson();
            this.Close();
        }
    }
}
