using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class MagicCardDisplay : CardDisplay
{
    public MagicCard card;

    public override void Awake()
    {
        base.Awake();
        if (PlayerPrefs.GetInt("save") == 0)
        {
            Init();
        }
        GetComponent<Button>().onClick.AddListener(ToMagicfield);
        GetComponent<DragDrop>().enabled = false;
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

        if (PlayerPrefs.GetInt("save") == 1)
        {
            if (transform.parent.name == "Shopfield")
            {

            }
            else if (transform.parent.name == "Packfield" || transform.parent.name == "Content")
            {
                priceText.transform.parent.gameObject.SetActive(false);
                button.gameObject.SetActive(false);
            }
            else
            {
                priceText.transform.parent.gameObject.SetActive(false);
                button.gameObject.SetActive(false);
                GetComponent<DragDrop>().enabled = true;
            }
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
            print("a");
            OffInfo();
        }
        else
        {
            if (transform.parent.name == "Shopfield")
            {
                info.transform.localPosition = new Vector3(-350, 0, 0);
            }
            else if (transform.parent.name == "Magicfield")
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

    public void Effect()
    {
        for (int i = 0; i < card.effectType.Length; i++)
        {
            if (card.effectType[i] == 0)
            {
                EffectManager.EMInstance.Effect(card.effectCode[i], card.args, card.sargs, pointText);
                break;
            }
        }
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

    public void LateEffect()
    {
        for (int i = 0; i < card.effectType.Length; i++)
        {
            if (card.effectType[i] == 2)
            {
                EffectManager.EMInstance.LateEffect(card.effectCode[i], card.args, card.sargs, pointText);
                break;
            }
        }
    }

    public void StartEffect()
    {
        for (int i = 0; i < card.effectType.Length; i++)
        {
            if (card.effectType[i] == 3)
            {
                EffectManager.EMInstance.StartEffect(card.effectCode[i], card.args, card.sargs);
                break;
            }
        }
    }

    public void DiscardEffect()
    {
        for (int i = 0; i < card.effectType.Length; i++)
        {
            if (card.effectType[i] == 7)
            {
                EffectManager.EMInstance.DiscardEffect(card.effectCode[i], card.args, card.sargs);
                break;
            }
        }
    }

    public void SkipEffect()
    {
        for (int i = 0; i < card.effectType.Length; i++)
        {
            if (card.effectType[i] == 8)
            {
                EffectManager.EMInstance.SkipEffect(card.effectCode[i], card.args, card.sargs);
                break;
            }
        }
    }

    public void EndEffect()
    {
        for (int i = 0; i < card.effectType.Length; i++)
        {
            if (card.effectType[i] == 9)
            {
                EffectManager.EMInstance.EndEffect(card.effectCode[i], card.args, card.sargs);
                break;
            }
        }
    }

    public void ToMagicfield()
    {
        button.onClick.RemoveAllListeners();

        if (button.gameObject.activeInHierarchy)
        {
            button.gameObject.SetActive(false);
            return;
        }

        OffInfo();

        foreach (GameObject troop in GMInstance.cardsInMagicfield.ToArray())
        {
            if (gameObject == troop)
            {
                int sell = cost / 2;
                if (gameObject == EffectManager.EMInstance.artifact)
                {
                    sell = cost / 2 + GMInstance.artifactSellValue;
                }
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Sell\n" + sell + "$";
                button.onClick.AddListener(SellCard);
                button.gameObject.SetActive(true);

                return;
            }
        }

        foreach (GameObject card in GMInstance.cardsInPackfield.ToArray())
        {
            if (card == gameObject)
            {
                if (GMInstance.cardsInMagicfield.Count < GMInstance.maxMagicInMagicfield)
                {
                    BuyCardFromPack();
                }
            }
        }

        foreach (GameObject card in GMInstance.cardsInShopfield.ToArray())
        {
            if (card == gameObject)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Buy\n" + cost + "$";
                button.onClick.AddListener(BuyCard);
                button.gameObject.SetActive(true);

                return;
            }
        }
    }

    private void SellCard()
    {
        int sell = cost / 2;
        if (gameObject == EffectManager.EMInstance.artifact)
        {
            sell = cost / 2 + GMInstance.artifactSellValue;
            GMInstance.artifactSellValue = 0;
        }
        else if (gameObject == EffectManager.EMInstance.herta)
        {
            GMInstance.maxMagicInMagicfield--;
        }
        else if (gameObject == EffectManager.EMInstance.negative)
        {
            GMInstance.maxMagicInMagicfield -= 2;
        }

        GameManager.GMInstance.isSellMagic = true;
        priceText.transform.parent.gameObject.SetActive(true);
        GMInstance.CardFromMagicfieldToShopfield(gameObject);
        GameManager.GMInstance.AddMoney(sell);
        AudioManager.AMInstance.PlayAudio(3);
        GetComponent<DragDrop>().enabled = false;
        button.gameObject.SetActive(false);
    }

    private void BuyCard()
    {
        if ((GameManager.GMInstance.money >= cost || (GMInstance.money >= -20 + cost && GMInstance.HasCreditCard())) && GMInstance.cardsInMagicfield.Count < GMInstance.maxMagicInMagicfield)
        {
            priceText.transform.parent.gameObject.SetActive(false);
            GMInstance.CardFromShopfieldToMagicfield(gameObject);
            GameManager.GMInstance.SpendMoney(cost);
            AudioManager.AMInstance.PlayAudio(2);
            button.gameObject.SetActive(false);
            GetComponent<DragDrop>().enabled = true;
            QuickEffect();
        }
    }

    private void BuyCardFromPack()
    {
        priceText.transform.parent.gameObject.SetActive(false);
        GMInstance.CardFromPackfieldToMagicfield(gameObject);
        GMInstance.DonePack();
        button.gameObject.SetActive(false);
        GetComponent<DragDrop>().enabled = true;
        QuickEffect();
    }

    public void BuyCardFromMagic()
    {
        priceText.transform.parent.gameObject.SetActive(false);
        GMInstance.CardFromShopfieldToMagicfield(gameObject);
        button.gameObject.SetActive(false);
        GetComponent<DragDrop>().enabled = true;
        QuickEffect();
    }
}
