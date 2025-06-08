using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class ShopUI : MonoBehaviour
{
    
    // public VisualTreeAsset cardTemplate;

    private VisualElement consumableContainer;
    private VisualElement boostContainer;
    private VisualElement statContainer;

    public VisualTreeAsset boostCardTemplate;
    public VisualTreeAsset statCardTemplate;
    public VisualTreeAsset consoCardTemplate;


    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        consumableContainer = root.Q<VisualElement>("consumable-container");
        boostContainer = root.Q<VisualElement>("boost-container");
        statContainer = root.Q<VisualElement>("stat-container");

      //  DisplayShop(GameSession.Instance.boostPool);
    }

    public void DisplayShop(List<BoostData> boosts, List<ConsumableData> consumables, StatUpgradeData stat)
    {
        boostContainer.Clear();
        statContainer.Clear();
        consumableContainer.Clear();
        foreach (var consumable in consumables)
        {
            var card = boostCardTemplate.Instantiate();

            card.Q<Label>("boost-name").text = consumable.consumableName;
            card.Q<Label>("boost-price").text = consumable.price + " 💰";
         //   card.Q<Label>("boost-description").text = consumable.description;

            var buyButton = card.Q<Button>("buy-button");

            buyButton.clicked += () =>
            {
                if (GameSession.Instance.playerMoney >= consumable.price)
                {
                    GameSession.Instance.playerMoney -= consumable.price;
                    // TODO     GameSession.Instance.ApplyBoost(boost);
                    buyButton.SetEnabled(false); // désactive le bouton après achat
                }
            };

            consumableContainer.Add(card);
        }
        foreach (var boost in boosts)
        {
            var card = boostCardTemplate.Instantiate();

            card.Q<Label>("boost-name").text = boost.boostName;
            card.Q<Label>("boost-price").text = boost.price + " 💰";
            card.Q<Label>("boost-description").text = boost.description;

            var buyButton = card.Q<Button>("buy-button");

            buyButton.clicked += () =>
            {
                if (GameSession.Instance.playerMoney >= boost.price)
                {
                    GameSession.Instance.playerMoney -= boost.price;
               // TODO     GameSession.Instance.ApplyBoost(boost);
                    buyButton.SetEnabled(false); // désactive le bouton après achat
                }
            };

            boostContainer.Add(card);
        }
        if (stat != null && !GameSession.Instance.unlockedStatUpgrades.Contains(stat))
        {
            var card = boostCardTemplate.Instantiate();

            card.Q<Label>("boost-name").text = stat.statName;
            card.Q<Label>("boost-price").text = stat.price + " 💰";
           //TODO card.Q<Label>("boost-description").text = stat.description;

            var buyButton = card.Q<Button>("buy-button");

            buyButton.clicked += () =>
            {
                if (GameSession.Instance.playerMoney >= stat.price)
                {
                    GameSession.Instance.playerMoney -= stat.price;
                    GameSession.Instance.ApplyStatUpgrade(stat);

                    GameSession.Instance.unlockedStatUpgrades.Add(stat);
                    GameSession.Instance.availableStatUpgrades.Remove(stat);
                    GameSession.Instance.currentStatUpThisLevel = null;

                    buyButton.SetEnabled(false);
                }
            };

            statContainer.Add(card);
        }
    }
    public void OnClick_Continuer()
    {
        string nextScene = GameSession.Instance.nextSceneName;

        if (!string.IsNullOrEmpty(nextScene))
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.LogWarning("Aucune scène suivante définie !");
        }
    }
}
