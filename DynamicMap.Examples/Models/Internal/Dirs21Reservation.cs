namespace DynamicMap.Example.Models.Internal;

public class Dirs21Reservation
{
    public long Id { get; set; }

    public Guid ReservationId { get; set; }

    public string CustomerEmail { get; set; }

    public string CustomerName { get; set; }

    public DateTime ReservationDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Dirs21Room Dirs21Room { get; set; }
}