module Records

open System.IO

type Student = {
    Id: string
    Name: string
    MeanScore: float
    MinScore: float
    MaxScore: float
}

module Student = 
    let fromString (s: string) = 
        let rows = s.Split('\t')
        let name = rows[0]
        let id = rows[1]
        let scores = 
            rows
            |> Array.skip 2
            |> Array.map float
        let minScore = scores |> Array.min
        let meanScore = scores |> Array.average
        let maxScore = scores |> Array.max
        {
            Name = name
            Id = id
            MeanScore = meanScore
            MaxScore = maxScore
            MinScore = minScore
        }

    let printSummary student =
        printfn "%s\t%s\t%.1f\t%.1f\t%.1f" student.Name student.Id student.MeanScore student.MaxScore student.MinScore

let summarize filePath = 
    let rows = File.ReadAllLines filePath
    let count = (rows |> Array.length) - 1
    printfn "Student count: %i" count
    rows 
    |> Array.skip 1
    |> Array.map Student.fromString
    |> Array.sortBy _.Name
    |> Array.iter Student.printSummary