using UnityEngine;


[CreateAssetMenu(fileName = "Resource Type", menuName = "Game/Resource", order = 0)]
public class ResourceType : ScriptableObject
{
    [Header("Runtime")]
    [SerializeField] public float amount = 0;
    [SerializeField] public float amountPerSecond = 0;
    public ResourceUI Uis;

    [Header("Editor")] 
    [SerializeField] public float initialAmount = 0;
    
    [SerializeField] public Sprite icon;
    [SerializeField] public ResourceUI UIPrefab;
    [SerializeField] public string name;

    public void Init(ResourceUI uis)
    {
        this.amount = this.initialAmount;
        this.amountPerSecond = 0;
        this.Uis = uis;
    }

    public void ProcessAmountPerSecond(int updatesPerSecond)
    {
        this.amount = this.amount + (amountPerSecond / updatesPerSecond);
        UpdateUI();
    }

    public void AddGrowthPerSecond(float amountPerSecond)
    {
        this.amountPerSecond += amountPerSecond;
    }

    public void UpdateUI()
    {
        Uis.amountText.text = Mathf.RoundToInt(amount).ToString();
        Uis.gpsText.text = (Mathf.Round(amountPerSecond * 10f) / 10f).ToString() + "/s";
    }
    
    public bool PurchaseAction(int cost)
    {
        if (amount >= cost)
        {
            amount -= cost;
            UpdateUI();
            return true;
        }
        return false;
    }

}