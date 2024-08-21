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