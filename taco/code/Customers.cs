using System;

namespace taco.code
{
    class Customers
    {
        private int amount_of_people;
        private DateTime departure_time;
        private int id_table;
        private int id_wait_ticket;

        public Customers(int id_wait)
        {
            try
            {
                amount_of_people = Comunes.get_random(1, 11);
                id_wait_ticket = id_wait;
            }
            catch (Exception e) { }
        }

        public int Amount_Of_People
        {
            get { return amount_of_people; }
            set { amount_of_people = value; }
        }

        public DateTime Departure_Time
        {
            get { return departure_time; }
            set { departure_time = value; }
        }

        public int Id_Table
        {
            get { return id_table; }
            set { id_table = value; }
        }

        public int Id_Wait_Ticket
        {
            get { return id_wait_ticket; }
            set { id_wait_ticket = value; }
        }
    }
}
