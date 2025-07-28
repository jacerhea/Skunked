namespace Skunked.Web.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Skunked.Cards

[<ApiController>]
[<Route("[controller]")>]
type DeckController(logger: ILogger<DeckController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Get() = new Deck()
