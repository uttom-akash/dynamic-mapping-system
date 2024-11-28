using DynamicMap.Example.Mapping.Strategies;
using DynamicMappingLibrary.Configurations;

namespace DynamicMap.Example.Mapping;

public class ExampleMapConfiguration : MapConfiguration
{
    public ExampleMapConfiguration(int maxRecursionDepth = 3)
    {
        MaxRecursionDepth = maxRecursionDepth;

        AddMap("Dirs21.Reservation",
                "Google.Reservation",
                new Dirs21ToGoogleReservationMap())
            .AddReverseMap();

        AddMap("Dirs21.Room",
                "Google.Room",
                new Dirs21RoomToGoogleRoomMap())
            .AddReverseMap();


        AddMap("Dirs21.User",
                "Google.User",
                new Dirs21UserToGoogleUserMap())
            .AddReverseMap();
    }
}