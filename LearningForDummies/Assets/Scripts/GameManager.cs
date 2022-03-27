using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public SessionData sessionData;
    public SessionPlayerData player1;
    public SessionPlayerData player2;
    public bool multiplayer = false;
    public bool player1_clickedCorrectAnswer = false;
    public bool player2_clickedCorrectAnswer = false;
    public int scorePerQuestion = 10;
    public int turn = 1; //Shows which Player-Turn it is: 1 for Player1 and 2 for Player2

    [Header("UI Setup")]
    public GameObject session_Panel;

    public Image player1_Picture;
    public TMP_Text player1_Name;
    public TMP_Text player1_Score;

    public GameObject player2_UI_Row;
    public Image player2_Picture;
    public TMP_Text player2_Name;
    public TMP_Text player2_Score;

    public TMP_Text questionCount;
    public TMP_Text turn_Display;
    public GameObject tappingProtection_Panel;

    [Header("Session Results UI Setup")]
    public GameObject sessionResults_Panel;
    public Image result_Player1_Picture;
    public TMP_Text result_Player1_Name;
    public TMP_Text result_Player1_Score;

    public GameObject player2_UI_Result_Row;
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


    private void Awake()
    {
        // Singleton Pattern
        if (GameManager.instance == null && instance == null)
        {
            Debug.Log("No existing GameManager Instance found. Creating new one.");
            instance = this;
        }
        else
        {
            Debug.Log("Existing GameManager Instance found. Secure Self Destruction executed.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sessionData = new SessionData();
        player1 = new SessionPlayerData();
        player2 = new SessionPlayerData();
    }

    public void startSession(float percentage)
    {
        sessionData.sessionCatalogue = SaveManager.instance.chosenQuestionCatalogueToPlay;
        sessionData.fillSessionData(percentage);
        Debug.Log("Initiating Session with Catalogue " + sessionData.sessionCatalogue.fileName + " with " + sessionData.questionList.Count + " chosen Questions!");
    }

    private void initiatePlayer1Data() //Should get called only once on start
    {
        player1.playerName = SaveManager.playerProfile.userName;
        player1.profilePicture_ID = SaveManager.playerProfile.profilePicture_ID;
    }

    public void fillPlayerUI() //Should get called only once on start
    {
        initiatePlayer1Data();
        player1_Picture.GetComponent<Image>().sprite = SaveManager.instance.spriteList[player1.profilePicture_ID];
        player1_Name.text = player1.playerName;
        player1_Score.text = (player1.currentScore).ToString();

        if (multiplayer)
        {
            player2_UI_Row.SetActive(true);
            player2_Picture.GetComponent<Image>().sprite = SaveManager.instance.spriteList[player2.profilePicture_ID];
            player2_Name.text = player2.playerName;
            player2_Score.text = (player2.currentScore).ToString();
            turn_Display.text = "Turn 1: " + player1_Name;
        }
        
        updateScreenScoreAndQuestionCount();
    }
    public void setMultiplayerTrue(bool multi)
    {
        multiplayer = multi;
    }

    private void updatePlayerScore() //Right Answer gives 10 Points to the Score
    {
        if (player1_clickedCorrectAnswer)
        {
            player1.currentScore += scorePerQuestion;
            player1_clickedCorrectAnswer = false;
        }
        if(player2_clickedCorrectAnswer)
        {
            player2.currentScore += scorePerQuestion;
            player2_clickedCorrectAnswer = false;
        }
    }

    public void updateScreenScoreAndQuestionCount()
    {
        player1_Score.text = "Score: " + (player1.currentScore).ToString();
        if (multiplayer)
        {
            player2_Score.text = "Score: " + (player2.currentScore).ToString();
        }
        questionCount.text = (sessionData.questionIndex + 1).ToString() + " / " + sessionData.questionCount_total.ToString();
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

            updateScreenScoreAndQuestionCount();
            sessionData.questionIndex++;
            showRightAnswer();
        }
        else if (turn == 1)
        {
            if (answerID == sessionData.rightAnswerPosition)
                player1_clickedCorrectAnswer = true;
            turn = 2;
            turn_Display.text = "Turn " + turn + ": " + player2.playerName;
        }
        else if (turn == 2)
        {
            if (answerID == sessionData.rightAnswerPosition)
                player2_clickedCorrectAnswer = true;
            turn = 1;
            updatePlayerScore();
            sessionData.questionIndex++;
            updateScreenScoreAndQuestionCount();
            showRightAnswer();
        }
    }
    public void nextQuestion() // Used by a button on Interaction Blocking Panel
    {
        if (sessionData.questionIndex == sessionData.questionCount_total)
        {
            showSessionResults();
            return;
        }
        updateScreenScoreAndQuestionCount();
        fillNextQuestion();
        if (multiplayer)
        {
            turn_Display.text = "Turn " + turn + ": " + player1.playerName;
        }
        tappingProtection_Panel.SetActive(false);
    }

    private void fillNextQuestion()
    {
        sessionData.activeQuestion = sessionData.questionList[sessionData.questionIndex];
        sessionData.rightAnswerPosition = sessionData.activeQuestion.rightAnswerPosition;
        question.text = sessionData.activeQuestion.questionName;
        answer1.text = sessionData.activeQuestion.answers[0];
        answer2.text = sessionData.activeQuestion.answers[1];
        answer3.text = sessionData.activeQuestion.answers[2];
        answer4.text = sessionData.activeQuestion.answers[3];
        answer1_Button.GetComponent<Image>().color = Color.white;
        answer2_Button.GetComponent<Image>().color = Color.white;
        answer3_Button.GetComponent<Image>().color = Color.white;
        answer4_Button.GetComponent<Image>().color = Color.white;
    }


    private void showSessionResults()
    {
        Debug.Log("-------\n-------Session Finished--------\n------");
        session_Panel.SetActive(false);
        sessionResults_Panel.SetActive(true);
        result_Player1_Picture.GetComponent<Image>().sprite = SaveManager.instance.spriteList[player1.profilePicture_ID];
        result_Player1_Name.text = player1.playerName;
        result_Player1_Score.text = (player1.currentScore).ToString();
        if (multiplayer)
        {
            player2_UI_Result_Row.SetActive(true);
            result_Player2_Picture.GetComponent<Image>().sprite = SaveManager.instance.spriteList[player2.profilePicture_ID];
            result_Player2_Name.text = player2.playerName;
            result_Player2_Score.text = (player2.currentScore).ToString();
        }

        SaveManager.instance.updateStatistic(sessionData.sessionCatalogue.fileName, player1.currentScore);
    }

    public void clearSessionData()
    {
        SaveManager.instance.chosenQuestionCatalogueToPlay = null;
        player1 = new SessionPlayerData();
        player2 = new SessionPlayerData();
        sessionData = new SessionData();
        multiplayer = false;
        player1_clickedCorrectAnswer = false;
        player2_clickedCorrectAnswer = false;
        player2_UI_Row.SetActive(false);
        player2_UI_Result_Row.SetActive(false);
        turn = 1;
    }
}