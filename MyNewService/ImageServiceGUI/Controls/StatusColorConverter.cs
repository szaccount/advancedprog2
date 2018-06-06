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
    /// <summary>
    /// calss containing converter from strings to brushe objects
    /// </summary>
    public class StatusColorConverter : IValueConverter
    {
        /// <summary>
        /// a converter for values from string to an object of type Brushe
        /// </summary>
        /// <param name="obj">object to convert</param>
        /// <param name="type">type to convert</param>
        /// <returns>object of type Brush</returns>
        public object Convert(object obj, Type type, object parameter, CultureInfo culture)
        {
            if (type.Name != "Brush")
                throw new Exception("Converting only Brush!");

            if (obj.ToString() == MessageTypeEnum.INFO.ToString())
                return Brushes.LightGreen;
            if ((obj.ToString() == MessageTypeEnum.WARNING.ToString()))
                return Brushes.Yellow;
            if ((obj.ToString() == MessageTypeEnum.FAIL.ToString()))
                return Brushes.Red;

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
