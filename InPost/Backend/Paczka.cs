using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPost.Backend
{
    public class Paczka
    {
       
        public int Id { get; set; }
        public string Artykul { get; set; }
        public int Lokalizacja { get; set; }
        
        // nie warto otwierać nową klasę tylko do tego....
    }
}
