// See https://aka.ms/new-console-template for more information

using RyanConnectionFinder;

Console.WriteLine("Enter from airport");
var fromCity = Console.ReadLine();
if (string.IsNullOrEmpty(fromCity))
{
    Console.WriteLine("Invalid input");
    return;
}

Console.WriteLine("Enter to airport");
var toCity = Console.ReadLine();
if (string.IsNullOrEmpty((toCity)))
{
    Console.WriteLine("Invalid input");
    return;
}

var airportsConnection = RyanairScraper.AreAirportsConnectable(fromCity, toCity);
if (airportsConnection == null || airportsConnection.Length == 0) {
    Console.WriteLine("Connection not found");
    return;
}

Console.WriteLine("Connection: " + string.Join(" -> ", airportsConnection));