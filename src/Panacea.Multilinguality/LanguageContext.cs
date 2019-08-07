using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Panacea.Multilinguality
{
    public sealed class LanguageContext
    {
        #region Fields

        public static readonly LanguageContext Instance = new LanguageContext();

        private CultureInfo _cultureInfo;
        private LanguageDictionary _dictionary = LanguageDictionary.Instance;

        #endregion

        #region Properties

        public event EventHandler CultureChanged;
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
                if (_cultureInfo != null && value.Name == _cultureInfo.Name)
                {
                    return;
                }
                
                _cultureInfo = value;
                Thread.CurrentThread.CurrentUICulture = _cultureInfo;
                CultureChanged?.Invoke(this, EventArgs.Empty);
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
                }
            }
        }

        #endregion

        #region Initialization

        private LanguageContext()
        {
        }

        #endregion

        public event EventHandler LanguageChanged
        {
            add
            {

            }
            remove
            {

            }
        }
    }
}
