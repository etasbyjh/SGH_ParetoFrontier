using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHParetoFrontier
{
    public class sParetoDataSet
    {
        public int ID { get; set; }
        public bool IsParetoFront { get; set; }
        public Dictionary<string, object> parameters { get; set; }
        public sParetoDataItem dataX { get; set; }
        public sParetoDataItem dataY { get; set; }
        public sParetoDataItem dataZ { get; set; }
        public double dataDistance { get; set; }
        public double dataDistanceToNormlizedIdealPoint { get; set; }

        public sParetoDataSet(int id)
        {
            this.ID = id;
            this.IsParetoFront = false;
            this.parameters = new Dictionary<string, object>();
            this.dataX = new sParetoDataItem();
            this.dataY = new sParetoDataItem();
            this.dataZ = new sParetoDataItem();
        }

        public void AddParameters(string paramKey, object paramVal)
        {
            object PrevVal;
            if (this.parameters.TryGetValue(paramKey, out PrevVal))
            {
                this.parameters[paramKey] = paramVal;
            }
            else
            {
                this.parameters.Add(paramKey, paramVal);
            }
        }

        public double DistanceTo(sParetoDataSet other, bool useNormalized = false)
        {
            double deltaX = other.dataX.value - this.dataX.value;
            double deltaY = other.dataY.value - this.dataY.value;
            double deltaZ = other.dataZ.value - this.dataZ.value;

            if (useNormalized)
            {
                deltaX  = other.dataX.value_Normalized - this.dataX.value_Normalized;
                deltaY  = other.dataY.value_Normalized - this.dataY.value_Normalized;
                deltaZ  = other.dataZ.value_Normalized - this.dataZ.value_Normalized;
            }

            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }

        public double DistanceTo(double x, double y, double z, bool useNormalized = false)
        {
            double deltaX = x - this.dataX.value;
            double deltaY = y - this.dataY.value;
            double deltaZ = z - this.dataZ.value;

            if (useNormalized)
            {
                deltaX = x - this.dataX.value_Normalized;
                deltaY = y - this.dataY.value_Normalized;
                deltaZ = z - this.dataZ.value_Normalized;
            }

            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }

        public sParetoDataSet getClosestDataSet(List<sParetoDataSet> data ,bool useNormalized = false)
        {
            sParetoDataSet clData = data[0];
            double minDis = double.MaxValue;
            foreach(sParetoDataSet d in data)
            {
                double dis = this.DistanceTo(d);
                if(dis < minDis)
                {
                    minDis = dis;
                    clData = d;
                }
            }
            this.dataDistance = minDis;
            return clData;
        }

        public bool IsDominatesOver(sParetoDataSet other, bool useNormalized = false)
        {
            bool dom = false;

            double xVal = this.dataX.getValueForCompare(useNormalized);
            double yVal = this.dataY.getValueForCompare(useNormalized);
            double zVal = this.dataZ.getValueForCompare(useNormalized);

            double xValOther = other.dataX.getValueForCompare(useNormalized);
            double yValOther = other.dataY.getValueForCompare(useNormalized);
            double zValOther = other.dataZ.getValueForCompare(useNormalized);

            if (this.dataZ.name != null && this.dataZ.name.Length > 0)
            {

                if (xVal >= xValOther && yVal >= yValOther && zVal >= zValOther)
                {
                    dom = true;
                }

            }
            else
            {
                if (xVal >= xValOther && yVal >= yValOther)
                {
                    dom = true;
                }

            }
            return dom;
        }
    }
    
}
