namespace DynamicMapTests.Setup;

public enum TestRoomType
{
    Single,
    Double,
    Suite,
    King,
    Queen,
    Family
}

public class TestDirs21Room
{
    public long Id { get; set; }

    public Guid RoomId { get; set; }

    public TestRoomType TestRoomType { get; set; }

    public decimal PricePerNight { get; set; }

    public int Capacity { get; set; }

    public bool IsAvailable { get; set; }
}