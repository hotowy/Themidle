using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Game/Upgrade")]
public class UpgradeType : ScriptableObject
{
    public string name = "Upgrade";
    public int level = 0;
    
    [SerializeField] 
    public UpgradeUI statPrefab;
    public Sprite upgradeImage;
    
    [Header("Initial Prices:")]
    public ResourceType[] initialPriceTypes;
    public int[] initialPriceAmount;
    
    [Header("Next Prices:")]
    public ResourceType[] nextPriceTypes;
    public int[] nextPriceAmount;
    public float upgradePriceMultiplier = 1.2f;

    [Header("Rewards:")] 
    public ResourceType[] rewardTypes;
    public float resourcesPerUpgrade = 0.1f;

    private UpgradeUI upgradeUI;
    public Dictionary<ResourceType, int> nextPrice = new();


    public void Init(UpgradeUI created)
    {
        level = 0;
        CalculateNextPrice();
        this.upgradeUI = created;
    }
    
    void CalculateNextPrice()
    {
        nextPrice.Clear();
        if (level == 0)
        {
            for (int i = 0; i < initialPriceTypes.Length; i++)
            {
                nextPrice[initialPriceTypes[i]] = initialPriceAmount[i];
            }
        }
        else
        {
            for (int i = 0; i < nextPriceTypes.Length; i++)
            {
                nextPrice[nextPriceTypes[i]] = Mathf.RoundToInt(nextPriceAmount[i] * level * upgradePriceMultiplier);
            }
        }
    }

    public void setActive(bool active)
    {
        upgradeUI.Button.interactable = active;
    }

    public void setVisibleIcon()
    {
        upgradeUI.characterImage.color = level>0 ? Color.white : Color.black;
    } 

    public void RaiseLevel()
    {
        level++;
        CalculateNextPrice();
        foreach (var rewardType in rewardTypes)
        {
            rewardType.AddGrowthPerSecond(resourcesPerUpgrade);
        }
        upgradeUI.UpdateUI();
        
    }
}
