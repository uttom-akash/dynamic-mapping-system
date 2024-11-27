namespace DynamicMap.Example.Models.Google;

public class GoogleRoom
{
    public string RoomId { get; set; }

    public string RoomType { get; set; }

    public decimal PricePerNight { get; set; }

    public int Capacity { get; set; }

    public bool IsAvailable { get; set; }
}