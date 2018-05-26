using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI.Controls
{
    public class StatusColorConverter : IValueConverter
    {
        /// <summary>
        /// Function converts the value from string to an object of Brushes. !!!!!!!!!!!!!!!!!!!!!!!!!!! change !!!!!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <param name="targetType">The type to convert.</param>
        /// <param name="parameter">Not critical for now.</param>
        /// <param name="culture">Not critical for now.</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType.Name != "Brush")
                throw new Exception("Converting only Brush!");

            if (value.ToString() == MessageTypeEnum.INFO.ToString())
                return Brushes.LightGreen;
            if ((value.ToString() == MessageTypeEnum.WARNING.ToString()))
                return Brushes.Yellow;
            if ((value.ToString() == MessageTypeEnum.FAIL.ToString()))
                return Brushes.Red;

            return Brushes.Transparent;
        }

        //implemented only to statisfy the interface, not used
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
