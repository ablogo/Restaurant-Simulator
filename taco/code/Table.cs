using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taco.code
{
    public class Table
    {
        private int id;
        private int chairs;
        private int taked_chairs;
        private bool busy = false;
        private int nos; //number of services
        private int noap; //number of attended people
        public Table()
        {
            chairs = Comunes.get_random(2, 11);
        }

        public Table(int id)
        {
            try
            {
                chairs = Comunes.get_random(2, 11, id);
                this.id = id;
            }
            catch (Exception e) { }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Chairs
        {
            get { return chairs; }
            set { chairs = value; }
        }

        public int TakedChairs
        {
            get { return taked_chairs; }
            set { taked_chairs = value; }
        }

        public bool Busy
        {
            get { return busy; }
            set { busy = value; }
        }

        public int Nos
        {
            get { return nos; }
            set { nos = value; }
        }

        public int Noap
        {
            get { return noap; }
            set { noap = value; }
        }

    }
}
