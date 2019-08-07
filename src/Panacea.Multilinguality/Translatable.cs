using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Panacea.Multilinguality
{
    [DataContract]
    public abstract class Translatable :INotifyPropertyChanged
    {

        protected Dictionary<string, Dictionary<string, object>> _translations;
        protected Dictionary<string, object> _defaults;

        public event PropertyChangedEventHandler PropertyChanged;

        protected Dictionary<string, object> Defaults
        {
            get
            {
                return _defaults;
            }
            set
            {
                _defaults = value;
            }
        }

        public Translatable()
        {
            Defaults = new Dictionary<string, object>();
            WeakEventManager<LanguageContext, EventArgs>
                .AddHandler(LanguageContext.Instance,
                            nameof(LanguageContext.CultureChanged),
                            OnLanguageChanged);
        }

        protected string GetTranslation([CallerMemberName] string prop = null)
        {
            var pi = GetType().GetProperty(prop);
            if (pi == null) return null;
            var jsonName = prop;
            var dm = pi.GetCustomAttribute<DataMemberAttribute>();
            if (dm != null) jsonName = dm.Name;
            if (Translations != null && Translations.ContainsKey(jsonName) && Translations[jsonName].ContainsKey(LanguageContext.Instance.Culture.Name))
                return Translations[jsonName][LanguageContext.Instance.Culture.Name].ToString();
            if (Defaults.ContainsKey(prop))
                return Defaults[prop].ToString();
            return null;
        }

        protected void SetTranslation(string val, [CallerMemberName] string prop = null)
        {
            if (!Defaults.ContainsKey(prop))
                Defaults[prop] = val;
        }

        protected TranslatableDictionary GetTranslations([CallerMemberName] string prop = null)
        {
            var pi = GetType().GetProperty(prop);
            if (pi == null) return null;
            var jsonName = prop;
            var dm = pi.GetCustomAttribute<DataMemberAttribute>();
            if (dm != null) jsonName = dm.Name;

            var dict = Translations[jsonName];
            if (!Defaults.ContainsKey(prop)) return null;

            foreach (var key in dict.Keys)
            {
                var trans = dict[key] as Dictionary<string, string>;
                if (trans.ContainsKey(LanguageContext.Instance.Culture.Name))
                {
                    (Defaults[prop] as TranslatableDictionary).SetString(key, trans[LanguageContext.Instance.Culture.Name].ToString());
                }
            }
            return Defaults[prop] as TranslatableDictionary;
        }

        protected void SetTranslations(TranslatableDictionary val, [CallerMemberName] string prop = null)
        {
            if (!Defaults.ContainsKey(prop))
                Defaults.Add(prop, val);
        }

        [DataMember(Name = "trans")]
        public Dictionary<string, Dictionary<string, object>> Translations
        {
            get { return _translations; }
            set
            {
                _translations = value;
                OnLanguageChanged(null, null);
            }
        }

        protected void OnLanguageChanged(object sender, EventArgs e)
        {
            Type t = GetType();
            var plist = t.GetProperties().Where(p => p.GetCustomAttributes(typeof(IsTranslatableAttribute)).Any());
            foreach (
                PropertyInfo pi in
                    plist)
            {
                OnPropertyChanged(pi.Name);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
       
    }
}
