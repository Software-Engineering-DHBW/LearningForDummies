using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SaveData;

public class SaveSystem : MonoBehaviour
{
    //Singleton Declaration
    public static SaveSystem instance;

    string filePath;
    string playerProfilePath;
    public enum StatusCodes{OK,Error};


    private void Awake()
    {
        filePath = Application.persistentDataPath + "/Test.qcat";
        playerProfilePath = Application.persistentDataPath + "/Player.pp";

        Debug.Log(filePath);
        Debug.Log(playerProfilePath);

        //Singleton Pattern
        if (SaveSystem.instance == null && instance == null)
        {
            Debug.Log("No existing SaveSystem Instance found. Creating new one.");
            instance = this;
        }
        else
        {
            Debug.Log("Existing SaveSystem Instance found. Secure Self Destruction executed.");
            Destroy(gameObject);
        }

    }

    public StatusCodes saveRawJsonTextToJson(string catalogueName, string data)
    {
        try{
        string filename = "/" + catalogueName + ".qcat";
        string content = data;
        System.IO.File.WriteAllText(Application.persistentDataPath + filename, content);
        Debug.Log("Speicherung erfolgt als... " + filename);
        return StatusCodes.OK;
        }
        catch{
            return StatusCodes.Error;
        }
    }
    
    public StatusCodes saveQuestionCatalogueToJson(QuestionCatalogue _questionCatalogue)
    {
        try{
        string fileName = "/" + _questionCatalogue.fileName + ".qcat";
        string content = JsonUtility.ToJson(_questionCatalogue);
        System.IO.File.WriteAllText(Application.persistentDataPath + fileName, content);
        Debug.Log("QuestionCatalogue has been successfully saved to the FileSystem");
        return StatusCodes.OK;
        }
        catch{
            return StatusCodes.Error;
        }
    }

    public StatusCodes savePlayerProfileToJson(PlayerProfile _playerProfile)
    {
        try{
        string fileName = "/" + _playerProfile.fileName + ".pp";
        string content = JsonUtility.ToJson(_playerProfile);
        System.IO.File.WriteAllText(Application.persistentDataPath + fileName, content);
        Debug.Log("PlayerProfile has been successfully saved to the FileSystem");
        return StatusCodes.OK;
        }
        catch{
            return StatusCodes.Error;
        }
    }

    public string loadTextRawFromJson(string catalogueName)
    {
        try{
        string extension = "*.qcat"; // The "*" ist really important. It is a placeholder for the rest of the File
        string[] Files = Directory.GetFiles(Application.persistentDataPath, extension);
        foreach (string file in Files)
        {
            Debug.Log("The following file has been found: " + Path.GetFileName(file));
            if (string.Equals(catalogueName, Path.GetFileName(file)))
            {
                string DataFromJson = File.ReadAllText(file);
                Debug.Log("----------\nERFOLGREICH\n----------");
                return DataFromJson;
            }
        }
        if (Files.Length == 0)
        {
            Debug.Log("No files have been found with the extension " + extension);
            return null;
        }
        Debug.Log("----------\nOPERATION FAILED\n----------");
        return null;
        }
        catch{
            return StatusCodes.Error.ToString();
        }
    }

    public List<QuestionCatalogue> loadQuestionCataloguesFromJson()
    {
        string extension = "*.qcat";
        List<QuestionCatalogue> allCatalogues = new List<QuestionCatalogue>();
        string[] Files = Directory.GetFiles(Application.persistentDataPath, extension);
        foreach (string file in Files)
        {
            Debug.Log("The following file has been found: " + Path.GetFileName(file));
            string DataFromJson = File.ReadAllText(file);
            try{
            QuestionCatalogue questionCatalogue = JsonUtility.FromJson<QuestionCatalogue>(DataFromJson);
            allCatalogues.Add(questionCatalogue);
            Debug.Log("A catalogue has been added to the List.");
            }
            catch{
                Debug.Log("The file format was invalid.");
            }
        }
        if (Files.Length == 0)
        {
            Debug.Log("No files with the extension " + extension + " have been found.");
            return null;
        }
        else if(allCatalogues.Count == 0){
            Debug.Log("A file was found but not compatible.");
            return null;
        }
        Debug.Log("The List of found QuestionCatalogues contains " + allCatalogues.Count + " QuestionCatalogues.");
        return allCatalogues;
    }

    public PlayerProfile loadPlayerProfileFromJson(string fileName)
    {
        try{
        string extension = "*.pp"; // The "*" ist really important. It is a placeholder for the rest of the File
        string[] Files = Directory.GetFiles(Application.persistentDataPath, extension);
        foreach (string file in Files)
        {
            Debug.Log("The following file has been found: " + Path.GetFileName(file));
            if (string.Equals(fileName, Path.GetFileName(file)))
            {
                string DataFromJson = File.ReadAllText(file);
                PlayerProfile playerProfile = JsonUtility.FromJson<PlayerProfile>(DataFromJson);

                Debug.Log("----------\nERFOLGREICH\n----------");
                return playerProfile;
            }
        }
        if (Files.Length == 0)
        {
            Debug.Log("No files have been found with the extension " + extension);
            return null;
        }
        Debug.Log("----------\nOPERATION FAILED\n----------");
        return null;
        }
        catch{
            return null;
        }
    }




}