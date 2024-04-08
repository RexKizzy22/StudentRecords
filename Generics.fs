module GenericPoints

type Point<'T> = 
    {
        X : 'T
        Y : 'T
    }

module Point = 
    let inline moveBy (dx: 'T) (dy: 'T) (p: Point<'T>) =
        {
            X = p.X + dx
            Y = p.Y + dy
        } 

    let inline scaleBy (dx: 'T) (dy: 'T) (p: Point<'T>) =
        {
            X = p.X * dx
            Y = p.Y * dy
        }

let pFLoat1 = { X = 1.0; Y = 2.0 }
let pFLoat2 = pFLoat1 |> Point.moveBy 3.0 4.0
printfn "pFLoat1: %A, pFLoat2: %A" pFLoat1 pFLoat2

let pIntt1 = { X = 1; Y = 2 }
let pInt2 = pIntt1 |> Point.moveBy 3 4
printfn "pIntt1: %A, pInt2: %A" pIntt1 pInt2

let pString1 = { X = "1"; Y = "2" }
let pString2 = pString1 |> Point.moveBy "3" "4"
printfn "pString1: %A, pString2: %A" pString1 pString2

let pFLoat3 = pFLoat2 |> Point.scaleBy 3.0 4.0
let pInt4 = pInt2 |> Point.scaleBy 3 4
