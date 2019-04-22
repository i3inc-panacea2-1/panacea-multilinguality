using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Panacea.Multilinguality
{
    public sealed class LanguageContext : PropertyChangedBase
    {
        #region Fields

        public static readonly LanguageContext Instance = new LanguageContext();

        private CultureInfo _cultureInfo;
        private LanguageDictionary _dictionary;

        #endregion

        #region Properties

        public CultureInfo Culture
        {
            get
            {
                if (_cultureInfo == null)
                {
                    _cultureInfo = CultureInfo.CurrentUICulture;
                }
                return _cultureInfo;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Culture must not be null");
                }
                if (value == _cultureInfo)
                {
                    return;
                }
                if (_cultureInfo != null)
                {
                }
                _cultureInfo = value;

                Thread.CurrentThread.CurrentUICulture = _cultureInfo;
                OnPropertyChanged("Culture");
                OnPropertyChanged("Dictionary");
            }
        }

        public LanguageDictionary Dictionary
        {
            get { return _dictionary; }
            set
            {
                if (value != null && value != _dictionary)
                {
                    _dictionary = value;
                    OnPropertyChanged("Dictionary");
                }
            }
        }

        #endregion

        #region Initialization

        private LanguageContext()
        {
        }

        #endregion

        public event EventHandler LanguageChanged;

    }
}
