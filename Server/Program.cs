using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal class Program
{
    private static Dictionary<string, string> countryCity = new Dictionary<string, string>()
    {
        {"Afghanistan", "Kabul"},
        {"Albania", "Tirana"},
        {"Algeria", "Algiers"},
        {"Andorra", "Andorra la Vella"},
        {"Angola", "Luanda"},
        {"Argentina", "Buenos Aires"},
        {"Australia", "Canberra"},
        {"Austria", "Vienna"},
        {"Belgium", "Brussels"},
        {"Brazil", "Brasília"},
        {"Canada", "Ottawa"},
        {"China", "Beijing"},
        {"Egypt", "Cairo"},
        {"France", "Paris"},
        {"Germany", "Berlin"},
        {"India", "New Delhi"},
        {"Italy", "Rome"},
        {"Japan", "Tokyo"},
        {"Mexico", "Mexico City"},
        {"Russia", "Moscow"},
        {"Spain", "Madrid"},
        {"Ukraine", "Kyiv" },
        {"United Kingdom", "London"},
        {"United States", "Washington, D.C."}
    };

    static void Main(string[] args)
    {
        TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 1234);
        listener.Start();

        Console.WriteLine("Сервер запущено. Очікування підключень...");

        while(true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Клієнт підключений!");

            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
            string country = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            string city = GetCityByCountry(country);

            byte[] response = Encoding.UTF8.GetBytes(city);
            stream.Write(response, 0, response.Length);

            client.Close();
        }

        SaveCityCountryToJson(countryCity, "countryCity.json");
    }

    private static string GetCityByCountry(string cityCountry)
    {
        if(countryCity.ContainsKey(cityCountry))
            return countryCity[cityCountry];

        return string.Empty;
    }

    private static void SaveCityCountryToJson(Dictionary<string, string> cityCountry, string filePath)
    {
        string json = JsonConvert.SerializeObject(cityCountry, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
}