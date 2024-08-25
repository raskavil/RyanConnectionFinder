namespace RyanConnectionFinder;

public class AvailabilitiesRequest
{
    
    public static string[]? FlightDates(string lhs, string transfer, string rhs) 
    { 
       /* var airports = (lhs, transfer, rhs);
        for (int i = 0; i < 2; i++) {
            string url = "https://www.ryanair.com/api/farfnd/3/oneWayFares/{lhs}/{transfer}/availabilities"
        }
        */

        
        string url1 = "https://www.ryanair.com/api/farfnd/3/oneWayFares/{lhs}/{transfer}/availabilities";
        string url2 = "https://www.ryanair.com/api/farfnd/3/oneWayFares/{transfer}/{rhs}/availabilities";
        
        string[] dates1 = NetworkClient.GetDataAsync<string[]>(url: url1).Result;
        string[] dates2 = NetworkClient.GetDataAsync<string[]>(url: url2).Result;

        var MergedDates = dates1.Intersect(dates2);
        return MergedDates.ToArray();
    }
}
