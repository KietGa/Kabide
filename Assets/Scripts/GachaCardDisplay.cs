using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class GachaCardDisplay : CardDisplay
{
    public GachaCard card;

    public override void Awake()
    {
        base.Awake();
        if (PlayerPrefs.GetInt("save") == 0)
        {
            Init();
        }
        GetComponent<Button>().onClick.AddListener(OpenPack);

        
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
        if (transform.parent.name == "ShopPackfield")
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
            if (transform.parent.name == "Content")
            {
                info.transform.localPosition = new Vector3(0, 300, 0);
            }
            else
            {
                info.transform.localPosition = new Vector3(-350, 0, 0);
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
        int[] costArr = new int[1] { cost };
        EffectManager.EMInstance.LuckyEffect(card.effectCode[0], costArr, card.sargs, card.args[0]);
    }

    public void OpenPack()
    {
        button.onClick.RemoveAllListeners();

        if (button.gameObject.activeInHierarchy)
        {
            button.gameObject.SetActive(false);
            return;
        }

        OffInfo();

        foreach (GameObject troop in GMInstance.cardsInShopPackfield.ToArray())
        {
            if (gameObject == troop)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Buy\n" + cost + "$";
                button.onClick.AddListener(BuyCard);
                button.gameObject.SetActive(true);
            }
        }
    }

    private void BuyCard()
    {
        if (GMInstance.money >= cost || (GMInstance.money >= -20 + cost && GMInstance.HasCreditCard()))
        {
            priceText.transform.parent.gameObject.SetActive(true);
            AudioManager.AMInstance.PlayAudio(2);
            Effect();
            gameObject.SetActive(false);
            button.gameObject.SetActive(false);
        }
    }
}
