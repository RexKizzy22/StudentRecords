open System
open System.IO

// open Records
open MissingData
// open AE

[<EntryPoint>]
let main argv =
    match argv with
    | [| sampleFile |] -> 
        if File.Exists sampleFile then
            try
                summarize sampleFile
                0
            with
                | :? FormatException as exn -> 
                    printfn $"Error %s{exn.Message}"
                    1
                | :? IOException as exn ->
                    printfn $"Error %s{exn.Message}"
                    2
                | _ as exn -> 
                    printfn $"Unexpected error occurred: %s{exn.Message}"
                    3
        else 
            printfn $"File not found: %s{sampleFile}"
            4
    | _ -> 
        printfn "You must provide a path to a file"
        5


// open Dictionary
//
// [<EntryPoint>]
// let main argv =
//     match argv with
//     | [| schoolCodesFile; sampleFile |] -> 
//         if File.Exists sampleFile then
//             if File.Exists schoolCodesFile then
//                 try
//                     summarize schoolCodesFile sampleFile
//                     0
//                 with
//                     | :? FormatException as exn -> 
//                         printfn "Error %s" exn.Message
//                         1
//                     | :? IOException as exn ->
//                         printfn "Error %s" exn.Message
//                         2
//                     | _ as exn -> 
//                         printfn "Unexpected error occurred: %s" exn.Message
//                         3
//             else 
//                 printfn "File not found: %s" schoolCodesFile
//                 5
//         else 
//             printfn "File not found: %s" sampleFile
//             6
//     | _ -> 
//         printfn "You must provide a path to a file"
//         7
