using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class IdolCardDisplay : CardDisplay
{
    public IdolCard card;

    public override void Awake()
    {
        base.Awake();
        if (PlayerPrefs.GetInt("save") == 0)
        {
            Init();
        }
        GetComponent<Button>().onClick.AddListener(Effect);

        
    }

    public override void Init()
    {
        artImage.sprite = card.art;
        type = card.type;
        cost = card.cost;
        nameText.text = card.name;
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
        if (transform.parent.name == "ShopIdolfield")
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
            if (transform.parent.name == "ShopIdolfield")
            {
                info.transform.localPosition = new Vector3(-350, 0, 0);
            }
            else if (transform.parent.name == "Idolfield")
            {
                info.transform.localPosition = new Vector3(0, -300, 0);
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

    public void Effect()
    {
        button.onClick.RemoveAllListeners();

        if (button.gameObject.activeInHierarchy)
        {
            button.gameObject.SetActive(false);
            return;
        }

        OffInfo();


        if (GMInstance.cardsInPackfield.Contains(gameObject))
        {
            priceText.transform.parent.gameObject.SetActive(false);
            GMInstance.CardFromPackfieldToIdolfield(gameObject);
            EffectManager.EMInstance.IdolEffect(card.effectCode[0], card.args, card.sargs);
            GMInstance.DoneIdolPack();
            return;
        }

        if (GMInstance.cardsInShopIdolfield.Contains(gameObject))
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Buy\n" + cost + "$";
            button.onClick.AddListener(BuyCard);
            button.gameObject.SetActive(true);
            return;
        }
    }

    private void BuyCard()
    {
        if (GMInstance.money >= cost || (GMInstance.money >= -20 + cost && GMInstance.HasCreditCard()))
        {
            GMInstance.CardFromShopIdolfieldToIdolfield(gameObject);
            priceText.transform.parent.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
            GameManager.GMInstance.SpendMoney(cost);
            AudioManager.AMInstance.PlayAudio(2);
            EffectManager.EMInstance.IdolEffect(card.effectCode[0], card.args, card.sargs);
        }
    }
}
