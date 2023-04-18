
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewsElement : MonoBehaviour
{

    public TextMeshProUGUI title, description;
    public Image image;

    public void SetData(string title, string description, Sprite image, int impact)
    {
        this.title.text = title;
        this.description.text = description;
        if (image != null)
        {
            this.image.sprite = image;
        }
        if (impact > 0)
        {
            //positive impact
        }
        else
        {
            //negative impact
        }
    }
}
