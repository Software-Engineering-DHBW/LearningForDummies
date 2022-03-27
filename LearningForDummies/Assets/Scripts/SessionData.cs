using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveData;
using System.Linq;

public class SessionData
{
    public QuestionCatalogue sessionCatalogue;
    public List<Question> questionList;
    public Question activeQuestion;
    public int rightAnswerPosition, questionCount_total, questionIndex = 0;

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

        questionCount_total = Mathf.RoundToInt(questionCount_total * percentage);
        questionList = (List<Question>)questionList.Take(questionCount_total);
    }


}
