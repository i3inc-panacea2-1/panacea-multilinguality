using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Panacea.Multilinguality
{
    [MarkupExtensionReturnType(typeof(string))]
    [ContentProperty("Parameters")]
    public class Translate : MarkupExtension
    {
        #region Fields

        private readonly Collection<BindingBase> _parameters = new Collection<BindingBase>();
        private object _default;
        private object _namespace;
        private DependencyProperty _property;
        private DependencyObject _target;
        private string _uid;

        #endregion

        #region Initialization

        public Translate()
        {
        }

        public Translate(object defaultValue, object namesp)
        {
            _default = defaultValue;
            _namespace = namesp;
        }

        Binding _binding;

        public Translate(Binding defaultValue, object namesp)
        {
            _binding = defaultValue;
            _namespace = namesp;
        }

        #endregion

        #region Properties

        public object Default
        {
            get { return _default; }
            set { _default = value; }
        }

        public object Namespace
        {
            get { return _namespace; }
            set { _namespace = value; }
        }

        public string Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }

        public Collection<BindingBase> Parameters
        {
            get { return _parameters; }
        }

        #region UidProperty DProperty

        public static readonly DependencyProperty UidProperty =
            DependencyProperty.RegisterAttached("Uid", typeof(string), typeof(Translate),
                new UIPropertyMetadata(string.Empty));

        public static string GetUid(DependencyObject obj)
        {
            return (string)obj.GetValue(UidProperty);
        }

        public static void SetUid(DependencyObject obj, string value)
        {
            obj.SetValue(UidProperty, value);
        }

        #endregion

        #endregion

        #region Overrides

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (service == null)
            {
                return this;
            }

            var property = service.TargetProperty as DependencyProperty;
            var target = service.TargetObject as DependencyObject;
            if (property == null || target == null)
            {
                return this;
            }

            _target = target;
            _property = property;

            return BindDictionary(serviceProvider);
        }

        #endregion

        #region Privates

        private object BindDictionary(IServiceProvider serviceProvider)
        {
            string uid = _uid ?? GetUid(_target);
            string vid = _property.Name;

            var binding = new Binding("Dictionary");
            binding.Source = LanguageContext.Instance;
            binding.Mode = BindingMode.TwoWay;
            var converter = new LanguageConverter(uid, vid, _default, _namespace);
            if (_parameters.Count == 0)
            {
                if (_binding == null)
                {
                    binding.Converter = converter;
                    object value = binding.ProvideValue(serviceProvider);
                    return value;
                }
                else
                {
                    if (_binding.Converter == null) _binding.Converter = converter;
                    object value = _binding.ProvideValue(serviceProvider);
                    return value;
                }
            }

            else
            {
                var multiBinding = new MultiBinding();
                multiBinding.Mode = BindingMode.TwoWay;
                multiBinding.Converter = converter;
                multiBinding.Bindings.Add(binding);
                if (string.IsNullOrEmpty(uid))
                {
                    var uidBinding = _parameters[0] as Binding;
                    if (uidBinding == null)
                    {
                        throw new ArgumentException("Uid Binding parameter must be the first, and of type Binding");
                    }
                }
                foreach (Binding parameter in _parameters)
                {
                    multiBinding.Bindings.Add(parameter);
                }
                object value = multiBinding.ProvideValue(serviceProvider);
                return value;
            }
        }

        #endregion
    }
}
