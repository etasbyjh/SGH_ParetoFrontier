using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHParetoFrontier
{
    public class sParetoDataItem
    {
        public string name { get; set; }
        public bool IsNegationRequired { get; set; } = false;
        public double value { get; set; }
        public double value_Normalized { get; set; }

        public double getValueForCompare(bool useNormalized = false)
        {
            double val = this.value;
            if (useNormalized)
            {
                val = this.value_Normalized;
            }

            if (this.IsNegationRequired)
            {
                val *= -1;
            }

            return val;
        }
    }
}
