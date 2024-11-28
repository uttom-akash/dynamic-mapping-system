namespace DynamicMap.Example.Models.External;

public class GoogleReservation
{
    public string ExternalId { get; set; }

    public string UserEmail { get; set; }

    public string UserName { get; set; }

    public DateTime Date { get; set; }

    public GoogleRoom Room { get; set; }
}