using System;

namespace GenerateMemTestPattern
{
    class Program
    {

        static void Main(string[] args)
        {
            //TODO            int maxArraySizeGB = 1;
            double maxArraySizeGB = 0.001;
            if (args.Length > 0 && 
               int.TryParse(args[0], out int maxArraySizeToTestInGigs))
            {
                maxArraySizeGB = maxArraySizeToTestInGigs;
            }

            Probe p = new Probe(maxArraySizeGB);
            p.Start();
        }
    }
}