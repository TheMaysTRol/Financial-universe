using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Company : MonoBehaviour
{

    public enum CompaniesTypes
    {
        IT,
        Cars,
        SocialMedias,
        Food,
        Clothes
    }
    
    public string companyName;
    public string description;
    public float risk;
    public float returnRate;
    public float currentStockPrice;
    public CompaniesTypes companyType;

    [HideInInspector]
    public int boughtstocks = 0;
    [HideInInspector]
    public float earnedMoney = 0;

    public List<float> laststocks = new List<float>();



    public void Awake()
    {
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = companyName;
    }
    public void AddHistory(float oldstockprice){
        float percentage = (oldstockprice - currentStockPrice) / currentStockPrice * 100;
        UiManager.Instance.AddHistory(Mathf.Round(percentage * 100f) / 100f, this);
        laststocks.Add(oldstockprice);
        if (laststocks.Count > 10)
        {
            laststocks.RemoveAt(laststocks.Count - 1);
        }
    }

    float CalculatePercentage(float y, float x)
    {
        return y / x * 100f;   // calculate the percentage
    }
    
    public float GetEarnings() {
        return earnedMoney ;
    }
    
    public override string ToString()
    {
        return $"Name: {companyName}, Stock Price: {currentStockPrice:C}, Risk: {risk:P2}, Return: {returnRate:P2}";
    }

}
