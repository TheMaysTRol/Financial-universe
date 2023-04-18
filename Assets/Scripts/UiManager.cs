using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UiManager : MonoBehaviour
{

    //singleton
    public static UiManager Instance;
    //Date
    private TimeSystem datetime;
    public TextMeshProUGUI datetimetext;

    //Player
    private Player player;
    public TextMeshProUGUI balance;


    //News menu
    public GameObject newsMenu;
    public Transform newsContent;
    public NewsElement newsElementPrefab;
    private NewsManager newsManager;

    //Companies menu
    public GameObject companiesUI;

    //building uis
    public CanvasGroup buildingsUI;

    //Companies ui
    public TextMeshProUGUI companyNameTxt, companyDescriptionTxt, totalEarningsTxt, stockSellTxt, stockBuyTxt;
    public Transform changesContent;
    public LiveChangesElementUI companyElement;
    private int sellingQuantity, buyginQuantity = 1;
    private Company currentSelectedCompany;
    public Button sellBtn, buyBtn;

    public Transform historyContent;
    public LiveChangesElementUI historyPrefab;

    public CanvasGroup notifPanel;

    public void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }
    public void Start()
    {
        datetime = this.GetComponent<TimeSystem>();
        player = this.GetComponent<Player>();
        newsManager = this.GetComponent<NewsManager>();
        newsManager.onAddNews.AddListener(AddNewsToMenu);
    }

    void Update()
    {
        if (!PauseManager.Instance.isPaused)
        {
            //datetimetext.text = "Day["+datetime.GetDayNumber() +"] "+ datetime.currentDate.ToString("hh:mm tt");
            balance.text = player.balance + "$";
        }
    }

    //news menu

    //public void ClearNewsMenu()
    //{
    //    for(int i =0;i<=newsContent.childCount; i++)
    //    {
    //        Destroy(newsContent.GetChild(i));
    //    }
    //}

    public void AddNewsToMenu(News mynew, Company comp)
    {
        NewsElement element = Instantiate(newsElementPrefab, newsContent);
        element.transform.SetSiblingIndex(0);
        element.SetData(comp.companyName + " " + mynew.headline, comp.description + " " + mynew.description, mynew.image, mynew.impact);
        if (newsContent.childCount > newsManager.newsLimitInMenu)
        {
            Destroy(newsContent.GetChild(newsContent.childCount - 1).gameObject);
        }
    }


    public void EnableNewsMenu(bool b)
    {
        if (b)
        {
            //ClearNewsMenu();
            //CreateNews();
            buildingsUI.interactable = false;
            newsMenu.SetActive(true);
        }
        else
        {
            buildingsUI.interactable = true;
            newsMenu.SetActive(false);
        }
    }

    //Companies

    public void EnableCompaniesMenu(bool b)
    {
        companiesUI.SetActive(b);
        buildingsUI.interactable = !b;


        if (!b)
        {
            currentSelectedCompany = null;
        }
    }

    public void OnClickCompany()
    {
        EnableCompaniesMenu(true);
        Company co = EventSystem.current.currentSelectedGameObject.GetComponent<Company>();
        if (co)
        {
            currentSelectedCompany = co;
            ImportHistory();
            companyNameTxt.text = co.companyName;
            companyDescriptionTxt.text = co.description;
            totalEarningsTxt.text = co.GetEarnings() + "";
            stockSellTxt.text = "-" + co.currentStockPrice;
            stockBuyTxt.text = "+" + co.currentStockPrice;
            HandleSellAndBuybtns();
        }
    }

    public void HandleSellAndBuybtns()
    {
        if (!player.investedCompanies.Contains(currentSelectedCompany))
        {
            sellBtn.interactable = false;
        }
        else
        {
            sellBtn.interactable = true;
        }
        if (player.balance < currentSelectedCompany.currentStockPrice)
        {
            buyBtn.interactable = false;
        }
        else
        {
            buyBtn.interactable = true;
        }
    }

    public void ChangeSellQuantityByOne(int sign = 1)
    {
        if (sign != 1 && sign != -1)
        {
            return;
        }
        int quant = sellingQuantity + (1 * sign);
        if (quant > 0)
        {
            if (quant <= currentSelectedCompany.boughtstocks)
            {
                sellingQuantity = quant;
                stockSellTxt.text = "-" + (currentSelectedCompany.currentStockPrice * sellingQuantity);
            }
        }
    }

    public void ChangeBuyQuantityByOne(int sign = 1)
    {
        if (sign != 1 && sign != -1)
        {
            return;
        }
        int quant = buyginQuantity + (1 * sign);
        if (quant > 0)
        {
            if ((currentSelectedCompany.currentStockPrice * quant) <= player.balance)
            {
                buyginQuantity = quant;
                stockBuyTxt.text = "+" + (currentSelectedCompany.currentStockPrice * buyginQuantity);
            }

        }
    }

    public void OnClickBuyStocks()
    {
        buyBtn.interactable = false;
        player.BuyStocks(currentSelectedCompany, buyginQuantity);
        totalEarningsTxt.text = currentSelectedCompany.GetEarnings() + "";
        HandleSellAndBuybtns();
    }

    public void OnClickSellStocks()
    {
        sellBtn.interactable = false;
        player.SellStocks(currentSelectedCompany, buyginQuantity);
        totalEarningsTxt.text = currentSelectedCompany.GetEarnings() + "";
        HandleSellAndBuybtns();
    }

    public void SetNewStockValues(Company comp)
    {
        if (currentSelectedCompany != null && currentSelectedCompany == comp)
        {

            buyginQuantity = 1;
            sellingQuantity = 1;
            stockBuyTxt.text = comp.currentStockPrice + "";
            stockSellTxt.text = comp.currentStockPrice + "";
            HandleSellAndBuybtns();
        }
    }


    public void ClearHistory()
    {
        foreach (Transform c in historyContent)
        {
            Destroy(c.gameObject);
        }
    }

    public void ImportHistory()
    {
        ClearHistory();
        if (currentSelectedCompany)
        {
            for (int i = 1; i < currentSelectedCompany.laststocks.Count; i++)
            {
                LiveChangesElementUI pre = Instantiate(historyPrefab, historyContent);
                float percentage = (currentSelectedCompany.laststocks[i - 1] - currentSelectedCompany.laststocks[i]) / currentSelectedCompany.laststocks[i] * 100;
                percentage = Mathf.Round(percentage * 100f) / 100f;
                if (percentage > 0)
                {
                    pre.SetText(percentage + "%", 1);
                }
                else
                {
                    pre.SetText(percentage + "%", -1);
                }
            }
        }
    }

    public void AddHistory(float f, Company comp)
    {
        if (currentSelectedCompany != null && currentSelectedCompany == comp)
        {
            LiveChangesElementUI pre = Instantiate(historyPrefab, historyContent);
            if (f > 0)
            {
                pre.SetText(f + "%", 1);
            }
            else
            {
                pre.SetText(f + "%", -1);
            }
        }
    }


    public IEnumerator ShowAnimateNotifWindow(bool b)
    {
        if (b)
        {
            yield return new WaitForSeconds(0);
            notifPanel.alpha = 0;
            notifPanel.gameObject.SetActive(true);
            notifPanel.DOFade(1, 2f);
            StartCoroutine(ShowAnimateNotifWindow(false));
        }
        else
        {
            yield return new WaitForSeconds(5);
            notifPanel.DOFade(0f, 2f).OnComplete(() =>
            {
                notifPanel.gameObject.SetActive(false);
            });
            
            
        }
    }
}
