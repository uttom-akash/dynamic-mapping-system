# Dynamic Mapping Library

## Overview
The **Dynamic Mapping Library** is a hight performant and light weight library designed to simplify data transformations between different structures.

---

## Table of Contents
- [Dynamic Mapping Library](#dynamic-mapping-library)
  - [Overview](#overview)
  - [Table of Contents](#table-of-contents)
  - [Features](#features)
  - [Installation](#installation)
  - [Quick Start](#quick-start)
    - [Another good way:](#another-good-way)
  - [Nested Map](#nested-map)
    - [Max Recursion Depth](#max-recursion-depth)
  - [Ways to Add Map Rules](#ways-to-add-map-rules)
      - [Class based mapper](#class-based-mapper)
      - [Functional mapper](#functional-mapper)

---

## Features
- **High Performance**: Designed for optimal performance avoiding reflection.  
<!-- - **Type Safety**: Ensures compile-time checks for mapping validity.   -->
- **Customizable**: Define custom mapping logic with ease.  
- **Supports Nested Structures**: Works with complex, multi-level data models.
- **External Dependency**: No use of any third party package.

---

## Installation

Add class library `DynamicMap.Core` to your application.

---
## Quick Start

First, specify the configuration and map after that.

```
// configuration

var mapConfiguration = new MapConfiguration()

mapConfiguration.AddMap("External.User", "Internal.User", (source, handlerContext) => 
{
    var src = TypeCastUtil.CastTypeBeforeMap<GoogleUser>(source);
    
    return new Internal.User
    {
        UserId = Guid.Parse(src.UserId),
        FullName = src.FirstName + " " + src.LastName
    };
})


// map

var mapHandler = new MapHandler(mapConfiguration)

var internalUser = (Internal.User)mapHandler.Map(externalUser, "External.User", "Internal.User");

```

### Another good way:

**Map Strategy:** Define the strategies to map between `Dirs21Room` and `GoogleRoom`. 

```
public class Dirs21RoomToGoogleRoomMap : MapStrategy<Dirs21Room, GoogleRoom>
{
    public override GoogleRoom Map(Dirs21Room src, IMapHandlerContext handlerContext)
    {
        return new GoogleRoom
        {
            RoomId = src.RoomId.ToString(),
            RoomType = src.RoomType.ToString()
        };
    }

    public override Dirs21Room ReverseMap(GoogleRoom target, IMapHandlerContext handlerContext)
    {
        return new Dirs21Room
        {
            RoomId = Guid.Parse(target.RoomId),
            RoomType = Enum.Parse<RoomType>(target.RoomType),
        };
    }
}

```
**Add Configuration:** add mappers to configuration.
```

public class ExampleMapConfiguration : MapConfiguration
{
    public ExampleMapConfiguration()
    {
        AddMap("Dirs21.Room",
                "Google.Room",
                new Dirs21RoomToGoogleRoomMap())
            .AddReverseMap();
    }
}
```

**Register dynamic map:** register the dynamic map dependencies using `Dependency Injection`. 
```
serviceCollection.AddDynamicMap(typeof(ExampleMapConfiguration))

```

**Map:** finally, map `googleRoom` to `Dirs21Room` or vise versa.

```
var mapHandler = serviceProvider.GetRequiredService<IMapHandler>();
var dirs21Room = (Dirs21Room)mapHandler.Map(googleRoom, "Google.Room", "Dirs21.Room");
```


## Nested Map

Here is an example of mapping nested objects. Don't forget to set the `MaxRecursionDepth` by `default = 3` . 

```
public class Dirs21ToGoogleReservationMap : MapStrategy<Dirs21Reservation, GoogleReservation>
{
    public override GoogleReservation Map(Dirs21Reservation src, IMapHandlerContext handlerContext)
    {
        return new GoogleReservation
        {
            ...
            ...
            Room = (GoogleRoom?)handlerContext.Map(src.Dirs21Room,
                "Dirs21.Room",
                "Google.Room")
        };
    }

    public override Dirs21Reservation ReverseMap(GoogleReservation dest, IMapHandlerContext handlerContext)
    {
        return new Dirs21Reservation
        {
            ...
            ...
            Dirs21Room = (Dirs21Room?)handlerContext.Map(dest.Room,
                "Google.Room",
                "Dirs21.Room")
        };
    }
}
```

### Max Recursion Depth

Set `maxRecursionDepth` in this way:

```
var mapConfiguration = new MapConfiguration(maxRecursionDepth: 5);

```
Another way, you can set it:

```
public class ExampleMapConfiguration : MapConfiguration
{
    public ExampleMapConfiguration()
    {
        MaxRecursionDepth = 5;
    }
}

```

## Ways to Add Map Rules 


#### Class based mapper

This approach provides strong type validation and casting. Here, `Dirs21RoomToGoogleRoomMap` is a class based mapper which should inherit `MapStrategy` or implement `IMapStrategy`.  

```
public class Dirs21RoomToGoogleRoomMap : MapStrategy<Dirs21Room, GoogleRoom>
{
    public override GoogleRoom Map(Dirs21Room src, IMapHandlerContext handlerContext)
    {
        return new GoogleRoom
        {
            RoomId = src.RoomId.ToString(),
        };
    }

    public override Dirs21Room ReverseMap(GoogleRoom target, IMapHandlerContext handlerContext)
    {
        return new Dirs21Room
        {
            RoomId = Guid.Parse(target.RoomId),
        };
    }
}
```

You can add this mapper:
```
AddMap("Model.Reservation",
        "Google.Reservation",
        new Dirs21ToGoogleReservationMap())
```


With this approach, you can also configure `ReverseMap` right here:

```
AddMap("Model.Reservation",
        "Google.Reservation",
        new Dirs21ToGoogleReservationMap())
      .AddReverseMap();

```


####  Functional mapper

With this approach you can quickly register a function `MapToGoogleUser` of type `Func<object, IMapHandlerContext ,object?>`:

```
public GoogleUser MapToGoogleUser(object source, IMapHandlerContext mapHandlerContext)
{
    var src = TypeCastUtil.CastTypeBeforeMap<Dirs21User>(source);

    return new GoogleUser
    {
        UserId = src.UserId.ToString(),
        Email = src.Email,
        FirstName = src.FullName.Split(" ").First(),
        LastName = src.FullName.Split(" ").Last()
    };
}
```

Now you can add to configuration:

```
....

AddMap("Model.User",
    "Google.User", MapToGoogleUser);
....

```
Currently, we are not supporting `ReverseMap` out of the box with this approach since you can just do `AddMap("Google.User", "Model.User", MapToModelUser);`

