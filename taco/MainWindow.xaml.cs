using System;
using System.Windows;
using taco.code;

namespace taco
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int current_hour = DateTime.Now.Hour;
                if (current_hour < Restaurant.start_work.Hours || current_hour > Restaurant.end_work.Hours)
                {
                    string ms = "Sorry, the Restaurant is closed.\r\n It is open from 12:00 pm to 8:00 pm.";
                    MessageBox.Show(ms, "UPS!!");
                }
                else
                {
                    Local w = new Local();
                    this.Close();
                    w.Show();
                }
            }
            catch (Exception exp)
            {
            }

        }
    }
}
