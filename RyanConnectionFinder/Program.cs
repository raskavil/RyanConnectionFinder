// See https://aka.ms/new-console-template for more information

using RyanConnectionFinder;

Console.WriteLine("Enter from airport");
var fromCity = Console.ReadLine();
if (string.IsNullOrEmpty(fromCity))
{
    Console.WriteLine("Invalid input");
    return;
}

/*
Console.WriteLine("Enter transfer");
var transferCity = Console.ReadLine();
if (string.IsNullOrEmpty((transferCity)))
{
    Console.WriteLine("Invalid input");
    return;
}
*/

Console.WriteLine("Enter to airport");
var toCity = Console.ReadLine();
if (string.IsNullOrEmpty((toCity)))
{
    Console.WriteLine("Invalid input");
    return;
}
var airportsConnection = RyanairScraper.Routes(fromCity, toCity);
if (airportsConnection == null || airportsConnection.Length == 0) {
    Console.WriteLine("Connection not found");
    return;
}
for(var i = 0; i < airportsConnection.Count(); i++)
{ 
    var flightDates = AvailabilitiesRequest.FlightDates(airportsConnection[i]);
        if (flightDates == null || flightDates.Length == 0) 
        {
            continue;
        }
        
    var flightTimes = RyanairScraper.Trips(airportsConnection[i], flightDates);    
    
    Utilities.PrintConnections(flightTimes);
}




//Console.WriteLine("Connection: " + string.Join(" -> ", airportsConnection));
//Console.WriteLine("Flight dates: " + string.Join(", ", FlightDates));
