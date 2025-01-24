namespace newGame.Components.Controller

open Giraffe
open Microsoft.AspNetCore.Http
open System.Threading.Tasks
open Microsoft.Extensions.Logging
open newGame.Components
open newGame.Components.Utlity
open newGame.Components.TypeManager
open newGame.Components.SQL_Manager


module latinAPIController =
  let myFunction () = "Hello from Controller"
  
  let utility =  newGame.Components.Utlity.PLTranslator.translate;   
    
  type UserSentence = TypeManager.UserSentence
  
  let mutable storedSentence: string option = None
  let error = "bug"
  let runEntry : HttpHandler =
      fun (next: HttpFunc) (ctx: HttpContext) ->
          task {
              let! body = ctx.BindJsonAsync<UserSentence>() // Parse incoming JSON
              storedSentence <- Some body.sentence // Store the sentence
              
              match storedSentence with
                | Some sentence ->
                  let logger = ctx.GetLogger("SentenceLogger")
                  logger.LogInformation($"Received sentence: {body.sentence}")
                  SQL_Manager.insertTranslation sentence (utility sentence)
                  return! json {| message = "Sentence stored successfully"; storedSentence = utility sentence |} next ctx
                | None ->
                  return! json {| message = "BUG"; storedSentence = "bug" |} next ctx            

          }
          
          
  let pullEntry : HttpHandler =
    fun (next: HttpFunc) (ctx:HttpContext) ->
      task {
        let pack = pullThree()
        
        return! json {| 
            message = "Fetched three translations successfully"
            dataOne = pack.dataOne
            dataTwo = pack.dataTwo
            dataThree = pack.dataThree
        |} next ctx
      }
          
          
  let webApp: HttpFunc ->  HttpContext -> HttpFuncResult =
    choose [
      GET >=>
        choose [
          route "/" >=> text "These are not the drones you are looking for."
          route "/pullEntries" >=> pullEntry
        ]
      POST >=>
        choose [  
          route "/runEntry" >=> runEntry
         ]
      setStatusCode 404 >=> text "Not Found"
    ]