namespace BreakQuestions

module GenericRepository =

    open System
    open System.IO
    open System.Text.Json
    open System.Text.Json.Serialization

    let options =
        JsonFSharpOptions.Default().ToJsonSerializerOptions()

    JsonFSharpOptions.Default().AddToJsonSerializerOptions(options)

    let ensureFolderExists () =
        match Directory.Exists("C:\\temp") with
        | true -> ()
        | false -> Directory.CreateDirectory("C:\\temp") |> ignore

    let ensureDatabaseExists (db : string)  = 
        ensureFolderExists()
        match File.Exists(db) with
        | true  -> ()
        | false -> File.WriteAllText(db, "[]")

    let createOrGetDb db =
        ensureDatabaseExists db
        db

    let deserializeDb db  =
        let json = createOrGetDb db |> File.ReadAllText
        JsonSerializer.Deserialize<'a list>(json, options)

    let serializeDb db (input : 'a list) =
        let newJson = JsonSerializer.Serialize(input, options)
        (createOrGetDb db, newJson) |> File.WriteAllText