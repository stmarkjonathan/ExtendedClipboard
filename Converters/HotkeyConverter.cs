using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace ExtendedClipboard.Converters
{
    public class HotkeyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int virtualKey = KeyInterop.VirtualKeyFromKey((Key)values[1]);
            Keys readableKey = (Keys)virtualKey;
            KeysConverter kc = new KeysConverter();

            return string.Format("{0} + {1}", Enum.GetName(typeof(ModifierKeys), values[0]), kc.ConvertToString(readableKey));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
