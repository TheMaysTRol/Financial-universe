using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LiveChangesElementUI : MonoBehaviour
{

    public TextMeshProUGUI changementText;

    public Color positive, negative;
    public void SetText(string txt,int sign)
    {
        changementText.text = txt;
        if (sign > 0)
        {
            changementText.color = positive;
        }
        else
        {
            changementText.color = negative;
        }
    }
}
