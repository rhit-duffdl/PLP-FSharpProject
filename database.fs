module Database

open System.Text.Json
open System.IO

// TODO: Implement function to transform the raw update data into a form readable by the database
// TODO: Implement functions to insert data in the database.

type User = {
    email:string
    zip:string
}

let user_info_path = "UserInfo.txt"
let last_data_path = "LastData.txt"

let writeData text = 
    File.WriteAllText(last_data_path, text)

let getLastData = 
    File.ReadAllText(last_data_path)

let getUserEmail =
    let userText = File.ReadAllText(user_info_path)
    let user = JsonSerializer.Deserialize<User> userText
    user.email

let getUserZIP =
    let userText = File.ReadAllText(user_info_path)
    let user = JsonSerializer.Deserialize<User> userText
    user.zip

let setUserEmail newEmail =
    let userText = File.ReadAllText(user_info_path)
    let user = JsonSerializer.Deserialize<User>(userText)
    user.email = newEmail
    File.WriteAllText(user_info_path, JsonSerializer.Serialize user)

let setUserZIP newZIP =
    let userText = File.ReadAllText(user_info_path)
    let user = JsonSerializer.Deserialize<User>(userText)
    user.zip = newZIP
    File.WriteAllText(user_info_path, JsonSerializer.Serialize user)

let weatherHasChanged update = 
    let oldWeather = getLastData
    writeData update
    update.Equals(oldWeather)