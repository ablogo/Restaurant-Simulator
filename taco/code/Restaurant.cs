using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace taco.code
{
    class Restaurant
    {
        public ConcurrentQueue<Customers> customers;
        public List<Customers> diners;
        public List<Table> tables;
        public static TimeSpan start_work = new TimeSpan(00, 0, 0);
        public static TimeSpan end_work = new TimeSpan(21, 0, 0);
        private int count = 0;

        public Restaurant()
        {
            customers = new ConcurrentQueue<Customers>();
            diners = new List<Customers>();
            tables = new List<Table>();
            int num_of_tables = Comunes.get_random(5, 10);

            for (int j = 1; j < num_of_tables; j++)
            {
                tables.Add(new Table(j));
            }
            tables = tables.OrderBy(x => x.Chairs).ToList();
        }

        public int check_free_tables(int number_of_people)
        {
            int table_id = 0;
            try
            {
                var table = tables.Where(x => x.Busy == false && x.Chairs >= number_of_people).Select(y => y).FirstOrDefault();
                if (table != null)
                {
                    table_id = table.Id;
                    table.Busy = true;
                    table.Nos += 1;
                    table.Noap += number_of_people;
                    table.TakedChairs = number_of_people;
                }
            }
            catch (Exception e)
            {
            }
            return table_id;
        }

        public bool check_chairs(int number_of_people, bool empty_tables)
        {
            try
            {
                var t = empty_tables ? tables.Where(q => q.Chairs >= number_of_people && q.Busy == false).FirstOrDefault() : tables.Where(x => x.Chairs >= number_of_people).Select(y => y).FirstOrDefault();
                if (t != null)
                {
                    return true;
                }
            }
            catch (Exception e) { }
            return false;
        }

        public void release_table()
        {
            try
            {
                var table_ids = diners.Where(p => p.Departure_Time <= DateTime.Now).Select(y => y.Id_Table).ToList();
                foreach (var id in table_ids)
                {
                    var q = tables.Where(x => x.Id == id).Select(y => y).FirstOrDefault();
                    q.Busy = false;
                    q.TakedChairs = 0;
                    diners.Remove(diners.Where(x => x.Id_Table == id).FirstOrDefault());
                }
            }
            catch (Exception e) { }
        }

        public Customers add_customer()
        {
            Customers customer = null;
            try
            {
                customer = new Customers(++count);
                customers.Enqueue(customer);
            }
            catch (Exception e) { }
            return customer;
        }

        public void convert_customer_to_diner()
        {
            try
            {
                Customers group = null;
                if (customers.TryDequeue(out group)) diners.Add(group);
            }
            catch (Exception e) { }
        }

        public void remove_customer()
        {
            try
            {
                Customers group = null;
                customers.TryDequeue(out group);
            }
            catch (Exception e) { }
        }

    }
}
