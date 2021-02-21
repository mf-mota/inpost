using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPost.Backend
{
    public class ZestawPaczek
    {
        public ZestawPaczek()
        {
            ListaPaczekWzestawie = new List<Paczka>();
        }
        public List<Paczka> ListaPaczekWzestawie { get; set; }

        public int Id { get; set; }

    }
}
