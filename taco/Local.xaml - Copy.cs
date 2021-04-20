using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using taco.code;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace taco
{
    /// <summary>
    /// Interaction logic for Local.xaml
    /// </summary>
    public partial class Local : Window
    {
        private restaurant sushi;
        private static Timer timer;
        private static DateTime arrive_people;
        private bool busy;
        private int number_customers;
        public Local()
        {
            InitializeComponent();
            start_work();
        }

        private void start_work()
        {
            try
            {
                sushi = new restaurant();

                show_put_tables();
                generate_arrival();
                timer = new Timer(play, null, 0, 20000);
                show_host_message("Welcome to Taco Restaurant.");
            }
            catch (Exception exp) { }
        }

        private void play(object state)
        {
            try
            {
                if (!busy)
                {
                    try
                    {
                        busy = true;
                        clean_tables();
                        add_customer();
                        pass_customer();
                        show_refresh_controls();
                        is_time_to_close();
                        busy = false;
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
                if (comunes.is_time(arrive_people))
                {
                    sushi.add_customer();
                    generate_arrival();
                    show_add_persons();
                    show_host_new_message("Hi, how many you are? Ok let me search table for " + sushi.list_customers.Peek().Amount + ".");
                }
            }
            catch (Exception exp) { }
        }

        private void pass_customer()
        {
            try
            {
                if (sushi.list_customers.Count > 0)
                {
                    customers group = sushi.list_customers.Peek();
                    if (sushi.check_chairs(group.Amount))
                    {
                        int uid_table = sushi.check_free_tables(group.Amount);
                        if (uid_table > 0)
                        {
                            group.Leave = DateTime.Now.AddSeconds(comunes.get_random(30, 90));
                            group.Uid_Table = uid_table;
                            show_host_new_message("Please, pass to table " + uid_table + " ");
                            sushi.pass_customer();
                            show_remove_persons();
                        }
                        else
                        {
                            show_host_new_message("Sorry, for the moment we don't a table. ");
                        }
                    }
                    else
                    {
                        sushi.remove_customer();
                        show_remove_persons();
                        show_host_new_message("Apologize, we do not have enough space. Next group please.");
                    }
                }
            }
            catch (Exception exp) { }
        }

        private void clean_tables()
        {
            try
            {
                sushi.release_table();
            }
            catch (Exception exp) { }
        }

        private void generate_arrival()
        {
            try
            {
                arrive_people = DateTime.Now.AddSeconds(comunes.get_random(10, 61));
            }
            catch (Exception exp) { }
        }

        private void show_put_tables()
        {
            try
            {
                if (sushi.tables.Count > 0)
                {
                    ltables.ItemsSource = sushi.tables;
                }
            }
            catch (Exception e) { }
        }

        private void show_add_persons()
        {
            try
            {
                if (sushi.list_customers.Count > 0)
                {
                    string x = sushi.list_customers.Peek().Amount.ToString();
                    x = "New Customers arrival, " + x + " persons.";
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() =>
                        {
                            ListBoxItem item = new ListBoxItem();
                            item.Content = x;
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
                                fila.Items.RemoveAt(0);
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
                            ltables.ItemsSource = sushi.tables;
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
                            tbox_host.Text = mensaje;
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
                            tbox_host.Text += mensaje;
                        }));
            }
            catch (Exception e) { }
        }

        private void get_report(bool end_work)
        {
            try
            {
                if (sushi.tables.Count > 0)
                {
                    string rs = "";
                    rs += sushi.tables.Sum(x => x.Noap).ToString() + " customers were served. \r\n";
                    var mo = sushi.tables.Where(y => y.Nos == (sushi.tables.Max(x => x.Nos))).Select(p => p).FirstOrDefault();
                    rs += ((mo != null) ? mo.Uid.ToString() : "0") + " table was the most used. \r\n";
                    var le = sushi.tables.Where(y => y.Nos == (sushi.tables.Min(x => x.Nos))).Select(p => p).FirstOrDefault();
                    rs += ((le != null) ? le.Uid.ToString() : "0") + " table was the less used.";

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
                if (comunes.is_time(DateTime.Now, 2))
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
