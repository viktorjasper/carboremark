using System;
using System.Collections.Generic;
using System.Text;

namespace GenerateMemTestPattern
{
    public class ReadRate
    {
        public static string Unit { get { return "B/s"; } }
        public static string Symbol { get { return "Byte/second"; } }

        public double Rate { get; set; }

        public double TotalSeconds { get; set; }

        public double Iterations { get; set; }

        public int ArraySize { get; internal set; }

        public string HumanFriendlyRate()
        {
            return HumanFriendly.ToMetricStyle(Rate, Unit);
        }

        public string HumanFriendlyArraySize
        {
            get
            {
                return HumanFriendly.ToMetricStyle(ArraySize, "B");

            }
        }

        internal void CombineWithMeasurement(ReadRate other)
        {
            // The resulting rate is the waited average of the original rates.
            Rate = (Rate * Iterations + other.Rate * other.Iterations) /
                    (Iterations + other.Iterations);

            TotalSeconds += other.TotalSeconds;
            Iterations += other.Iterations;
            // ArraySize does not change
        }
    }
}
