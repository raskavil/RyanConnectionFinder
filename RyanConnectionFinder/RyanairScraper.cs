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
                    ["User-Agent"] = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36",
                    ["Cookie"] = "rid=00f28350-c157-4f29-bec9-9a5c40dcba2d; mkt=/cz/cs/; STORAGE_PREFERENCES={\"STRICTLY_NECESSARY\":true,\"PERFORMANCE\":true,\"FUNCTIONAL\":true,\"TARGETING\":true,\"SOCIAL_MEDIA\":true,\"PIXEL\":true,\"__VERSION\":3}; RY_COOKIE_CONSENT=true; bid_FRwdAp7a9G2cnLnTsgyBNeduseKcPcRy=4f5c5a52-95b0-4d03-8b7d-95c1a0e03607; sid=4f5c5a52-95b0-4d03-8b7d-95c1a0e03607; agsd=DY-Fn1Mo27x_S_91OjzQ7PmZn06Zd0Dca-qahGxPazmzjqMH; _cc=AZC73zDQ3aieDymyeiofYx4r; _cid_cc=AZC73zDQ3aieDymyeiofYx4r; fr-correlation-id=00769eff-721d-4470-97cd-687a30ee14ca; _gid=GA1.2.1249461912.1724594535; _gat_gtag_UA_153938230_2=1; _ga_YBBVD7Z3XL=GS1.1.1724594535.7.1.1724594554.0.0.0; rid.sig=jyna6R42wntYgoTpqvxHMK7H+KyM6xLed+9I3KsvYZaVt7P36AL6zp9dGFPu5uVxaIiFpNXrszr+LfNCdY3IT3oCSYLeNv/ujtjsDqOzkY5JmUFsCdAEz3kpPbhCUwiArp5oaa75tpJtO3kFwYQ8l0DbH67AtcN/PMbniLsiM5qn+2AjrrtoNJicE3ZQwFHVipe4lWPSRfq2OIyUrlFhwEDt20+wCX7l1mCubNXtG6nZrUA07sFUFhn4RUxnjwjJ6d9qjjBasXLvYSqyYN7UacgfF9y/scZw/NZV1ONoKUzfsS/OAhQjV3kvIsD6R6JolIyrOljr0KbVF3osjtw6UmmLHoSFSunHQFvAG4jH7KrpDewd6A6tfKbrmKH2W6KfTjm4+f+pydiHpCG6BECH6b6rHAjNB+OHdntwtjCbItRSW/CdjMSoOkM+tB7NZw9QGa67ewPpWGIu7ooPzNlFqhagFccALCb3lwHi1O6MSakFLUkaWnH3VtwYn5SmlogFtEhpPLC/2y8hpzhu31xIt83/lzcJMOMk/Likb/DGQ0lkf7cII/XrFToo8jNY1vAjKdfnwU68bB/b5j/CMG28e7amDoCGalxPrj05VvluwOO+OzafC7F4ZKdtaL1/6DI7oU8LuZFlwvlwgJVexKXEa5PA0b6cFE4HfHSQ2FD99UNLYDjaKmMaZr+w98V93KZK/4Lu36vLYCfAyf+IxHeidnZXOBiDbbSNEoR8UgQ6FiKxTO80TIYWAqhZthq3aeMYMFiGXJ1tEeY3mSu2qPaaKCz9w8wxjNR4MSddpkguJWP+pE7VlNacRY5L9s/FN6RjC3VKPDl65WdpSeE5LAVsZt7+J2nnHFwl2yyc1IEpl/Tg9BUIFhVyeb8CgwpJQL8tBMYHf090LK21RYiBouy+RKrZhmt51oqmYvZKT6QCGNeN/wz8MOn+l08HKGW+OpjjsYUInwuAnifaQkah5K15zjlyrNJ41QAJ3ZsKxEXUhezSiywVXYENR8zI5F63rWVv2smialOcRWMKO/QLHv3FO9BNlsxJCYDgX6aRIL7F3fKDJMtA5av3StXOHJh5qgWTQb3EihGuItJt7MhaNBycORmM4O+0/OOazLfXWRpKqnJF3PKwbNuzV/wQ6bJg6q2WRx7wYc9gSr045rgj4Vh2sWaEsRwIb0Nt8BD5QTM2wxPX5lPJgwzmqSMOwW3ZL/6O; _ga=GA1.2.2049226240.1713628915"
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
    }
}