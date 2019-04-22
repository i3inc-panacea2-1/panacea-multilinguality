using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panacea.Multilinguality
{
    public class TranslatableDictionary : Dictionary<string, string>, INotifyCollectionChanged
    {
        public TranslatableDictionary()
            : base()
        {
        }

        internal void SetString(string key, string value)
        {
            this[key] = value;
            OnCollectionChanged();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
