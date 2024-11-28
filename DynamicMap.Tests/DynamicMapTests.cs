using AutoFixture;
using DynamicMap.Example.Mapping;
using DynamicMap.Example.Mapping.Strategies;
using DynamicMap.Example.Models.External;
using DynamicMap.Example.Models.Internal;
using DynamicMappingLibrary.Common.Exceptions;
using DynamicMappingLibrary.Configurations;
using DynamicMappingLibrary.Handlers;
using Xunit;

namespace DynamicMapTests;

public class DynamicMapTests
{
    [Fact]
    public void ShouldThrowArgumentException_WhenRecursionDepthLessThanOne()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new ExampleMapConfiguration(0));
    }

    [Fact]
    public void ShouldThrowMappingRulesNotFoundException_WhenMapRuleIsNotAdded_()
    {
        // Arrange
        var testMapConfiguration = new MapConfiguration();
        var mapHandler = new MapHandler(testMapConfiguration);

        var fixture = new Fixture();
        var dirs21Room = fixture.Create<Dirs21Room>();

        // Act & Assert
        Assert.Throws<MappingRulesNotFoundException>(() => mapHandler
            .Map(dirs21Room, "Dirs21.Room", "Google.Room"));
    }

    [Fact]
    public void ShouldThrowMappingRulesAlreadyExistsException_WhenMapRuleIsAddedTwice()
    {
        // Arrange
        var testMapConfiguration = new MapConfiguration();
        var mapHandler = new MapHandler(testMapConfiguration);

        testMapConfiguration.AddMap("Dirs21.Room", "Google.Room",
            new Dirs21RoomToGoogleRoomMap());

        // Act & Assert
        Assert.Throws<MappingRulesAlreadyExistsException>(() => testMapConfiguration
            .AddMap("Dirs21.Room", "Google.Room",
                (src, ctx) => null));
    }

    [Fact]
    public void ShouldReturnGoogleRoom_WhenDirs21ToGoogleRoomMapIsConfigured()
    {
        //Arrange
        var testMapConfiguration = new MapConfiguration();
        var mapHandler = new MapHandler(testMapConfiguration);

        testMapConfiguration.AddMap("Dirs21.Room", "Google.Room",
            new Dirs21RoomToGoogleRoomMap());

        var fixture = new Fixture();
        var dirs21Room = fixture.Create<Dirs21Room>();

        // Act
        var googleRoom = (GoogleRoom)mapHandler.Map(dirs21Room,
            "Dirs21.Room",
            "Google.Room");

        //Assert
        Assert.NotNull(googleRoom);
        Assert.Equal(googleRoom.IsAvailable, dirs21Room.IsAvailable);
    }

    [Fact]
    public void ShouldReturnDirs21Room_WhenDirs21ToGoogleRoomReverseMapIsAdded()
    {
        //Arrange
        var testMapConfiguration = new MapConfiguration();
        var mapHandler = new MapHandler(testMapConfiguration);
        testMapConfiguration.AddMap("Dirs21.Room", "Google.Room",
                new Dirs21RoomToGoogleRoomMap())
            .AddReverseMap();

        var fixture = new Fixture();
        var googleRoom = fixture.Create<GoogleRoom>();
        googleRoom.RoomId = Guid.NewGuid().ToString();
        googleRoom.RoomType = RoomType.Double.ToString();

        // Act
        var dirs21Room = (Dirs21Room)mapHandler.Map(googleRoom,
            "Google.Room",
            "Dirs21.Room");

        //Assert
        Assert.NotNull(dirs21Room);
        Assert.Equal(dirs21Room.IsAvailable, googleRoom.IsAvailable);
    }

    [Fact]
    public void ShouldThrowArgumentNullException_WhenNullIsProvidedAsSource()
    {
        //Arrange
        var testMapConfiguration = new MapConfiguration();
        var mapHandler = new MapHandler(testMapConfiguration);

        testMapConfiguration.AddMap("Dirs21.Room", "Google.Room",
            new Dirs21RoomToGoogleRoomMap());

        Dirs21Room? dirs21Room = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            mapHandler.Map(dirs21Room, "Dirs21.Room", "Google.Room"));
    }

    [Fact]
    public void ShouldThrowInvalidCastException_WhenSourceObjectTypeIsNotMatched()
    {
        //Arrange
        var testMapConfiguration = new MapConfiguration();
        var mapHandler = new MapHandler(testMapConfiguration);

        testMapConfiguration.AddMap("Dirs21.Room", "Google.Room",
            new Dirs21RoomToGoogleRoomMap());

        var fixture = new Fixture();
        var googleRoom = fixture.Create<GoogleRoom>();

        // Act & Assert
        Assert.Throws<InvalidCastException>(() =>
            mapHandler.Map(googleRoom, "Dirs21.Room", "Google.Room"));
    }

    [Fact]
    public void ShouldThrowNullMappingResultException_WhenProvidedMappedResultIsNull()
    {
        //Arrange
        var testMapConfiguration = new MapConfiguration();
        var mapHandler = new MapHandler(testMapConfiguration);

        testMapConfiguration.AddMap(
            "Dirs21.Room",
            "Google.Room", (source, handlerContext) => { return null; });

        var fixture = new Fixture();
        var dirs21Room = fixture.Create<Dirs21Room>();

        // Act & Assert
        Assert.Throws<NullMappingResultException>(() =>
            mapHandler.Map(dirs21Room, "Dirs21.Room", "Google.Room"));
    }

    [Fact]
    public void ShouldMapOneLevelNestedObject_WhenRecursionDepthIsTwo()
    {
        //Arrange
        var exampleMapContext = new ExampleMapConfiguration(2);
        var mapHandler = new MapHandler(exampleMapContext);

        var fixture = new Fixture();
        var dirs21Reservation = fixture.Create<Dirs21Reservation>();

        // Act
        var googleReservation =
            (GoogleReservation)mapHandler.Map(dirs21Reservation,
                "Dirs21.Reservation", "Google.Reservation");

        //Assert
        Assert.NotNull(googleReservation);

        Assert.NotNull(googleReservation.Room);
    }

    [Fact]
    public void ShouldNotMapOneLevelNestedObject_WhenRecursionDepthLessThanTwo()
    {
        //Arrange
        var exampleMapContext = new ExampleMapConfiguration(1);
        var mapHandler = new MapHandler(exampleMapContext);

        var fixture = new Fixture();
        var dirs21Reservation = fixture.Create<Dirs21Reservation>();

        // Act
        var googleReservation = (GoogleReservation)mapHandler.Map(dirs21Reservation,
            "Dirs21.Reservation",
            "Google.Reservation");

        //Assert
        Assert.NotNull(googleReservation);

        Assert.Null(googleReservation.Room);
    }
}