using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF
{
    class Conversions
    {
        public static float StandardAtmInPa = 101325;

        public static float CeliusToFahrenheit(float celcius)
        {
            return celcius * 9 / 5 + 32;
        }

        public static float PaToPsi(float pa)
        {
            return pa * 14.696f / StandardAtmInPa;
        }

        public static float PaToMmHg(float pa)
        {
            return pa * 760f / StandardAtmInPa;
        }
    }
}
