![alt text](https://fablecode.visualstudio.com/_apis/public/build/definitions/9e9640ec-37b8-4d8b-8cb2-19c074a1fa41/7/badge?maxAge=0 "Visual studio team services build status")

# DuelTank Api
A C# .NET Core 2.1 api for [DuelTank](https://www.dueltank.com) data such as Decks, Banlists and Archetypes.

## Installing
```
 $ git clone https://github.com/fablecode/ygo-api.git
```

## Api Url
[https://api.dueltank.com](https://api.dueltank.com)

## Prerequisite
1. Setup the [Database](https://github.com/fablecode/dueltank/tree/master/src/Database/src)

## Built With
* [Visual Studio 2017](https://www.visualstudio.com/downloads/)
* [Onion Architecture](http://jeffreypalermo.com/blog/the-onion-architecture-part-1/) and [CQRS](https://martinfowler.com/bliki/CQRS.html).
* [.NET Core 2.1](https://www.microsoft.com/net/download/core)
* [Swagger](https://swagger.io/)
* [Mediatr](https://www.nuget.org/packages/MediatR/) for CQRS and the Mediator Design Pattern. Mediator design pattern defines how a set of objects interact with each other. You can think of a Mediator object as a kind of traffic-coordinator, it directs traffic to appropriate parties.
* [Entity Framework Core 2](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)
* [Fluent Validations](https://www.nuget.org/packages/FluentValidation)
* [Fluent Assertions](https://www.nuget.org/packages/FluentAssertions)
* [NUnit](https://nunit.org/)
* [Visual Studio Team Services](https://www.visualstudio.com/team-services/release-management/) for CI and deployment.

## License
This project is licensed under the MIT License - see the [LICENSE.md](/LICENSE) file for details.
