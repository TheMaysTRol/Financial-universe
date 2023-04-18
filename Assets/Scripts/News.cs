using System;
using UnityEngine;

[Serializable]
public class News
{
    
    public string headline;
    public string description;
    public int impact; // 1 for positive, -1 for negative
    [HideInInspector]
    public Company affectedCompany;
    public Sprite image;
    public Company.CompaniesTypes companyType;
}