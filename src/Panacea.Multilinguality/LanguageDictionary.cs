using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panacea.Multilinguality
{
    public sealed class LanguageDictionary
    {
        private LanguageDictionary()
        {

        }
        private static LanguageDictionary _instance;

        public static LanguageDictionary Instance
        {
            get { return _instance ?? (_instance = new LanguageDictionary()); }
        }
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> translations;

        public string Translate(string s, string namesp)
        {
            if (namesp == null) return s;
            try
            {
                if (translations != null)
                {
                    if (translations.ContainsKey(LanguageContext.Instance.Culture.Name))
                    {
                        var trans = translations[LanguageContext.Instance.Culture.Name];
                        if (trans.ContainsKey(namesp))
                        {
                            var res = translations[LanguageContext.Instance.Culture.Name][namesp];
                            if (res.ContainsKey(s))
                            {

                                return
                                    res[s];
                            }
                            return s;
                        }
                        // patch to search for core
                        else if (trans.ContainsKey("core"))
                        {
                            var res = translations[LanguageContext.Instance.Culture.Name]["core"];
                            if (res.ContainsKey(s))
                            {
                                return
                                    res[s];
                            }
                            return s;
                        }
                    }
                }
            }
            catch
            {
            }
            return s;
        }
    }
}
