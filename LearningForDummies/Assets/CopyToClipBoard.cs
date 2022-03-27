using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyToClipBoard : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("----------\nSTRING COPY TO CLIPBOARD IN PROGRSSS\n----------");
        string catalogueName = "Mein_Katalog.qcat";
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
}
