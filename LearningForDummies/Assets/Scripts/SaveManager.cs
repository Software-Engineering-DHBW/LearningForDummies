using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveData;
using UnityEngine.UI;
using TMPro;

public class SaveManager : MonoBehaviour
{
    //Singleton Declaration
    public static SaveManager instance;

    [Header("QuestionCatalogue Stuff")]
    public List<QuestionCatalogue> questionCatalogueList;
    public QuestionCatalogue createdQuestionCatalogue;
    public QuestionCatalogue chosenQuestionCatalogueToPlay;

    [Header("PlayerProfile Stuff")]
    public static PlayerProfile playerProfile;
    public Sprite[] spriteList;

    [Header("General UI Setup")]
    public GameObject username_Panel;
    public Text questionCount_Label;

    [Header("Input Fields UI")]
    public TMP_InputField questionName_InputField;
    public TMP_InputField questionCatalogueName_Inputfield;
    public TMP_InputField username_Inputfield;
    public TMP_InputField[] answer_Inputfields;

    [Header("Question Catalogue UI before Session Start")]
    public GameObject contentPanel;
    public GameObject cataloguePrefab;


    private void Awake()
    {
        // Singleton Pattern
        if (SaveManager.instance == null && instance == null)
        {
            Debug.Log("No existing SaveManager Instance found. Creating new one.");
            instance = this;
        }
        else
        {
            Debug.Log("Existing SaveManager Instance found. Secure Self Destruction executed.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerProfile = SaveSystem.instance.loadPlayerProfileFromJson("Player.pp");
        questionCatalogueList = SaveSystem.instance.loadQuestionCataloguesFromJson();

        
        if (playerProfile == null)
        {
            openUsernamePanel();
        }
        else
        {
            playerProfileStatistics();
        }

    }

    public void fillCatalogueScrollList()
    {
        foreach (QuestionCatalogue questionCatalogue in questionCatalogueList)
        {
            GameObject newCatalogueItem = Instantiate(cataloguePrefab) as GameObject;
            newCatalogueItem.transform.parent = contentPanel.transform;
            newCatalogueItem.transform.localScale = Vector3.one;
            newCatalogueItem.GetComponentInChildren<TMP_Text>().text = questionCatalogue.fileName;
        }
    }

    public void clearOwnCatalogue()
    {
        createdQuestionCatalogue = null;
        questionCatalogueName_Inputfield.text = "";
        questionCatalogueName_Inputfield.interactable = true;
    }

    public void clearAllInputFields()
    {
        questionName_InputField.text = "";
        username_Inputfield.text = "";

        foreach (TMP_InputField answerInputField in answer_Inputfields)
        {
            answerInputField.text = "";            
        }
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

        questionName = questionName_InputField.text;

        foreach (TMP_InputField answerInputField in answer_Inputfields)
        {
            string answerText = answerInputField.text;
            Debug.Log("Länge des Textes aus Answer: " + answerText.Length);
            if (string.IsNullOrWhiteSpace(answerText))
            {
                Debug.Log("An Answer was found to be Empty or unusable.\nQuestion Adding Process has been stopped.\nPlease Fill Answer InputFields with reasonable Information.");
                return; //Exits the Function
            }
            else
            {
                Debug.Log("Adding " + answerText + " as Answer");
                answers.Add(answerText);
            }           
        }

        GameObject[] answerToggles = GameObject.FindGameObjectsWithTag("Created_Question_Answer_Toggle");   //UI-InputFields for Question-Answers must have Tag "Created_Question_Answer"
                                                                                                            //The Toggles must all belong to the same Toggle-Group and an option to only nable one Toggle must be activated
        int answerID = 0;
        foreach (GameObject answerToggle in answerToggles)
        {
            Debug.Log(answerToggle.name);
            Debug.Log(answerToggle.GetComponent<Toggle>().isOn.ToString());
            if (answerToggle.GetComponent<Toggle>().isOn == true)
            {
                rightAnswerPosition = answerID;
            }
            else
            {
                answerID++;
            }
        }
        Debug.Log(questionName + " , " + rightAnswerPosition);
        Question question = new Question(questionName, answers, rightAnswerPosition);
        createdQuestionCatalogue.questions.Add(question);

        // Code um InputField leer zu machen

        displayQuestionCount();
        questionCatalogueName_Inputfield.interactable = false;
        clearAllInputFields();
        Debug.Log("A new Question has been added to the currently active QuestionCatalogue.");
    }

    public void displayQuestionCount()
    {
        int questionCount = createdQuestionCatalogue.questions.Count;
        questionCount_Label.GetComponent<Text>().text = questionCount.ToString() + " Qs";
    }

    public void saveQuestionCatalogueToFileSystem()
    {
        string fileName = questionCatalogueName_Inputfield.text;
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
        //createdQuestionCatalogue = null;
        clearOwnCatalogue();
        questionCatalogueList = SaveSystem.instance.loadQuestionCataloguesFromJson();

    }
    public void setUsername()
    {
        string username = username_Inputfield.text;
        if (string.IsNullOrWhiteSpace(username))
        {
            Debug.Log("Username was found to be Empty or unusable.\nUsername Saving Process has been stopped.\nPlease Fill the Username InputField with reasonable Information.");
            return; //Exits the Function
        }
        else
        {
            playerProfile = new PlayerProfile();
            playerProfile.userName = username;
            SaveSystem.instance.savePlayerProfileToJson(playerProfile);
            playerProfileStatistics();
            username_Panel.SetActive(false);
        }
    }

    public void openUsernamePanel()
    {
        //Open a UI-Panel with an InputField for the User to Input his Username
        Debug.Log("No Playerprofile found. Opening UsernamePanel...");
        username_Panel.SetActive(true);
    }

    public void playerProfileStatistics()
    {
        bool newStatistic = false;
        //Compares Available Statistics to Available Catalogues
        foreach (QuestionCatalogue questionCatalogue in questionCatalogueList)
        {
            bool found = false;
            Debug.Log("---!!!--- Checking Catalogue with filename: " + questionCatalogue.fileName);
            foreach (Statistic statistic in playerProfile.statistics)
            {
                Debug.Log("---!!!--- Checking Statistic Label: " + statistic.label);
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
   

}
