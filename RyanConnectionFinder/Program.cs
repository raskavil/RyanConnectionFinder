// See https://aka.ms/new-console-template for more information

using RyanConnectionFinder;

Console.WriteLine("Enter from airport");
var fromCity = Console.ReadLine();
if (string.IsNullOrEmpty(fromCity))
{
    Console.WriteLine("Invalid input");
    return;
}

Console.WriteLine("Enter transfer");
var transferCity = Console.ReadLine();
if (string.IsNullOrEmpty((transferCity)))
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
/*var airportsConnection = RyanairScraper.AreAirportsConnectable(fromCity, toCity);
if (airportsConnection == null || airportsConnection.Length == 0) {
    Console.WriteLine("Connection not found");
    return;
}
*/
var FlightDates = AvailabilitiesRequest.FlightDates([fromCity, transferCity, toCity]);
if (FlightDates == null || FlightDates.Length == 0) {
    Console.WriteLine("No flights same day");
    return;
}


//Console.WriteLine("Connection: " + string.Join(" -> ", airportsConnection));
Console.WriteLine("Flight dates: " + string.Join(" or ", FlightDates));
