namespace BreakQuestions

module QuestionRepository =

    open System
    open QuestionModel
    open GenericRepository

    //#region DATABASE STRING

    let db = "C:\\temp\\database.json"

    //#endregion

    //#region SERIALIZATION / DESERIALIZATION

    let deserializeDb () = GenericRepository.deserializeDb db
    let serializeDb = GenericRepository.serializeDb db

    //#endregion

    //#region DATABASE ACTIONS

    let insert (question : QuestionModel) =
        question :: deserializeDb ()
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

    let selectBy predicate  =
        deserializeDb ()
        |> List.tryFind predicate

    let delete questionId =
        deserializeDb ()
        |> List.filter(fun o -> o.id <> questionId)
        |> serializeDb

    //#endregion

    //#region METHODS

    let getRandomQuestion() =
        let random = Random(int DateTime.Now.Ticks);
        deserializeDb ()
        |> List.sortBy (fun _ -> random.Next())
        |> List.tryHead

    //#endregion