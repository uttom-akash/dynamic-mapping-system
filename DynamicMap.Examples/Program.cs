using DynamicMap.Example.Mapping;
using DynamicMap.Example.Models.External;
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

        // When we haven't implemented third party data model type in our project 

        var dirs21User = FecthGoogleUserWithoutThirdPartyModel(googleClientMock, mapHandler);

        PostDirs21UserWithoutThirdPartyModel(dirs21User, googleClientMock, mapHandler);

        //When we have implemented third party data model type in our project

        var dirs21Reservation = FecthGoogeReservationData(googleClientMock, mapHandler);

        PostDirs21ReservationDataToGoogle(dirs21Reservation, mapHandler, googleClientMock);
    }

    private static Dirs21User FecthGoogleUserWithoutThirdPartyModel(GoogleClientMock googleClientMock,
        IMapHandler mapHandler)
    {
        var googleUserJsonString = googleClientMock.FetchGoogleUser();

        var dirs21User = (Dirs21User)mapHandler.Map(googleUserJsonString, "Google.User", "Dirs21.User");

        Console.WriteLine($"{nameof(FecthGoogleUserWithoutThirdPartyModel)} - Dirs1 Format :");
        Console.WriteLine($"User FullName: {dirs21User.FullName}\n");

        return dirs21User;
    }

    private static void PostDirs21UserWithoutThirdPartyModel(Dirs21User dirs21User, GoogleClientMock googleClientMock,
        IMapHandler mapHandler)
    {
        var googleUserJsonString = (string)mapHandler.Map(dirs21User, "Dirs21.User", "Google.User");

        Console.WriteLine($"{nameof(PostDirs21UserWithoutThirdPartyModel)} - Google Format:");
        Console.WriteLine($"User: \n{googleUserJsonString}\n");

        googleClientMock.PostGoogleUser(googleUserJsonString);
    }

    private static object FecthGoogeReservationData(GoogleClientMock googleClientMock,
        IMapHandler mapHandler)
    {
        var googleReservation = googleClientMock.GetGoogleReservation();

        if (googleReservation == null) throw new NullReferenceException("Google reservation was null");

        var dirs21Reservation = (Dirs21Reservation)mapHandler.Map(googleReservation,
            "Google.Reservation",
            "Dirs21.Reservation");

        Console.WriteLine($"{nameof(FecthGoogeReservationData)} - Dirs21 Format :");
        Console.WriteLine($"Reservation Id: {dirs21Reservation.ReservationId}\n");

        return dirs21Reservation;
    }

    private static void PostDirs21ReservationDataToGoogle(object dirs21Reservation,
        IMapHandler mapHandler,
        GoogleClientMock googleClientMock)
    {
        var googleReservation = (GoogleReservation)mapHandler.Map(dirs21Reservation,
            "Dirs21.Reservation",
            "Google.Reservation");

        Console.WriteLine($"{nameof(PostDirs21ReservationDataToGoogle)} - Google Format:");
        Console.WriteLine($"Reservation Id: {googleReservation.ExternalId}\n");

        googleClientMock.PostGoogleReservation(googleReservation);
    }

    private static ServiceProvider ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDynamicMap(typeof(ExampleMapConfiguration))
            .AddSingleton<GoogleClientMock>()
            .AddLogging();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        return serviceProvider;
    }
}