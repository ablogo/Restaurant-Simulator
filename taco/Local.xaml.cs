using System;
using System.Linq;
using System.Threading;
using taco.code;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace taco
{
    /// <summary>
    /// Interaction logic for Local.xaml
    /// </summary>
    public partial class Local : Window
    {
        private Restaurant restaurant;
        private static Timer timer;
        private static DateTime time_to_arrive_people;
        private bool in_progress;

        public Local()
        {
            InitializeComponent();
            start_work();
        }

        private void start_work()
        {
            try
            {
                restaurant = new Restaurant();

                show_put_tables();
                generate_new_arrival();
                timer = new Timer(play, null, 0, 2000);
                show_host_message("Welcome to Taco Restaurant.");
            }
            catch (Exception exp) { }
        }

        private void play(object state)
        {
            try
            {
                if (!in_progress)
                {
                    try
                    {
                        in_progress = true;
                        clean_tables();
                        add_customer();
                        pass_customer();
                        show_refresh_controls();
                        is_time_to_close();
                        in_progress = false;
                    }
                    catch (Exception exp) { }
                }
            }
            catch (Exception e) { }
        }

        private void add_customer()
        {
            try
            {
                if (Comunes.is_time(time_to_arrive_people))
                {
                    Customers customer_group = restaurant.add_customer();
                    show_add_persons(customer_group);
                    generate_new_arrival();

                    string conversation = @"Host: Hi, How many you are?" + Environment.NewLine +
                        "Customer: Hi, we are " + customer_group.Amount_Of_People + Environment.NewLine +
                        "Host: Ok let me search a table. Your wait number is # " + customer_group.Id_Wait_Ticket;
                    show_host_new_message(conversation);
                }
            }
            catch (Exception exp) { }
        }

        private void pass_customer()
        {
            try
            {
                Customers customer_group = null;
                do
                {
                    if (restaurant.customers.Count > 0)
                    {
                        restaurant.customers.TryPeek(out customer_group);
                        if (restaurant.check_chairs(customer_group.Amount_Of_People, false))
                        {
                            int id_table = restaurant.check_free_tables(customer_group.Amount_Of_People);
                            if (id_table > 0)
                            {
                                customer_group.Departure_Time = DateTime.Now.AddSeconds(Comunes.get_random(30, 90));
                                customer_group.Id_Table = id_table;
                                show_host_message("Host: Please, wait number: " + customer_group.Id_Wait_Ticket + " pass to table " + id_table + " ");
                                restaurant.convert_customer_to_diner();
                                show_remove_persons();
                            }
                            else
                            {
                                show_host_new_message("Host: Sorry, for the moment we don't have a empty table. Please wait. ");
                            }
                        }
                        else
                        {
                            restaurant.remove_customer();
                            show_remove_persons();
                            show_host_message("Host: Apologize, we do not have table with enough space. Next group please.");
                        }
                    }
                }
                while (restaurant.customers.Count > 0 && restaurant.check_chairs(customer_group.Amount_Of_People, true));
            }
            catch (Exception exp) { }
        }

        private void clean_tables()
        {
            try
            {
                restaurant.release_table();
            }
            catch (Exception exp) { }
        }

        private void generate_new_arrival()
        {
            try
            {
                time_to_arrive_people = DateTime.Now.AddSeconds(Comunes.get_random(1, 10));
            }
            catch (Exception exp) { }
        }

        private void show_put_tables()
        {
            try
            {
                if (restaurant.tables.Count > 0)
                {
                    ltables.ItemsSource = restaurant.tables;
                }
            }
            catch (Exception e) { }
        }

        private void show_add_persons(Customers customer_group)
        {
            try
            {
                if (restaurant.customers.Count > 0)
                {
                    string message = @"A new group of customers arrived, there are " + customer_group.Amount_Of_People + " persons." + Environment.NewLine +
                        " Wait number assigned: " + customer_group.Id_Wait_Ticket;
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() =>
                        {
                            ListBoxItem item = new ListBoxItem();
                            item.Content = message;
                            fila.Items.Add(item);
                            fila.Items.Refresh();
                        }));
                }
            }
            catch(Exception e) { }
        }

        private void show_remove_persons()
        {
            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() =>
                        {
                            if (fila.Items.Count > 0)
                            {
                                fila.Items.RemoveAt(1);
                                fila.Items.Refresh();
                            }
                        }));
            }
            catch(Exception e) { }
        }

        private void show_refresh_controls()
        {
            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() =>
                        {
                            ltables.ItemsSource = restaurant.tables;
                            ltables.Items.Refresh();
                        }));
            }
            catch (Exception e) { }
        }

        private void show_host_new_message(string mensaje)
        {
            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() =>
                        {
                            tbox_host.Items.Clear();
                            ListBoxItem item = new ListBoxItem();
                            item.Content = mensaje + Environment.NewLine;
                            tbox_host.Items.Add(item);
                            tbox_host.Items.Refresh();
                        }));
            }
            catch(Exception e) { }
        }

        private void show_host_message(string mensaje)
        {
            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() =>
                        {
                            ListBoxItem item = new ListBoxItem();
                            item.Content = mensaje + Environment.NewLine;
                            tbox_host.Items.Add(item);
                            tbox_host.Items.Refresh();
                        }));
            }
            catch (Exception e) { }
        }

        private void get_report(bool end_work)
        {
            try
            {
                if (restaurant.tables.Count > 0)
                {
                    string rs = "";
                    rs += restaurant.tables.Sum(x => x.Noap).ToString() + " customers were served. \r\n";
                    var mo = restaurant.tables.Where(y => y.Nos == (restaurant.tables.Max(x => x.Nos))).Select(p => p).FirstOrDefault();
                    rs += ((mo != null) ? mo.Id.ToString() : "0") + " table was the most used. \r\n";
                    var le = restaurant.tables.Where(y => y.Nos == (restaurant.tables.Min(x => x.Nos))).Select(p => p).FirstOrDefault();
                    rs += ((le != null) ? le.Id.ToString() : "0") + " table was the less used.";

                    if (end_work)
                    {
                        System.IO.File.WriteAllText("./Report.txt", rs);
                        return;
                    }

                    Microsoft.Win32.SaveFileDialog op = new Microsoft.Win32.SaveFileDialog();
                    op.DefaultExt = ".txt";
                    op.FileName = "Report.txt";
                    op.Filter = "Text Document (.txt)|*.txt";

                    if (op.ShowDialog() == true)
                    {
                        System.IO.File.WriteAllText(op.FileName, rs);
                    }
                }
            }
            catch (Exception e) { }
        }

        private void is_time_to_close()
        {
            try
            {
                //descarga el reporte automaticamente 2 minutos antes de cerrar
                if (Comunes.is_time(DateTime.Now, 2))
                {
                    get_report(true);
                }
            }
            catch (Exception e) { }
        }

        private void img_host_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                get_report(false);
            }
            catch (Exception exp) { }

        }
    }
}
