using AutoFixture;
using DynamicMap.Example;
using DynamicMap.Example.Models.Google;
using DynamicMap.Example.Models.Internal;
using DynamicMappingLibrary.Exceptions;
using DynamicMappingLibrary.Handlers;
using DynamicMapTests.Setup;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using TestDirs21Room = DynamicMapTests.Setup.TestDirs21Room;

namespace DynamicMapTests;

public class DynamicMapTests
{
    private readonly ILogger<MapHandler> _logger;
    private readonly MapHandler _mapHandler;
    private readonly TestMapContext _testMapContext;

    public DynamicMapTests()
    {
        _testMapContext = new TestMapContext();
        _logger = new Mock<ILogger<MapHandler>>().Object;
        _mapHandler = new MapHandler(_testMapContext, _logger);
    }

    [Fact]
    public void Map_MapRuleIsNotAdded_ThrowsMappingRulesNotFoundException()
    {
        // Arrange
        var fixture = new Fixture();
        var dirs21Room = fixture.Create<TestDirs21Room>();

        // Act & Assert
        Assert.Throws<MappingRulesNotFoundException>(() => _mapHandler
            .Map(dirs21Room, "Model.Room", "Google.Room"));
    }

    [Fact]
    public void Map_MapRuleIsAddedTwice_ThrowsMappingRulesAlreadyExistsException()
    {
        // Arrange
        _testMapContext.AddMap("Model.Room", "Google.Room",
            new TestDirs21RoomToGoogleRoomMap());

        // Act & Assert
        Assert.Throws<MappingRulesAlreadyExistsException>(() => _testMapContext
            .AddMap("Model.Room", "Google.Room",
                new TestDirs21RoomToGoogleRoomMap()));
    }

    [Fact]
    public void Map_Dirs21ToGoogleRoomMapAdded_ShouldReturnGoogleRoom()
    {
        //Arrange
        _testMapContext.AddMap("Model.Room", "Google.Room",
            new TestDirs21RoomToGoogleRoomMap());

        var fixture = new Fixture();
        var dirs21Room = fixture.Create<TestDirs21Room>();

        var googleRoom = (TestGoogleRoom)_mapHandler.Map(dirs21Room, "Model.Room", "Google.Room");

        Assert.NotNull(googleRoom);
        Assert.Equal(googleRoom.IsAvailable, dirs21Room.IsAvailable);
    }

    [Fact]
    public void Map_Dirs21ToGoogleRoomReverseMapAdded_ShouldReturnDirs21Room()
    {
        //Arrange
        _testMapContext.AddMap("Model.Room", "Google.Room",
                new TestDirs21RoomToGoogleRoomMap())
            .AddReverseMap();

        var fixture = new Fixture();
        var googleRoom = fixture.Create<TestGoogleRoom>();
        googleRoom.RoomId = Guid.NewGuid().ToString();
        googleRoom.RoomType = TestRoomType.Double.ToString();

        var dirs21Room = (TestDirs21Room)_mapHandler.Map(googleRoom, "Google.Room", "Model.Room");

        Assert.NotNull(dirs21Room);
        Assert.Equal(dirs21Room.IsAvailable, googleRoom.IsAvailable);
    }

    [Fact]
    public void Map_NullArgument_ThrowsArgumentNullException()
    {
        //Arrange
        _testMapContext.AddMap("Model.Room", "Google.Room",
            new TestDirs21RoomToGoogleRoomMap());

        TestDirs21Room dirs21Room = null;

        Assert.Throws<ArgumentNullException>(() =>
            _mapHandler.Map(dirs21Room, "Model.Room", "Google.Room"));
    }

    [Fact]
    public void Map_SourceObjectTypeMissMatched_ThrowsInvalidCastException()
    {
        //Arrange
        _testMapContext.AddMap("Model.Room", "Google.Room",
            new TestDirs21RoomToGoogleRoomMap());

        var fixture = new Fixture();
        var googleRoom = fixture.Create<TestGoogleRoom>();

        Assert.Throws<InvalidCastException>(() =>
            _mapHandler.Map(googleRoom, "Model.Room", "Google.Room"));
    }

    [Fact]
    public void Map_NullMapResult_ThrowsNullMappingResultException()
    {
        //Arrange
        var testDirs21ToGoogleRoomMap = new TestDirs21RoomToGoogleRoomMap();

        _testMapContext.AddMap("Model.Room", "Google.Room", (source, handlerContext) => { return null; });

        var fixture = new Fixture();
        var dirs21Room = fixture.Create<TestDirs21Room>();

        Assert.Throws<NullMappingResultException>(() =>
            _mapHandler.Map(dirs21Room, "Model.Room", "Google.Room"));
    }

    [Fact]
    public void Map_RecursionDepthTwo_ShouldMapNestedObject()
    {
        //Arrange
        var exampleMapContext = new ExampleMapContext(2);

        var fixture = new Fixture();
        var dirs21Reservation = fixture.Create<Dirs21Reservation>();

        var mapHandler = new MapHandler(exampleMapContext, _logger);

        var googleReservation =
            (GoogleReservation)mapHandler.Map(dirs21Reservation, "Model.Reservation", "Google.Reservation");

        Assert.NotNull(googleReservation);

        Assert.NotNull(googleReservation.Room);

        Assert.NotNull(googleReservation.User);
    }

    [Fact]
    public void Map_RecursionDepthOne_ShouldNotMapNestedObject()
    {
        //Arrange
        var exampleMapContext = new ExampleMapContext(1);

        var fixture = new Fixture();
        var dirs21Reservation = fixture.Create<Dirs21Reservation>();

        var mapHandler = new MapHandler(exampleMapContext, _logger);

        var googleReservation = (GoogleReservation)mapHandler.Map(dirs21Reservation,
            "Model.Reservation",
            "Google.Reservation");

        Assert.NotNull(googleReservation);

        Assert.Null(googleReservation.Room);

        Assert.Null(googleReservation.User);
    }
}