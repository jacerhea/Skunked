namespace Skunked.Web.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Skunked.Web
open Skunked.Cards

[<ApiController>]
[<Route("[controller]")>]
type DeckController (logger : ILogger<DeckController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Get() =
        new Deck()
