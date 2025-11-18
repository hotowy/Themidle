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
        
        DropAnimation dropAnimation = obj.AddComponent<DropAnimation>();
        dropAnimation.sr = obj.AddComponent<SpriteRenderer>();
        dropAnimation.sr.sprite = resourceType.icon;
        dropAnimation.sr.sortingOrder = 10;
        dropAnimation.transform.localScale = new Vector2(0.08f, 0.08f);
        
        //TMP_Text tmpText = obj.AddComponent<TMP_Text>();
        //tmpText.text = amount.ToString();
        
        Instantiate(obj, pointOfDeath + new Vector2(0, yAxisOffset), Quaternion.identity);
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // ruch w górę
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        transform.localScale = transform.localScale - new Vector3(0.08f, 0.08f) * Time.deltaTime;

        // zanik (alpha maleje)
        //sr.color -= fadeSpeed * Time.deltaTime;
        //sr.color = color;
    }
}
