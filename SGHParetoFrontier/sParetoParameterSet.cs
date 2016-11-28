using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHParetoFrontier
{
    public class sParetoParameterSet
    {
        public string paramName { get; set; }
        public List<double> data { get; set; }

        public sParetoParameterSet()
        {
            this.paramName = "";
            this.data = new List<double>();
        }

        public sParetoParameterSet(string name, List<double> dataIn)
        {
            this.paramName = name;
            this.data = dataIn.ToList();
        }

    }
}
