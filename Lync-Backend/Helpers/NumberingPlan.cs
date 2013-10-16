using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lync_Backend.Helpers
{
    class NumberingPlan
    {
        public long Dialing_prefix { get; set; }
        public string CountryName { get; set; }
        public string Two_Digits_country_code { get; set; }
        public string Three_Digits_Country_Code { get; set; }
        public string City { get; set; }
        public string Provider { get; set; }
        public string Type_Of_Service { get; set; }
        
        
    }
}
