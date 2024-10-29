module ColourHistory

open System.Drawing

type ColourHistory(initialColors: seq<Color>, maxLength: int) = 
    let mutable colors = 
        initialColors
        |> Seq.truncate maxLength
        |> List.ofSeq

    member this.Colors() = 
        colors 
        |> List.ofSeq

    member this.Add(color: Color) =
        let color' = 
            color :: colors
            |> List.distinct
            |> List.truncate maxLength
        colors <- color'

    member this.Trylatest() = 
        match colors with 
        | head :: _ -> head |> Some 
        | [] -> None

    member this.RemoveLatest() =
        match colors with
        | _ :: tail -> 
            colors <- tail
        | [] -> ()