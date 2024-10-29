module Dictionary

open System.IO
open System.Collections.Generic

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

module SchoolCode = 
    let load filePath =
        // mutable dictionary
        // let pairs =
        //     File.ReadLines filePath
        //     |> Seq.skip 1
        //     |> Seq.map (fun line -> 
        //         let elements = line.Split('\t')
        //         let id = elements[0] |> int
        //         let name = elements[1]
        //         KeyValuePair.Create(id, name))
        // new Dictionary<_, _>(pairs)

        // immutable dictionary
        // File.ReadLines filePath
        //     |> Seq.skip 1
        //     |> Seq.map (fun line -> 
        //         let elements = line.Split('\t')
        //         let id = elements[0] |> int
        //         let name = elements[1]
        //         id, name)
        //     |> dict

        // fsharp Map implements the IDictionary interface and 
        // offers an easy way to create a new dictionary from an existing immutable one
        File.ReadLines filePath
            |> Seq.skip 1
            |> Seq.map (fun line -> 
                let elements = line.Split('\t')
                let id = elements[0] |> int
                let name = elements[1]
                id, name)
            |> Map.ofSeq
            |> Map.add 0 "(External)"

module SchoolCodeAlpha =
    let load filePath = 
        File.ReadLines filePath
            |> Seq.skip 1
            |> Seq.map (fun line -> 
                let elements = line.Split('\t')
                let id = elements[0]
                let name = elements[1]
                id, name)
            |> Map.ofSeq
            |> Map.add "*" "(External)"



type Student = {
    Id: string
    Surname: string
    GivenName: string
    SchoolName: string
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

    let fromString (schoolCodes: IDictionary<int, string>) (s: string) = 
        let rows = s.Split('\t')
        let name = rows[0] |> nameParts
        let id = rows[1]
        let schoolCode = rows[2] |> int
        // let schoolName = schoolCodes[schoolCode]

        // when loading files with unknown school school codes
        let schoolName = 
            match schoolCodes.TryGetValue(schoolCode) with
            | true, name -> name
            | false, _ -> "(Unknown)"

        // when schoolCodes is an fsharp Map 
        // let schoolName =
            // schoolCodes.TryFind(schoolCode)
            // |> Option.defaultValue "(Unknown)"

            // schoolCodes
            // |> Map.tryFind(schoolCode)
            // |> Option.defaultValue "(Unknown)"

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
            SchoolName = schoolName
            MeanScore = meanScore
            MaxScore = maxScore
            MinScore = minScore
        }

    let fromStringAlpha (schoolCodes: Map<_, _>) (s: string) =
        let rows = s.Split('\t')
        let name = rows[0] |> nameParts
        let id = rows[1]
        let schoolCode = rows[2]
        let schoolName = 
            schoolCodes
            |> Map.tryFind(schoolCode)
            |> Option.defaultValue "(Unknown)"

        let scores = 
            rows
            |> Array.skip 3
            |> Array.map TestResult.fromString
            |> Array.choose TestResult.tryEffectiveScore

        let minScore = scores |> Array.min
        let meanScore = scores |> Array.average
        let maxScore = scores |> Array.max
        {
            Surname = name.Surname
            GivenName = name.GivenName
            Id = id
            SchoolName = schoolName
            MeanScore = meanScore
            MaxScore = maxScore
            MinScore = minScore
        }


    let printSummary student =
        printfn $"%s{student.Surname}\t%s{student.GivenName}\t%s{student.SchoolName}\t%s{student.Id}\t%.1f{student.MeanScore}\t%.1f{student.MaxScore}\t%.1f{student.MinScore}"

    let printGroupSummary (surname: string, student: Student[]) =
        printfn $"%s{surname.ToUpperInvariant()}"
        student
        |> Array.sortBy _.GivenName
        |> Array.iter(fun student -> 
            printfn $"\t%20s{student.GivenName}\t%s{student.Id}\t%0.1f{student.MeanScore}\t%0.1f{student.MaxScore}\t%0.1f{student.MinScore}\t")

let summarize schoolCodeFilePath filePath = 
    let rows = File.ReadAllLines filePath
    let count = (rows |> Array.length) - 1
    printfn $"Student count: %i{count}"
    rows 
    |> Array.skip 1
    |> Array.map (Student.fromString (SchoolCode.load schoolCodeFilePath))
    |> Array.sortBy _.Surname
    |> Array.iter Student.printSummary

// let summarize filePath = 
//     let rows = File.ReadAllLines filePath
//     let count = (rows |> Array.length) - 1
//     printfn $"Student count: %i{count}"
//     rows 
//     |> Array.skip 1
//     |> Array.map Student.fromString
//     |> Array.sortBy _.Surname
//     |> Array.groupBy _.Surname
//     |> Array.iter Student.printGroupSummary