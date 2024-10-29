module Classes

open System

type CommandPrompt(message: string, maxTries: int) =
    do
        if String.IsNullOrEmpty(message) then
            raise <| ArgumentException("Please provide a message")

    let trimmedMessage = message.Trim()
    let mutable currentTries = 0
    let mutable foreground = ConsoleColor.White
    let mutable background = ConsoleColor.Black

    member this.ColorScheme
        with get() =
            foreground, background
        and set(fg, bg) = 
            if fg = bg then 
                raise <| ArgumentException("Foreground color and background color must be different")
            foreground <- fg
            background <- bg
            

    member val BeepOnError = true with get, set 

    member this.GetValue() = 
        currentTries <- currentTries + 1
        Console.ForegroundColor <- foreground
        Console.BackgroundColor <- background
        printf $"%s{trimmedMessage}: "
        Console.ResetColor()
        let input = Console.ReadLine()
        if String.IsNullOrEmpty(input) && currentTries < maxTries then
            if this.BeepOnError then
                Console.Beep()
            this.GetValue()
        else
            input

// let prompt = CommandPrompt("Please enter your name", 3)
// prompt.BeepOnError <- false
// prompt.ColorScheme <- ConsoleColor.Cyan, ConsoleColor.DarkGray
// let name = prompt.GetValue()
// printfn $"Hello %s{name}"


type Person(name: string, favoriteColor: string) =
    do 
        if String.IsNullOrEmpty(name) then 
            raise <| ArgumentException("Please enter a name")
    
    member this.Description() = 
        $"Name: %s{name}, Favourite Colour: %s{favoriteColor}"

let prompt = CommandPrompt("Please enter your name", 1)
let name = prompt.GetValue()
printfn $"Hello %s{name}"

let favoriteColor = CommandPrompt("What is your favorite colour (Press enter if you don't have one): ", 1)
let inputColour = prompt.GetValue()
let colour = 
        if String.IsNullOrEmpty(inputColour) then
            "(None)"
        else inputColour

let person = Person(name, colour)
printfn $"%s{person.Description()}"
