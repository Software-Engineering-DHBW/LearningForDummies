using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SessionData sessionData;


    public void Start()
    {

    }

}

public enum GameMode
{
    singleplayer = 1,
    multiplayer = 2
}