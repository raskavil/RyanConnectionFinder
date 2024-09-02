using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;

namespace RyanConnectionFinder;

public static class RyanairScraper {

    public static string[][]? Routes(string lhs, string rhs)
    {
        const string url = "https://www.ryanair.com/api/views/locate/3/aggregate/all/en";
        var locations = NetworkClient.GetDataAsync<RyanairLocations>(url: url).Result;
        var fromRoutes = locations.Airports.FirstOrDefault(x => x.IataCode == lhs).AirportRoutes;
        if (fromRoutes.Length == 0) {
            return null;
        }
        
        if (fromRoutes.Any(x => x == rhs))
        {
            return [[lhs, rhs]];
        }

        var toRoutes = locations.Airports.FirstOrDefault(x => x.IataCode == rhs).AirportRoutes;
        
        if (toRoutes.Length == 0) {
            return null;
        }

        var transfer = fromRoutes.Intersect(toRoutes).ToArray();

        if (transfer.Length == 0)
        {
            #warning Currently only route of three airports is considered. Open to additional implementation.
            return null;
            
        }
        
        return transfer.Select(x => new string[] { lhs, x, rhs }).ToArray();
    }

    public static List<List<Connection>> Trips(string[] route, string[] dates)
    {
        var returnValue = new List<List<Connection>>();
        foreach (var date in dates)
        {
            var connections = new List<Connection>();
            for (var i = 0; i < route.Length - 1; i++)
            {
                var from = route[i];
                var to = route[i + 1];
            
                var url = "https://www.ryanair.com/api/booking/v4/cs-cz/availability?ADT=1&Origin=" 
                          + from + "&Destination=" + to + "&DateOut=" + date + "&ToUs=AGREED";
                var headers = new Dictionary<string, string>
                {
                    ["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36",
                    ["Cookie"] = "fr-correlation-id=129fb6d7-790e-4685-be8e-40c0535adef9; rid=276e7386-bf03-4d84-a193-8ac3e2e3d4b2; mkt=/cz/cs/; STORAGE_PREFERENCES={\"STRICTLY_NECESSARY\":true,\"PERFORMANCE\":true,\"FUNCTIONAL\":true,\"TARGETING\":true,\"SOCIAL_MEDIA\":true,\"PIXEL\":true,\"__VERSION\":3}; RY_COOKIE_CONSENT=true; sid=3fdecac9-363b-4585-88cd-9148b2b5277e; .AspNetCore.Session=CfDJ8HAoCIDr1QdKiQbuuuFaZVWUWS%2BBMvLQnyxQEnHDnGmtXzZq2ruTDs5Pab%2B9lRDwxOrr%2FECPa9U6n5wLw6LbfHbtPbm6qQ9fADswc47sLJMRRlDnpyDU0EHBNtJ6Bcsb0Fkuy%2BEwo1CApC1TdUtcvq4PVjNJpe9RS4K52vcbKaoS; rid.sig=jyna6R42wntYgoTpqvxHMK7H+KyM6xLed+9I3KsvYZaVt7P36AL6zp9dGFPu5uVxaIiFpNXrszr+LfNCdY3IT3oCSYLeNv/ujtjsDqOzkY5JmUFsCdAEz3kpPbhCUwiArp5oaa75tpJtO3kFwYQ8l0DbH67AtcN/PMbniLsiM5qn+2AjrrtoNJicE3ZQwFHVipe4lWPSRfq2OIyUrlFhwEDt20+wCX7l1mCubNXtG6nZrUA07sFUFhn4RUxnjwjJ6d9qjjBasXLvYSqyYN7UacgfF9y/scZw/NZV1ONoKUzfsS/OAhQjV3kvIsD6R6JolIyrOljr0KbVF3osjtw6UmmLHoSFSunHQFvAG4jH7KqF47gePsLoTHTWmJyq4/ERGaFM1z3EnLFK80SZBLUvrrVu/1JT5NHyDWTEu8kYGKnrJBUl4isQGedMt2fD1kx22C/sOgtp6fL5VacSX1fV8N32MEoSb3KiCBsJe2ljbtZe8hP3WubeAPEYv4xfIBB8todkjoS9zccRRvlmY2mTMwD44gYZM6SfzjEdeI9zjH54P782Rsvm3ynbeUVYRls2sln43nMCAV1sEaB/qM9kO0HO6VaTvTjTXWFzUZii6+TcCD6U2iExfxY7qcGGcQ+5I06pWAed2z3VsLtwNGK6jpRiEPfAQckXOe4JFbxX+6ph31cqV+jHtDfgdTm/Ms2E6KkxMeEa2Rh1MKqKMARj+w3XHWhskLuLXXE48K6ztXK8BouHCQ98v6a1WiFnJm3fSgtl6kqH14H58b/TpxU/ouoipG+ZpFItuLKaVLNF8slP32hoQUGDIiZtt3s6E8wlJoeI3KEUpeEKyZnT4yCPGy++Ne7Cch1L+vFoSvxc9M0y47WFuQCS9yoRHaw0R/2fXwSxoM3hMaPg+KbCkxKlIulvHzwR3T8/so6QmSZdDFk="
                };
                
                var response = NetworkClient.GetDataAsync<RyanairSearch>(url: url, headers: headers).Result;
                var flight = response.Trips.FirstOrDefault().Dates.FirstOrDefault().Flights.FirstOrDefault();
                
                if (connections.Count != 0 && String.CompareOrdinal(connections.Last().ArrivalUtc, flight.TimeUtc.First()) > 0)
                { 
                    break;
                }

                connections.Add(new Connection()
                {
                    ArrivalUtc = flight.TimeUtc.Last(),
                    DepartureUtc = flight.TimeUtc.First(),
                    Arrival = flight.Time.Last(),
                    Departure = flight.Time.First(),
                    From = from,
                    To = to
                });
            }

            if (connections.Count == route.Length - 1)
            {
                returnValue.Add(connections);
            }
        }

        return returnValue;
    }
    
    public struct Connection
    {
        public string From;
        public string To;
        public string DepartureUtc;
        public string ArrivalUtc;
        public string Departure;
        public string Arrival;
    }
}