using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystemJson : MonoBehaviour
{
    //Singleton Dekleration
    public static SaveSystemJson instance;

    string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/Test.json";

        Debug.Log(filePath);

        //Singleton Pattern
        if (SaveSystemJson.instance == null && instance == null)
        {
            Debug.Log("Es bestand noch keine SaveSystem Instanz einer Vorherigen Szene");
            instance = this;
        }
        else
        {
            Debug.Log("Es existiert bereits eine SaveSystem Instanz einer vorherigen Szene");
            Destroy(gameObject);
        }

    }

    public void SaveToJson(Fragenliste _Fragenliste, string file_name)
    {
        string fileName = "/" + file_name + ".json";
        string inhalt = JsonUtility.ToJson(_Fragenliste);
        System.IO.File.WriteAllText(Application.persistentDataPath + fileName, inhalt);
    }

    public void ShowAllFiles()
    {
       string[] Files = Directory.GetFiles(Application.persistentDataPath);

       foreach (string file in Files)
        {
            Debug.Log("Die Folgenden Dateien wurden gefunden: " + Path.GetFileName(file));

            string DataFromJson = File.ReadAllText(file);
            Fragenliste fragenliste = JsonUtility.FromJson<Fragenliste>(DataFromJson);

            Debug.Log(fragenliste.datei_name);

            int i = 1;
            foreach ( var x in fragenliste.frage )
            {
                Debug.Log( "Frage " + i + " von " + fragenliste.frage.Count);

                Debug.Log("Frage: " + x.frage_name);
                Debug.Log("Antwort 1: " + x.antwort_1);
                Debug.Log("Antwort 2: " + x.antwort_2);
                Debug.Log("Antwort 3: " + x.antwort_3);
                Debug.Log("Antwort 4: " + x.antwort_4);
                Debug.Log("Die Richtige Antwort ist: " + x.richtig);


                i++;

            }

        }
       if ( Files.Length == 0)
        {
            Debug.Log("Es wurden keine Dateien gefunden");
        }
    }

    public Fragenliste LoadJson(string fileName)
    {

        string[] Files = Directory.GetFiles(Application.persistentDataPath);
        foreach (string file in Files)
        {
            Debug.Log("Die Folgenden Dateien wurden gefunden: " + Path.GetFileName(file));
            if(string.Equals(fileName, Path.GetFileName(file)))
            {
                string DataFromJson = File.ReadAllText(file);
                Fragenliste fragenliste = JsonUtility.FromJson<Fragenliste>(DataFromJson);

                Debug.Log("----------\nERFOLGREICH\n----------");
                return fragenliste;
            }
        }
        if (Files.Length == 0)
        {
            Debug.Log("Es wurden keine Dateien gefunden");
            return null;
        }
        Debug.Log("----------\nFEHLSCHLAG ENDE\n----------");
        return null;
    }
}
