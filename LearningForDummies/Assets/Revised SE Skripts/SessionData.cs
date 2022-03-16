using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveData;

public class SessionData
{
    QuestionCatalogue sessionCatalogue;
    List<Question> questionList;
    Question activeQuestion;
    string questionString, answer_1, answer_2, answer_3, answer_4;
    int rightAnswerPosition, questionCount_total, questionCount_current, questionIndex = 0;

    public void fillSessionData(float percentage)
    {
        questionList = sessionCatalogue.questions;
        questionCount_total = questionList.Count;

        for (int i = 0; i < questionCount_total; i++) //List Shuffle
        {
            Question tempQ = questionList[i];
            int randomIndex = Random.Range(i, questionCount_total);
            questionList[i] = questionList[randomIndex];
            questionList[randomIndex] = tempQ;
        }



    }


}
