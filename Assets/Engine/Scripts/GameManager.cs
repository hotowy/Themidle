using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game properties:")]
    [SerializeField] private int updatesPerSecond = 5;

    [Header("Resources:")]
    [SerializeField] public ResourceType[] resourceTypes;
    [SerializeField] private VerticalLayoutGroup resourceTypesContainer;
    
    [Header("Upgrades (new):")]
    [SerializeField] public UpgradeType[] upgrades;
    [SerializeField] private VerticalLayoutGroup upgradesContainer;
    
    public PaymentController paymentController;
    
    private float nextTimeCheck = 0;
    
    void Start()
    {
        InitializeResourceStatistics();
        InitializeUpgradeStatistics();
        paymentController = new PaymentController(resourceTypes);
    }
    void Update()
    {
        if (nextTimeCheck < Time.timeSinceLevelLoad)
        {
            IdleCalculate();
            nextTimeCheck = Time.timeSinceLevelLoad + 1f / updatesPerSecond;
        }
        
        
    }  
    
    private void InitializeResourceStatistics()
    {
        foreach (Transform child in resourceTypesContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var rt in resourceTypes)
        {
            ResourceUI created = Instantiate<ResourceUI>(rt.UIPrefab, resourceTypesContainer.transform, false);
            created.Fill(rt);
            rt.Init(created);
        }
    }

    private void InitializeUpgradeStatistics()
    {
        foreach (Transform child in upgradesContainer.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var upgrade in upgrades)
        {
            UpgradeUI created = Instantiate<UpgradeUI>(upgrade.statPrefab, upgradesContainer.transform, false);
            upgrade.Init(created);
            created.Init(upgrade);
        }
    }



    void IdleCalculate()
    {
        foreach (var rt in resourceTypes)
        {
            rt.ProcessAmountPerSecond(updatesPerSecond);
        }
        
        foreach (var upgrade in upgrades)
        {
            upgrade.setActive(paymentController.CanAfford(upgrade.nextPrice));
            upgrade.setVisibleIcon();
        }
    }


}
