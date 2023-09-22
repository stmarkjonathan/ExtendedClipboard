using ExtendedClipboard.ViewModels;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExtendedClipboard.Views
{
    /// <summary>
    /// Interaction logic for ClearConfirmationWindow.xaml
    /// </summary>
    public partial class ClearConfirmationWindow : Window
    {
        public ClearConfirmationWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void ConfirmationWindow_Click(object sender, RoutedEventArgs e)
        {
            Owner.Effect = null;
            this.Close();

        }
    }
}
