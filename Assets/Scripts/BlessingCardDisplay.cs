using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlessingCardDisplay : CardDisplay
{
    public BlessingCard card;

    public override void Awake()
    {
        base.Awake();
        Init();
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
    }

    public void OpenInfo()
    {
        if (transform.parent.name == "Content")
        {
            info.transform.localPosition = new Vector3(0, 250, 0);
        }
        else
        {
            info.transform.localPosition = new Vector3(0, -350, 0);
        }
        info.SetActive(true);
    }

    public void OffInfo()
    {
        info.SetActive(false);
    }
}
