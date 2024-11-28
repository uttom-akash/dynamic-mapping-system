using System.Text.Json;
using DynamicMap.Example.Models.External;

namespace DynamicMap.Example;

public class GoogleClientMock
{
    public GoogleReservation GetGoogleReservation()
    {
        // Hard-coded JSON-like string representing a GoogleReservation
        var googleClientReservationJson = @"{
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

        return JsonSerializer.Deserialize<GoogleReservation>(googleClientReservationJson);
    }

    public int PostGoogleReservation(GoogleReservation googleReservation)
    {
        var googleReservationJson = JsonSerializer.Serialize(googleReservation);

        // call google with googleReservationJson

        return 200;
    }

    public string FetchGoogleUser()
    {
        // Hard-coded JSON-like string representing a GoogleUser
        return @"{
                ""UserId"": ""C56a418065aa426ca9455fd21deC0538"",
                ""Email"":  ""user@example.com"",
                ""FirstName"":  ""Foo"",
                ""LastName"":  ""Bar"",
            }";
    }

    public int PostGoogleUser(string json)
    {
        return 200;
    }
}