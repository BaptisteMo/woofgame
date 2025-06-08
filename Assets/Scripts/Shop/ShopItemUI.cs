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
    private StatUpgradeData stat;

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

    public void Setup(StatUpgradeData data)
    {
        stat = data;
        nameText.text = data.statName;
     //   iconImage.sprite = data.icon;
        priceText.text = data.price + "💰";

        buyButton.onClick.AddListener(() => BuyStatUpgrade());
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

    void BuyStatUpgrade()
    {
        if (GameSession.Instance.playerMoney >= stat.price)
        {
            GameSession.Instance.playerMoney -= stat.price;

            GameSession.Instance.ApplyStatUpgrade(stat);
            GameSession.Instance.unlockedStatUpgrades.Add(stat);
            GameSession.Instance.availableStatUpgrades.Remove(stat);

            Destroy(gameObject);
        }
    }
}
