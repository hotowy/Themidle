using TMPro;
using UnityEngine;

public class DropAnimation : MonoBehaviour
{
    public float moveSpeed = 0.3f;
    public float fadeSpeed = 1.5f;
    public float lifeTime = 1f;

    private SpriteRenderer sr;
    private int amount;
    
    public static float yAxisOffset = 0.01f;

    public static void InstantiateAnimation(ResourceType resourceType, int amount, Vector2 pointOfDeath)
    {
        var obj = new GameObject("DropAnimation");
        obj.transform.position = pointOfDeath + new Vector2(0, yAxisOffset);
        
        DropAnimation dropAnimation = obj.AddComponent<DropAnimation>();
        dropAnimation.sr = obj.AddComponent<SpriteRenderer>();
        dropAnimation.sr.sprite = resourceType.icon;
        dropAnimation.sr.sortingOrder = 10;
        dropAnimation.transform.localScale = new Vector2(0.08f, 0.08f);
        
        // var tmpText = obj.AddComponent<TMPro.TextMeshPro>();
        // tmpText.text = "x" + amount;
        // tmpText.fontSize = 3f;
        // tmpText.sortingOrder = 20;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        //TODO magic numbers
        transform.localScale -= new Vector3(0.08f, 0.08f) * Time.deltaTime;
    }
}
