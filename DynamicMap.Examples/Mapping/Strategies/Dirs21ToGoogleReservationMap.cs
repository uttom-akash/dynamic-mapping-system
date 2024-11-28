using DynamicMap.Example.Models.External;
using DynamicMap.Example.Models.Internal;
using DynamicMappingLibrary.Contracts;
using DynamicMappingLibrary.Strategies;

namespace DynamicMap.Example.Mapping.Strategies;

public class Dirs21ToGoogleReservationMap : MapStrategy<Dirs21Reservation, GoogleReservation>
{
    public override GoogleReservation Map(Dirs21Reservation src, IMapHandlerContext handlerContext)
    {
        return new GoogleReservation
        {
            ExternalId = src.ReservationId.ToString(),
            UserEmail = src.CustomerEmail,
            UserName = src.CustomerName,
            Date = src.ReservationDate,
            Room = (GoogleRoom)handlerContext.Map(src.Dirs21Room,
                "Dirs21.Room",
                "Google.Room")
        };
    }

    public override Dirs21Reservation ReverseMap(GoogleReservation dest, IMapHandlerContext handlerContext)
    {
        return new Dirs21Reservation
        {
            ReservationId = Guid.Parse(dest.ExternalId),
            CustomerEmail = dest.UserEmail,
            CustomerName = dest.UserName,
            ReservationDate = dest.Date,
            CreatedAt = dest.Date,
            UpdatedAt = DateTime.UtcNow,
            Dirs21Room = (Dirs21Room)handlerContext.Map(dest.Room,
                "Google.Room",
                "Dirs21.Room")
        };
    }
}