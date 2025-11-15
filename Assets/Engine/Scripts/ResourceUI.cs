using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] public Image icon;
    [SerializeField] public TMP_Text amountText;
    [SerializeField] public TMP_Text gpsText;
    [SerializeField] public TMP_Text nameText;
    
    public void Fill(ResourceType resourceType)
    {
        icon.sprite = resourceType.icon;
        nameText.text = resourceType.name;
        amountText.text = resourceType.amount.ToString();
    }
}
