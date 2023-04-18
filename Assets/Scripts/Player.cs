using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;
    public float balance;
    public List<Company> investedCompanies;

    // Method to buy stocks of a company
    public void BuyStocks(Company company, int numStocks)
    {
        float cost = company.currentStockPrice * numStocks;
        if (cost <= balance)
        {
            balance -= cost;
            for (int i = 0; i < numStocks; i++)
            {
                investedCompanies.Add(company);
            }
            company.earnedMoney = company.earnedMoney - cost;
            company.boughtstocks = company.boughtstocks+ numStocks;
        }
    }

    // Method to sell stocks of a company
    public void SellStocks(Company company, int numStocks)
    {
        int count = 0;
        for (int i = investedCompanies.Count - 1; i >= 0; i--)
        {
            if (investedCompanies[i] == company)
            {
                investedCompanies.RemoveAt(i);
                count++;
                if (count == numStocks)
                {
                    break;
                }
            }
        }
        balance += count * company.currentStockPrice;
        company.earnedMoney = company.earnedMoney + (count * company.currentStockPrice);
        company.boughtstocks = company.boughtstocks - numStocks;
    }

    // Method to update the player's balance based on the current stock prices of the invested companies
    public void UpdateBalance()
    {
        foreach (Company company in investedCompanies)
        {
            balance += company.currentStockPrice * company.returnRate;
        }
    }
}
