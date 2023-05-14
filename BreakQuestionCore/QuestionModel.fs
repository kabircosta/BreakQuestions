namespace BreakQuestions

module QuestionModel =

    open System.Drawing

    type Category =
        | English
        | Math
        | History

    type QuestionModel =
        {
            id : int
            question : string
            correctAnswer : string
            selectedAnswer : string
            fails : string list
            image : Image option
            category : Category
        }

    let checkAnswer question =
        question.selectedAnswer = question.correctAnswer