namespace DynamicMap.Example;

public class GoogleClientMock
{
    public string GetGoogleReservation()
    {
        // Hard-coded JSON-like string representing a GoogleReservation
        return @"{
            ""ExternalId"": ""C56a418065aa426ca9455fd21deC0538"",
            ""UserEmail"": ""user@example.com"",
            ""UserName"": ""John Doe"",
            ""Date"": ""2024-11-27T12:34:56"",
            ""Room"": {
                ""RoomId"": ""C56a418065aa426ca9455fd21deC0538"",
                ""RoomType"": ""Single"",
                ""PricePerNight"": 150.00,
                ""Capacity"": 2,
                ""IsAvailable"": true
            }
        }";
    }

    public int PostGoogleReservation(string json)
    {
        return 200;
    }
}