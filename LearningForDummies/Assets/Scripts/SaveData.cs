using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    
}

[System.Serializable]
public class Fragenliste
{
    public string datei_name;
    public List<Frage> frage = new List<Frage>();
}

[System.Serializable]
public struct Frage
{
    public string frage_name;
    public string antwort_1;
    public string antwort_2;
    public string antwort_3;
    public string antwort_4;
    public int richtig;

   
    
}
