namespace newGame.Components.Controller

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Giraffe

module main =
    
    let configureCors (builder: CorsPolicyBuilder) =
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            |> ignore
            
    let configureApp (app: IApplicationBuilder) =
        app
            .UseCors(configureCors)
            .UseRouting()
            .UseGiraffe(latinAPIController.webApp)

    let configureServices (services : IServiceCollection) =
        services.AddCors()  |> ignore

        services.AddGiraffe() |> ignore

    let createHostBuilder args =
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(fun webBuilder ->
                webBuilder
                    .ConfigureServices(configureServices)
                    .Configure(configureApp)
                |> ignore)

    [<EntryPoint>]
    let main args =
        createHostBuilder args
        |> fun builder -> builder.Build()
        |> fun host -> host.Run()
        0