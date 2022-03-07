using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    Fragenliste fragenliste = new Fragenliste();
    public GameObject Box;
    private string fileName;

   
    public void addFrage()
    {
        var temp = "";

        Box = GameObject.Find("InputField_Thema");
        temp = Box.GetComponentInChildren< Text >().text;
        fileName = temp;

        Box = GameObject.Find("InputField_Frage");
        temp = Box.GetComponentInChildren< Text >().text;
        string input_frage = temp;

        Box = GameObject.Find("InputField_Antwort_1");
        temp = Box.GetComponentInChildren< Text >().text;
        string input_antwort_1 = temp;

        Box = GameObject.Find("InputField_Antwort_2");
        temp = Box.GetComponentInChildren< Text >().text;
        string input_antwort_2 = temp;

        Box = GameObject.Find("InputField_Antwort_3");
        temp = Box.GetComponentInChildren< Text >().text;
        string input_antwort_3 = temp;

        Box = GameObject.Find("InputField_Antwort_4");
        temp = Box.GetComponentInChildren< Text >().text;
        string input_antwort_4 = temp;


        int input_richtig = 0;
        if ( GameObject.Find("Toggle_1").GetComponent<Toggle>().isOn == true )
        {
            input_richtig = 1;
        }
        else if( GameObject.Find("Toggle_2").GetComponent< Toggle >().isOn == true )
        {
            input_richtig = 2;
        }
        else if ( GameObject.Find("Toggle_3").GetComponent<Toggle>().isOn == true )
        {
            input_richtig = 3;
        }
        else if ( GameObject.Find("Toggle_4").GetComponent<Toggle>().isOn == true )
        {
            input_richtig = 4;
        }


        fragenliste.datei_name = fileName;
        Frage frage = new Frage();
        frage.frage_name = input_frage;
        frage.antwort_1 = input_antwort_1;
        frage.antwort_2 = input_antwort_2;
        frage.antwort_3 = input_antwort_3;
        frage.antwort_4 = input_antwort_4;
        frage.richtig = input_richtig;
        fragenliste.frage.Add(frage);

        Debug.Log("Frage wurde hinzugefügt.");
    }

    public void SaveButton()
    {
        SaveSystemJson.instance.SaveToJson(fragenliste, fileName);
        Debug.Log("Saved data as " + fileName + ".json " + "in " + Application.persistentDataPath + "");
    }

    public void ShowFilesButton()
    {
        SaveSystemJson.instance.ShowAllFiles();
    }
    public void LoadFileButton()
    {
        Box = GameObject.Find("InputField_Dateiname");
        string filename = Box.GetComponentInChildren<Text>().text;

        fragenliste = SaveSystemJson.instance.LoadJson(filename);
        Box = GameObject.Find("Text_Fragenkatalog_Name");
        Text myText = Box.GetComponent<Text>();
        myText.text = filename;

    }
}
