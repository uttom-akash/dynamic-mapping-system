using DynamicMap.Example.Models.Internal;
using DynamicMappingLibrary.Contracts;
using DynamicMappingLibrary.Strategies;
using Newtonsoft.Json.Linq;

namespace DynamicMap.Example.Mapping.Strategies;

public class Dirs21UserToGoogleUserMap : MapStrategy<Dirs21User, string>
{
    public override string Map(Dirs21User source, IMapHandlerContext handlerContext)
    {
        return new JObject
        {
            { "UserId", source.UserId.ToString() },
            { "Email", source.Email },
            { "FirstName", source.FullName.Split(" ").First() },
            { "LastName", source.FullName.Split(" ").Last() }
        }.ToString();
    }

    public override Dirs21User ReverseMap(string target, IMapHandlerContext handlerContext)
    {
        var jObject = JObject.Parse(target);

        if (!jObject.TryGetValue("Email", out var email))
            throw new InvalidDataException("Email is required.");

        return new Dirs21User
        {
            UserId = Guid.TryParse(jObject["UserId"]?.ToString(), out var userId) ? userId : Guid.Empty,
            Email = email.ToString(),
            FullName = $"{jObject["FirstName"]} {jObject["LastName"]}"
        };
    }
}