namespace API_latin.Controllers

open Giraffe
open Microsoft.AspNetCore.Http
open System.Threading.Tasks
open Microsoft.Extensions.Logging

module latinAPIController =
  module latinAPIController =
    let myFunction () = "Hello from Controller"
    
  let runWord : HttpHandler =
      fun (next: HttpFunc) (ctx: HttpContext) ->
          task {
              let logger = ctx.GetLogger("ErrorEmailTest")
              logger.LogError("Test error email triggered")
              
              ctx.SetStatusCode 500
              return! text "Test error email triggered" next ctx
          }
      
  let webApp: HttpFunc ->  HttpContext -> HttpFuncResult =
    choose [
      GET >=>
        choose [
          route "/" >=> text "These are not the drones you are looking for."
        ]
      POST >=>
        choose [  
          route "runWord" >=> runWord
         ]
      setStatusCode 404 >=> text "Not Found"
    ]