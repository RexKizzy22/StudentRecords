module Bricks

open System

type Brick =
    {
        StudColumns : int
        StudRows : int
        Color : ConsoleColor
    }

module Brick =

    let printConsole (brick : Brick) =
        let rowChar = 
            match brick.StudRows with
            | 1 -> "·"
            | 2 -> ":"
            | _ -> raise <| ArgumentException("Unsupported row count")
        let pattern = String.replicate brick.StudColumns rowChar            
        printf $"%s{brick.Color.ToString().Substring(0,1)} "
        Console.BackgroundColor <- brick.Color
        Console.ForegroundColor <- ConsoleColor.Black
        printf $"[%s{pattern}]"
        Console.ResetColor()
        printf " "

let bricks =

    [|
        (3, 2, ConsoleColor.Yellow)
        (4, 2, ConsoleColor.Green)
        (2, 1, ConsoleColor.Magenta)
        (1, 1, ConsoleColor.Blue)
        (2, 2, ConsoleColor.Red)
        (4, 2, ConsoleColor.Blue)
        (4, 2, ConsoleColor.Magenta)
        (2, 2, ConsoleColor.Magenta)
        (2, 2, ConsoleColor.Red)
        (4, 2, ConsoleColor.Blue)
        (3, 2, ConsoleColor.Magenta)
        (4, 2, ConsoleColor.Green)
        (3, 2, ConsoleColor.Red)
        (4, 1, ConsoleColor.Blue)
        (4, 2, ConsoleColor.Yellow)
        (4, 2, ConsoleColor.Yellow)
        (1, 1, ConsoleColor.Blue)
        (1, 1, ConsoleColor.Green)
        (2, 1, ConsoleColor.Yellow)
        (4, 1, ConsoleColor.Magenta)
    |]
    |> Array.map (fun (sc, sr, cc) -> { StudColumns = sc; StudRows = sr; Color = cc })

printfn "All the bricks:"
bricks
|> Array.iter (Brick.printConsole)
printfn "\n"

printfn "Count of the bricks:"
let count = bricks |> Array.length
printfn $"Count: %i{count}\n"

printfn "Stud counts:"
bricks
|> Array.map (fun b -> b.StudColumns * b.StudRows)
|> Array.iter (fun c -> printf $"%i{c}; ")
printfn "\n"

printfn "Red bricks (Array.filter):"
bricks
|> Array.filter (fun b -> b.Color = ConsoleColor.Red)
|> Array.iter Brick.printConsole
printfn "\n"

printfn "Grouped by color (Array.groupBy):"
let groupedByColor =
    bricks
    |> Array.groupBy (fun b -> b.Color)

groupedByColor
|> Array.iter (fun (color, bricks) ->
    printfn $"%s{color.ToString()}:"
    bricks
    |> Array.iter Brick.printConsole
    printfn ""
)
printfn ""

printfn "Grouped by studCount (Array.groupBy):"
let groupedByStudCount =
    bricks
    |> Array.groupBy (fun b -> b.StudColumns * b.StudRows)

groupedByStudCount
|> Array.sortByDescending fst
|> Array.iter (fun (studCount, bricks) -> 
    printfn $"%i{studCount}: stud"
    bricks
    |> Array.iter Brick.printConsole
    printfn "")
printfn ""

