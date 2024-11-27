using DynamicMap.Example.Mappers;
using DynamicMap.Example.Models.Google;
using DynamicMap.Example.Models.Internal;
using DynamicMappingLibrary.Context;
using DynamicMappingLibrary.Contracts;
using DynamicMappingLibrary.Helpers;

namespace DynamicMap.Example;

public class ExampleMapContext : MapContext
{
    public ExampleMapContext(int maxRecursionDepth = 3)
    {
        MaxRecursionDepth = maxRecursionDepth;

        AddMap("Model.Reservation",
                "Google.Reservation",
                new Dirs21ToGoogleReservationMap())
            .AddReverseMap();

        AddMap("Model.Room",
                "Google.Room",
                new Dirs21RoomToGoogleRoomMap())
            .AddReverseMap();


        AddMap("Model.User",
            "Google.User", MapToGoogleUser);

        AddMap("Google.User",
            "Model.User", MapToGoogleUser);
    }

    public GoogleUser MapToGoogleUser(object source, IMapHandlerContext mapHandlerContext)
    {
        var src = TypeCastUtil.CastTypeBeforeMap<Dirs21User>(source);

        return new GoogleUser
        {
            UserId = src.UserId.ToString(),
            Email = src.Email,
            FirstName = src.FullName.Split(" ").First(),
            LastName = src.FullName.Split(" ").Last()
        };
    }

    public Dirs21User MapToDirs21User(object source, IMapHandlerContext mapHandlerContext)
    {
        var src = TypeCastUtil.CastTypeBeforeMap<GoogleUser>(source);
        Guid.TryParse(src.UserId, out var userId);
        return new Dirs21User
        {
            UserId = userId,
            Email = src.Email,
            FullName = src.FirstName + " " + src.LastName
        };
    }
}