using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float lifetime = 1.5f;    
    public float moveSpeed = 50f;  
    public float fadeSpeed = 2f;    

    private TextMeshProUGUI damageText; 
    private CanvasGroup canvasGroup;   

    void Awake()
    {
        damageText = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (damageText == null)
        {
            Debug.LogError("TextMeshProUGUI component is not found! Make sure the prefab has a Text (TMP) object.");
        }

        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup component is not attached to the prefab!");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        if (canvasGroup != null)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float damage)
    {
        if (damageText != null)
        {
            damageText.text = Mathf.RoundToInt(damage).ToString();
        }
        else
        {
            Debug.LogWarning("DamageText is not set in the DamageNumber prefab!");
        }
    }
}
