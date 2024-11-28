namespace DynamicMap.Example.Models.Internal;

public enum RoomType
{
    Single,
    Double,
    Suite,
    King,
    Queen,
    Family
}

public class Dirs21Room
{
    public long Id { get; set; }

    public Guid RoomId { get; set; }

    public RoomType RoomType { get; set; }

    public decimal PricePerNight { get; set; }

    public int Capacity { get; set; }

    public bool IsAvailable { get; set; }
}