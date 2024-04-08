module Sequences

open System
open System.IO

type TestResult = 
    | Absent
    | Excused
    | Score of float

module TestResult = 
    let fromString s =
        if s = "A" then
            Absent
        else if s = "E" then
            Excused
        else
            let score = s |> float
            Score score

    let effectiveScore (testResult: TestResult) =
        match testResult with
        | Absent -> 0.0
        | Excused -> 50.0
        | Score score -> score

    let tryEffectiveScore (testResult: TestResult) =
        match testResult with
        | Absent -> Some 0.0
        | Excused -> None
        | Score score -> Some score


type Student = {
    Id: string
    Surname: string
    GivenName: string
    MeanScore: float
    MinScore: float
    MaxScore: float
}

module Student = 
    let nameParts (s: string) =
        let names = s.Split(",")
        match names with
        | [| surname; givenName |] -> {| Surname = surname.Trim(); GivenName = givenName.Trim() |}
        | [| surname |] -> {| Surname = surname.Trim(); GivenName = "(None)" |}
        | _ -> 
            raise (FormatException(sprintf "Invalid name format: \"%s\"" s))

    let fromString (s: string) = 
        let rows = s.Split('\t')
        let name = rows[0] |> nameParts
        let id = rows[1]
        // let scores = 
        //     rows
        //     |> Array.skip 2
        //     |> Array.map TestResult.fromString
        //     |> Array.map TestResult.effectiveScore
        let scores = 
            rows
            |> Array.skip 2
            |> Array.map TestResult.fromString
            |> Array.choose TestResult.tryEffectiveScore
        let minScore = scores |> Array.min
        let meanScore = scores |> Array.average
        let maxScore = scores |> Array.max
        {
            Surname = name.Surname
            GivenName = name.GivenName
            Id = id
            MeanScore = meanScore
            MaxScore = maxScore
            MinScore = minScore
        }

    let printSummary student =
        printfn "%s\t%s\t%s\t%.1f\t%.1f\t%.1f" student.Surname student.GivenName student.Id student.MeanScore student.MaxScore student.MinScore

    let printGroupSummary (surname: string, student: Student[]) =
        printfn "%s" (surname.ToUpperInvariant())
        student
        |> Array.sortBy _.GivenName
        |> Array.iter(fun student -> 
            printfn "\t%20s\t%s\t%0.1f\t%0.1f\t%0.1f\t" 
                student.GivenName student.Id
                student.MeanScore student.MaxScore student.MinScore)


let summarize filePath = 
    let rows = 
        filePath
        |> File.ReadLines
        |> Seq.cache
    let count = (rows |> Seq.length) - 1
    printfn "Student count: %i" count
    rows 
    |> Seq.skip 1
    |> Seq.map Student.fromString
    |> Seq.sortBy _.Surname
    |> Seq.iter Student.printSummary


module Dates = 
    let from (startDate: DateTime) =
        Seq.initInfinite (fun i -> startDate.AddDays(float i))

Dates.from DateTime.Now
|> Seq.filter (fun i -> i.Month = 1 && i.Day = 1)
|> Seq.truncate 10
|> Seq.iter (fun d -> printfn "%i %s" d.Year (d.DayOfWeek.ToString()) )


module PellSequence = 
    let pell = 
        (0, 0, 0)
        |>Seq.unfold (fun (n, pn2, pn1) -> 
            let pn = 
                match n with
                | 0 | 1 
                    -> n
                | _ 
                    -> 2 * pn2 + pn1
            let n' = n + 1 
            Some (pn, (n', pn1, pn)))

    type Pell = { N: int; PN2: int; PN1: int }

    let pell2 = 
        { N = 0; PN2 = 0; PN1 = 0 }
        |>Seq.unfold (fun initPell -> 
            let pn = 
                match initPell.N with
                | 0 | 1
                    -> initPell.N
                | _ 
                    -> 2 * initPell.PN2 + initPell.PN1
            let n' = initPell.N + 1 
            Some (pn, {N = n'; PN2 = initPell.PN1; PN1 = pn}))

PellSequence.pell
|> Seq.truncate 10
|> Seq.iter (fun x -> printf "%i, " x)

printfn "..."


module Drunkard =
    let r =Random()

    let step () =
        r.Next(-1, 2) 

    type Position = { X: int; Y: int}

    let walk =
        { X = 0; Y = 0}
        |> Seq.unfold (fun position -> 
            let x' = position.X + step()
            let y' = position.Y + step()
            let position' = { X = x'; Y = y' }
            Some(position', position'))
        
Drunkard.walk
|> Seq.take 10
|> Seq.iter (fun p -> printfn "X: %i, Y: %i" p.X p.Y)