using DynamicMap.Example.Models.Google;
using DynamicMap.Example.Models.Internal;
using DynamicMappingLibrary.Contracts;
using DynamicMappingLibrary.Strategies;

namespace DynamicMap.Example.Mappers;

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
            Room = (GoogleRoom?)handlerContext.Map(src.Dirs21Room,
                "Model.Room",
                "Google.Room"),
            User = (GoogleUser?)handlerContext.Map(src.Dirs21User,
                "Model.User",
                "Google.User")
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
            Dirs21Room = (Dirs21Room?)handlerContext.Map(dest.Room,
                "Google.Room",
                "Model.Room"),
            Dirs21User = (Dirs21User?)handlerContext.Map(dest.User,
                "Google.User",
                "Model.User")
        };
    }
}