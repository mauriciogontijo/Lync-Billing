using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Lync_Backend.Helpers
{

    public class BillableTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey=true, IsRequired=true)]
        public string Name
        {
            get { return this["name"].ToString(); }
        }

        [ConfigurationProperty("value")]
        public int Price
        {
            get { return Convert.ToInt32(this["value"]); }
        }
    }

    public class BillableTypeCollection : ConfigurationElementCollection
    {
         protected override ConfigurationElement CreateNewElement()
        {
        return new BillableTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
        return ((BillableTypeElement)element).Name;
        }
    }

    public class BillableCallTypesSection : ConfigurationSection
    {
        [ConfigurationProperty("BillableTypes")]
        public BillableTypeCollection BillableTypes
        {
            get { return (BillableTypeCollection)this["BillableTypes"]; }
        }
        
    }
}
