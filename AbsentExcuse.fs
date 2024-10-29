module AE

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
            raise (System.FormatException $"Invalid name format: \"%s{s}\"")

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
        printfn $"%s{student.Surname}\t%s{student.GivenName}\t%s{student.Id}\t%.1f{student.MeanScore}\t%.1f{student.MaxScore}\t%.1f{student.MinScore}"

    let printGroupSummary (surname: string, student: Student[]) =
        printfn $"%s{surname.ToUpperInvariant()}"
        student
        |> Array.sortBy _.GivenName
        |> Array.iter(fun student -> 
            printfn $"\t%20s{student.GivenName}\t%s{student.Id}\t%0.1f{student.MeanScore}\t%0.1f{student.MaxScore}\t%0.1f{student.MinScore}\t")

// let summarize filePath = 
//     let rows = File.ReadAllLines filePath
//     let count = (rows |> Array.length) - 1
//     printfn "Student count: %i" count
//     rows 
//     |> Array.skip 1
//     |> Array.map Student.fromString
//     |> Array.sortBy _.Surname
//     |> Array.iter Student.printSummary

let summarize filePath = 
    let rows = File.ReadAllLines filePath
    let count = (rows |> Array.length) - 1
    printfn $"Student count: %i{count}"
    rows 
    |> Array.skip 1
    |> Array.map Student.fromString
    |> Array.sortBy _.Surname
    |> Array.groupBy _.Surname
    |> Array.iter Student.printGroupSummary