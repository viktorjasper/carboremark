using System;
using System.Collections.Generic;
using System.Text;

namespace GenerateMemTestPattern
{
    public class HumanFriendly
    {
        public static string HumanFriendlyFormat { get { return "###.###"; } }

        public static string ToMetricStyle(double value, string unit)
        {
            string sign = string.Empty;
            if (value < 0)
            {
                sign = "-";
            }

            string prefix = string.Empty;
            string quantity = string.Empty;

            if (value == 0)
            {
                quantity = "0";
            }
            else if (value < 1e-27D)
            {
                quantity = "≈0";
            }
            else if (value < 1.0D)
            {
                if (value >= 1e-3D)
                {
                    quantity = (value * 1e-3D).ToString(HumanFriendlyFormat);
                    prefix = "m"; // milli
                }
                else if (value >= 1e-6D)
                {
                    quantity = (value * 1e-6D).ToString(HumanFriendlyFormat);
                    prefix = "μ"; // micro
                }
                else if (value < 1e-9D)
                {
                    quantity = (value * 1e-9D).ToString(HumanFriendlyFormat);
                    prefix = "n"; // nano
                }
                else if (value < 1e-12D)
                {
                    quantity = (value * 1e-12D).ToString(HumanFriendlyFormat);
                    prefix = "p"; // pico
                }
                else if (value < 1e-15D)
                {
                    quantity = (value * 1e-15D).ToString(HumanFriendlyFormat);
                    prefix = "f"; // femto
                }
                else if (value < 1e-18D)
                {
                    quantity = (value * 1e-18D).ToString(HumanFriendlyFormat);
                    prefix = "a"; // atto
                }
                else if (value < 1e-21D)
                {
                    quantity = (value * 1e-21D).ToString(HumanFriendlyFormat);
                    prefix = "z"; // zepto
                }
                else
                {
                    quantity = (value * 1e-24D).ToString(HumanFriendlyFormat);
                    prefix = "y"; // yotta
                }
            }
            else
            {
                if (value < 1e+3D)
                {
                    quantity = (value * 1e-0D).ToString(HumanFriendlyFormat);
                }
                else if (value < 1e+6D)
                {
                    quantity = (value / 1e+3D).ToString(HumanFriendlyFormat);
                    prefix = "k"; // kilo
                }
                else if (value < 1e+9D)
                {
                    quantity = (value / 1e+6D).ToString(HumanFriendlyFormat);
                    prefix = "M"; // mega
                }
                else if (value < 1e+12D)
                {
                    quantity = (value / 1e+9D).ToString(HumanFriendlyFormat);
                    prefix = "G"; // giga
                }
                else if (value < 1e+15D)
                {
                    quantity = (value / 1e+12D).ToString(HumanFriendlyFormat);
                    prefix = "T"; // tera
                }
                else if (value < 1e+18D)
                {
                    quantity = (value / 1e+15D).ToString(HumanFriendlyFormat);
                    prefix = "P"; // peta
                }
                else if (value < 1e+21D)
                {
                    quantity = (value / 1e+18D).ToString(HumanFriendlyFormat);
                    prefix = "E"; // exa
                }
                else if (value < 1e+24D)
                {
                    quantity = (value / 1e+21D).ToString(HumanFriendlyFormat);
                    prefix = "Z"; // zetta
                }
                else if (value < 1e+27D)
                {
                    quantity = (value / 1e+24D).ToString(HumanFriendlyFormat);
                    prefix = "Y"; // yotta
                }
            }

            return sign + quantity + " " + prefix + unit;
        }
    }
}
