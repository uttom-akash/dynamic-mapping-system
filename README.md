# Dynamic Mapping Library

## Overview
The **Dynamic Mapping Library** is a lightweight, high-performance, and extensible library designed to simplify data transformations between different structures.

---

## Table of Contents
- [Dynamic Mapping Library](#dynamic-mapping-library)
  - [Overview](#overview)
  - [Table of Contents](#table-of-contents)
  - [Assumption](#assumption)
  - [Features](#features)
  - [Potential Issues](#potential-issues)
  - [Installation](#installation)
  - [Quick Start](#quick-start)
    - [Another good way:](#another-good-way)
  - [Third Party Data Model](#third-party-data-model)
  - [Nested Map](#nested-map)
    - [Max Recursion Depth](#max-recursion-depth)
  - [Ways to Add Map Rules](#ways-to-add-map-rules)
      - [Class based mapper](#class-based-mapper)
      - [Functional mapper](#functional-mapper)
    - [Key Errors](#key-errors)
      - [MapConfiguration](#mapconfiguration)
      - [IMapHandler](#imaphandler)
  - [Layered Architecture](#layered-architecture)
  - [Key Classes](#key-classes)
  - [Future Work](#future-work)


---

## Assumption
- I tried to adhere to the given signature `_mapHandler.Map(object data, string sourceType, string targetType);`.
- I am uncertain whether the third-party data models will always have corresponding concrete types (such as classes or records) in our project. If third-party data models have corresponding concrete types in our project, using generic is better option. 
- Since passed argument `data` is of type `object` in `_mapHandler.Map(object data, string sourceType, string targetType);` so I am returing the type `object` as well.
- Mapping logics between source and target will be provided by developer for now.
- Pure mapping library to evaluate the thought process.

---

## Features
- **High Performance**: Designed for optimal performance avoiding reflection and assembly scanning.
- **Supports Nested Structures**: Works with complex, multi-level data models.
- **Extensible Mapping Template**: Provides an extensible template to take care of mapping logics.
- **Customizable**: Multple ways to provide custom mapping logic with ease.  
- **External Dependency**: No third party packages are used.
- **Test Coverage**: Thoroughly validated through comprehensive unit and integration tests. 


---

## Potential Issues

- **MaxRecursionDepth** : Enforced a maximum depth to limit the depth when mapping nested objects and circular references. Caution should be exercised when setting it to a large value. `Default value is 3`

---

## Installation

Add class library `DynamicMap.Core` to your application.

---
## Quick Start

First, specify the configuration between source and target. Second map between them.

```
// configuration

var mapConfiguration = new MapConfiguration()

mapConfiguration.AddMap("GoogleUser", "Dirs21User", (source, handlerContext) => 
{
    var src = TypeCastUtil.CastTypeBeforeMap<GoogleUser>(source);
    
    return new Dirs21User
    {
        UserId = Guid.Parse(src.UserId),
        FullName = src.FirstName + " " + src.LastName
    };
})


// map

var mapHandler = new MapHandler(mapConfiguration)

var dirs21User = (Dirs21User)mapHandler.Map(externalUser, "GoogleUser", "Dirs21User");

```

---

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

---

## Third Party Data Model

Incase we don't want to implement third part provider's type in our project. Here our project doesn't have the data model for `GoogleUser`. We want to map incoming google user data model directly to our type `Dirs21User` and vise-versa.

```
public class Dirs21UserToGoogleUserJsonMap : MapStrategy<Dirs21User, string>
{
    public override string Map(Dirs21User source, IMapHandlerContext handlerContext)
    {
        return new JObject
        {
            { "FirstName", source.FullName.Split(" ").First() },
            { "LastName", source.FullName.Split(" ").Last() }
        }.ToString();
    }

    public override Dirs21User ReverseMap(string target, IMapHandlerContext handlerContext)
    {
        var jObject = JObject.Parse(target);

        return new Dirs21User
        {
            FullName = $"{jObject["FirstName"]} {jObject["LastName"]}"
        };
    }
}
```

Add the mapper to configuration:

```
AddMap("Dirs21User", "GoogleUserJsonString",new Dirs21UserToGoogleUserJsonMap())
    .AddReverseMap();

```

---

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

---

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

```
Currently, we are not supporting `ReverseMap` out of the box with this approach since you can just do `AddMap("Google.User", "Model.User", MapToModelUser);`

---

### Key Errors

#### MapConfiguration

- **MaxRecursionDepth**
  - `ArgumentException` : 1 <= value <= 100
- **AddMap**
  - `MappingRulesAlreadyExistsException` 
  
#### IMapHandler
- **Map**
  - `MappingRulesNotFoundException`,`NullMappingResultException`,`InvalidCastException`

## Layered Architecture

- **DynamicMap.Core:** Holds the core skeleton and logics of mapping library.
- **DynamicMap.Examples:** Depends on the `DynamicMap.Core` layer to use it's mapping capablities.
- **DynamicMap.Tests:**  Depends on both `DynamicMap.Core` and `DynamicMap.Examples` to tests the functionalities.

---

## Key Classes

- **MapConfiguration:** A configuration store that provides interface to add and retrieve map configuration. 
- **MapHandler:** Provides a method to map an object from a specified source type to a target type.
- **MapHandlerContext:** Manage map execution context and recursion while mapping. It also provides a method to handle nested map.
- **MapStrategy<TSource, TTarget>:** A template to provide mapping logics between source and target. Strategy pattern is followd here.

---

## Future Work
Designing a map library takes a lot thoughts and time. Due to time time constraint, everything is not taken care of. We can focus on following in the future.
- Implement an approach to map properties automatically. 
- Have separate config meta data for each map rule. For example `MaxRecursionDepth` is same for all mapping rules now. 

  