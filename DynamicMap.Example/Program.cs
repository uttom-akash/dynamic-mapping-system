// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using DynamicMap.Example.Models.Google;
using DynamicMap.Example.Models.Internal;
using DynamicMappingLibrary;
using DynamicMappingLibrary.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicMap.Example;

internal static class Program
{
    private static void Main(string[] args)
    {
        var serviceProvider = ConfigureServices();
        var mapHandler = serviceProvider.GetRequiredService<IMapHandler>();
        var googleClientMock = serviceProvider.GetRequiredService<GoogleClientMock>();

        var dirs21Reservation = FecthGoogeReservationDataIntoDirs21Reservation(googleClientMock, mapHandler);

        PostDirs21ReservationAsGoogleReservation(mapHandler, dirs21Reservation, googleClientMock);
    }

    private static object FecthGoogeReservationDataIntoDirs21Reservation(GoogleClientMock googleClientMock,
        IMapHandler mapHandler)
    {
        var googleClientReservationJson = googleClientMock.GetGoogleReservation();
        var googleReservation = JsonSerializer.Deserialize<GoogleReservation>(googleClientReservationJson);

        var dirs21Reservation = (Dirs21Reservation)mapHandler.Map(googleReservation,
            "Google.Reservation",
            "Model.Reservation");

        Console.WriteLine($"{nameof(FecthGoogeReservationDataIntoDirs21Reservation)}:");
        Console.WriteLine($"Reservation Id: {dirs21Reservation.ReservationId}");
        Console.WriteLine($"Room Id: {dirs21Reservation?.Dirs21Room?.RoomId}\n");

        return dirs21Reservation;
    }

    private static void PostDirs21ReservationAsGoogleReservation(IMapHandler mapHandler, object dirs21Reservation,
        GoogleClientMock googleClientMock)
    {
        var googleReservation = (GoogleReservation)mapHandler.Map(dirs21Reservation,
            "Model.Reservation",
            "Google.Reservation");

        var googleReservationJson = JsonSerializer.Serialize(googleReservation);

        Console.WriteLine($"{nameof(PostDirs21ReservationAsGoogleReservation)}:");
        Console.WriteLine(googleReservationJson);

        googleClientMock.PostGoogleReservation(googleReservationJson);
    }

    private static ServiceProvider ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDynamicMap(new ExampleMapContext())
            .AddSingleton<GoogleClientMock>()
            .AddLogging();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        return serviceProvider;
    }
}