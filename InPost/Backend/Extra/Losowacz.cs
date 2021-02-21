using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InPost.Backend.Extra
{
    public static class Losowacz
    {
        // to jest kod na później
        // nie będziemy się tego uczyć poki co
        // tu są dwie napisane przeze mnie gotowe metody które pozwolą Nam na losowanie pytań
        // może je sobie wykorzystamy



        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        /// <summary>
        /// Zwraca listę liczb w losowej kolejności z zakresu od 1 do maksymalnaWartosc
        /// </summary>
        /// <param name="ileLiczb">Określa ilość liczb, która ma być wylosowana</param>
        /// <param name="maksymalnaWartosc">Okresla maksymalną wartość wylosowanych liczb</param>
        /// <returns></returns>
        public static List<int> WygenerujListeLiczbLosowych(int ileLiczb, int maksymalnaWartosc)
        {
            var wynik = new List<int>();
            while (wynik.Count() < ileLiczb)
            {
                int los = WygenerujLiczbeLosowa(maksymalnaWartosc);
                if (!wynik.Contains(los))
                {
                    wynik.Add(los);
                }
            }

            return wynik;
        }

        /// <summary>
        /// Zwraca losową liczbę z zakresu od 1 do maksymalnaWartosc
        /// </summary>
        /// <param name="maksymalnaWartosc">Okresla maksymalną wartość wylosowanej liczby</param>
        /// <returns></returns>
        public static int WygenerujLiczbeLosowa(int maksymalnaWartosc)
        {
            byte[] randomNumber = new byte[1];
            _generator.GetBytes(randomNumber);
            double asciiValueOfRandomChar = Convert.ToDouble(randomNumber[0]);
            double multiplier = Math.Max(0, (asciiValueOfRandomChar / 255d) - 0.00000000001d);
            double randomVAlueInRange = Math.Floor(multiplier * maksymalnaWartosc);
            return (int)(1 + randomVAlueInRange);
        }

    }
}
