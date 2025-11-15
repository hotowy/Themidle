using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PaymentController
{
    private ResourceType[] resourceTypes;

    public PaymentController(ResourceType[] resourceTypes)
    {
        this.resourceTypes = resourceTypes;
    }
    
    public bool CanAfford(Dictionary<ResourceType, int> prices)
    {
        foreach (var pair in prices)
        {
            var priceResource = pair.Key;
            var priceAmount = pair.Value;
            
            foreach (var currentResource in resourceTypes)
            {
                if (currentResource.name == priceResource.name && currentResource.amount < priceAmount)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool TryBuy(UpgradeType upgradeType)
    {
        if (CanAfford(upgradeType.nextPrice))
        {
            foreach (var pair in upgradeType.nextPrice)
            {
                var priceResource = pair.Key;
                var priceAmount = pair.Value;
            
                foreach (var currentResource in resourceTypes)
                {
                    if (currentResource.name == priceResource.name)
                    {
                        currentResource.amount -= priceAmount;
                    }
                }
            }
        }
        else
        {
            return false;
        }

        upgradeType.RaiseLevel();
        return true;
    }
}