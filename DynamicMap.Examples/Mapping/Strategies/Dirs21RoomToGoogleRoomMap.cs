using DynamicMap.Example.Models.External;
using DynamicMap.Example.Models.Internal;
using DynamicMappingLibrary.Contracts;
using DynamicMappingLibrary.Strategies;

namespace DynamicMap.Example.Mapping.Strategies;

public class Dirs21RoomToGoogleRoomMap : MapStrategy<Dirs21Room, GoogleRoom>
{
    public override GoogleRoom Map(Dirs21Room src, IMapHandlerContext handlerContext)
    {
        return new GoogleRoom
        {
            RoomId = src.RoomId.ToString(),
            RoomType = src.RoomType.ToString(),
            PricePerNight = src.PricePerNight,
            Capacity = src.Capacity,
            IsAvailable = src.IsAvailable
        };
    }

    public override Dirs21Room ReverseMap(GoogleRoom src, IMapHandlerContext handlerContext)
    {
        Guid.TryParse(src.RoomId, out var roomId);
        Enum.TryParse(src.RoomType, true, out RoomType roomType);

        return new Dirs21Room
        {
            RoomId = roomId,
            RoomType = roomType,
            PricePerNight = src.PricePerNight,
            Capacity = src.Capacity,
            IsAvailable = src.IsAvailable
        };
    }
}