using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image iconImage;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    private ConsumableData consumable;
    private BoostData boost;
    public void Setup(ConsumableData data)
    {
        consumable = data;
        nameText.text = data.consumableName;
        //iconImage.sprite = data.icon;
        priceText.text = data.price + "💰";

        buyButton.onClick.AddListener(() => BuyConsumable());
    }

    public void Setup(BoostData data)
    {
        boost = data;
        nameText.text = data.boostName;
        //iconImage.sprite = data.icon;
        priceText.text = data.price + "💰";

        buyButton.onClick.AddListener(() => BuyBoost());
    }

   

    void BuyConsumable()
    {
        if (GameSession.Instance.playerMoney >= consumable.price)
        {
            GameSession.Instance.playerMoney -= consumable.price;
            // Ajoute à une liste dans GameSession ? À définir.
            Destroy(gameObject);
        }
    }

    void BuyBoost()
    {
        if (GameSession.Instance.playerMoney >= boost.price)
        {
            GameSession.Instance.playerMoney -= boost.price;
            // Appliquer boost (à définir)
            Destroy(gameObject);
        }
    }

  
}
