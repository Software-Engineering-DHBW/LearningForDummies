using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public SessionData sessionData;
    public SessionPlayerData player1;
    public SessionPlayerData player2;
    public bool multiplayer = false;
    public bool player1_clickedCorrectAnswer = false;
    public bool player2_clickedCorrectAnswer = false;
    public int scorePerQuestion = 10;
    public int turn = 1; //Shows which Player-Turn it is: 1 for Player1 and 2 for Player2

    [Header("UI Setup")]
    public Image player1_Picture;
    public TMP_Text player1_Name;
    public TMP_Text player1_Score;

    public Image player2_Picture;
    public TMP_Text player2_Name;
    public TMP_Text player2_Score;

    public TMP_Text questionCount;
    public GameObject tappingProtection_Panel;

    [Header("Session Results UI Setup")]
    public GameObject sessionResults_Panel;
    public Image result_Player1_Picture;
    public TMP_Text result_Player1_Name;
    public TMP_Text result_Player1_Score;

    public Image result_Player2_Picture;
    public TMP_Text result_Player2_Name;
    public TMP_Text result_Player2_Score;


    [Header("Question UI Setup")]
    public TMP_Text question;
    public Button answer1_Button;
    public Button answer2_Button;
    public Button answer3_Button;
    public Button answer4_Button;
    public TMP_Text answer1;
    public TMP_Text answer2;
    public TMP_Text answer3;
    public TMP_Text answer4;


    public void startSession(float percentage)
    {
        sessionData.sessionCatalogue = SaveManager.instance.chosenQuestionCatalogueToPlay;
        sessionData.fillSessionData(percentage);
    }

    private void initiatePlayer1Data() //Should get called only once on start
    {
        player1.playerName = SaveManager.playerProfile.userName;
        player1.profilePicture_ID = SaveManager.playerProfile.profilePicture_ID;
    }

    public void fillPlayerUI() //Should get called only once on start
    {
        player1_Picture.GetComponent<Image>().sprite = SaveManager.instance.spriteList[player1.profilePicture_ID];
        player1_Name.GetComponent<Text>().text = player1.playerName;
        player1_Score.GetComponent<Text>().text = (player1.currentScore).ToString();

        if (multiplayer)
        {
            player2_Picture.GetComponent<Image>().sprite = SaveManager.instance.spriteList[player2.profilePicture_ID];
            player2_Name.GetComponent<Text>().text = player2.playerName;
            player1_Score.GetComponent<Text>().text = (player2.currentScore).ToString();
        }
    }
    public void setMultiplayerTrue()
    {
        multiplayer = true;
    }

    private void updatePlayerScore() //Right Answer gives 10 Points to the Score
    {
        if (player1_clickedCorrectAnswer)
        {
            player1.currentScore += scorePerQuestion;
            player1_clickedCorrectAnswer = false;
        }
        if(multiplayer && player2_clickedCorrectAnswer)
        {
            player2.currentScore += scorePerQuestion;
            player2_clickedCorrectAnswer = false;
        }
    }

    public void updateScreenScoreAndQuestionCount()
    {
        player1_Score.GetComponent<Text>().text = (player1.currentScore).ToString();
        if (multiplayer)
        {
            player2_Score.GetComponent<Text>().text = (player2.currentScore).ToString();
        }
        questionCount.GetComponent<Text>().text = (sessionData.questionIndex + 1).ToString() + " / " + sessionData.questionCount_total.ToString();
    }
    private void showRightAnswer() //And tap on Screen to continue
    {
        tappingProtection_Panel.SetActive(true);
        answer1_Button.GetComponent<Image>().color = Color.red;
        answer2_Button.GetComponent<Image>().color = Color.red;
        answer3_Button.GetComponent<Image>().color = Color.red;
        answer4_Button.GetComponent<Image>().color = Color.red;
        switch (sessionData.rightAnswerPosition)
        {
            case 0:
                answer1_Button.GetComponent<Image>().color = Color.green;
                break;
            case 1:
                answer2_Button.GetComponent<Image>().color = Color.green;
                break;
            case 2:
                answer3_Button.GetComponent<Image>().color = Color.green;
                break;
            case 3:
                answer4_Button.GetComponent<Image>().color = Color.green;
                break;
            default:
                Debug.LogError("SHOW RIGHT ANSWER ERROR");
                break;
        }
    }

    public void buttonPress(int answerID)
    {
        if(!multiplayer)
        {
            if (answerID == sessionData.rightAnswerPosition)
                player1_clickedCorrectAnswer = true;
                updatePlayerScore();

            sessionData.questionIndex++;
            updateScreenScoreAndQuestionCount();
            showRightAnswer();
        }
        else if (turn == 1)
        {
            if (answerID == sessionData.rightAnswerPosition)
                player1_clickedCorrectAnswer = true;
            turn = 2;
        }
        else if (turn == 2)
        {
            if (answerID == sessionData.rightAnswerPosition)
                player1_clickedCorrectAnswer = true;
            turn = 1;
            updatePlayerScore();
            sessionData.questionIndex++;
            updateScreenScoreAndQuestionCount();
            showRightAnswer();
        }
    }
    public void nextQuestion() // Used by a button on Interaction Blocking Panel
    {
        if (sessionData.questionIndex > sessionData.questionCount_total)
        {
            showSessionResults();
            return;
        }

        fillNextQuestion();
        tappingProtection_Panel.SetActive(false);
    }

    private void fillNextQuestion()
    {
        sessionData.activeQuestion = sessionData.questionList[sessionData.questionIndex];
        sessionData.rightAnswerPosition = sessionData.activeQuestion.rightAnswerPosition;
        question.GetComponent<Text>().text = sessionData.activeQuestion.questionName;
        answer1.GetComponent<Text>().text = sessionData.activeQuestion.answers[0];
        answer2.GetComponent<Text>().text = sessionData.activeQuestion.answers[1];
        answer3.GetComponent<Text>().text = sessionData.activeQuestion.answers[2];
        answer4.GetComponent<Text>().text = sessionData.activeQuestion.answers[3];
        answer1_Button.GetComponent<Image>().color = Color.white;
        answer2_Button.GetComponent<Image>().color = Color.white;
        answer3_Button.GetComponent<Image>().color = Color.white;
        answer4_Button.GetComponent<Image>().color = Color.white;
    }


    private void showSessionResults()
    {
        sessionResults_Panel.SetActive(true);
        result_Player1_Picture.GetComponent<Image>().sprite = SaveManager.instance.spriteList[player1.profilePicture_ID];
        result_Player1_Name.GetComponent<Text>().text = player1.playerName;
        result_Player1_Score.GetComponent<Text>().text = (player1.currentScore).ToString();
        if (multiplayer)
        {
            result_Player2_Picture.GetComponent<Image>().sprite = SaveManager.instance.spriteList[player2.profilePicture_ID];
            result_Player2_Name.GetComponent<Text>().text = player2.playerName;
            result_Player2_Score.GetComponent<Text>().text = (player2.currentScore).ToString();
        }

        SaveManager.instance.updateStatistic(sessionData.sessionCatalogue.fileName, player1.currentScore);
    }
}