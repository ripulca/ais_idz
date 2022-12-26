using System;
using System.Collections.Generic;
using System.Windows;
using System.Management;

namespace ais_idz
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
        }

        private List<string> GetSoftwareInfo()
        {
            ConnectionOptions connection = new ConnectionOptions();

            List<string> result = new List<string>();

            try
            {

                ManagementObjectSearcher searcher;

                if ((bool)isDistansed.IsChecked)
                {
                    connection.Username = username.Text;
                    connection.Password = password.Text;
                    connection.Authority = "ntlmdomain:" + domain.Text;

                    ManagementScope scope = new ManagementScope("\\\\" + IPAddress.Text + "\\root\\CIMV2", connection);
                    scope.Connect();

                    ObjectQuery query = new ObjectQuery("SELECT * FROM CIM_Product"); //SELECT * FROM Win32_SoftwareElement

                    searcher = new ManagementObjectSearcher(scope, query);
                }
                else
                {
                    searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
                }

                string text = "";
                foreach (ManagementObject obj in searcher.Get())
                {
                    text += "Path: " + obj.ToString().Trim() + "\n";
                    text += "\n";
                }

                output.Text = text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetSoftwareInfo();
        }
    }
}
