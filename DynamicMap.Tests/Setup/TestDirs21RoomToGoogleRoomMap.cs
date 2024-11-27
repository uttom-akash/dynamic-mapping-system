using DynamicMappingLibrary.Contracts;
using DynamicMappingLibrary.Strategies;

namespace DynamicMapTests.Setup;

public class TestDirs21RoomToGoogleRoomMap : MapStrategy<TestDirs21Room, TestGoogleRoom>
{
    public override TestGoogleRoom Map(TestDirs21Room src, IMapHandlerContext handlerContext)
    {
        return new TestGoogleRoom
        {
            RoomId = src.RoomId.ToString(),
            RoomType = src.TestRoomType.ToString(),
            PricePerNight = src.PricePerNight,
            Capacity = src.Capacity,
            IsAvailable = src.IsAvailable
        };
    }

    public override TestDirs21Room ReverseMap(TestGoogleRoom target, IMapHandlerContext handlerContext)
    {
        return new TestDirs21Room
        {
            RoomId = Guid.Parse(target.RoomId),
            TestRoomType = Enum.Parse<TestRoomType>(target.RoomType), //todo: validate
            PricePerNight = target.PricePerNight,
            Capacity = target.Capacity,
            IsAvailable = target.IsAvailable
        };
    }
}