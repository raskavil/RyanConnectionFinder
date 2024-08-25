namespace RyanConnectionFinder;

public struct RyanairLocations
{
    public RyanairAirport[] Airports { get; set; }
}
public struct RyanairAirport
{
    public string IataCode { get; set; }
    public string Name { get; set; }
    public string[] Routes { get; set; }

    public string[] AirportRoutes
    {
        get
        {
            return Routes
                .Where(x => x.StartsWith("airport:"))
                .Select(x => x.Replace("airport:", ""))
                .ToArray();
        }
    }
}

public struct RyanairSearch
{
    public RyanairTrip[] Trips { get; set; }
}

public struct RyanairTrip
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public RyanairFlightsDate[] Dates { get; set; }
}

public struct RyanairFlightsDate
{
    public string DateOut { get; set; }
    public RyanairFlight[] Flights { get; set; }
}

public struct RyanairFlight
{
    public string[] Time { get; set; }
    public string[] TimeUtc { get; set; }
}