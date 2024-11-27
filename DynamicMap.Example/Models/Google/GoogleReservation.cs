namespace DynamicMap.Example.Models.Google;

public class GoogleReservation
{
    public string ExternalId { get; set; }
    public string UserEmail { get; set; }
    public string UserName { get; set; }
    public DateTime Date { get; set; }
    public GoogleRoom? Room { get; set; }

    public GoogleUser? User { get; set; }
}