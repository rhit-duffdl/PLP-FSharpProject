// TODO: Implement a function to detect changes in the data being tracked.
// TODO: Implement a function to create notifications based on the changes detected.
// TODO: Implement a function to send email notifications to users.
open System
open SendGrid
open SendGrid.Helpers.Mail
let sendEmail (apiKey: string) senderEmail recipientEmail subject body =
    let client = new SendGridClient(apiKey)
    let from = new EmailAddress(senderEmail, "Example user")
    let to' = new EmailAddress(recipientEmail, "Example user")
    let content = new Content(body, body) // Specify both plain text and HTML content
    let msg = MailHelper.CreateSingleEmail(from, to',subject, body, body)
    let response = client.SendEmailAsync(msg).Result

    printfn "Email sent with status code: %A" response.StatusCode


let apiKey = "I can't explicitly put it here, it will suspend my account" //Environment.GetEnvironmentVariable("SendGridAPI")
let senderEmail = "zhuz9@rose-hulman.edu"
let recipientEmail = "receiver@receiver.com"
let subject = "Hello from F#"
let body = "<h1>Hello, world!</h1>"
sendEmail apiKey senderEmail recipientEmail subject body