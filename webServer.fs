open Suave

open Suave.Operators

open Suave.Filters

open Suave.Successful

open System
open System.Threading
let api_key = "b2d0c12ba7c445fa8d130308231205"

let apiEndpoint = "http://api.weatherapi.com/v1/current.json?key=b2d0c12ba7c445fa8d130308231205&q=47803&aqi=no"

let sendApiRequest () =
    let httpClient = new System.Net.Http.HttpClient()
    let response = httpClient.GetAsync(apiEndpoint).Result
    let content = response.Content.ReadAsStringAsync().Result
    printfn "API Response: %s" content

let startApiRequestTimer () =
    let timer = new Timer(
        TimerCallback(fun _ -> sendApiRequest()),
        null,
        TimeSpan.Zero,
        TimeSpan.FromSeconds(30)  // Adjust the interval as needed
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
              POST >=> choose [ path "/" >=> OK "Request Received" ] ]

    let apiRequestTimer = startApiRequestTimer()




    try
        startWebServer cfg app
    finally
        apiRequestTimer.Dispose()

[<EntryPoint>]

let main argv =

    runWebServer argv

    0
