using UnityEngine;

public class ExpandingRingAttack : MonoBehaviour
{
     public float expansionSpeed = 5f;

    public float maxScale = 20f;

    public int damageAmount = 10;

    private float currentScale = 0f;

    private float initialSpriteScale;



    void Start()
    {
        initialSpriteScale = transform.localScale.x;

        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        ExpandRing();
    }

    void ExpandRing()
    {
        currentScale += expansionSpeed * Time.deltaTime;

        float scale = currentScale / initialSpriteScale;
        transform.localScale = new Vector3(scale, scale, 1f);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealthManager playerHealth = collision.GetComponent<PlayerHealthManager>();
            
            if (playerHealth != null)
            {
                playerHealth.deal_damage(damageAmount);
            }
            
            Destroy(gameObject);

        }

        if(collision.CompareTag("Wall"))
        {
            Destroy(gameObject);

        }

    }

}
