using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateMemTestPattern
{
    class Probe
    {
        /// <summary>
        /// Easily fits into a L1 cache which
        /// is around 32 KiB on modern computers.
        /// </summary>
        internal const int minArraySizeB = 256;

        /// <summary>
        /// The maximum difference allowed between two measurements. 
        /// </summary>
        internal const double maxDifference = 0.05;

        /// <summary>
        /// The maximum array size use when doing the tests
        /// </summary>
        internal double _maxArraySizeGB;

        private ShuffledIndexes _indexes = new ShuffledIndexes();

        public Probe(double maxArraySizeGB)
        {
            _maxArraySizeGB = maxArraySizeGB;
        }

        public void Start()
        {
            // Print title
            Console.WriteLine(
                "Benchmarking random reading of an array...\n");

            // Print heading
            Console.WriteLine(
                "Arraysize" +
                "\t" +
                "Read rate" +
                "\t" +
                "Arraysize (human friendly)" +
                "\t" +
                "Read rate(human friendly)"
                );

            SortedDictionary<int, ReadRate> measurements =
                new SortedDictionary<int, ReadRate>();

            ReadRate rate = Measure(minArraySizeB);
            measurements.Add(rate.ArraySize, rate);

            rate = Measure((int)Math.Round(
                _maxArraySizeGB * 1024 * 1024 * 1024));
            measurements.Add(rate.ArraySize, rate);

            while (RefineMeasurements(measurements))
            {
                RepeatMeasurements(measurements);
            }

            Console.WriteLine("Result: \n\n");
            foreach (ReadRate finalRate in measurements.Values)
            {
                // Feedback of the measurement
                Console.WriteLine(
                    finalRate.ArraySize.ToString("0") +
                    "\t" +
                    finalRate.Rate.ToString("0") +
                    "\t" +
                    finalRate.HumanFriendlyArraySize +
                    "\t" +
                    finalRate.HumanFriendlyRate()
                    );
            }
        }

        private void RepeatMeasurements(
            SortedDictionary<int, ReadRate> measurements)
        {
            foreach(ReadRate rate in measurements.Values)
            {
                ReadRate newRate = Measure(rate.ArraySize);
                measurements[rate.ArraySize].CombineWithMeasurement(newRate);
            }
        }

        private bool RefineMeasurements(
            SortedDictionary<int, ReadRate> measurements)
        {
            Tuple<ReadRate, ReadRate> firstCrude = null;
            bool foundCrude = false;
            do
            {
                firstCrude = GranularityCheck(measurements, 0.1);
                if (firstCrude == null)
                {
                    break;
                }
                else
                {
                    foundCrude = true;
                }

                // Refine granularity by repeating the measurements.
                // Remasuring improves a possibly wrong measurement.
                ReadRate rate1 = Measure(firstCrude.Item1.ArraySize);
                ReadRate rate2 = Measure(firstCrude.Item2.ArraySize);
                measurements[rate1.ArraySize].CombineWithMeasurement(rate1);
                measurements[rate2.ArraySize].CombineWithMeasurement(rate2);

                // Refine granularity by adding a new measuring point.
                double newArraySize =
                    (firstCrude.Item1.ArraySize +
                    firstCrude.Item2.ArraySize) / 2;
                ReadRate rateNew = Measure((int)Math.Round(newArraySize));
                measurements.Add(rateNew.ArraySize, rateNew);
            } while (firstCrude != null);
            return foundCrude;
        }

        private Tuple<ReadRate, ReadRate> GranularityCheck(
            SortedDictionary<int, ReadRate> measurements,
            double maxAllowableRationDifference)
        {
            double minRatio = 1 - maxAllowableRationDifference;
            double maxRatio = 1 + maxAllowableRationDifference;
            if (measurements.Count < 2)
            {
                throw new InvalidOperationException(
                    "Two measurements expected at least!");
            }

            ReadRate previous = null;
            foreach (ReadRate rate in measurements.Values)
            {
                if(previous == null)
                {
                    previous = rate;
                    continue;
                }

                double ration = previous.Rate / rate.Rate;
                if (ration > maxRatio || ration < minRatio)
                {
                    return new Tuple<ReadRate, ReadRate>(previous, rate);
                }
                previous = rate;
            }

            return null;
        }

        private ReadRate Measure(int arraySize)
        {
            _indexes.Generate(arraySize);
            ReadRate rate = _indexes.MeasurePerformance(1);
            double iterations = 1.0 / rate.TotalSeconds;
            if (iterations > 1)
            {
                // Repeat measurement so that it will
                // take approximately one second.
                rate = _indexes.MeasurePerformance(iterations);
                rate.Iterations = iterations;
            }
            else
            {
                rate.Iterations = 1;
            }

            // Feedback of the measurement
            Console.WriteLine(
                _indexes.SizeOfArray.ToString("0") +
                "\t" +
                rate.Rate.ToString("0") +
                "\t" +
                _indexes.HumanFriendlySizeOfArray +
                "\t" +
                rate.HumanFriendlyRate()
                );
            return rate;
        }
    }
}
