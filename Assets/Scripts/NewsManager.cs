using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Data;

public class NewsManager : MonoBehaviour
{

    public List<News> _positiveNews = new List<News>();
    public List<News> _negativeNews = new List<News>();
    public List<Company> _companies;
    public List<News> activeNews;
    public UnityEvent<News,Company> onAddNews;
    public UiManager uimanager;

    public float newsMinRandTime = 60;
    public float newsMaxRandTime = 120;

    public int newsLimitInMenu = 8;

    public NewsManager(List<Company> companies)
    {
        _companies = companies;
    }


    public void Start()
    {
        StartCoroutine(GenerateNews());
    }


    public IEnumerator GenerateNews()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(newsMinRandTime, newsMaxRandTime));
        double randomChance = UnityEngine.Random.Range(0, 100);
        if (randomChance <= 50)
        {
            // Company is affected by negative news
            News news = _negativeNews[UnityEngine.Random.Range(0, _negativeNews.Count)];
            Company company = FindRandomCompanyByType(news.companyType);
            float oldstock = company.currentStockPrice;
            company.currentStockPrice -= Mathf.Round(company.currentStockPrice * UnityEngine.Random.Range(0.01f, 0.05f) * 100f) / 100f;
            company.AddHistory(oldstock);
            activeNews.Add(news);
           // Debug.Log($"{company.companyName} {news.headline} Stock price decreased to {company.currentStockPrice:C}");
            uimanager.AddNewsToMenu(news,company);
            uimanager.SetNewStockValues(company);
        }
        else
        {
            // Company is affected by positive news
            News news = _positiveNews[UnityEngine.Random.Range(0, _positiveNews.Count)];
            Company company = FindRandomCompanyByType(news.companyType);
            float oldstock = company.currentStockPrice;
            company.currentStockPrice += Mathf.Round(company.currentStockPrice * UnityEngine.Random.Range(0.01f, 0.05f) * 100f) / 100f;
            company.AddHistory(oldstock);
            activeNews.Add(news);
            //Debug.Log($"{company.companyName} {news.headline} Stock price increased to {company.currentStockPrice:C}");
            uimanager.AddNewsToMenu(news,company);
            uimanager.SetNewStockValues(company);
        }
        StartCoroutine(uimanager.ShowAnimateNotifWindow(true));
        StartCoroutine(GenerateNews());
    }

    public Company FindRandomCompanyByType(Company.CompaniesTypes type)
    {
        List<Company> matchingCompanies = new List<Company>();

        foreach (Company company in _companies)
        {
            if (company.companyType == type)
            {
                matchingCompanies.Add(company);
            }
        }
        
        if (matchingCompanies.Count == 0)
        {
            return null;
        }


        int randomIndex = UnityEngine.Random.Range(0, matchingCompanies.Count);
        return matchingCompanies[randomIndex];
    }

}
