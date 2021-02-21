using InPost.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPost
{
    class Program
    {
        static void Main(string[] args)
        {
            Dostawa dostawa = new Dostawa();
WybracDzielnice:
            Console.WriteLine("Prosze wybrac dzielnice z której startujesz dostaw (1-12)");
            Console.WriteLine();
            Console.Write("Dzielnica: ");
            int dzielnicaNaStarcie = int.Parse(Console.ReadLine().Trim());
            if (dzielnicaNaStarcie <= 12 && dzielnicaNaStarcie > 0)
            {
                Console.Clear();
                W($"Zaczyasz dostaw od dzielnicy {dzielnicaNaStarcie}");
                W("");
                W("Naciśnij klawisz ENTER aby kontynuować");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                W("Error: Proszę wybrac ponownie dzielnice 1-12");
                Console.ForegroundColor = ConsoleColor.White;
                W("");
                W("Naciśnij klawisz ENTER aby spróbowac ponownie");
                Console.ReadLine();
                Console.Clear();
                goto WybracDzielnice;
                
            }
GodzinySzczytu:
            Console.WriteLine("Jezdzisz w godzinach szczytu? (Tak/Nie)");
            string godzinaSzczytuStr = Console.ReadLine().Trim().ToLower();
            bool godzinaSzczytu = new bool();
            if (godzinaSzczytuStr == "tak")
            {
                godzinaSzczytu = true;
                Console.Clear();
                W("Jezdzisz w godzinach szczytu");
                W("");

            }
            else if (godzinaSzczytuStr == "nie")
            {
                Console.Clear();
                W("Nie jezdzisz w godzinach szczytu");
                W("");
                // godzinaSzczytu bedzie false (default)
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                W("Error: Proszę napisac TAK albo NIE");
                Console.ForegroundColor = ConsoleColor.White;
                W("");
                W("Naciśnij klawisz ENTER aby spróbowac ponownie");
                Console.ReadLine();
                Console.Clear();
                goto GodzinySzczytu;
            }
            List<ZestawPaczek> losowaneZestawy = dostawa.WylosujZestawyPaczek();
WybracZestawPaczek:
            W("Prosze wybrac Zestaw Paczek 1, 2, 3 albo 4");
            WyswietlZestawPaczek(losowaneZestawy);
            Console.Write("Opcja: ");
            int wybranaopcja = Convert.ToInt32(Console.ReadLine());
            W("Naciśnij klawisz ENTER aby potwierdzic");
            Console.ReadLine();
            
            if (wybranaopcja >= 1 && wybranaopcja <= 4)
            {
                Console.Clear();
                List<Paczka> wybrany = losowaneZestawy[wybranaopcja-1].ListaPaczekWzestawie;
                ObliczycSciezke(wybrany, dzielnicaNaStarcie, godzinaSzczytu, wybranaopcja); //meter aqui o bool
                Console.ReadLine();
            }
            else
            {
                W("Prosze wybrac 1, 2, 3 lub 4");
                W("Naciśnij klawisz ENTER aby spróbowac ponownie");
                Console.ReadLine();
                Console.Clear();
                goto WybracZestawPaczek;
            }
        }

        static void ObliczycSciezke(List<Paczka> wybranaOpcja, int dzielnicaNaStarcie, bool godzinaSzczytu, int nrzestaw)
        {
            List<int> lokalizacji = new List<int>();
            List<Paczka> wybranaOpcjaSortowana = wybranaOpcja.OrderBy(x => x.Lokalizacja).ToList();
            foreach (Paczka paczka in wybranaOpcjaSortowana)
            {
                lokalizacji.Add(paczka.Lokalizacja);
            }
            int czas1do6 = 5;// mb meter isto la em cima
            int czas7do12 = 0;
            int przejscie = 0;
            List<Paczka> Listanajlepsza = new List<Paczka>();
            List<Paczka> Listanajgorsza = new List<Paczka>();
            if (godzinaSzczytu == false)
            {
                czas7do12 += 5;
            }
            else
            {
                czas7do12 += 8;
            }
            int sumalewo = 0;
            int sumaprawo = 0;
            int start = 0;
            int najlepszasumalaczna = 0;
            int najgorszasumalaczna = 0;
            if (lokalizacji.Contains(dzielnicaNaStarcie))
            {
                int dzielnicaNaStarcieIndex = lokalizacji.IndexOf(dzielnicaNaStarcie);
                //lewo
                for (int i = 0; i <= dzielnicaNaStarcieIndex - 1; i++)
                {
                    if (lokalizacji[i] <= 6 && lokalizacji[i + 1] <= 7)
                    {
                        sumalewo += (lokalizacji[i + 1] - lokalizacji[i]) * czas1do6;
                    }
                    if (lokalizacji[i] > 6 && lokalizacji[i + 1] > 6)
                    {
                        sumalewo += (lokalizacji[i + 1] - lokalizacji[i]) * czas7do12;
                    }
                    if (lokalizacji[i] < 7 && lokalizacji[i + 1] > 7)
                    {
                        sumalewo += ((lokalizacji[i + 1] - 6) * czas7do12 + (6 - lokalizacji[i]) * czas1do6);
                    }
                }
                for (int i = dzielnicaNaStarcieIndex; i <= lokalizacji.Count - 2; i++)
                {
                    if (lokalizacji[i] <= 6 && lokalizacji[i + 1] <= 7)
                    {
                        sumaprawo += (lokalizacji[i + 1] - lokalizacji[i]) * czas1do6;
                    }
                    if (lokalizacji[i] > 6 && lokalizacji[i + 1] > 6)
                    {
                        sumaprawo += (lokalizacji[i + 1] - lokalizacji[i]) * czas7do12;
                    }
                    if (lokalizacji[i] < 7 && lokalizacji[i + 1] > 7)
                    {
                        sumaprawo += (lokalizacji[i + 1] - 6) * czas7do12 + (6 - lokalizacji[i]) * czas1do6;
                    }
                }
                if (sumalewo == 0)
                {
                    najlepszasumalaczna = (sumaprawo);
                    najgorszasumalaczna = (sumaprawo);
                    for (int i = dzielnicaNaStarcieIndex; i <= lokalizacji.Count - 1; i++) //criar as lista
                    {
                        Listanajlepsza.Add(wybranaOpcjaSortowana[i]);
                        Listanajgorsza.Add(wybranaOpcjaSortowana[i]);
                    }
                }
                if (sumaprawo == 0)
                {
                    najlepszasumalaczna = (sumalewo);
                    najgorszasumalaczna = (sumalewo);
                    for (int i = 0; i <= dzielnicaNaStarcieIndex; i++) //criar as lista
                    {
                        Listanajlepsza.Add(wybranaOpcjaSortowana[dzielnicaNaStarcieIndex - i]);
                        Listanajgorsza.Add(wybranaOpcjaSortowana[dzielnicaNaStarcieIndex - i]);
                    }
                }
                if (sumalewo <= sumaprawo && sumalewo != 0)
                {
                    if (lokalizacji[dzielnicaNaStarcieIndex] <= 6 && lokalizacji[dzielnicaNaStarcieIndex + 1] <= 7)
                    {
                        przejscie += (lokalizacji[dzielnicaNaStarcieIndex + 1] - lokalizacji[dzielnicaNaStarcieIndex]) * czas1do6;
                    }
                    if (lokalizacji[dzielnicaNaStarcieIndex] > 6 && lokalizacji[dzielnicaNaStarcieIndex + 1] > 6)
                    {
                        przejscie += (lokalizacji[dzielnicaNaStarcieIndex + 1] - lokalizacji[dzielnicaNaStarcieIndex]) * czas7do12;
                    }
                    if (lokalizacji[dzielnicaNaStarcieIndex] < 7 && lokalizacji[dzielnicaNaStarcieIndex + 1] > 7)
                    {
                        przejscie += ((lokalizacji[dzielnicaNaStarcieIndex + 1] - 6) * czas7do12 - (6 - lokalizacji[dzielnicaNaStarcieIndex]) * czas1do6);
                    }
                    najlepszasumalaczna = (sumalewo * 2) + przejscie + sumaprawo;
                    najgorszasumalaczna = (sumaprawo * 2) + przejscie + sumalewo;
                    for (int i = 0; i <= dzielnicaNaStarcieIndex; i++) //criar as lista
                    {
                        Listanajlepsza.Add(wybranaOpcjaSortowana[dzielnicaNaStarcieIndex - i]);
                        if (i == dzielnicaNaStarcieIndex && wybranaOpcjaSortowana[i] == wybranaOpcjaSortowana[i+1])
                        {
                            Listanajlepsza.Add(wybranaOpcjaSortowana[dzielnicaNaStarcieIndex - i + 1]);
                        }
                    }
                    for (int i = dzielnicaNaStarcieIndex + 1; i <= (lokalizacji.Count - 1); i++)
                    {
                        if (i == dzielnicaNaStarcieIndex && wybranaOpcjaSortowana[i] == wybranaOpcjaSortowana[i - 1])
                        { }
                        else
                        {
                            Listanajlepsza.Add(wybranaOpcjaSortowana[i]);
                        }
                    }
                    for (int i = dzielnicaNaStarcieIndex; i <= (lokalizacji.Count - 1); i++)
                    {
                        Listanajgorsza.Add(wybranaOpcjaSortowana[i]);
                    }
                    for (int i = 0; i <= dzielnicaNaStarcieIndex - 1; i++) //create the lists
                    {
                        Listanajgorsza.Add(wybranaOpcjaSortowana[dzielnicaNaStarcieIndex - i-1]);
                    }

                }
                if (sumalewo > sumaprawo && sumaprawo != 0)
                {
                    if (lokalizacji[dzielnicaNaStarcieIndex] <= 6 && lokalizacji[dzielnicaNaStarcieIndex + 1] <= 7)
                    {
                        przejscie += (lokalizacji[dzielnicaNaStarcieIndex + 1] - lokalizacji[dzielnicaNaStarcieIndex]) * czas1do6;
                    }
                    if (lokalizacji[dzielnicaNaStarcieIndex] > 6 && lokalizacji[dzielnicaNaStarcieIndex + 1] > 6)
                    {
                        przejscie += (lokalizacji[dzielnicaNaStarcieIndex + 1] - lokalizacji[dzielnicaNaStarcieIndex]) * czas7do12;
                    }
                    if (lokalizacji[dzielnicaNaStarcieIndex] <= 6 && lokalizacji[dzielnicaNaStarcieIndex + 1] > 7)
                    {
                        przejscie += ((lokalizacji[dzielnicaNaStarcieIndex + 1] - 6) * czas7do12 - (6 - lokalizacji[dzielnicaNaStarcieIndex]) * czas1do6);
                    }
                    if (lokalizacji[dzielnicaNaStarcieIndex] > 7 && lokalizacji[dzielnicaNaStarcieIndex + 1] < 7)
                    {
                        przejscie += (lokalizacji[dzielnicaNaStarcieIndex + 1] - lokalizacji[dzielnicaNaStarcieIndex]) * czas7do12;
                    }
                    najlepszasumalaczna = (sumaprawo * 2) + przejscie + sumalewo;
                    najgorszasumalaczna = (sumalewo * 2) + przejscie + sumaprawo;
                    for (int i = 0; i <= dzielnicaNaStarcieIndex; i++) //criar as listas
                    {
                        Listanajgorsza.Add(wybranaOpcjaSortowana[dzielnicaNaStarcieIndex - i]);
                    }
                    for (int i = dzielnicaNaStarcieIndex + 1; i <= (lokalizacji.Count - 1); i++)
                    {
                        Listanajgorsza.Add(wybranaOpcjaSortowana[i]);
                    }
                    for (int i = dzielnicaNaStarcieIndex; i <= (lokalizacji.Count - 1); i++)
                    {
                        Listanajlepsza.Add(wybranaOpcjaSortowana[i]);
                    }
                    for (int i = 0; i <= dzielnicaNaStarcieIndex - 1; i++) //criar as lista
                    {
                        Listanajlepsza.Add(wybranaOpcjaSortowana[dzielnicaNaStarcieIndex - i - 1]);
                    }
                }
            }
            else // czyli nie ma paczki w dzelnicieNaStarcie
            {
                int najblizsza = lokalizacji.OrderBy(x => Math.Abs((long)x - dzielnicaNaStarcie)).First();
                int najblizszaIndex = lokalizacji.IndexOf(najblizsza);

                for (int i = 0; i <= najblizszaIndex - 1; i++)
                {
                    if (lokalizacji[i] <= 6 && lokalizacji[i + 1] <= 7)
                    {
                        sumalewo += (lokalizacji[i + 1] - lokalizacji[i]) * czas1do6;
                    }
                    if (lokalizacji[i] > 6 && lokalizacji[i + 1] > 6)
                    {
                        sumalewo += (lokalizacji[i + 1] - lokalizacji[i]) * czas7do12;
                    }
                    if (lokalizacji[i] < 7 && lokalizacji[i + 1] > 7)
                    {
                        sumalewo += ((lokalizacji[i + 1] - 6) * czas7do12 + (6 - lokalizacji[i]) * czas1do6);
                    }
                }
                for (int i = najblizszaIndex; i <= lokalizacji.Count - 2; i++)
                {
                    if (lokalizacji[i] <= 6 && lokalizacji[i + 1] <= 7)
                    {
                        sumaprawo += (lokalizacji[i + 1] - lokalizacji[i]) * czas1do6;
                    }
                    if (lokalizacji[i] > 6 && lokalizacji[i + 1] > 6)
                    {
                        sumaprawo += (lokalizacji[i + 1] - lokalizacji[i]) * czas7do12;
                    }
                    if (lokalizacji[i] < 7 && lokalizacji[i + 1] > 7)
                    {
                        sumaprawo += (lokalizacji[i + 1] - 6) * czas7do12 + (6 - lokalizacji[i]) * czas1do6;
                    }
                }
                if (sumalewo == 0)
                {
                    if (lokalizacji[najblizszaIndex] > 6 && dzielnicaNaStarcie > 6)
                    {
                        start += Math.Abs((lokalizacji[najblizszaIndex] - dzielnicaNaStarcie)) * czas7do12;
                    }
                    if (lokalizacji[najblizszaIndex] <= 6 && dzielnicaNaStarcie > 7)
                    {
                        start += ((dzielnicaNaStarcie - 6) * czas7do12 - (6 - lokalizacji[najblizszaIndex]) * czas1do6);
                    }
                    if (lokalizacji[najblizszaIndex] > 7 && dzielnicaNaStarcie <= 6)
                    {
                        start += ((lokalizacji[najblizszaIndex] - 6) * czas7do12 - (6 - dzielnicaNaStarcie) * czas1do6);
                    }
                    najlepszasumalaczna = (sumaprawo + start);
                    najgorszasumalaczna = (sumaprawo + start);
                    for (int i = najblizszaIndex; i <= lokalizacji.Count - 1; i++) 
                    {
                        Listanajlepsza.Add(wybranaOpcjaSortowana[i]);
                        Listanajgorsza.Add(wybranaOpcjaSortowana[i]);
                    }
                }
                if (sumaprawo == 0)
                {
                    if (lokalizacji[najblizszaIndex] > 6 && dzielnicaNaStarcie > 6)
                    {
                        start += Math.Abs((lokalizacji[najblizszaIndex] - dzielnicaNaStarcie)) * czas7do12;
                    }
                    if (lokalizacji[najblizszaIndex] <= 6 && dzielnicaNaStarcie > 7)
                    {
                        start += ((dzielnicaNaStarcie - 6) * czas7do12 - (6 - lokalizacji[najblizszaIndex]) * czas1do6);
                    }
                    if (lokalizacji[najblizszaIndex] > 7 && dzielnicaNaStarcie <= 6)
                    {
                        start += ((lokalizacji[najblizszaIndex] - 6) * czas7do12 - (6 - dzielnicaNaStarcie) * czas1do6);
                    }
                    najlepszasumalaczna = (sumalewo + start);
                    najgorszasumalaczna = (sumalewo + start);
                    for (int i = 0; i <= najblizszaIndex; i++) 
                    {
                        Listanajlepsza.Add(wybranaOpcjaSortowana[najblizszaIndex - i]);
                        Listanajgorsza.Add(wybranaOpcjaSortowana[najblizszaIndex - i]);
                    }
                }
                if (sumalewo <= sumaprawo && sumalewo != 0) 
                {
                    if (lokalizacji[najblizszaIndex] <= 6 && lokalizacji[najblizszaIndex + 1] <= 7)
                    {
                        przejscie += (lokalizacji[najblizszaIndex + 1] - lokalizacji[najblizszaIndex]) * czas1do6;
                    }
                    if (lokalizacji[najblizszaIndex] > 6 && lokalizacji[najblizszaIndex + 1] > 6)
                    {
                        przejscie += (lokalizacji[najblizszaIndex + 1] - lokalizacji[najblizszaIndex]) * czas7do12;
                    }
                    if (lokalizacji[najblizszaIndex] <= 6 && lokalizacji[najblizszaIndex + 1] > 7)
                    {
                        przejscie += ((lokalizacji[najblizszaIndex + 1] - 6) * czas7do12 - (6 - lokalizacji[najblizszaIndex]) * czas1do6);
                    }
                    if (lokalizacji[najblizszaIndex] <= 6 && dzielnicaNaStarcie <= 7)
                    {
                        start += Math.Abs((lokalizacji[najblizszaIndex] - dzielnicaNaStarcie) * czas1do6);
                    }
                    if (lokalizacji[najblizszaIndex] > 6 && dzielnicaNaStarcie > 6)
                    {
                        start += Math.Abs((lokalizacji[najblizszaIndex] - dzielnicaNaStarcie)) * czas7do12;
                    }
                    if (lokalizacji[najblizszaIndex] <= 6 && dzielnicaNaStarcie > 7)
                    {
                        start += ((dzielnicaNaStarcie - 6) * czas7do12 - (6 - lokalizacji[najblizszaIndex]) * czas1do6);
                    }
                    if (lokalizacji[najblizszaIndex] > 7 && dzielnicaNaStarcie <= 6)
                    {
                        start += ((lokalizacji[najblizszaIndex] - 6) * czas7do12 - (6 - dzielnicaNaStarcie) * czas1do6);
                    }
                    najlepszasumalaczna = (sumalewo * 2) + przejscie + sumaprawo + start;
                    najgorszasumalaczna = (sumaprawo * 2) + przejscie + sumalewo + start;
                    for (int i = 0; i <= najblizszaIndex; i++) 
                    {
                        Listanajlepsza.Add(wybranaOpcjaSortowana[najblizszaIndex - i + 1]);
                    }
                    for (int i = najblizszaIndex + 1; i <= (lokalizacji.Count - 1); i++)
                    {
                        Listanajlepsza.Add(wybranaOpcjaSortowana[i]);
                    }
                    for (int i = najblizszaIndex; i <= (lokalizacji.Count - 1); i++)
                    {
                        Listanajgorsza.Add(wybranaOpcjaSortowana[i]);
                    }
                    for (int i = 0; i <= najblizszaIndex - 1; i++) //criar as lista
                    {
                        Listanajgorsza.Add(wybranaOpcjaSortowana[najblizszaIndex - i - 1]);
                    }
                }

                if (sumalewo > sumaprawo && sumaprawo != 0)
                {
                    if (lokalizacji[najblizszaIndex] <= 6 && lokalizacji[najblizszaIndex + 1] <= 7)
                    {
                        przejscie += (lokalizacji[najblizszaIndex + 1] - lokalizacji[najblizszaIndex]) * czas1do6;
                    }
                    if (lokalizacji[najblizszaIndex] > 6 && lokalizacji[najblizszaIndex + 1] > 6)
                    {
                        przejscie += (lokalizacji[najblizszaIndex + 1] - lokalizacji[najblizszaIndex]) * czas7do12;
                    }
                    if (lokalizacji[najblizszaIndex] <= 6 && lokalizacji[najblizszaIndex + 1] > 7)
                    {
                        przejscie += ((lokalizacji[najblizszaIndex + 1] - 6) * czas7do12 - (6 - lokalizacji[najblizszaIndex]) * czas1do6);
                    }
                    if (lokalizacji[najblizszaIndex] > 7 && lokalizacji[najblizszaIndex + 1] <= 6)
                    {
                        przejscie += (lokalizacji[najblizszaIndex + 1] - lokalizacji[najblizszaIndex]) * czas7do12;
                    }
                    if (lokalizacji[najblizszaIndex] > 6 && dzielnicaNaStarcie > 6)
                    {
                        start += Math.Abs((lokalizacji[najblizszaIndex] - dzielnicaNaStarcie)) * czas7do12;
                    }
                    if (lokalizacji[najblizszaIndex] <= 6 && dzielnicaNaStarcie > 7)
                    {
                        start += ((dzielnicaNaStarcie - 6) * czas7do12 - (6 - lokalizacji[najblizszaIndex]) * czas1do6);
                    }
                    if (lokalizacji[najblizszaIndex] > 7 && dzielnicaNaStarcie <= 6)
                    {
                        start += ((lokalizacji[najblizszaIndex] - 6) * czas7do12 - (6 - dzielnicaNaStarcie) * czas1do6);
                    }
                    najlepszasumalaczna = (sumaprawo * 2) + przejscie + sumalewo + start;
                    najgorszasumalaczna = (sumalewo * 2) + przejscie + sumaprawo + start;
                    for (int i = 0; i <= najblizszaIndex; i++) //criar as lista
                    {
                        Listanajgorsza.Add(wybranaOpcjaSortowana[najblizszaIndex - i]);
                    }
                    for (int i = najblizszaIndex + 1; i <= (lokalizacji.Count - 1); i++)
                    {
                        Listanajgorsza.Add(wybranaOpcjaSortowana[i]);
                    }
                    for (int i = najblizszaIndex; i <= (lokalizacji.Count - 1); i++)
                    {
                        Listanajlepsza.Add(wybranaOpcjaSortowana[i]);
                    }
                    for (int i = 0; i <= najblizszaIndex - 1; i++) //criar as lista
                    {
                        Listanajlepsza.Add(wybranaOpcjaSortowana[najblizszaIndex - i - 1]);
                    }
                }
            }
            najlepszasumalaczna += 10; // 1min/paczka x 10 paczki
            najgorszasumalaczna += 10;
            W($"Wybrales zestaw paczek nr {nrzestaw}");
            W("");
            W("");
            W($"Najszybsza trasa dla zestawu paczek: ");
            W("");
            W($"Czas Przyjazdu: {najlepszasumalaczna} min");
            string najlepszadisplay = "Kolejnosc: ";
            string najgorszadisplay = "Kolejnosc: ";
            for (int j = 0; j < 9; j++)
            {
                najlepszadisplay += $"({Listanajlepsza[j].Artykul} - {Listanajlepsza[j].Lokalizacja}), ";
            }
            najlepszadisplay += $"({Listanajlepsza[9].Artykul} - {Listanajlepsza[9].Lokalizacja})";
            WP(najlepszadisplay);
            W("");
            W("");
            W("");
            W($"Najdluzsza trasa dla zestawu paczek:");
            W("");
            W($"Czas Przyjazdu: {najgorszasumalaczna} min");
            for (int j = 0; j < 9; j++)
            {
                najgorszadisplay += $"({Listanajgorsza[j].Artykul} - {Listanajgorsza[j].Lokalizacja}), ";
            }
            najgorszadisplay += $"({Listanajgorsza[9].Artykul} - {Listanajgorsza[9].Lokalizacja})";
            WP(najgorszadisplay);


        }
        static void WyswietlZestawPaczek(List<ZestawPaczek> zestawy)
        {
            for (int i = 0; i < 4; i++)
            {
                string zestaw = $"Opcja {i + 1}:  ";
                for (int j = 0; j < 9; j++)
                {
                    zestaw += $"({zestawy[i].ListaPaczekWzestawie[j].Artykul} - {zestawy[i].ListaPaczekWzestawie[j].Lokalizacja}), ";
                }
                zestaw += $"({zestawy[i].ListaPaczekWzestawie[9].Artykul} - {zestawy[i].ListaPaczekWzestawie[9].Lokalizacja})";
                WP(zestaw);
                Console.WriteLine();
            }
        }
        static void W(string tekst)
        {
            Console.WriteLine(tekst);
        }

        static void WP(string tekst)
        {
            Console.WriteLine();
            Console.WriteLine(tekst);
        }
    }
}
