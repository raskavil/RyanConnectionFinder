namespace RyanConnectionFinder;

public class AvailabilitiesRequest
{
    
    public static string[]? FlightDates(string[] route)
    {
        string[]? MergedDates = null; 
        string urlbase = "https://www.ryanair.com/api/farfnd/3/oneWayFares/{0}/{1}/availabilities";
        
        for (int i = 0; i < route.Length-1; i++) 
        {
            string url = urlbase.Replace("{0}", route[i]).Replace("{1}", route[i + 1]);
            var dates = NetworkClient.GetDataAsync<string[]>(url: url).Result;

            if (i == 0)
            {
                MergedDates = dates;
            }
                
            else
            {
                MergedDates = MergedDates.Intersect(dates).ToArray();
                if (MergedDates.Length == 0)
                {
                    break;
                }
            }
            
        }
        return MergedDates?.ToArray();
        
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
