using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveData;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour
{
    //Singleton Declaration
    public static SaveManager instance;

    [Header("QuestionCatalogue Stuff")]
    public List<QuestionCatalogue> questionCatalogueList;
    public QuestionCatalogue createdQuestionCatalogue;
    public QuestionCatalogue chosenQuestionCatalogueToPlay;

    [Header("Import UI")]
    public TMP_InputField rawJsonText_InputField;
    public TMP_InputField enteredCatalogueName;

    [Header("PlayerProfile Stuff")]
    public static PlayerProfile playerProfile;
    public Sprite[] spriteList;

    [Header("General UI Setup")]
    public GameObject username_Panel;
    public Text questionCount_Label;

    [Header("Profile UI Setup")]
    public Image profilePicture;
    public TMP_InputField profileName_Inputfield;
    public int currentProfilePicture_Selection;

    [Header("Input Fields UI")]
    public TMP_InputField questionName_InputField;
    public TMP_InputField questionCatalogueName_Inputfield;
    public TMP_InputField username_Inputfield;
    public TMP_InputField[] answer_Inputfields;

    [Header("Question Catalogue UI before Session Start")]
    public GameObject contentPanel;
    public GameObject cataloguePrefab;

    public GameObject contentPanelAndStatistic;
    public GameObject cataloguePrefabAndStatistic;

    public GameObject sessionSetup_Panel;
    public TMP_Text cataloguename_display;
    public GameObject catalogueSelect_Panel;

    public GameObject catalogueCreation_Panel;
    public GameObject catalogueAndStatisticSelectPanel;


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
        createdQuestionCatalogue = new QuestionCatalogue();
        
        if(questionCatalogueList == null)
        {
            questionCatalogueList = new List<QuestionCatalogue>();
        }
        if (playerProfile == null)
        {
            openUsernamePanel();
        }
        else
        {
            playerProfileStatistics();
        }

    }

    public void saveImportedQuestionCatalogueToFileSystem()
    {
        if (string.IsNullOrWhiteSpace(enteredCatalogueName.text) || string.IsNullOrWhiteSpace(rawJsonText_InputField.text))
        {
            Debug.Log("ERROR: Katalogname oder Import-Textfeld sind leer!");
            return;
        }
        else
        {
            SaveSystem.instance.saveRawJsonTextToJson(enteredCatalogueName.text, rawJsonText_InputField.text);
            questionCatalogueList = SaveSystem.instance.loadQuestionCataloguesFromJson();
            playerProfileStatistics();
            clearImportInputFields();
        }
    }

    public void clearImportInputFields()
    {
        enteredCatalogueName.text = "";
        rawJsonText_InputField.text = "";
    }

    public void OnClickPasteClipboardIntoInputField()
    {
        rawJsonText_InputField.text = GUIUtility.systemCopyBuffer;
    }

    public void OnClickCopyRawTextFromJsonToClipboard(int catalogue_ID)
    {
        Debug.Log("My ID is:" + catalogue_ID);

        Debug.Log("----------\nSTRING COPY TO CLIPBOARD IN PROGRSSS\n----------");

        string catalogueName = questionCatalogueList[catalogue_ID].fileName + ".qcat";

        SaveSystem.instance.loadTextRawFromJson(catalogueName);

        string catalogueRawText = SaveSystem.instance.loadTextRawFromJson(catalogueName);

        if (catalogueRawText != null)
        {
            GUIUtility.systemCopyBuffer = catalogueRawText;
        }
        else
        {
            GUIUtility.systemCopyBuffer = "Didn't Work :(";
        }
        Debug.Log("----------\nSTRING COPY TO CLIPBOARD DONE\n----------");
    }


    public void OnProfileArrowClick(bool right)
    {
        if (right)
        {
            currentProfilePicture_Selection++;

            if(currentProfilePicture_Selection > spriteList.Length - 1)
            {
                currentProfilePicture_Selection = 0;
            }
            profilePicture.GetComponent<Image>().sprite = spriteList[currentProfilePicture_Selection];
        }
        else
        {
            currentProfilePicture_Selection--;

            if (currentProfilePicture_Selection < 0)
            {
                currentProfilePicture_Selection = spriteList.Length - 1;
            }
        }
        profilePicture.GetComponent<Image>().sprite = spriteList[currentProfilePicture_Selection];
    }

    public void fillProfilePanel()
    {
        currentProfilePicture_Selection = playerProfile.profilePicture_ID;
        profilePicture.GetComponent<Image>().sprite = spriteList[currentProfilePicture_Selection];
        profileName_Inputfield.text = playerProfile.userName;
    }
    public void OnClickSaveProfileSettings()
    {
        if (string.IsNullOrWhiteSpace(profileName_Inputfield.text))
        {
            Debug.Log("Username not allowed!");
            return;
        }
        playerProfile.userName = profileName_Inputfield.text;
        playerProfile.profilePicture_ID = currentProfilePicture_Selection;
        savePlayerProfileToFile();
    }

    private void openSessionSetupPanel()
    {
        catalogueSelect_Panel.SetActive(false);
        sessionSetup_Panel.SetActive(true);
    }

    private void openCatalogueCreationPanel()
    {
        catalogueAndStatisticSelectPanel.SetActive(false);
        catalogueCreation_Panel.SetActive(true);
        questionCatalogueName_Inputfield.text = createdQuestionCatalogue.fileName;
        questionCatalogueName_Inputfield.interactable = false;
    }

    private void setQuestionCatalogueToPlay(int nameID)
    {
        Debug.Log("My name is:" + nameID);
        chosenQuestionCatalogueToPlay = questionCatalogueList[nameID];
        Debug.Log(chosenQuestionCatalogueToPlay.fileName + " " + chosenQuestionCatalogueToPlay.questions.Count);
    }
    private void setQuestionCatalogueToEdit(int nameID)
    {
        Debug.Log("My name is:" + nameID);
        createdQuestionCatalogue = questionCatalogueList[nameID];
        Debug.Log(createdQuestionCatalogue.fileName + " " + createdQuestionCatalogue.questions.Count);
    }

    public void fillCatalogueScrollList()
    {
        int index = 0;
        UnityAction callback2 = () => SaveManager.instance.openSessionSetupPanel();
        UnityAction callback3 = () => SaveManager.instance.fillCatalogueNameInSessionSetupPanel();

        foreach (QuestionCatalogue questionCatalogue in questionCatalogueList)
        {
            GameObject newCatalogueItem = Instantiate(cataloguePrefab) as GameObject;
            newCatalogueItem.transform.parent = contentPanel.transform;
            newCatalogueItem.transform.localScale = Vector3.one;
            newCatalogueItem.GetComponentInChildren<TMP_Text>().text = questionCatalogue.fileName;
            newCatalogueItem.name = index.ToString();

            UnityAction callback = () => SaveManager.instance.setQuestionCatalogueToPlay( int.Parse(newCatalogueItem.name));

            newCatalogueItem.GetComponentInChildren<Button>().onClick.AddListener(callback);
            newCatalogueItem.GetComponentInChildren<Button>().onClick.AddListener(callback2);
            newCatalogueItem.GetComponentInChildren<Button>().onClick.AddListener(callback3);

            index++;
        }
    }

    public void fillCatalogueAndStaticsticScrollList()
    {
        int index = 0;
        UnityAction callback2 = () => SaveManager.instance.openCatalogueCreationPanel();
        UnityAction callback3 = () => SaveManager.instance.displayQuestionCount();

        foreach (QuestionCatalogue questionCatalogue in questionCatalogueList)
        {
            GameObject newCatalogueItem = Instantiate(cataloguePrefabAndStatistic) as GameObject;
            newCatalogueItem.transform.parent = contentPanelAndStatistic.transform;
            newCatalogueItem.transform.localScale = Vector3.one;
            TMP_Text[] texts = newCatalogueItem.GetComponentsInChildren<TMP_Text>();
            texts[0].text = questionCatalogue.fileName;

            foreach (Statistic statistic in playerProfile.statistics)
            {
                Debug.Log("---!!!--- Checking Statistic Label: " + statistic.label);
                if (string.Equals(questionCatalogue.fileName, statistic.label))
                {
                    texts[1].text = "Highscore:\n" + statistic.score;
                }
            }
            newCatalogueItem.name = index.ToString();
            UnityAction callback0 = () => SaveManager.instance.OnClickCopyRawTextFromJsonToClipboard(int.Parse(newCatalogueItem.name));
            UnityAction callback = () => SaveManager.instance.setQuestionCatalogueToEdit(int.Parse(newCatalogueItem.name));
            Button[] buttons = newCatalogueItem.GetComponentsInChildren<Button>();
            buttons[0].onClick.AddListener(callback0);
            buttons[1].onClick.AddListener(callback);
            buttons[1].onClick.AddListener(callback2);
            buttons[1].onClick.AddListener(callback3);

            index++;
        }
    }

    public void clearCatalogueScrollList()
    {
        foreach (Transform child in contentPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void clearCatalogueScrollListAndStatistic()
    {
        foreach (Transform child in contentPanelAndStatistic.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void fillCatalogueNameInSessionSetupPanel()
    {
        Debug.Log("Trying to change Display to: " + chosenQuestionCatalogueToPlay.fileName);
        cataloguename_display.text = chosenQuestionCatalogueToPlay.fileName;
    }

    public void clearOwnCatalogue()
    {
        createdQuestionCatalogue = new QuestionCatalogue();
        questionCatalogueName_Inputfield.text = "";
        questionCatalogueName_Inputfield.interactable = true;
    }

    public void clearAllInputFields()
    {
        questionName_InputField.text = "";
        //username_Inputfield.text = "";

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
        clearOwnCatalogue();
        questionCatalogueList = SaveSystem.instance.loadQuestionCataloguesFromJson();
        displayQuestionCount();
        playerProfileStatistics();

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

            questionCatalogueList.Add(SaveSystem.instance.addStandard("Funknetze"));
            questionCatalogueList.Add(SaveSystem.instance.addStandard("Machine_Learning"));
            questionCatalogueList.Add(SaveSystem.instance.addStandard("Verteilte_Systeme"));

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
