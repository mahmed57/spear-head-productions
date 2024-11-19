using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter instance;
    public TextMeshProUGUI coinText;
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
    }

    public bool RemoveCoin(int amount)
    {
        if((coinCount - amount) >= 0)
        {
            coinCount = coinCount - amount;

            coinText.text = coinCount.ToString();
            
            return true;
        }

        return false;
    }
}
