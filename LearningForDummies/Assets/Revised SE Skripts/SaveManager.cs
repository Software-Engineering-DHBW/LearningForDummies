using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveData;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public List<QuestionCatalogue> questionCatalogueList;
    public PlayerProfile playerProfile;
    public QuestionCatalogue createdQuestionCatalogue;
    public Canvas globalCanvas;

    private void Start()
    {
        playerProfile = SaveSystem.instance.loadPlayerProfileFromJson("Player");
        questionCatalogueList = SaveSystem.instance.loadQuestionCataloguesFromJson();

        if (playerProfile.userName == null)
        {
            openUsernamePanel();
        }
        playerProfileStatistics();
    }

    public void savePlayerProfileToFile()
    {
        SaveSystem.instance.savePlayerProfileToJson(playerProfile);
        Debug.Log("Saved Active PlayerProfile to FileSystem in " + Application.persistentDataPath);
    }

    

    public void addQuestionToOwnQuestionCatalogue()
    {
        string questionName;
        int rightAnswerPosition = 0; //By Default the First Answer is always the correct answer
        List<string> answers = new List<string>(); //The Starting Index of such a List is 0. The First Element of this List can be accessed via "answers[0]".

        questionName = GameObject.Find("InputField_QuestionName").GetComponentInChildren<Text>().text; //UI-InputField for QuestionName must be named "InputField_QuestionName"
        GameObject[] answerInputFields = GameObject.FindGameObjectsWithTag("Created_Question_Answer"); //UI-InputFields for Question-Answers must have Tag "Created_Question_Answer"

        foreach (GameObject answerInputField in answerInputFields)
        {
            string answerText = answerInputField.GetComponentInChildren<Text>().text;
            if (string.IsNullOrWhiteSpace(answerText))
            {
                Debug.Log("An Answer was found to be Empty or unusable.\nQuestion Adding Process has been stopped.\nPlease Fill Answer InputFields with reasonable Information.");
                return; //Exits the Function
            }
            else
            {
                answers.Add(answerText);
            }           
        }

        GameObject[] answerToggles = GameObject.FindGameObjectsWithTag("Created_Question_Answer_Toggle");   //UI-InputFields for Question-Answers must have Tag "Created_Question_Answer"
                                                                                                            //The Toggles must all belong to the same Toggle-Group and an option to only nable one Toggle must be activated
        int answerID = 0;
        foreach (GameObject answerToggle in answerToggles)
        {
            bool active = answerToggle.GetComponent<Toggle>().isOn;
            if (active == true)
            {
                rightAnswerPosition = answerID;
                break;
            }
            else
            {
                answerID++;
                break;
            }
        }
        Question question = new Question(questionName, answers, rightAnswerPosition);
        createdQuestionCatalogue.questions.Add(question);

        // Code um InputField leer zu machen

        Debug.Log("A new Question has been added to the currently active QuestionCatalogue.");
    }

    public void displayQuestionCount()
    {
        int questionCount = createdQuestionCatalogue.questions.Count;
        GameObject.Find("Label_QuestionCount").GetComponentInChildren<Text>().text = questionCount.ToString(); //UI-Label for the Display of the current Question Count has to be named "Label_QuestionCount"
    }

    public void saveQuestionCatalogueToFileSystem()
    {
        string fileName = GameObject.Find("InputField_QuestionCatalogueName").GetComponentInChildren<Text>().text;
        if (string.IsNullOrWhiteSpace(fileName))
        {
            Debug.Log("Catalogue Name was found to be Empty or unusable.\nCatalogue Saving Process has been stopped.\nPlease Fill the Catalogue Name InputField with reasonable Information.");
            return; //Exits the Function
        }
        else
        {
            createdQuestionCatalogue.fileName = fileName;
        }
        SaveSystem.instance.saveQuestionCatalogueToJson(createdQuestionCatalogue);


    }
    public void setUsername()
    {
        string username = GameObject.Find("InputField_Username").GetComponentInChildren<Text>().text;
        if (string.IsNullOrWhiteSpace(username))
        {
            Debug.Log("Username was found to be Empty or unusable.\nUsername Saving Process has been stopped.\nPlease Fill the Username InputField with reasonable Information.");
            return; //Exits the Function
        }
        else
        {
            playerProfile.userName = username;
            SaveSystem.instance.savePlayerProfileToJson(playerProfile);
        }
    }

    public void openUsernamePanel()
    {
        //Open a UI-Panel with an InputField for the User to Input his Username
        GameObject usernamePanel = globalCanvas.transform.Find("Panel_Username").gameObject; //The Panel named "Panel_Username" must be a direct child of the global UI canvas
        usernamePanel.SetActive(true);
    }

    public void playerProfileStatistics()
    {
        bool newStatistic = false;
        //Compares Available Statistics to Available Catalogues
        foreach (QuestionCatalogue questionCatalogue in questionCatalogueList)
        {
            bool found = false;
            foreach (Statistic statistic in playerProfile.statistics)
            {
                if (string.Equals(questionCatalogue.fileName, statistic.label))
                {
                    found = true;
                }
            }
            if (found == false)
            {
                addStatistic(questionCatalogue.fileName, 0);
                newStatistic = true;
            }
           
        }
        if (newStatistic == true)
        {
            SaveSystem.instance.savePlayerProfileToJson(playerProfile);
        }

    }
    private void addStatistic(string label, int score)
    {
        Statistic statistic = new Statistic(label, score);
        playerProfile.statistics.Add(statistic);
        Debug.Log("A new Statistic " + statistic.label + " has been added." );
    }

    public void updateStatistic(string label, int score) //Can be called after every QuestionCatalogue-Session
    {
        bool changed = false;
        foreach (Statistic statistic in playerProfile.statistics)
        {
            if (string.Equals(label, statistic.label) && score > statistic.score)
            {
                statistic.score = score;
                changed = true;
            }
        }
        if (changed == true)
        {
            SaveSystem.instance.savePlayerProfileToJson(playerProfile);
        }
    }
    //Recent Score hinzufügen (auch in SaveData)
   

}
