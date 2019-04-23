using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Panacea.Multilinguality
{
    public class LanguageConverter : IValueConverter, IMultiValueConverter
    {
        #region Fields

        private readonly object _defaultValue;
        private readonly object _namespace;
        private readonly string _vid;
        private bool _isStaticUid;
        private string _uid;

        #endregion

        #region Initialization

        public LanguageConverter(string uid, string vid, object defaultValue, object namesp)
        {
            _uid = uid;
            _vid = vid;
            _defaultValue = defaultValue;
            _namespace = namesp;
            _isStaticUid = true;
        }

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LanguageDictionary dictionary = ResolveDictionary();
            object translation = dictionary.Translate((string)(_defaultValue == null ? value : _defaultValue), (string)_namespace);
            return translation;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        #endregion

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int parametersCount = _isStaticUid ? values.Length - 1 : values.Length - 2;
                if (string.IsNullOrEmpty(_uid))
                {
                    if (values[1] == null)
                    {
                        throw new ArgumentNullException(
                            "Uid must be provided as the first Binding element, and must not be null");
                    }
                    _isStaticUid = false;
                    _uid = values[1].ToString();
                    --parametersCount;
                }
                LanguageDictionary dictionary = ResolveDictionary();
                object translatedObject = dictionary.Translate((string)_defaultValue, (string)_namespace);
                if (translatedObject != null && parametersCount != 0)
                {
                    var parameters = new object[parametersCount];
                    Array.Copy(values, values.Length - parametersCount, parameters, 0, parameters.Length);
                    try
                    {
                        translatedObject = string.Format(translatedObject.ToString(), parameters);
                    }
                    catch (Exception)
                    {
                        #region Trace

                        Debug.WriteLine(string.Format("LanguageConverter failed to format text {0}", translatedObject));

                        #endregion
                    }
                }
                return translatedObject;
            }
            catch (Exception ex)
            {
                #region Trace

                Debug.WriteLine(string.Format("LanguageConverter failed to convert text: {0}", ex.Message));

                #endregion
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[0];
        }

        #endregion

        #region Privates

        private bool ShouldTranslateText
        {
            get { return string.IsNullOrEmpty(_vid); }
        }

        private static LanguageDictionary ResolveDictionary()
        {
            return LanguageDictionary.Instance;
        }

        #endregion
    }

}
