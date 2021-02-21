using Newtonsoft.Json;
using InPost.Backend.Extra;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPost.Backend
{
    public class Dostawa
    {
        public Dostawa()
        {
            UtworzBazePaczek();
        }
        public int dzielnicaNaStarcie { get; set; }
        public string GodzinaSzczytuStr { get; set; }
        public bool GodzinaSzczytu { get; set; }
        public List<ZestawPaczek> BazaZestawowPaczek { get; set; }

        void UtworzBazePaczek()
        {
            string sciezkaDoPlikuZzestawamiPaczek = $"{Directory.GetCurrentDirectory()}\\zestawyPaczek.json";
            // meter plik json no bin, debug, copy always
            string tekstPliku = File.ReadAllText(sciezkaDoPlikuZzestawamiPaczek);
            BazaZestawowPaczek = JsonConvert.DeserializeObject<List<ZestawPaczek>>(tekstPliku);
        }
        public List<ZestawPaczek> WylosujZestawyPaczek() // can only return one so....
        {
            List<int> liczby = Losowacz.WygenerujListeLiczbLosowych(4, BazaZestawowPaczek.Count);
            List<ZestawPaczek> lista1 = BazaZestawowPaczek.Where(x => x.Id == liczby[0]).ToList();
            ZestawPaczek zestaw1 = lista1[0];
            // zestaw 1
            List<ZestawPaczek> lista2 = BazaZestawowPaczek.Where(x => x.Id == liczby[1]).ToList();
            ZestawPaczek zestaw2 = lista2[0];
            //zestaw 2
            List<ZestawPaczek> lista3 = BazaZestawowPaczek.Where(x => x.Id == liczby[2]).ToList();
            ZestawPaczek zestaw3 = lista3[0];
            //zestaw 3
            List<ZestawPaczek> lista4 = BazaZestawowPaczek.Where(x => x.Id == liczby[3]).ToList();
            ZestawPaczek zestaw4 = lista4[0];
            //zestaw 4
            List<ZestawPaczek> losowaneZestawy = new List<ZestawPaczek>
            {
                zestaw1,
                zestaw2,
                zestaw3,
                zestaw4
            }; 

            return losowaneZestawy;
               
        }
        
    }
}


