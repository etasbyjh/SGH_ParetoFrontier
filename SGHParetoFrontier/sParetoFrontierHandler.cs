using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHParetoFrontier
{
    public class sParetoFrontierHandler
    {
        public List<sParetoDataSet> data { get; set; }

        public sParetoFrontierHandler()
        {
            this.data = new List<sParetoDataSet>();
        }

        public void CalculateParetoFrontiers(double factor)
        {
            List<sParetoDataSet> temp = this.data.ToList();
            List<int> paretoIDs = new List<int>();
            int candidateRowNr = 0;
            while (true)
            {
                sParetoDataSet candidateRow = this.data[candidateRowNr];
                this.data.RemoveAt(candidateRowNr);

                int rowNr = 0;
                bool nonDominated = true;

                while (this.data.Count != 0 && rowNr < this.data.Count)
                {
                    sParetoDataSet row = this.data[rowNr];

                    if (candidateRow.IsDominatesOver(row, false))
                    {
                        this.data.RemoveAt(rowNr);
                    }
                    else if (row.IsDominatesOver(candidateRow, false))
                    {
                        nonDominated = false;
                        rowNr += 1;
                    }
                    else
                    {
                        rowNr += 1;
                    }
                }

                if (nonDominated)
                {
                    paretoIDs.Add(candidateRow.ID);
                }

                if (this.data.Count == 0)
                {
                    break;
                }
            }

            foreach(sParetoDataSet ps in temp)
            {
                foreach(int idd in paretoIDs)
                {
                    if(ps.ID == idd)
                    {
                        ps.IsParetoFront = true;
                        break;
                    }
                }
            }

            this.data.Clear();
            this.data = temp.ToList();

            this.NormalizeData(factor);
        }

        public void AwareDistances()
        {
            List<sParetoDataSet> pareto = new List<sParetoDataSet>();
            List<sParetoDataSet> non = new List<sParetoDataSet>();
            foreach (sParetoDataSet d in this.data)
            {
                if (d.IsParetoFront)
                {
                    d.dataDistance = 0.0;
                    pareto.Add(d);
                }
                else
                {
                    non.Add(d);
                }
            }

            foreach(sParetoDataSet d in non)
            {
                d.getClosestDataSet(pareto);
            }
        }

        public void AwareDistancesFromIdealPoint(double idealX, double idealY, double idealZ, bool useNormalized = true)
        {
            foreach(sParetoDataSet d in this.data)
            {
                d.dataDistanceToNormlizedIdealPoint = d.DistanceTo(idealX, idealY, idealZ, useNormalized);
            }
        }

        public void NormalizeData(double factor)
        {
            double xMin = double.MaxValue;
            double xMax = double.MinValue;
            double yMin = double.MaxValue;
            double yMax = double.MinValue;
            double zMin = double.MaxValue;
            double zMax = double.MinValue;

            foreach (sParetoDataSet d in this.data)
            {
                if (d.dataX.value < xMin)
                {
                    xMin = d.dataX.value;
                }
                if (d.dataX.value > xMax)
                {
                    xMax = d.dataX.value;
                }

                if (d.dataY.value < yMin)
                {
                    yMin = d.dataY.value;
                }
                if (d.dataY.value > yMax)
                {
                    yMax = d.dataY.value;
                }

                if (d.dataZ.value < zMin)
                {
                    zMin = d.dataZ.value;
                }
                if (d.dataZ.value > zMax)
                {
                    zMax = d.dataZ.value;
                }
            }

            foreach (sParetoDataSet d in this.data)
            {
                d.dataX.value_Normalized = ((d.dataX.value - xMin) * (factor)) / (Math.Abs(xMax - xMin));
                d.dataY.value_Normalized = ((d.dataY.value - yMin) * (factor)) / (Math.Abs(yMax - yMin));

                if (Math.Abs(zMax - zMin) > 0.0)
                {
                    d.dataZ.value_Normalized = ((d.dataZ.value - zMin) * (factor)) / (Math.Abs(zMax - zMin));
                }
                else
                {
                    d.dataZ.value_Normalized = 0.0;
                }
            }
        }

        public List<sParetoDataSet> GetDataSet(bool IsParetoOnly)
        {
            List<sParetoDataSet> dataOut = new List<sParetoDataSet>();

            foreach(sParetoDataSet d in this.data)
            {
                if (IsParetoOnly)
                {
                    if (d.IsParetoFront)
                    {
                        dataOut.Add(d);
                    }
                }
                else
                {
                    dataOut.Add(d);
                }
            }
            return dataOut;
        }
        

        public static List<Dictionary<string, List<double>>> GetParameterRanges(List<sParetoDataSet> dataSets)
        {
            int paramCount = dataSets[0].parameters.Count;
            List<Dictionary<string, List<double>>> paramFullData = new List<Dictionary<string, List<double>>>();

            for (int i = 0; i < paramCount; ++i)
            {
                Dictionary<string, List<double>> paramData = new Dictionary<string, List<double>>();
                string paramName = "";
                List<double> paramVals = new List<double>();
                foreach (sParetoDataSet d in dataSets)
                {
                    paramName = d.parameters.Keys.ElementAt(i);
                    double paramVal = (double)d.parameters.Values.ElementAt(i);

                    paramVals.Add(paramVal);
                }
                paramData.Add(paramName, paramVals);
                paramFullData.Add(paramData);
            }
            return paramFullData;
        }

        public static List<Dictionary<string, List<double>>> GetDataRanges(List<sParetoDataSet> dataSets)
        {
            List<Dictionary<string, List<double>>> fullData = new List<Dictionary<string, List<double>>>();

            string xDataName = "";
            string yDataName = "";
            string zDataName = "";
            List<double> xDatas = new List<double>();
            List<double> yDatas = new List<double>();
            List<double> zDatas = new List<double>();
            foreach (sParetoDataSet d in dataSets)
            {
                xDataName = d.dataX.name;
                xDatas.Add(d.dataX.value);
                yDataName = d.dataY.name;
                yDatas.Add(d.dataY.value);
                zDataName = d.dataZ.name;
                zDatas.Add(d.dataZ.value);
            }
            Dictionary<string, List<double>> xData = new Dictionary<string, List<double>>();
            Dictionary<string, List<double>> yData = new Dictionary<string, List<double>>();
            Dictionary<string, List<double>> zData = new Dictionary<string, List<double>>();

            xData.Add(xDataName, xDatas);
            yData.Add(yDataName, yDatas);
            zData.Add(zDataName, zDatas);

            fullData.Add(xData);
            fullData.Add(yData);
            fullData.Add(zData);
            return fullData;
        }

    }
}
