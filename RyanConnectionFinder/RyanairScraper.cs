namespace RyanConnectionFinder;

public static class RyanairScraper {

    public static string[]? AreAirportsConnectable(string lhs, string rhs)
    {
        const string url = "https://www.ryanair.com/api/views/locate/3/aggregate/all/en";
        var locations = NetworkClient.GetDataAsync<RyanairLocations>(url: url).Result;
        var fromRoutes = locations.Airports.FirstOrDefault(x => x.IataCode == lhs).AirportRoutes;
        if (fromRoutes.Length == 0) {
            return null;
        }
        
        if (fromRoutes.Any(x => x == rhs))
        {
            return [lhs, rhs];
        }

        var toRoutes = locations.Airports.FirstOrDefault(x => x.IataCode == rhs).AirportRoutes;
        
        if (toRoutes.Length == 0) {
            return null;
        }

        var transfer = fromRoutes.Intersect(toRoutes).First();

        return [lhs, transfer, rhs];
    }
}