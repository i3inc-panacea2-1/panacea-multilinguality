using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Panacea.Multilinguality
{
    public class Translator
    {
        private readonly string _namespac;

        public Translator(string namespac)
        {
            _namespac = namespac;
        }

        public string Translate(string text)
        {
            return LanguageContext.Instance.Dictionary.Translate(text, _namespac);
        }

        public string Translate(string text, params object[] args)
        {
            return string.Format(LanguageContext.Instance.Dictionary.Translate(text, _namespac), args);
        }

        public void CreateBinding(DependencyProperty property, FrameworkElement element, string text,
            params object[] args)
        {
            BindingOperations.ClearBinding(element, property);
            var myBinding = new Binding(nameof(TranslatableObject.Text));
            var obj = new TranslatableObject(text, this, args);
            myBinding.Source = obj;
            element.SetBinding(property, myBinding);
        }

        public void CreateBindingJson(DependencyProperty property, FrameworkElement element, dynamic text)
        {
            BindingOperations.ClearBinding(element, property);
            var myBinding = new Binding(nameof(TranslatableObject.Text));
            var obj = new TranslatableObject(text, this);
            myBinding.Source = obj;
            element.SetBinding(property, myBinding);
        }
    }

   
}
