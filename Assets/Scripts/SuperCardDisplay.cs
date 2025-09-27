using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class SuperCardDisplay : CardDisplay
{
    public SuperCard card;
    public Button sellButton;
    public bool isUseOutside;

    public override void Awake()
    {
        base.Awake();
        if (PlayerPrefs.GetInt("save") == 0)
        {
            Init();
        }
        GetComponent<Button>().onClick.AddListener(ToSuperfield);

        
    }

    public override void Init()
    {
        artImage.sprite = card.art;
        type = card.type;
        cost = card.cost;
        nameText.text = card.name;
        isUseOutside = card.isUseOutside;
        typeText.text = card.type[0];
        for (int i = 2; i < type.Length; i++)
        {
            typeText.text += "/" + type[i];
        }

        switch (card.type[1])
        {
            case "Common":
                rarityText.color = Color.white;
                break;

            case "Uncommon":
                rarityText.color = Color.green;
                break;

            case "Rare":
                rarityText.color = Color.cyan;
                break;

            case "Epic":
                rarityText.color = new Color(1, 0, 1);
                break;

            case "Mythic":
                rarityText.color = Color.red;
                break;

            case "Legendary":
                rarityText.color = Color.yellow;
                break;
        }

        rarityText.text = card.type[1];
        effectText.text = card.effect;
        priceText.text = card.cost + "$";

        if (transform.parent.name == "Shopfield")
        {

        }
        else
        {
            priceText.transform.parent.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        OffInfo();
    }

    public void OpenInfo()
    {
        if (button.gameObject.activeSelf)
        {
            OffInfo();
        }
        else
        {
            if (transform.parent.name == "Shopfield")
            {
                info.transform.localPosition = new Vector3(-350, 0, 0);
            }
            else if (transform.parent.name == "Superfield")
            {
                info.transform.localPosition = new Vector3(0, -300, 0);
            }
            else if (transform.parent.name == "Packfield")
            {
                info.transform.localPosition = new Vector3(0, 300, 0);
            }
            else if (transform.parent.name == "Content")
            {
                info.transform.localPosition = new Vector3(0, 300, 0);
            }

            info.SetActive(true);
        }
    }

    public void OffInfo()
    {
        info.SetActive(false);
    }

    public void QuickEffect()
    {
        for (int i = 0; i < card.effectType.Length; i++)
        {
            if (card.effectType[i] == 1)
            {
                EffectManager.EMInstance.QuickEffect(card.effectCode[i], card.args, card.sargs);
                break;
            }
        }
    }

    public void ToSuperfield()
    {
        button.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();

        if (button.gameObject.activeInHierarchy)
        {
            sellButton.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
            return;
        }

        OffInfo();

        foreach (GameObject troop in GMInstance.cardsInSuperfield.ToArray())
        {
            if (gameObject == troop)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
                button.onClick.AddListener(UseCard);

                if (card.effectCode[0] == 11 && (GMInstance.cardsInBattlefield.Count > card.args[0] || GMInstance.cardsInBattlefield.Count <= 0))
                {
                    button.interactable = false;
                }
                else if (card.effectCode[0] == 12 && GMInstance.cardsInMagicfield.Count >= GMInstance.maxMagicInMagicfield)
                {
                    button.interactable = false;
                }
                else if (GMInstance.isCount)
                {
                    button.interactable = false;
                }
                else if (!isUseOutside && !GMInstance.isBattle)
                {
                    button.interactable = false;
                }
                else
                {
                    button.interactable = true;
                }

                sellButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sell\n" + cost / 2 + "$";
                sellButton.onClick.AddListener(SellCard);

                button.gameObject.SetActive(true);
                sellButton.gameObject.SetActive(true);

                return;
            }
        }

        foreach (GameObject card in GMInstance.cardsInPackfield.ToArray())
        {
            if (card == gameObject)
            {
                if (GMInstance.cardsInSuperfield.Count < GMInstance.maxSuperInSuperfield)
                {
                    BuyCardFromPack();
                    return;
                }
            }
        }


        foreach (GameObject card in GMInstance.cardsInShopfield.ToArray())
        {
            if (card == gameObject)
            {
                button.interactable = true;
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Buy\n" + cost + "$";
                button.onClick.AddListener(BuyCard);
                button.gameObject.SetActive(true);
            }
        }
    }

    private void UseCard()
    {
        if (card.effectCode[0] == 11 && (GMInstance.cardsInBattlefield.Count > card.args[0] || GMInstance.cardsInBattlefield.Count <= 0))
        {
            button.interactable = false;
        }
        else if (card.effectCode[0] == 12 && GMInstance.cardsInMagicfield.Count >= GMInstance.maxMagicInMagicfield)
        {
            button.interactable = false;
        }
        else if (GMInstance.isCount)
        {
            button.interactable = false;
        }
        else if (!isUseOutside && !GMInstance.isBattle)
        {
            button.interactable = false;
        }
        else
        {
            priceText.transform.parent.gameObject.SetActive(true);
            QuickEffect();
            GameManager.GMInstance.CoutingStatus();
            GMInstance.CardFromSuperfieldToShopfield(gameObject);
            sellButton.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
        }
    }

    private void SellCard()
    {
        priceText.transform.parent.gameObject.SetActive(true);
        GMInstance.CardFromSuperfieldToShopfield(gameObject);
        GameManager.GMInstance.AddMoney(cost / 2);
        AudioManager.AMInstance.PlayAudio(3);
        button.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(false);
    }

    private void BuyCard()
    {
        if ((GMInstance.money >= cost || (GMInstance.money >= -20 + cost && GMInstance.HasCreditCard())) && GMInstance.cardsInSuperfield.Count < GMInstance.maxSuperInSuperfield)
        {
            priceText.transform.parent.gameObject.SetActive(false);
            GMInstance.CardFromShopfieldToSuperfield(gameObject);
            GameManager.GMInstance.SpendMoney(cost);
            AudioManager.AMInstance.PlayAudio(2);
            sellButton.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
        }
    }

    private void BuyCardFromPack()
    {
        priceText.transform.parent.gameObject.SetActive(false);
        GMInstance.CardFromPackfieldToSuperfield(gameObject);
        GMInstance.DonePack();
        sellButton.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
    }

    public void BuyCardFromMagic()
    {
        priceText.transform.parent.gameObject.SetActive(false);
        GMInstance.CardFromShopfieldToSuperfield(gameObject);
        sellButton.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
    }
}
