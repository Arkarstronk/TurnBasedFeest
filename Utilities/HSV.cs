using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBasedFeest.Utilities
{
    class HSV
    {
        //nitpicked code from internet

        public static Color FromHue(double h, double saturation = 0.8, double value = 0.85)
        {
            double hue = ToLegacyHue(h);
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return new Color(v, t, p, 255);
            else if (hi == 1)
                return new Color(q, v, p, 255);
            else if (hi == 2)
                return new Color(p, v, t, 255);
            else if (hi == 3)
                return new Color(p, q, v, 255);
            else if (hi == 4)
                return new Color(t, p, v, 255);
            else
                return new Color(v, p, q, 255);
        }

        static double ToLegacyHue(double modernHue)
        {
            modernHue = ((modernHue % 360) + 360) % 360; // normalize 360 > modernHue >= 0
            double ret = 0;
            if (modernHue < 60)
            {
                ret = modernHue * 2;
            }
            else if (modernHue < 120)
            {
                ret = modernHue + 60;
            }
            else
            {
                ret = (modernHue - 120) * 0.75 + 180;
            }
            return ret;
        }

        static double FromLegacyHue(double legacyHue)
        {
            legacyHue = ((legacyHue % 360) + 360) % 360; // normalize 360 > legacyHue >= 0
            double ret = 0;
            if (legacyHue < 120)
            {
                ret = legacyHue / 2;
            }
            else if (legacyHue < 180)
            {
                ret = legacyHue - 60;
            }
            else
            {
                ret = (legacyHue - 180) / 0.75 + 120;
            }
            return ret;
        }
    }
}
