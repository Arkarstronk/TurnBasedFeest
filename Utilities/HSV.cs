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
        public static Color FromHue(double h, double saturation = 0.6, double value = 0.85)
        {
            double hue = FromLegacyHue(h);
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
                
        //lineair gradient the hue from human perception angles to hsl hue colorspace. 
        public static double FromLegacyHue(double legacyHue)
        {
            legacyHue = legacyHue % 360;

            if(legacyHue <= 60)
            {
                return (legacyHue / 60) * 30;
            }
            else if (legacyHue <= 120)
            {                
                return 30 + (65 - 30) * (double)((legacyHue - 60) / 60);
            }
            else if (legacyHue <= 180)
            {
                return 65 + (120 - 65) * (double)((legacyHue - 120) / 60);
            }
            else if (legacyHue <= 240)
            {
                return 120 + (225 - 120) * (double)((legacyHue - 180) / 60);
            }
            else if (legacyHue <= 300)
            {
                return 225 + (285 - 225) * (double)((legacyHue - 240) / 60);
            }
            else
            {
                return 285 + (360 - 285) * (double)((legacyHue - 300) / 60);
            }
        }
    }
}
