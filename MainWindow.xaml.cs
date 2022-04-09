using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using static System.Text.Json.JsonDocument;
using RestSharp;

namespace IP_Tracker
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

        private void checkButton_Click(object sender, RoutedEventArgs e)
        {
            appProcess();
        }

        public string getDataFromIP(string ip)
        {
            const string URL = "URL";
            var client = new RestClient($"{URL}{ip}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-RapidAPI-Host", "HOST");
            request.AddHeader("X-RapidAPI-Key", "KEY");
            IRestResponse response = client.Execute(request);
            string data = response.Content.ToString();
            return data;
        }

        public void appProcess()
        {
            string ipTxt = ipData.Text;
            string defaultIP = "172.253.62.103";

            if (ipTxt != "")
            {
                try
                {
                    runProcess(ipTxt);
                }
                catch (Exception error)
                {
                    errorProcess(ipTxt, defaultIP);
                }
            }
            else
            {
                ipTxt = defaultIP;
                ipData.Clear();
                ipData.AppendText(ipTxt);
            }
        }

        public void runProcess(string ipTxt)
        {
            string data = getDataFromIP(ipTxt);

            JsonDocument doc = JsonDocument.Parse(data);
            JsonElement element = doc.RootElement;

            string ip = element.GetProperty("ip").ToString();
            string type = element.GetProperty("type").ToString();
            string country = element.GetProperty("location").GetProperty("country").GetProperty("name").ToString();
            string code = element.GetProperty("location").GetProperty("country").GetProperty("code").ToString();
            string city = element.GetProperty("location").GetProperty("city").ToString();
            string postal = element.GetProperty("location").GetProperty("postal").ToString();
            string callingCode = element.GetProperty("location").GetProperty("country").GetProperty("calling_code").ToString();
            string latitude = element.GetProperty("location").GetProperty("latitude").ToString();
            string longitude = element.GetProperty("location").GetProperty("longitude").ToString();

            displayIP.Content = $"IP Address : {ip}";
            displayType.Content = $"Type : {type}";
            displayCountry.Content = $"Country : {country} ({code})";
            displayCity.Content = $"City : {city}";
            displayPostal.Content = $"Postal : {postal}";
            displayCallingCode.Content = $"Calling Code : {callingCode}";
            displayLatitude.Content = $"Latitude : {latitude}";
            displayLongitude.Content = $"Longitude {longitude}";
        }

        public void errorProcess(string ipTxt,string defaultIP)
        {
            MessageBox.Show("Error, Your IP hasn't followed the format of IPv4 or IPv6", "IP Tracker", MessageBoxButton.OK, MessageBoxImage.Error);

            ipTxt = defaultIP;
            ipData.Clear();
            ipData.AppendText(ipTxt);

            displayIP.Content = $"IP Address : None";
            displayType.Content = $"Type : None";
            displayCountry.Content = $"Country : None";
            displayCity.Content = $"City : None";
            displayPostal.Content = $"Postal : None";
            displayCallingCode.Content = $"Calling Code : None";
            displayLatitude.Content = $"Latitude : None";
            displayLongitude.Content = $"Longitude : None";
        }
    }
}