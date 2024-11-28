namespace DynamicMap.Example.Models.External;

public class GoogleRoom
{
    public string RoomId { get; set; }

    public string RoomType { get; set; }

    public decimal PricePerNight { get; set; }

    public int Capacity { get; set; }

    public bool IsAvailable { get; set; }
}