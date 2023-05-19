// TODO: Implement a function to detect changes in the data being tracked.
// TODO: Implement a function to create notifications based on the changes detected.
// TODO: Implement a function to send email notifications to users.
module Notification

open System
open SendGrid
open SendGrid.Helpers.Mail
//open System.Text.RegularExpressions
open System.Text.Json

open Database

type Location = {
    name: string
    region: string
    country: string
    lat: float
    lon: float
    tz_id: string
    localtime_epoch: int64
    localtime: string
}
type condition = {
    text: string
    icon: string
    code: int
}

type Current = {
    last_updated_epoch: int64
    last_updated: string
    temp_c: float
    temp_f: float
    is_day: int
    condition: condition
    wind_mph: float
    wind_kph: float
    wind_degree: int
    wind_dir: string
    pressure_mb: float
    pressure_in: float
    precip_mm: float
    precip_in: float
    humidity: int
    cloud: int
    feelslike_c: float
    feelslike_f: float
    vis_km: float
    vis_miles: float
    uv: float
    gust_mph: float
    gust_kph: float
}

type WeatherInfo = {
    location: Location
    current: Current
}

let generateMsg (msg : string) =
    let json = JsonSerializer.Deserialize<WeatherInfo> msg
    let location = json.location
    let current = json.current

    let message =
        sprintf "<h3>Weather information for %s:\n</h3>" location.name +
        sprintf "<h3>Region: %s\n</h3>" location.region +
        sprintf "<h3>Country: %s\n</h3>" location.country +
        sprintf "<h3>Temperature: %.1f °C (%.1f °F)\n</h3>" current.temp_c current.temp_f +
        sprintf "<h3>Condition: %s\n</h3>" current.condition.text +
        sprintf "<h3>Wind: %.1f kph from %s\n</h3>" current.wind_kph current.wind_dir +
        sprintf "<h3>Humidity: %d%%\n</h3>" current.humidity +
        sprintf "<h3>Visibility: %.1f km (%.1f miles)\n</h3>" current.vis_km current.vis_miles

    message

let sendEmail body =
    if weatherHasChanged body then
        let apiKey = "" //"I can't explicitly put it here, it will suspend my account"
        let senderEmail = "duffman2332@gmail.com"
        let recipientEmail = "duffdl@rose-hulman.edu"
        let subject = "Weather Notification from F#"
        let client = new SendGridClient(apiKey)
        let from = new EmailAddress(senderEmail, "F# Weather Notification")
        let to' = new EmailAddress(recipientEmail)
        let content = new Content(body, body) // Specify both plain text and HTML content
        let msg = MailHelper.CreateSingleEmail(from, to',subject, body, body)
        let response = client.SendEmailAsync(msg).Result
        printfn "Email sent with status code: %A" response.StatusCode
    else 
        printfn "Weather has not changed. No email sent."

