using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Lync_Billing.ConfigurationSections
{

    public class BillableTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public int Value
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


        public List<int> BillableTypesList
        {
            get
            {
                List<int> billableTypesList = new List<int>() ;

                foreach (BillableTypeElement el in BillableTypes)
                {
                    billableTypesList.Add(el.Value);
                }

                return billableTypesList;
            }
        }

        //Easier to compare against agnostic keyValuePair with typeof functions
        public Array BillableTypesArrayList
        {
            get
            {
                List<int> billableTypesList = new List<int>();

                foreach (BillableTypeElement el in BillableTypes)
                {
                    billableTypesList.Add(el.Value);
                }

                return billableTypesList.ToArray();
            }
        }
        
    }
}
