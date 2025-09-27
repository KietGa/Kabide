using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class TroopCardDisplay : CardDisplay
{
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TroopCard card;
    
    public int attack { get; set; }
    public int defense { get; set; }

    public override void Awake()
    {
        base.Awake();
        if (PlayerPrefs.GetInt("save") == 0)
        {
            Init();
        }
        GetComponent<Button>().onClick.AddListener(ToBattlefield);
        GetComponent<DragDrop>().enabled = false;
    }

    public override void Init()
    {
        artImage.sprite = card.art;
        attack = card.attack;
        defense = card.defense;
        cost = card.cost;
        type = card.type;
        nameText.text = card.name;
        typeText.text = card.type[0];
        for (int i = 2; i < type.Length; i++)
        {
            typeText.text += "/" + type[i];
        }
        effectText.text = card.effect;
        
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
                rarityText.color = new Color(1,0,1);
                break;

            case "Mythic":
                rarityText.color = Color.red;
                break;

            case "Legendary":
                rarityText.color = Color.yellow;
                break;
        }

        rarityText.text = card.type[1];
        attackText.text = "ATK: " + attack + "";
        defenseText.text = "DEF: " + defense + "";
        priceText.text = card.cost + "$";

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
            else if (transform.parent.name == "Campfield")
            {
                info.transform.localPosition = new Vector3(0, -300, 0);
            }
            else if (transform.parent.name == "Packfield")
            {
                info.transform.localPosition = new Vector3(0, 300, 0);
            }
            else if (transform.parent.name == "Handfield")
            {
                info.transform.localPosition = new Vector3(0, 300, 0);
            }
            else if (transform.parent.name == "Battefield")
            {
                info.transform.localPosition = new Vector3(0, 300, 0);
            }
            else if (transform.parent.name == "Altarfield")
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
        for (int i = 0; i < card.effectType.Length; i++)
        {
            if (card.effectType[i] == 0)
            {
                EffectManager.EMInstance.Effect(card.effectCode[i], card.args, card.sargs, pointText);
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

    public void DestroyEffect()
    {
        for (int i = 0; i < card.effectType.Length; i++)
        {
            if (card.effectType[i] == 4)
            {
                EffectManager.EMInstance.DestroyEffect(card.effectCode[i], card.args, card.sargs);
                break;
            }
        }
    }

    public void AltarEffect()
    {
        for (int i = 0; i < card.effectType.Length; i++)
        {
            if (card.effectType[i] == 5)
            {
                EffectManager.EMInstance.AltarEffect(card.effectCode[i], card.args, card.sargs);
                break;
            }
        }
    }

    public void HandEffect()
    {
        for (int i = 0; i < card.effectType.Length; i++)
        {
            if (card.effectType[i] == 6)
            {
                EffectManager.EMInstance.HandEffect(card.effectCode[i], card.args, card.sargs, pointText);
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

    public void Point(int code)
    {
        StartCoroutine(PointDelay(code));
    }

    private IEnumerator PointDelay(int code)
    {
        pointText.gameObject.SetActive(true);

        if (code == 0)
        {
            pointText.text = "+" + attack;
            pointText.color = Color.red;
        }
        else if (code == 1)
        {
            pointText.text = "+" + defense;
            pointText.color = Color.cyan;
        }

        yield return new WaitForSeconds(0.25f);

        pointText.gameObject.SetActive(false);
    }

    public void ToBattlefield()
    {
        button.onClick.RemoveAllListeners();

        if (button.gameObject.activeInHierarchy)
        {
            button.gameObject.SetActive(false);
            return;
        }

        OffInfo();

        if (GMInstance.cardsInShopfield.Contains(gameObject))
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Buy\n" + cost + "$";
            button.onClick.AddListener(BuyCard);
            button.gameObject.SetActive(true);
            return;
        }
        
        if (GMInstance.cardsInPackfield.Contains(gameObject))
        {
            BuyCardFromPack();
            return;
        }
        
        if (GMInstance.cardsInBattlefield.Contains(gameObject) && !GMInstance.isCount)
        {
            GMInstance.CardFromBattlefieldToHandfield(gameObject);
            return;
        }

        if (GMInstance.cardsInCampfield.Contains(gameObject))
        {
            return;
        }

        if (GMInstance.cardsInAltarfield.Contains(gameObject))
        {
            return;
        }

        if (GMInstance.cardsInHandfield.Contains(gameObject) && GMInstance.cardsInBattlefield.Count < GMInstance.currentBattlefield && !GMInstance.isCount)
        {
            GMInstance.CardFromHandfieldToBattlefield(gameObject);
        }
    }

    private void BuyCard()
    {
        if (GMInstance.money >= cost || (GMInstance.money >= -20 + cost && GMInstance.HasCreditCard()))
        {
            priceText.transform.parent.gameObject.SetActive(false);
            GMInstance.CardFromShopfieldToCampfield(gameObject);
            GameManager.GMInstance.SpendMoney(cost);
            AudioManager.AMInstance.PlayAudio(2);
            button.gameObject.SetActive(false);
            GetComponent<DragDrop>().enabled = true;
        }
    }

    private void BuyCardFromPack()
    {
        priceText.transform.parent.gameObject.SetActive(false);
        GMInstance.CardFromPackfieldToCampfield(gameObject);
        GMInstance.DonePack();
        GetComponent<DragDrop>().enabled = true;
    }

    public void BuyCardFromMagic()
    {
        priceText.transform.parent.gameObject.SetActive(false);
        GMInstance.CardFromShopfieldToCampfield(gameObject);
        GetComponent<DragDrop>().enabled = true;
    }

    public void DestroyCard()
    {
        DestroyEffect();
        GetComponent<DragDrop>().enabled = false;
        priceText.transform.parent.gameObject.SetActive(true);
        button.gameObject.SetActive(false);
    }

    public void ChangeAttack(int atk)
    {
        attack += atk;
        attackText.text = "ATK: " + attack + "";
    }

    public void SetAttack(int atk)
    {
        attack = attack + atk;
        attackText.text = "ATK: " + attack + "";
    }

    public void ChangeDefense(int def)
    {
        defense += def;
        defenseText.text = "DEF: " + defense + "";
    }

    public void SetDefense(int def)
    {
        defense = defense + def;
        defenseText.text = "DEF: " + defense + "";
    }
}
