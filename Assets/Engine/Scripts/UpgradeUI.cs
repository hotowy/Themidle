using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [Header("Components")] 
    public TMP_Text priceText;
    public TMP_Text incomeInfoText;
    public Button Button;
    public Image characterImage;
    public TMP_Text upgradeNameText;
    
    private GameManager gameManager;
    private UpgradeType upgradeType;
    
    
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void Init(UpgradeType upgradeType)
    {
        this.upgradeType = upgradeType;
        upgradeNameText.text = upgradeType.name;
        characterImage.sprite = upgradeType.upgradeImage;
        priceText.text = CreateCostsText();
        incomeInfoText.text = CreateIncomeText();
    }

    public void UpdateUI()
    {
        priceText.text = CreateCostsText();
        incomeInfoText.text = CreateIncomeText();
    }

    private string CreateCostsText()
    {
        string text = "";
        Dictionary<ResourceType, int> upgradeTypeNextPrice = upgradeType.nextPrice;
        foreach (var pair in upgradeTypeNextPrice)
        {
            if (text.Length > 0) text += " + ";
            text += pair.Value.ToString() + " x " + pair.Key.name.ToString();
        }
        return text;
    }

    private string CreateIncomeText()
    {
        return upgradeType.level.ToString() + " x " + upgradeType.resourcesPerUpgrade.ToString() + "/s";
    }
    
    public void ClickAction()
    {
        gameManager.paymentController.TryBuy(upgradeType);

    }
}
