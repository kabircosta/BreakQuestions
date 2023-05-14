namespace BreakQuestions

module ReportRepository =

    open System
    open System.IO
    open System.Text.Json
    open System.Text.Json.Serialization
    open QuestionModel

    // DATABASE- REPORT
    // QUESTIONS OK - X (%)
    // QUESTIONS NOT OK - Y (%)
    // QUESTION HISTORY ...

    // CORRECTANSWER == SELECTEDANSWER
    // QUESTION MATH...
    // QUESTIONID, BOOLEAN CORRECT

    let options =
        JsonFSharpOptions.Default().ToJsonSerializerOptions()

    JsonFSharpOptions.Default().AddToJsonSerializerOptions(options)


    //#region DATABASE STRING

    let db = "C:\\temp\\reportDatabase.json"

    let ensureFolderExists () =
        match Directory.Exists("C:\\temp") with
        | true -> ()
        | false -> Directory.CreateDirectory("C:\\temp") |> ignore

    let ensureDatabaseExists () = 
        ensureFolderExists()
        match File.Exists(db) with
        | true  -> ()
        | false -> File.WriteAllText(db, "[]")

    let createOrGetDb () =
        ensureDatabaseExists()
        db

    //#endregion

    //#region SERIALIZATION / DESERIALIZATION

    let deserializeDb () =
        let json = createOrGetDb() |> File.ReadAllText
        JsonSerializer.Deserialize<QuestionModel list>(json, options)

    let serializeDb(questions : QuestionModel list) =
        let newJson = JsonSerializer.Serialize(questions, options)
        (createOrGetDb(), newJson) |> File.WriteAllText

    //#endregion

    //#region DATABASE ACTIONS

    let insert (question : QuestionModel) =
        question :: deserializeDb()
        |> serializeDb

    let update (newQuestion : QuestionModel) =
        let updateQuestionIfMatching newQuestionsList existingQuestions =
            if existingQuestions.id = newQuestion.id then 
                newQuestion :: newQuestionsList
            else
                existingQuestions :: newQuestionsList

        deserializeDb ()
        |> List.fold updateQuestionIfMatching []
        |> serializeDb

    let select questionId =
        deserializeDb ()
        |> List.tryFind (fun o -> o.id = questionId)

    let delete questionId =
        deserializeDb ()
        |> List.filter(fun o -> o.id <> questionId)
        |> serializeDb

    //#endregion

    //#region METHODS

    open System
    let getRandomQuestion() =
        let random = Random(int DateTime.Now.Ticks);
        deserializeDb()
        |> List.sortBy (fun _ -> random.Next())
        |> List.tryHead

    //#endregion
