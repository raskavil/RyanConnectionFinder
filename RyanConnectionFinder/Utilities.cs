using System.Globalization;

namespace RyanConnectionFinder;

public static class Utilities
{
    public static void PrintConnections(List<List<RyanairScraper.Connection>> connections)
    {
        if (connections.Count == 0)
        {
            Console.WriteLine("No routes found.");
        }

        foreach (var route in connections)
        {
            if (connections.Count == 0)
            {
                throw new Exception("Route should always contain connections.");
            }

            Console.WriteLine($"Route {route.First().From} - {route.Last().To}");
            foreach (var connection in route)
            {
                Console.WriteLine("-----");
                Console.WriteLine($"{connection.From} - {connection.To}");
                var departure = DateTime.Parse(connection.Departure, null, DateTimeStyles.RoundtripKind);
                var arrival = DateTime.Parse(connection.Arrival, null, DateTimeStyles.RoundtripKind);

                string format;
                if (departure.Date != arrival.Date)
                {
                    format = "dd.MM., HH:mm";
                }
                else
                {
                    Console.WriteLine(departure.ToString("dd.MM.yyyy"));
                    format = "HH:mm";
                }
                Console.WriteLine($"{departure.ToString(format)} - {arrival.ToString(format)}");
            }
            Console.WriteLine("---------------");
        }
    }
}