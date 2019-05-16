using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Panacea.Multilinguality
{
    public class TranslatableObject : INotifyPropertyChanged
    {
        private string _text;
        private readonly Translator _translator;
        private readonly object[] _args;

        public int MaxChars { get; set; } = int.MaxValue;

        public TranslatableObject()
        {
        }

        public TranslatableObject(string realText)
        {
            RealText = realText;
            Text = realText;
        }

        public TranslatableObject(string text, Translator translator, params object[] args)
        {
            _translator = translator;
            _args = args;
            Text = string.Format(translator.Translate(text), args);
            WeakEventManager<LanguageContext, PropertyChangedEventArgs>
                .AddHandler(LanguageContext.Instance,
                            nameof(LanguageContext.CultureChanged),
                            OnLanguageContextPropertyChanged);

        }

        private void OnLanguageContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Culture")
            {
                Text = string.Format(_translator?.Translate(_text), _args);
            }
        }

        public TranslatableObject(dynamic obj, params string[] fields)
        {
            dynamic obj2 = obj;
            foreach (string s in fields)
            {
                obj2 = obj2[s];
            }
            try
            {
                dynamic trans = null;
                try
                {
                    trans = obj.trans;
                    foreach (string s in fields)
                    {
                        if (trans != null)
                            trans = trans[s];
                    }
                }
                catch
                {
                }
                if (trans != null)
                {
                    WeakEventManager<LanguageContext, EventArgs>
                        .AddHandler(LanguageContext.Instance,
                           nameof(LanguageContext.CultureChanged),
                           (oo, ee) =>
                           {
                               try
                               {
                                   Text = trans[LanguageContext.Instance.Culture.Name].Value;
                               }
                               catch
                               {
                                   try
                                   {
                                       Text = obj2.Value;
                                   }
                                   catch
                                   {
                                   }
                               }
                           });
                    try
                    {
                        Text = trans[LanguageContext.Instance.Culture.Name].Value;
                    }
                    catch
                    {
                        Text = obj2.Value;
                    }
                }
                else
                {
                    Text = obj2.Value;
                }
                RealText = obj2.Value;
            }
            catch
            {
            }
        }

        public string RealText { get; set; }

        public string Text
        {
            get
            {
                if (MaxChars != int.MaxValue)
                    return SmartTextCrop(_text, MaxChars);
                return _text;
            }
            set
            {
                if (value != _text)
                {
                    _text = value;
                    OnPropertyChanged("Text");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string SmartTextCrop(string text, int maxChars)
        {
            if (maxChars < text.Length)
            {
                text = text.Substring(0, maxChars);
                int Space = text.LastIndexOf(" ");
                int LastDot = text.LastIndexOf(".");
                if (Space > LastDot)
                {
                    return text.Substring(0, Space) + "...";
                }
                else
                {
                    return text.Substring(0, LastDot);
                }
            }
            return text;
        }
    }
}
