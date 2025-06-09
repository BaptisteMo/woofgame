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

    public VisualTreeAsset boostCardTemplate;
    public VisualTreeAsset consoCardTemplate;
    private Label moneyLabel;
    

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var nextLevelButton = root.Q<VisualElement>("next-level");
        var retryButton = root.Q<VisualElement>("retry");
        consumableContainer = root.Q<VisualElement>("consumable-container");
        boostContainer = root.Q<VisualElement>("boost-container");
      
        nextLevelButton.RegisterCallback<ClickEvent>(OnClick_Continuer);
        retryButton.RegisterCallback<ClickEvent>(OnClick_Retry);
        
        moneyLabel = GetComponent<UIDocument>().rootVisualElement.Q<Label>("money-label");
       UpdateMoneyUI();

      //  DisplayShop(GameSession.Instance.boostPool);
    }

    public void DisplayShop(List<BoostData> boosts, List<ConsumableData> consumables)
    {
        boostContainer.Clear();
       
        consumableContainer.Clear();
        
        foreach (var consumable in consumables)
        {
            var card = consoCardTemplate.Instantiate();

            card.Q<Label>("boost-name").text = consumable.consumableName;
            card.Q<Label>("boost-price").text = consumable.price + " 💰";
         //   card.Q<Label>("boost-description").text = consumable.description;

            var buyButton = card.Q<Button>("buy-button");

            buyButton.clicked += () =>
            {
                if (GameSession.Instance.playerMoney >= consumable.price)
                {
                    GameSession.Instance.playerMoney -= consumable.price;
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
                    BoostManager.Instance.RegisterBoost(boost); // ✅ Ajout à la liste

                    buyButton.SetEnabled(false);
                    UpdateMoneyUI(); // <----- met à jour l'affichage !
                }
            };

            boostContainer.Add(card);
        }
    }
    
    public void UpdateMoneyUI()
    {
        if (moneyLabel != null)
        {
            moneyLabel.text = "Monnaie : " + GameSession.Instance.playerMoney;
        }
    }

    public void OnClick_Continuer(ClickEvent evt)
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
    public void OnClick_Retry(ClickEvent evt)
    {
        string nextScene = GameSession.Instance.RetryLevel;

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
