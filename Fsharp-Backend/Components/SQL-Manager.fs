module newGame.Components.SQL_Manager

open System
open System.Data.SQLite
open newGame.Components.TypeManager
type UserSentence = TypeManager.UserSentence
let dbPath = @"C:\Users\nataliekclark\SQLite\secondTry.db"

let insertTranslation (beforeTranslation: string)(afterTranslation: string)=
    let connectionString = sprintf "Data Source=%s;Version=3;" dbPath
    use conn = new SQLiteConnection(connectionString)
    conn.Open()

    let sql = "INSERT INTO translationPost (bt, at) VALUES (@bt, @at)"
    
    use cmd = new SQLiteCommand(sql, conn)
    cmd.Parameters.AddWithValue("@bt",  beforeTranslation) |> ignore
    cmd.Parameters.AddWithValue("@at",  afterTranslation) |> ignore

    let rowsAffected = cmd.ExecuteNonQuery()
    printfn "Inserted %d row(s)" rowsAffected

    conn.Close()



let pullThree () =
    let connectionString = sprintf "Data Source=%s;Version=3;" dbPath
    use conn = new SQLiteConnection(connectionString)
    conn.Open()

    let sql = "SELECT * FROM translationPost ORDER BY id DESC LIMIT 3;"
    
    use cmd = new SQLiteCommand(sql, conn)
    use reader = cmd.ExecuteReader()
    
    let mutable results = []

    while reader.Read() do
        let bt = reader.["bt"] :?> string
        let at = reader.["at"] :?> string
        results <- results @ [{ bt = bt; at = at }]
    
    conn.Close()

    let packet = 
        match results with
        | [r1; r2; r3] -> { dataOne = Some r1; dataTwo = Some r2; dataThree = Some r3 }
        | [r1; r2] -> { dataOne = Some r1; dataTwo = Some r2; dataThree = None }
        | [r1] -> { dataOne = Some r1; dataTwo = None; dataThree = None }
        | _ -> { dataOne = None; dataTwo = None; dataThree = None }
    
    packet