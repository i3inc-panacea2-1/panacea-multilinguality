using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Panacea.Multilinguality
{
    internal class BindingProxy : INotifyPropertyChanged
    {
        public BindingProxy()
        {
            WeakEventManager<LanguageContext, EventArgs>.AddHandler(LanguageContext.Instance, nameof(LanguageContext.CultureChanged),
                LanguageChanged);
        }

        private void LanguageChanged(object sender, EventArgs e)
        {
            ProxyProperty = !ProxyProperty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        bool _languageChanged;
        public bool ProxyProperty
        {
            get => _languageChanged;
            set
            {
                _languageChanged = value;
                OnPropChanged();
            }
        }
    }
}
