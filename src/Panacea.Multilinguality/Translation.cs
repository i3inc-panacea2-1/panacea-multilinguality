using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Panacea.Multilinguality
{
    [DataContract]
    public class Translation
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "trans")]
        public string Trans { get; set; }

    }

}
