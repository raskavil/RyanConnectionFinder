namespace RyanConnectionFinder;

public static class AvailabilitiesRequest
{
    
    public static string[]? FlightDates(string[] route)
    {
        string[]? mergedDates = null; 
        
        for (var i = 0; i < route.Length-1; i++) 
        {
            var url = $"https://www.ryanair.com/api/farfnd/3/oneWayFares/{route[i]}/{route[i+1]}/availabilities";
            var dates = NetworkClient.GetDataAsync<string[]>(url: url).Result;
            
            if (dates == null)
            {
                return null;
            }

            if (i == 0)
            {
                mergedDates = dates;
            }
            else
            {
                mergedDates = mergedDates?.Intersect(dates).ToArray();
                if (mergedDates?.Length == 0)
                {
                    return null;
                }
            }
            
        }
        return mergedDates?.ToArray();
        
        /*
        OLD IMPLEMENTATION:
        string url1 = "https://www.ryanair.com/api/farfnd/3/oneWayFares/{lhs}/{transfer}/availabilities";
        string url2 = "https://www.ryanair.com/api/farfnd/3/oneWayFares/{transfer}/{rhs}/availabilities";
        
        string[] dates1 = NetworkClient.GetDataAsync<string[]>(url: url1).Result;
        string[] dates2 = NetworkClient.GetDataAsync<string[]>(url: url2).Result;

        var MergedDates = dates1.Intersect(dates2);
        return MergedDates.ToArray();
        
        */
    }
}
