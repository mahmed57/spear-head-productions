using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter instance;
    public TextMeshProUGUI coinText;
    public AudioSource coinSound;
    private int coinCount = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        coinText.text = coinCount.ToString();

        if (coinSound != null)
        {
            coinSound.Play();
        }
        else
        {
            Debug.LogWarning("Coin sound is not assigned in the inspector!");
        }
    }

    public bool RemoveCoin(int amount)
    {
        if ((coinCount - amount) >= 0)
        {
            coinCount = coinCount - amount;
            coinText.text = coinCount.ToString();
            return true;
        }

        return false;
    }
}
