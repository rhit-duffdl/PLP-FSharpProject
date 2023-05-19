open Suave

open Suave.Operators

open Suave.Filters

open Suave.Successful

open System
open System.Threading
open Notification
open Database

let api_key = "b2d0c12ba7c445fa8d130308231205"

let apiEndpoint = "http://api.weatherapi.com/v1/current.json?key=b2d0c12ba7c445fa8d130308231205&q=47803&aqi=no"

let sendApiRequest () =
    let httpClient = new System.Net.Http.HttpClient()
    let response = httpClient.GetAsync(apiEndpoint).Result
    let content = response.Content.ReadAsStringAsync().Result
    // printfn "API Response: %s" content
    let emailMsg = generateMsg content
    sendEmail emailMsg

let handlePostRequest : WebPart =
    request (fun (ctx : HttpRequest) ->
        let email: Choice<string,string> = ctx.queryParam "email"
        match email with
        | Choice1Of2 addr -> setUserEmail addr
        | Choice2Of2 _ -> failwith "Invalid email"
        OK "Updated email"
    )

let startApiRequestTimer () =
    let timer = new Timer(
        TimerCallback(fun _ -> sendApiRequest()),
        null,
        TimeSpan.Zero,
        TimeSpan.FromSeconds(5)  // Adjust the interval as needed
    )
    timer

let runWebServer argv =


    let port = 8080


    let cfg =

        { defaultConfig with

            bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" port ] }


    let app: WebPart =

        choose
            [ GET >=> choose [ path "/" >=> OK "Hello World!" ]
              POST >=> choose [ path "/" >=> handlePostRequest ] 
              ]

    let apiRequestTimer = startApiRequestTimer()



    try
        startWebServer cfg app
    finally
        apiRequestTimer.Dispose()



[<EntryPoint>]

let main argv =

    runWebServer argv

    0

