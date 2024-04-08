module MissingData

open System.IO

type Student = {
    Id: string
    Surname: string
    GivenName: string
    MeanScore: float
    MinScore: float
    MaxScore: float
}

module Float =
    let tryFromString s = 
        if s = "N/A" then
            None
        else 
            (float s) |> Some

    let fromStringOrDefaultValue dv s =
        s 
        |> tryFromString
        |> Option.defaultValue dv

module Student = 
    let namePart (s: string) =
        let names = s.Split(",")
        names[0].Trim(), names[1].Trim()

    let fromString (s: string) = 
        let rows = s.Split('\t')
        let surname, givenName = namePart rows[0]
        let id = rows[1]
        // let scores = 
        //     rows
        //     |> Array.skip 2
        //     |> Array.choose Float.tryFromString
        let scores = 
            rows
            |> Array.skip 2
            |> Array.map (Float.fromStringOrDefaultValue 50.0)

        let minScore = scores |> Array.min
        let meanScore = scores |> Array.average
        let maxScore = scores |> Array.max
        {
            Surname = surname
            GivenName = givenName
            Id = id
            MeanScore = meanScore
            MaxScore = maxScore
            MinScore = minScore
        }

    let printSummary student =
        printfn "%s\t%s\t%s\t%.1f\t%.1f\t%.1f" student.Surname student.GivenName student.Id student.MeanScore student.MaxScore student.MinScore

let summarize filePath = 
    let rows = File.ReadAllLines filePath
    let count = (rows |> Array.length) - 1
    printfn "Student count: %i" count
    rows 
    |> Array.skip 1
    |> Array.map Student.fromString
    |> Array.sortBy _.Surname
    |> Array.iter Student.printSummary
