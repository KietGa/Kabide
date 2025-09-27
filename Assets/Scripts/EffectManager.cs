using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using static GameManager;
using static BossManager;

public class EffectManager : MonoBehaviour
{
    public static EffectManager EMInstance {  get; private set; }
    public TroopCard[] troopCards;
    public MagicCard[] magicCards;
    public SuperCard[] superCards;
    public List<GameObject> idolCards;
    public GameObject softAndWet;
    public GameObject tusk;
    public GameObject yone;
    public GameObject qiqi;
    public GameObject blueMirror;
    public GameObject magicMirror;
    public GameObject yoshikageKira;
    public GameObject convenienceStore;
    public GameObject artifact;
    public GameObject godBless;
    public GameObject herta;
    public GameObject friend;
    public GameObject anihilate;
    public GameObject deathNote;
    public GameObject soloLeveling;
    public GameObject heartSteel;
    public GameObject creditCard;
    public GameObject nope;
    public GameObject goldExperience;
    public GameObject kingCrimson;
    public GameObject ahri;
    public GameObject xiao;
    public GameObject hutao;
    public GameObject veigar;
    public GameObject challenger;
    public GameObject coward;
    public GameObject negative;
    public GameObject nasus;
    public GameObject zedShadow;
    public GameObject emergency;
    public GameObject vpn;
    public GameObject aine;
    public GameObject braveHeart;
    public GameObject lonely;
    public GameObject mountain;
    public GameObject mon3tr;
    public GameObject crownslayer;

    private void Awake()
    {
        EMInstance = this;
    }

    public void Point(TextMeshProUGUI pointText, string effect, Color color)
    {
        StartCoroutine(PointDelay(pointText, effect, color));
    }

    private IEnumerator PointDelay(TextMeshProUGUI pointText, string effect, Color color)
    {
        pointText.gameObject.SetActive(true);

        pointText.text = effect;
        pointText.color = color;

        yield return new WaitForSeconds(0.25f);

        pointText.gameObject.SetActive(false);
    }

    public void StartEffect(int effectCode, int[] args, string[] sargs)
    {
        //t3
        switch (effectCode)
        {
            case -1:
                break;

            case 0: //When Blind is selected, gain +A Hands and lose all discards
                GMInstance.ChangeHand(0, args[0]);
                GMInstance.ChangeDiscard(1, 0);
                break;

            case 1: //Blue Mirror
                for (int i = 0; i < GMInstance.magicfieldTrans.childCount - 1; i++)
                {
                    if (GMInstance.magicfieldTrans.GetChild(i).gameObject == blueMirror)
                    {
                        GMInstance.magicfieldTrans.GetChild(i+1).GetComponent<MagicCardDisplay>().StartEffect();
                        break;
                    }
                }

                break;

            case 2: //Magic Mirror
                if (GMInstance.cardsInMagicfield.Contains(magicMirror))
                {
                    if (GMInstance.magicfieldTrans.GetChild(0).gameObject != magicMirror)
                    {
                        GMInstance.magicfieldTrans.GetChild(0).GetComponent<MagicCardDisplay>().StartEffect();
                        break;
                    }
                }

                break;

            case 3: //+a Hand
                GMInstance.ChangeHand(0, args[0]);
                break;

            case 4: //+a Discard
                GMInstance.ChangeDiscard(0, args[0]);
                break;

            case 5: //+a Hand size
                GMInstance.handSize += args[0];
                break;

            case 6: //+a Hand & +a Discard & +a Hand size
                GMInstance.ChangeHand(0, args[0]);
                GMInstance.ChangeDiscard(0, args[1]);
                GMInstance.handSize += args[2];
                break;

            case 7: //Set hand to 1
                GMInstance.ChangeHand(1, 1);
                break;

            case 8:
                GMInstance.currentBattlefield += args[0];
                break;

            case 9:
                if (GMInstance.cardsInCampfield.Count != 0)
                {
                    int ran1 = Random.Range(0, GMInstance.cardsInCampfield.Count);
                    GMInstance.CardFromCampfieldToShopfield(GMInstance.cardsInCampfield[ran1]);
                    EMInstance.SpecialEffect(2, EMInstance.deathNote.GetComponent<MagicCardDisplay>().card.args, null);
                    EMInstance.SpecialEffect(4, EMInstance.yoshikageKira.GetComponent<TroopCardDisplay>().card.args, null);
                    AudioManager.AMInstance.PlayAudio(10);
                }

                break;

            case 10:
                int ran = Random.Range(args[0], args[1] + 1);
                GMInstance.ChangeHand(1, ran);
                break;

            case 11: //+a Hand size
                GMInstance.handSize += args[1];
                break;

            case 12:
                GMInstance.ChangeHand(1, GMInstance.maxDiscards);
                break;

            case 13:
                GMInstance.ChangeDiscard(1, GMInstance.maxHands);
                break;

            case 14:
                GMInstance.baseAttack += args[0];
                break;

            case 15:
                GMInstance.baseDefense += args[0];
                break;

            case 16:
                int count = 0;

                if (GMInstance.cardsInMagicfield.Count < GMInstance.maxMagicInMagicfield)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        List<GameObject> tempList = new List<GameObject>();

                        while (tempList.Count == 0 && count < 400)
                        {
                            count++;

                            if (tempList.Count != 0)
                            {
                                tempList.RemoveRange(0, tempList.Count);
                            }

                            string rarity = GMInstance.GachaRate();

                            foreach (Transform card in GMInstance.shopfieldTrans)
                            {
                                if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf && card.GetComponent<CardDisplay>().type[0] == "Magic")
                                {
                                    tempList.Add(card.gameObject);
                                }
                            }
                        }

                        int ran1 = Random.Range(0, tempList.Count);
                        tempList[ran1].SetActive(true);
                        tempList[ran1].GetComponent<MagicCardDisplay>().BuyCardFromMagic();
                    }
                    break;
                }

                break;

            case 17:
                count = 0;
                
                for (int i = 0; i < 1; i++)
                {
                    List<GameObject> tempList = new List<GameObject>();

                    while (tempList.Count == 0 && count < 400)
                    {
                        count++;

                        if (tempList.Count != 0)
                        {
                            tempList.RemoveRange(0, tempList.Count);
                        }

                        string rarity = GMInstance.GachaRate();

                        foreach (Transform card in GMInstance.shopfieldTrans)
                        {
                            if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf && card.GetComponent<CardDisplay>().type[0] == "Troop")
                            {
                                tempList.Add(card.gameObject);
                            }
                        }
                    }

                    int ran1 = Random.Range(0, tempList.Count);
                    tempList[ran1].SetActive(true);
                    tempList[ran1].GetComponent<TroopCardDisplay>().BuyCardFromMagic();
                }
                break;

            case 18:

                if (GMInstance.cardsInSuperfield.Count < GMInstance.maxSuperInSuperfield)
                {
                    count = 0;

                    for (int i = 0; i < 1; i++)
                    {
                        List<GameObject> tempList = new List<GameObject>();

                        while (tempList.Count == 0 && count < 400)
                        {
                            count++;

                            if (tempList.Count != 0)
                            {
                                tempList.RemoveRange(0, tempList.Count);
                            }

                            string rarity = GMInstance.GachaRate();

                            foreach (Transform card in GMInstance.shopfieldTrans)
                            {
                                if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf && card.GetComponent<CardDisplay>().type[0] == "Super")
                                {
                                    tempList.Add(card.gameObject);
                                }
                            }
                        }

                        int ran1 = Random.Range(0, tempList.Count);
                        tempList[ran1].SetActive(true);
                        tempList[ran1].GetComponent<SuperCardDisplay>().BuyCardFromMagic();
                    }
                }
                break;
        }
    }

    //t9
    public void EndEffect(int effectCode, int[] args, string[] sargs)
    {
        switch (effectCode)
        {
            case -1:
                break;

            case 0:
                bool isSafe = false;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().name == sargs[0])
                    {
                        isSafe = true;
                        break;
                    }
                }

                if (!isSafe)
                {
                    GMInstance.CardFromBattlefieldToShopfield(mon3tr);
                }

                break;

            case 1:
                try
                {
                    if (GMInstance.battlefieldTrans.GetChild(args[0]).GetComponent<TroopCardDisplay>().card.name != sargs[0])
                    {
                        GMInstance.CardFromBattlefieldToShopfield(crownslayer);
                    }
                }
                catch
                {
                }
                break;
        }
    }


    public void SkipEffect(int effectCode, int[] args, string[] sargs)
    {
        //t8
        switch (effectCode)
        {
            case -1:
                break;

            case 0: 
                GMInstance.AddMoney(args[0]);
                break;

            case 1: //Blue Mirror
                for (int i = 0; i < GMInstance.magicfieldTrans.childCount - 1; i++)
                {
                    if (GMInstance.magicfieldTrans.GetChild(i).gameObject == blueMirror)
                    {
                        GMInstance.magicfieldTrans.GetChild(i + 1).GetComponent<MagicCardDisplay>().SkipEffect();
                        break;
                    }
                }

                break;

            case 2: //Magic Mirror
                if (GMInstance.cardsInMagicfield.Contains(magicMirror))
                {
                    if (GMInstance.magicfieldTrans.GetChild(0).gameObject != magicMirror)
                    {
                        GMInstance.magicfieldTrans.GetChild(0).GetComponent<MagicCardDisplay>().SkipEffect();
                        break;
                    }
                }

                break;

            case 4:
                GMInstance.nasusCounter += args[0];
                nasus.GetComponent<TroopCardDisplay>().ChangeAttack(args[0]);
                break;

            case 5:
                GMInstance.baseAttack += args[0];
                break;

            case 6:
                GMInstance.baseDefense += args[0];
                break;

            case 7:
                int count = 0;

                if (GMInstance.cardsInMagicfield.Count < GMInstance.maxMagicInMagicfield)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        List<GameObject> tempList = new List<GameObject>();

                        while (tempList.Count == 0 && count < 400)
                        {
                            count++;

                            if (tempList.Count != 0)
                            {
                                tempList.RemoveRange(0, tempList.Count);
                            }

                            string rarity = GMInstance.GachaRate();

                            foreach (Transform card in GMInstance.shopfieldTrans)
                            {
                                if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf && card.GetComponent<CardDisplay>().type[0] == "Magic")
                                {
                                    tempList.Add(card.gameObject);
                                }
                            }
                        }

                        int ran1 = Random.Range(0, tempList.Count);
                        tempList[ran1].SetActive(true);
                        tempList[ran1].GetComponent<MagicCardDisplay>().BuyCardFromMagic();
                    }
                    break;
                }

                break;

            case 8:
                count = 0;

                for (int i = 0; i < 1; i++)
                {
                    List<GameObject> tempList = new List<GameObject>();

                    while (tempList.Count == 0 && count < 400)
                    {
                        count++;

                        if (tempList.Count != 0)
                        {
                            tempList.RemoveRange(0, tempList.Count);
                        }

                        string rarity = GMInstance.GachaRate();

                        foreach (Transform card in GMInstance.shopfieldTrans)
                        {
                            if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf && card.GetComponent<CardDisplay>().type[0] == "Troop")
                            {
                                tempList.Add(card.gameObject);
                            }
                        }
                    }

                    int ran1 = Random.Range(0, tempList.Count);
                    tempList[ran1].SetActive(true);
                    tempList[ran1].GetComponent<TroopCardDisplay>().BuyCardFromMagic();
                }
                break;

            case 9:

                if (GMInstance.cardsInSuperfield.Count < GMInstance.maxSuperInSuperfield)
                {
                    count = 0;

                    for (int i = 0; i < 1; i++)
                    {
                        List<GameObject> tempList = new List<GameObject>();

                        while (tempList.Count == 0 && count < 400)
                        {
                            count++;

                            if (tempList.Count != 0)
                            {
                                tempList.RemoveRange(0, tempList.Count);
                            }

                            string rarity = GMInstance.GachaRate();

                            foreach (Transform card in GMInstance.shopfieldTrans)
                            {
                                if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf && card.GetComponent<CardDisplay>().type[0] == "Super")
                                {
                                    tempList.Add(card.gameObject);
                                }
                            }
                        }

                        int ran1 = Random.Range(0, tempList.Count);
                        tempList[ran1].SetActive(true);
                        tempList[ran1].GetComponent<SuperCardDisplay>().BuyCardFromMagic();
                    }
                }
                break;
        }
    }

    public void SpecialEffect(int effectCode, int[] args, string[] sargs)
    {
        switch (effectCode)
        {
            case 1:
                int count = GMInstance.numberOfAdd / args[0] + 1;
                friend.GetComponent<MagicCardDisplay>().effectText.text = "Gain x1 ATK after Add " + args[0] + " cards\n (Currently: x" + count + " ATK)";
                break;

            case 2:
                count = GMInstance.numberOfDestroy * args[0] + 1;
                deathNote.GetComponent<MagicCardDisplay>().effectText.text = "Gain x" + args[0] + " ATK after Destroy a troop card\n (Currently: x" + count + " ATK)";
                break;

            case 3:
                count = GMInstance.numberOfDiscard / args[0] + 1;
                anihilate.GetComponent<MagicCardDisplay>().effectText.text = "Gain x1 ATK after Discard " + args[0] + " troop cards\n (Currently: x" + count + " ATK)";
                break;

            case 4: //Kira
                yoshikageKira.GetComponent<TroopCardDisplay>().ChangeAttack(args[0]);
                break;

            case 5:
                count = GMInstance.battleTime / args[0] + 1;
                challenger.GetComponent<MagicCardDisplay>().effectText.text = "Gain x" + args[0] + " ATK per challenge\n (Currently: x" + count +" ATK)";
                break;

            case 6:
                count = GMInstance.skipTime * args[0] + 1;
                coward.GetComponent<MagicCardDisplay>().effectText.text = "Gain x" + args[0] + " ATK per skipped\n (Currently: x" + count +" ATK)";
                break;
        }
    }

    public void DestroyEffect(int effectCode, int[] args, string[] sargs)
    {
        //t4
        switch (effectCode)
        {
            case -1:
                break;

            case 0: //Yone
                yone.GetComponent<TroopCardDisplay>().card = troopCards[2];
                yone.GetComponent<TroopCardDisplay>().Init();
                yone.name = troopCards[2].name;
                break;

            case 1: //Qiqi
                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().name == sargs[0])
                    {
                        GMInstance.qiqiCounter += card.GetComponent<TroopCardDisplay>().attack * args[0];
                        card.GetComponent<TroopCardDisplay>().ChangeAttack(card.GetComponent<TroopCardDisplay>().attack * args[0]);
                    }
                }
                
                break;

            case 2:
                GMInstance.AddMoney(args[0]);
                break;
        }
    }

    public void AltarEffect(int effectCode, int[] args, string[] sargs)
    {
        //t5
        switch (effectCode)
        {
            case -1:
                break;

            case 1: //Go back to Camp instead of Alter after Battle
                foreach (GameObject card in GMInstance.cardsInAltarfield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().name == sargs[0])
                    {
                        GMInstance.CardFromAltarfieldToCampfield(card);
                    }
                }
                break;

            case 2:
                GMInstance.CardFromAltarfieldToShopfield(zedShadow);
                break;
        }
    }

    public void DiscardEffect(int effectCode, int[] args, string[] sargs)
    {
        //t7
        switch (effectCode)
        {
            case -1:
                break;

            case 0: //Discard this card to get 1 random card from Alter to Camp
                if (GMInstance.cardsInAltarfield.Count > 0)
                {
                    int ran = Random.Range(0, GMInstance.cardsInAltarfield.Count);
                    GMInstance.CardFromAltarfieldToCampfield(GMInstance.cardsInAltarfield[ran]);
                    AudioManager.AMInstance.PlayAudio(5);
                }

                break;

            case 2: //If first discard of round has only 1 card, destroy it 
                if (GMInstance.cardsInBattlefield.Count == 1 && GMInstance.dturn == 1)
                {
                    GMInstance.CardFromBattlefieldToShopfield(GMInstance.cardsInBattlefield[0]);
                    EMInstance.SpecialEffect(2, EMInstance.deathNote.GetComponent<MagicCardDisplay>().card.args, null);
                    EMInstance.SpecialEffect(4, EMInstance.yoshikageKira.GetComponent<TroopCardDisplay>().card.args, null);
                    AudioManager.AMInstance.PlayAudio(10);
                }

                break;

            case 3: //Discard this card to get a$
                GMInstance.AddMoney(args[0]);
                AudioManager.AMInstance.PlayAudio(4);
                break;
        }
    }

    public void QuickEffect(int effectCode, int[] args, string[] sargs) 
    {
        //t1
        switch (effectCode)
        {
            case -1:
                break;

            case 0: //Free reroll 1 time per visit shop
                GMInstance.rerollBtn.onClick.RemoveAllListeners();
                GMInstance.rerollBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Reroll - " + 0 + "$";
                GMInstance.rerollBtn.onClick.AddListener(() =>
                {
                    GMInstance.FreeRerollShopEffect();
                });
                break;

            case 1: //Blue Mirror
                for (int i = 0; i < GMInstance.magicfieldTrans.childCount - 1; i++)
                {
                    if (GMInstance.magicfieldTrans.GetChild(i).gameObject == blueMirror)
                    {
                        GMInstance.magicfieldTrans.GetChild(i + 1).GetComponent<MagicCardDisplay>().QuickEffect();
                        break;
                    }
                }

                break;

            case 2: //Magic Mirror
                if (GMInstance.cardsInMagicfield.Contains(magicMirror))
                {
                    if (GMInstance.magicfieldTrans.GetChild(0).gameObject != magicMirror)
                    {
                        GMInstance.magicfieldTrans.GetChild(0).GetComponent<MagicCardDisplay>().QuickEffect();
                        break;
                    }
                }
                
                break;

            case 3:
                GMInstance.ChangeTotalAttack(args[0]);
                AudioManager.AMInstance.PlayAudio(1);
                break;

            case 4:
                GMInstance.ChangeTotalDefense(args[0]);
                AudioManager.AMInstance.PlayAudio(1);
                break;

            case 5:
                int money = Mathf.Clamp(GMInstance.money, 0, args[0]);
                AudioManager.AMInstance.PlayAudio(4);
                GMInstance.AddMoney(money);
                break;

            case 6:
                try
                {
                    if (sargs[0] == "Joker")
                    {
                        AudioManager.AMInstance.PlayAudio(7);
                    }
                }
                catch
                {

                }

                GMInstance.maxMagicInMagicfield += args[0];
                break;

            case 7:
                money = 0;

                foreach (Transform card in GMInstance.magicfieldTrans)
                {
                    if (card.gameObject == EffectManager.EMInstance.artifact)
                    {
                        money += card.GetComponent<MagicCardDisplay>().cost / 2 + GMInstance.artifactSellValue;
                    }
                    else
                    {
                        money += card.GetComponent<MagicCardDisplay>().cost / 2;
                    }
                }

                money = Mathf.Clamp(money, 0, args[0]);
                AudioManager.AMInstance.PlayAudio(4);
                GMInstance.AddMoney(money);
                break;

            case 8:
                AudioManager.AMInstance.PlayAudio(5);
                GMInstance.ChangeHand(0, 1);
                break;

            case 9:
                AudioManager.AMInstance.PlayAudio(6);
                GMInstance.ChangeDiscard(0, 1);
                break;

            case 10:
                AudioManager.AMInstance.PlayAudio(8);
                GMInstance.WinMatch();
                break;

            case 11: //Destroy a troop card in battlefield
                if (GMInstance.cardsInBattlefield.Count <= args[0])
                {
                    foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                    {
                        GMInstance.CardFromBattlefieldToShopfield(card.gameObject);
                        EMInstance.SpecialEffect(2, EMInstance.deathNote.GetComponent<MagicCardDisplay>().card.args, null);
                        EMInstance.SpecialEffect(4, EMInstance.yoshikageKira.GetComponent<TroopCardDisplay>().card.args, null);
                    }

                    AudioManager.AMInstance.PlayAudio(10);
                }
                break;

            case 12:
                int count = 0;

                if (GMInstance.cardsInMagicfield.Count < GMInstance.maxMagicInMagicfield)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        List<GameObject> tempList = new List<GameObject>();

                        while (tempList.Count == 0 && count < 400)
                        {
                            count++;

                            if (tempList.Count != 0)
                            {
                                tempList.RemoveRange(0, tempList.Count);
                            }

                            string rarity = GMInstance.GachaRate();

                            foreach (Transform card in GMInstance.shopfieldTrans)
                            {
                                if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf && card.GetComponent<CardDisplay>().type[0] == "Magic")
                                {
                                    tempList.Add(card.gameObject);
                                }
                            }
                        }

                        int ran1 = Random.Range(0, tempList.Count);
                        tempList[ran1].SetActive(true);
                        tempList[ran1].GetComponent<MagicCardDisplay>().BuyCardFromMagic();
                        AudioManager.AMInstance.PlayAudio(5);
                    }
                }

                break;

            case 13:
                count = 0;

                for (int i = 0; i < 1; i++)
                {
                    List<GameObject> tempList = new List<GameObject>();

                    while (tempList.Count == 0 && count < 400)
                    {
                        count++;

                        if (tempList.Count != 0)
                        {
                            tempList.RemoveRange(0, tempList.Count);
                        }

                        string rarity = GMInstance.GachaRate();

                        foreach (Transform card in GMInstance.shopfieldTrans)
                        {
                            if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf && card.GetComponent<CardDisplay>().type[0] == "Troop")
                            {
                                tempList.Add(card.gameObject);
                            }
                        }
                    }

                    int ran1 = Random.Range(0, tempList.Count);
                    tempList[ran1].SetActive(true);
                    tempList[ran1].GetComponent<TroopCardDisplay>().BuyCardFromMagic();
                }

                AudioManager.AMInstance.PlayAudio(5);
                break;

            case 14:
                money = BossManager.BMInstance.stage;
                AudioManager.AMInstance.PlayAudio(4);
                GMInstance.money = 2 * money;
                GMInstance.AddMoney(0);
                break;

            case 15:
                try
                {
                    if (sargs[0] == "Leader")
                    {
                        AudioManager.AMInstance.PlayAudio(7);
                    }
                }
                catch
                {

                }

                GMInstance.maxTroopInBattlefield += args[0];
                GMInstance.currentBattlefield = GMInstance.maxTroopInBattlefield;
                break;

            case 16:
                GMInstance.RerollPack();
                AudioManager.AMInstance.PlayAudio(5);
                break;

            case 17:
                int ran = Random.Range(0, 2);
                if (ran == 0)
                {
                    GMInstance.AddMoney(GMInstance.money);
                }
                else
                {
                    GMInstance.money = 0;
                    GMInstance.AddMoney(0);
                }

                AudioManager.AMInstance.PlayAudio(4);
                break;

            case 18:
                GMInstance.RerollIdol();
                AudioManager.AMInstance.PlayAudio(5);
                break;
        }
    }

    public void LateEffect(int effectCode, int[] args, string[] sargs, TextMeshProUGUI pointText)
    {
        //t2
        switch (effectCode)
        {
            case -1:
                return;

            case 0: //Soft & Wet
                if (GMInstance.isBattle)
                {
                    int ran = Random.Range(0, 8);
                    if (ran == 0)
                    {
                        GMInstance.CardFromMagicfieldToShopfield(softAndWet);
                        softAndWet.GetComponent<MagicCardDisplay>().card = magicCards[0];
                        softAndWet.GetComponent<MagicCardDisplay>().Init();
                        softAndWet.GetComponent<MagicCardDisplay>().priceText.transform.parent.gameObject.SetActive(true);
                        softAndWet.name = magicCards[0].name;
                    }
                    else
                    {
                        Point(pointText, "Nope!", Color.yellow);
                        return;
                    }
                }

                break;

            case 1: //Soft & Wet Go Beyond
                if (GMInstance.isBattle)
                {
                    int ran1 = Random.Range(0, 1000);
                    if (ran1 == 0)
                    {
                        GMInstance.CardFromMagicfieldToShopfield(softAndWet);
                    }
                }

                break;

            case 2:
                break;

            case 3: //Earn an extra $1 of interest for every $5 you have at end of round
                int bonus = GMInstance.money / 5;
                if (bonus > 20)
                {
                    bonus = 20;
                }
                BossManager.BMInstance.currentBossReward += bonus;
                break;

            case 4: //Earn $A at end of round
                BossManager.BMInstance.currentBossReward += args[0];
                break;

            case 5: //Gains $A of sell value at end of round
                if (GMInstance.cardsInMagicfield.Contains(artifact))
                {
                    GMInstance.artifactSellValue += 3;
                }
                break;

            case 6: //Blue Mirror
                for (int i = 0; i < GMInstance.magicfieldTrans.childCount - 1; i++)
                {
                    if (GMInstance.magicfieldTrans.GetChild(i).gameObject == blueMirror)
                    {
                        GMInstance.magicfieldTrans.GetChild(i + 1).GetComponent<MagicCardDisplay>().LateEffect();
                        break;
                    }
                }

                break;

            case 7: //Magic Mirror
                if (GMInstance.cardsInMagicfield.Contains(magicMirror))
                {
                    if (GMInstance.magicfieldTrans.GetChild(0).gameObject != magicMirror)
                    {
                        GMInstance.magicfieldTrans.GetChild(0).GetComponent<MagicCardDisplay>().LateEffect();
                        break;
                    }
                }

                break;

            case 8: //Earn $2 per discard if no discards are used by end of the round
                if (GMInstance.discards == GMInstance.maxDiscards)
                {
                    BossManager.BMInstance.currentBossReward += args[0] * GMInstance.discards;
                }

                break;

            case 9: //firefly
                foreach (GameObject card in GMInstance.cardsInCampfield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().name == sargs[0])
                    {
                        GMInstance.fireflyCounter++;
                        if (GMInstance.fireflyCounter == args[0])
                        {
                            card.GetComponent<TroopCardDisplay>().card = troopCards[0];
                            card.GetComponent<TroopCardDisplay>().Init();
                            card.name = troopCards[0].name;
                        }
                        break;
                    }
                }

                break;

            case 10: //Free reroll 1 time per visit shop
                GMInstance.rerollBtn.onClick.RemoveAllListeners();
                GMInstance.rerollBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Reroll - " + 0 + "$";
                GMInstance.rerollBtn.onClick.AddListener(() =>
                {
                    GMInstance.FreeRerollShopEffect();
                });
                break;

            case 11: //Ahri
                foreach (GameObject card in GMInstance.cardsInCampfield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().name == sargs[0])
                    {
                        GMInstance.ahriCounter += args[0];
                        card.GetComponent<TroopCardDisplay>().ChangeAttack(args[0]);
                        break;
                    }
                }
                break;

            case 12: //Tusk
                if (GMInstance.isBattle)
                {
                    GMInstance.tuskCounter++;
                    if (GMInstance.tuskCounter == 2 && tusk.GetComponent<MagicCardDisplay>().card.name == "Tusk Act I")
                    {
                        GMInstance.tuskCounter = 0;
                        GMInstance.CardFromMagicfieldToShopfield(tusk);
                        tusk.name = magicCards[1].name;
                        tusk.GetComponent<MagicCardDisplay>().card = magicCards[1];
                        tusk.GetComponent<MagicCardDisplay>().Init();
                        tusk.GetComponent<MagicCardDisplay>().priceText.transform.parent.gameObject.SetActive(true);
                    }
                    else if (GMInstance.tuskCounter == 3 && tusk.GetComponent<MagicCardDisplay>().card.name == "Tusk Act II")
                    {
                        GMInstance.tuskCounter = 0;
                        tusk.name = magicCards[2].name;
                        GMInstance.CardFromMagicfieldToShopfield(tusk);
                        tusk.GetComponent<MagicCardDisplay>().card = magicCards[2];
                        tusk.GetComponent<MagicCardDisplay>().Init();
                        tusk.GetComponent<MagicCardDisplay>().priceText.transform.parent.gameObject.SetActive(true);
                    }
                    else if (GMInstance.tuskCounter == 4 && tusk.GetComponent<MagicCardDisplay>().card.name == "Tusk Act III")
                    {
                        GMInstance.tuskCounter = 0;
                        tusk.name = magicCards[3].name;
                        GMInstance.CardFromMagicfieldToShopfield(tusk);
                        tusk.GetComponent<MagicCardDisplay>().card = magicCards[3];
                        tusk.GetComponent<MagicCardDisplay>().Init();
                        tusk.GetComponent<MagicCardDisplay>().priceText.transform.parent.gameObject.SetActive(true);
                    }
                    else
                    {
                        Point(pointText, GMInstance.tuskCounter + "/" + args[1], Color.yellow);
                        return;
                    }
                }
                break;
        }

        Point(pointText, "Effect!", Color.yellow);
    }

    public void IdolEffect(int effectCode, int[] args, string[] sargs)
    {
        //t0
        switch (effectCode) 
        {
            case 0:
                GMInstance.baseAttack += args[0];
                break;

            case 1:
                GMInstance.baseDefense += args[0];
                break;

            case 2:
                GMInstance.maxRerollMoney -= args[0];
                GMInstance.rerollMoney = GMInstance.maxRerollMoney;
                GMInstance.rerollBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Reroll - " + GMInstance.rerollMoney + "$";
                break;

            case 4:
                GMInstance.maxHandSize += args[0];
                break;

            case 5:
                GMInstance.maxHands += args[0];
                break;

            case 6:
                GMInstance.maxDiscards += args[0];
                break;

            case 7:
                GMInstance.maxMagicInMagicfield += args[0];
                break;

            case 8:
                GMInstance.baseAttack += args[0];
                GMInstance.baseDefense += args[1];
                break;

            case 9:
                GMInstance.maxTroopInBattlefield += args[0];
                GMInstance.currentBattlefield = GMInstance.maxTroopInBattlefield;
                break;

            case 10:
                GMInstance.maxCardInShop++;
                GMInstance.shopfieldTrans.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(GMInstance.shopfieldTrans.GetComponent<RectTransform>().rect.width + 160, 220);
                
                for (int i = 0; i < 1; i++)
                {
                    List<GameObject> tempList = new List<GameObject>();

                    while (tempList.Count == 0)
                    {
                        if (tempList.Count != 0)
                        {
                            tempList.RemoveRange(0, tempList.Count);
                        }

                        string rarity = GMInstance.GachaRate();

                        foreach (Transform card in GMInstance.shopfieldTrans)
                        {
                            if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf)
                            {
                                tempList.Add(card.gameObject);
                            }
                        }
                    }

                    int ran = Random.Range(0, tempList.Count);
                    tempList[ran].SetActive(true);
                }

                break;

            case 11:
                GMInstance.maxSuperInSuperfield += args[0];
                break;

            case 12:
                GMInstance.AddMoney(args[0]);
                break;

            case 13:
                BMInstance.BackStage();
                break;

            case 14:
                GMInstance.maxCardInShopPackfield++;
                GMInstance.shopPackfieldTrans.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(GMInstance.shopPackfieldTrans.GetComponent<RectTransform>().rect.width + 160, 220);
                GMInstance.shopPackfieldTrans.localPosition = new UnityEngine.Vector2(GMInstance.shopPackfieldTrans.localPosition.x + 80, GMInstance.shopPackfieldTrans.localPosition.y);

                for (int i = 0; i < 1; i++)
                {
                    List<GameObject> tempList = new List<GameObject>();

                    while (tempList.Count == 0)
                    {
                        if (tempList.Count != 0)
                        {
                            tempList.RemoveRange(0, tempList.Count);
                        }

                        string rarity = GMInstance.GachaRate();

                        foreach (Transform card in GMInstance.shopPackfieldTrans)
                        {
                            if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf)
                            {
                                tempList.Add(card.gameObject);
                            }
                        }
                    }

                    int ran = Random.Range(0, tempList.Count);
                    tempList[ran].SetActive(true);
                }
                break;

            case 15:
                GMInstance.battleBonus++;
                break;

            case 16:
                GMInstance.skipBonus++;
                break;

            case 17:
                GMInstance.maxTroopInBattlefield += args[0];
                GMInstance.maxHandSize += args[0];
                GMInstance.currentBattlefield = GMInstance.maxTroopInBattlefield;
                break;
        }
    }

    public void LuckyEffect(int effectCode, int[] args, string[] sargs, int reset)
    {
        GMInstance.OpenPack(args[0], effectCode, sargs[0], reset);
    }

    public void HandEffect(int effectCode, int[] args, string[] sargs, TextMeshProUGUI pointText)
    {
        //t6
        switch (effectCode)
        {
            case -1:
                break;

            case 0: //+A ATK for each "A" card played in battlefield if hold this card in hand
                int count = 0;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalAttack += args[0];
                            break;
                        }
                    }
                }

                Point(pointText, "+" + count, Color.red);
                break;

            case 1: //+A DEF for each "A" card played in battlefield if hold this card in hand
                count = 0;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalDefense += args[0];
                            break;
                        }
                    }
                }

                Point(pointText, "+" + count, Color.cyan);
                break;

            case 2: //xA ATK for each "A" card played in battlefield if hold this card in hand
                count = 1;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count *= args[0];
                            GMInstance.totalDefense *= args[0];
                            break;
                        }
                    }
                }

                Point(pointText, "x" + count, Color.cyan);
                break;

            case 3:
                count = 1;
                int count1 = 0;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count1++;
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInHandfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count1++;
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInCampfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count1++;
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInAltarfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count1++;
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInMagicfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<MagicCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<MagicCardDisplay>().type[i] == sargs[0])
                        {
                            count1++;
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInIdolfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<IdolCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<IdolCardDisplay>().type[i] == sargs[0])
                        {
                            count1++;
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                count1 /= args[1];
                for (int i = 0; i < count1; i++)
                {
                    count *= args[0];
                }

                Point(pointText, "x" + count, Color.red);
                break;
        }
    }

    public void Effect(int effectCode, int[] args, string[] sargs, TextMeshProUGUI pointText)
    {
        //t0
        switch (effectCode)
        {
            case -1:
                break;

            case 0:
                GMInstance.totalAttack += args[0];
                Point(pointText, "+" + args[0], Color.red);
                break;

            case 1:
                GMInstance.totalAttack *= args[0];
                Point(pointText, "x" + args[0], Color.red);
                break;

            case 2:
                GMInstance.totalDefense += args[0];
                Point(pointText, "+" + args[0], Color.cyan);
                break;

            case 3:
                GMInstance.totalDefense *= args[0];
                Point(pointText, "x" + args[0], Color.cyan);
                break;

            case 4: //+A ATK for each "A" or "A" card in battlefield
                int count = 0;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0] || card.GetComponent<TroopCardDisplay>().type[i] == sargs[1])
                        {
                            count += args[0];
                            break;
                        }
                    }
                }
                GMInstance.totalAttack += count;
                Point(pointText, "+" + count, Color.red);

                break;

            case 5: //+A DEF for each "A" or "A" played in battlefield
                count = 0;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0] || card.GetComponent<TroopCardDisplay>().type[i] == sargs[1])
                        {
                            count += args[0];
                            GMInstance.totalDefense += args[0];
                            break;
                        }
                    }
                }

                Point(pointText, "+" + count, Color.cyan);
                break;

            case 6: //+A ATK for each "A" card played in battlefield
                count = 0;
                
                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalAttack += args[0];
                            break;
                        }
                    }
                }

                Point(pointText, "+" + count, Color.red);
                break;

            case 7: //Xiao
                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().name == sargs[0])
                    {
                        GMInstance.xiaoCounter += args[0];  
                        card.GetComponent<TroopCardDisplay>().ChangeDefense(args[0]);
                        break;
                    }
                }

                Point(pointText, "Effect!", Color.yellow);
                break;

            case 8: //+A DEF for each "A" card in all field
                count = 0;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalDefense += args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInHandfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalDefense += args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInCampfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalDefense += args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInAltarfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalDefense += args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInMagicfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<MagicCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<MagicCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalDefense += args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInIdolfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<IdolCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<IdolCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalDefense += args[0];
                            break;
                        }
                    }
                }

                Point(pointText, "+" + count, Color.cyan);
                break;

            case 9: //Hu Tao
                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().name == sargs[0])
                    {
                        GMInstance.hutaoATKCounter += args[1];
                        GMInstance.hutaoDEFCounter += args[0];
                        card.GetComponent<TroopCardDisplay>().ChangeDefense(args[0]);
                        card.GetComponent<TroopCardDisplay>().ChangeAttack(args[1]);
                        break;
                    }
                }

                Point(pointText, "Effect!", Color.yellow);
                break;

            case 10: //+A ATK for each "A" card in all field
                count = 0;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalAttack += args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInHandfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalAttack += args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInCampfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalAttack += args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInAltarfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalAttack += args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInMagicfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<MagicCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<MagicCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalAttack += args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInIdolfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<IdolCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<IdolCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalAttack += args[0];
                            break;
                        }
                    }
                }

                Point(pointText, "+" + count, Color.red);
                break;

            case 11: //+A ATK for each $1 you have
                GMInstance.totalAttack += args[0] * GMInstance.money;
                Point(pointText, "+" + args[0] * GMInstance.money, Color.red);
                break;

            case 12: //+A ATK for each magic card you have 
                count = 0;
                count += GMInstance.cardsInMagicfield.Count * args[0];
                GMInstance.totalAttack += count;
                Point(pointText, "+" + count, Color.red);
                break;

            case 13: //xA ATK for each "A" card play in battlefield 
                count = 1;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count *= args[0];
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                Point(pointText, "x" + count, Color.red);
                break;

            case 14: //xA ATK first hand played
                count = 1;

                if (GMInstance.turn == 1 || GMInstance.HasGoldExperience())
                {
                    count *= args[0];
                    GMInstance.totalAttack *= args[0];
                }

                Point(pointText, "x" + count, Color.red);
                break;

            case 15: //+(A -> A) ATK 
                count = Random.Range(args[0], args[1]);
                GMInstance.totalAttack += count;
                Point(pointText, "+" + count, Color.red);
                break;

            case 16: //+A DEF for each "A" magic card you have
                count = 0;
                count += GMInstance.cardsInMagicfield.Count * args[0];
                GMInstance.totalDefense += count;
                Point(pointText, "+" + count, Color.cyan);
                break;

            case 17: //+A DEF & +A ATK
                GMInstance.totalDefense += args[0];
                GMInstance.totalAttack += args[1];
                Point(pointText, "Effect!", Color.yellow);
                break;

            case 18: //Played cards with DEF's first digit is odd give +a DEF
                count = 0;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    string firstDigitStr = card.GetComponent<TroopCardDisplay>().defense.ToString();
                    int firstDigit = int.Parse(firstDigitStr[0] + "");
                    print(firstDigit);
                    if (firstDigit % 2 != 0)
                    {
                        count += args[0];
                        GMInstance.totalDefense += args[0];
                    }
                }

                Point(pointText, "+" + count, Color.cyan);
                break;

            case 19: //Played cards with original ATK's first digit is even give +a ATK
                count = 0;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    string firstDigitStr = card.GetComponent<TroopCardDisplay>().attack.ToString();
                    int firstDigit = int.Parse(firstDigitStr[0] + "");
                    print(firstDigit);
                    if (firstDigit % 2 == 0)
                    {
                        count += args[0];
                        GMInstance.totalAttack += args[0];
                    }
                }

                Point(pointText, "+" + count, Color.red);
                break;

            case 20: //xA ATK on final hand of round
                count = 1;

                if (GMInstance.hands == 0 || GMInstance.HasKingCrimson())
                {
                    count *= args[0];
                    GMInstance.totalAttack *= args[0];
                }

                Point(pointText, "x" + count, Color.red);
                break;

            case 21: //Blue Mirror
                for (int i = 0; i < GMInstance.magicfieldTrans.childCount - 1; i++)
                {
                    if (GMInstance.magicfieldTrans.GetChild(i).gameObject == blueMirror)
                    {
                        GMInstance.magicfieldTrans.GetChild(i + 1).GetComponent<MagicCardDisplay>().Effect();
                        break;
                    }
                }

                break;

            case 22: //Magic Mirror
                if (GMInstance.cardsInMagicfield.Contains(magicMirror))
                {
                    if (GMInstance.magicfieldTrans.GetChild(0).gameObject != magicMirror)
                    {
                        GMInstance.magicfieldTrans.GetChild(0).GetComponent<MagicCardDisplay>().Effect();
                        break;
                    }
                }

                break;

            case 23: //+A DEF for each remaining discard
                GMInstance.totalDefense += GMInstance.discards * args[0];
                Point(pointText, "+" + GMInstance.discards * args[0], Color.cyan);
                break;

            case 24: //1 in a chance WIN the match
                int ran = Random.Range(0, args[0]);
                if (ran == 1)
                {
                    GMInstance.isEnd = true;
                    Point(pointText, "Win!", Color.yellow);
                    return;
                }

                Point(pointText, "Nope!", Color.yellow);
                break;

            case 25: //x(Number of hand size you have)
                GMInstance.totalAttack *= GMInstance.handSize;
                Point(pointText, "x" + GMInstance.handSize, Color.red);
                break;

            case 26: //x2 Mult for each empty "Magic" slot. Vagabondl included
                int mul = (GMInstance.maxMagicInMagicfield - GMInstance.cardsInMagicfield.Count) * 2;
                GMInstance.totalAttack *= mul;
                Point(pointText, "x" + mul, Color.red);
                break;

            case 27: //+A DEF for each "A" card played in battlefield
                count = 0;
                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.totalDefense += args[0];
                            break;
                        }
                    }
                }

                Point(pointText, "+" + count, Color.cyan);
                break;

            case 28: //xA DEF if played this card in battlefield when have at least A "A" card in battlefield
                int counter = 0;
                count = 1;
                
                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            counter++;
                            break;
                        }
                    }
                }

                if (counter >= args[1])
                {
                    count *= args[0];
                    GMInstance.totalDefense *= args[0];
                }

                Point(pointText, "x" + count, Color.cyan);
                break;

            case 29: //xA ATK for each "A" cards in the all field 
                count = 1;
                int count1 = 0;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count1++;
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInHandfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count1++;
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInCampfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count1++;
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInAltarfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count1++;
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInMagicfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<MagicCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<MagicCardDisplay>().type[i] == sargs[0])
                        {
                            count1++;
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                foreach (GameObject card in GMInstance.cardsInIdolfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<IdolCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<IdolCardDisplay>().type[i] == sargs[0])
                        {
                            count1++;
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                count1 /= args[1];
                for (int i = 0; i < count1; i++)
                {
                    count *= args[0];
                }

                Point(pointText, "x" + count, Color.red);
                break;

            case 30: //xA ATK if played in battlefield with "A"  
                count = 1;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().name == sargs[0])
                    {
                        count *= args[0];
                        GMInstance.totalAttack *= args[0];
                        break;
                    }
                }

                Point(pointText, "x" + count, Color.red);
                break;

            case 31: //Gain xA ATK for each "A" card played in battlefield
                count = 0;
                
                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            break;
                        }
                    }
                }

                GMInstance.totalAttack *= count;
                Point(pointText, "x" + count, Color.red);
                break;

            case 32: //xa ATK if played this card alone in battlefield | xa ATK if played only one card in battlefield
                count = 1;

                if (GMInstance.cardsInBattlefield.Count == 1 || GMInstance.HasLonely())
                {
                    count *= args[0];
                    GMInstance.totalAttack *= count;
                }

                Point(pointText, "x" + count, Color.red);
                break;

            case 33: //xa ATK & xa DEF
                GMInstance.totalAttack *= args[0];
                GMInstance.totalDefense *= args[1];

                Point(pointText, "Effect!", Color.yellow);
                break;

            case 34: //x(Number of "Magic" card) ATK
                GMInstance.totalAttack *= GMInstance.cardsInMagicfield.Count * args[0];

                Point(pointText, "x" + GMInstance.cardsInMagicfield.Count * args[0], Color.red);
                break;

            case 35: //xa DEF each "a" card played in battlefield
                count = 1;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count *= args[0];
                            GMInstance.totalDefense *= args[0];
                            break;
                        }
                    }
                }

                Point(pointText, "x" + count, Color.cyan);
                break;

            case 36: //+a ATK if played in final hand
                count = 0;

                if (GMInstance.hands == 0 || GMInstance.HasKingCrimson())
                {
                    count += args[0];
                    GMInstance.totalAttack += args[0];
                }

                Point(pointText, "+" + count, Color.red);
                break;

            case 37: //x(a -> a) ATK
                count = Random.Range(args[0], args[1] + 1);
                GMInstance.totalAttack *= count;
                Point(pointText, "x" + count, Color.red);
                break;

            case 38: //+A DEF for each "A" magic card you have 
                count = 0;
                count += GMInstance.cardsInMagicfield.Count * args[0];
                GMInstance.totalDefense += count;
                Point(pointText, "+" + count, Color.cyan);
                break;

            case 39: //+a ATK & +a DEF if played in final hand
                if (GMInstance.hands == 0 || GMInstance.HasKingCrimson())
                {
                    GMInstance.totalAttack += args[0];
                    GMInstance.totalAttack += args[1];
                }

                Point(pointText, "Effect!", Color.yellow);
                break;

            case 40: //Gain xA DEF for each "A" idol cards 
                count = 1;

                foreach (GameObject card in GMInstance.cardsInIdolfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<IdolCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<IdolCardDisplay>().type[i] == sargs[0])
                        {
                            count *= args[0];
                            GMInstance.totalDefense *= args[0];
                            break;
                        }
                    }
                }

                Point(pointText, "x" + count, Color.cyan);
                break;

            case 41: //xA DEF if played in battlefield with "A" & xA ATK if not played in battlefield with "A" 
                count = 1;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().name == sargs[0])
                    {
                        count *= args[0];
                        GMInstance.totalDefense *= args[0];
                        Point(pointText, "x" + count, Color.cyan);
                        return;
                    }
                }

                count *= args[1];
                GMInstance.totalAttack *= args[1];
                Point(pointText, "x" + count, Color.red);
                break;

            case 42: //+a$ if played in battlefield
                count = args[0];
                GMInstance.AddMoney(args[0]);
                Point(pointText, "+" + count + "$", Color.yellow);
                break;

            case 43: //kayn
                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().name == sargs[0])
                    {
                        GMInstance.kaynCounter++;
                        if (GMInstance.kaynCounter == args[0])
                        {
                            card.GetComponent<TroopCardDisplay>().card = troopCards[1];
                            card.GetComponent<TroopCardDisplay>().Init();
                            card.name = troopCards[1].name;
                            Point(pointText, "Effect!", Color.yellow);
                            return;
                        }
                        Point(pointText, GMInstance.kaynCounter + "/" + args[0], Color.yellow);
                        break;
                    }
                }

                
                break;

            case 44: //+a$ for each "a" card played in battlefield
                count = 0;
                
                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            GMInstance.AddMoney(args[0]);
                            break;
                        }
                    }
                }

                Point(pointText, "+" + count + "$", Color.yellow);
                break;

            case 45: //Score at least a% TCP Required = WIN the match
                BigInteger total = GMInstance.totalScore + GMInstance.totalAttack * GMInstance.totalDefense;
                BigInteger require = args[0] * BossManager.BMInstance.currentBossScore / 100;

                if (total >= require)
                {
                    print("a");
                    GMInstance.isEnd = true;
                    Point(pointText, "Win!", Color.yellow);
                    return;
                }

                Point(pointText, "Nope!", Color.yellow);
                break;

            case 46: //1 in a chance xa ATK
                ran = Random.Range(0, args[0]);
                if (ran == 1)
                {
                    count = args[1];
                    GMInstance.totalAttack *= count;
                    Point(pointText, "x" + count, Color.red);
                    return;
                }

                Point(pointText, "Nope!", Color.yellow);
                break;

            case 47: //xa ATK if not played in battlefield with "a" 
                count = 1;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().name == sargs[0])
                    {
                        Point(pointText, "x" + count, Color.red);
                        return;
                    }
                }

                count *= args[0];
                GMInstance.totalAttack *= args[0];
                Point(pointText, "x" + count, Color.red);
                break;

            case 48: //xA DEF if played in battlefield with "A"  
                count = 1;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().name == sargs[0])
                    {
                        count *= args[0];
                        GMInstance.totalDefense *= args[0];
                        break;
                    }
                }

                Point(pointText, "x" + count, Color.cyan);
                break;

            case 49: //Veigar
                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().name == sargs[0])
                    {
                        GMInstance.veigarCounter += args[0];
                        card.GetComponent<TroopCardDisplay>().ChangeAttack(args[0]);
                        break;
                    }
                }

                Point(pointText, "Effect!", Color.yellow);
                break;

            case 50: //xA ATK for each "A" magic card play in battlefield 
                count = 1;

                foreach (GameObject card in GMInstance.cardsInMagicfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<MagicCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<MagicCardDisplay>().type[i] == sargs[0])
                        {
                            count *= args[0];
                            GMInstance.totalAttack *= args[0];
                            break;
                        }
                    }
                }

                Point(pointText, "x" + count, Color.red);
                break;

            case 51: //+a DEF if DEF > a & xa DEF if DEF > a
                if (GMInstance.totalDefense > args[0])
                {
                    GMInstance.totalDefense += args[1];
                }
                if (GMInstance.totalDefense > args[2])
                {
                    GMInstance.totalDefense *= args[3];
                }

                Point(pointText, "Effect!", Color.yellow);
                break;

            case 52: //Heartsteel
                count = 0;

                foreach (GameObject card in GMInstance.cardsInMagicfield.ToArray())
                {
                    if (card.GetComponent<MagicCardDisplay>().name == sargs[0])
                    {
                        count += GMInstance.heartSteelCounter;
                        GMInstance.totalDefense += count;
                        GMInstance.heartSteelCounter += args[0];
                        card.GetComponent<MagicCardDisplay>().effectText.text = "+" + args[0] +" DEF per played\n(Currently: +" + GMInstance.heartSteelCounter + " DEF)";
                    }
                }

                Point(pointText, "+" + count, Color.cyan);
                break;

            case 53: //Solo Leveling
                count = 0;

                foreach (GameObject card in GMInstance.cardsInMagicfield.ToArray())
                {
                    if (card.GetComponent<MagicCardDisplay>().name == sargs[0])
                    {
                        count += GMInstance.soloLevelingCounter;
                        GMInstance.totalAttack += count;
                        GMInstance.soloLevelingCounter += args[0];
                        card.GetComponent<MagicCardDisplay>().effectText.text = "+" + args[0] + " ATK per played\n(Currently: +" + GMInstance.soloLevelingCounter + " ATK)";
                    }
                }

                Point(pointText, "+" + count, Color.red);
                break;

            case 54: //xA ATK for each "A" card play in battlefield (rarity) 
                count = 1;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().type[1] == sargs[0])
                    {
                        count *= args[0];
                        GMInstance.totalAttack *= args[0];
                    }
                }

                Point(pointText, "x" + count, Color.red);
                break;

            case 55: //xA ATK base on counter
                count = 1;

                if (sargs[0] == "Annihilate")
                {
                    count = GMInstance.numberOfDiscard / args[0] + 1;
                    print(count);
                    GMInstance.totalAttack *= count;
                }
                else if (sargs[0] == "Friend")
                {
                    count = GMInstance.numberOfAdd / args[0] + 1;
                    print(count);
                    GMInstance.totalAttack *= count;
                }
                else if (sargs[0] == "Death Note")
                {
                    count = GMInstance.numberOfDestroy * args[0] + 1;
                    print(count);
                    GMInstance.totalAttack *= count;
                }
                else if (sargs[0] == "Coward")
                {
                    count = GMInstance.skipTime * args[0] + 1;
                    print(count);
                    GMInstance.totalAttack *= count;
                }
                else if (sargs[0] == "Challenger")
                {
                    count = GMInstance.battleTime / args[0] + 1;
                    print(count);
                    GMInstance.totalAttack *= count;
                }
                

                Point(pointText, "x" + count, Color.red);
                break;

            case 56:
                count = args[0];
                GMInstance.totalAttack = args[0];
                Point(pointText, "=" + count, Color.red);
                break;

            case 57:
                count = args[0];
                GMInstance.totalDefense = args[0];
                Point(pointText, "=" + count, Color.cyan);
                break;

            case 58: //xa ATK at hand b
                count = 1;

                if (GMInstance.hands == args[1])
                {
                    count = args[0];
                    GMInstance.totalAttack *= args[0];
                }

                Point(pointText, "x" + count, Color.red);
                break;

            case 59: //+A ATK ff played in first hand
                count = 0;

                if (GMInstance.turn == 1 || GMInstance.HasGoldExperience())
                {
                    count += args[0];
                    GMInstance.totalAttack += args[0];
                }

                Point(pointText, "+" + count, Color.red);
                break;

            case 60: //Random 1 of 3 effects: +200 ATK | x2 ATK | +4$
                ran = Random.Range(0, 3);
                if (ran == 0)
                {
                    count = args[0];
                    GMInstance.totalAttack += args[0];
                    Point(pointText, "+" + count, Color.red);
                    return;
                }
                else if (ran == 1)
                {
                    count = args[1];
                    GMInstance.totalAttack *= args[1];
                    Point(pointText, "x" + count, Color.red);
                    return;
                }
                else
                {
                    count = args[2];
                    GMInstance.AddMoney(count);
                    Point(pointText, "+" + count + "$", Color.yellow);
                }
                break;

            case 61: //xa DEF if battlefield only have "a" card
                count = 1;
                count1 = 0;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count1++;
                            break;
                        }
                    }
                }

                if (count1 == GMInstance.cardsInBattlefield.Count)
                {
                    count *= args[0];
                    GMInstance.totalAttack *= args[0];
                    Point(pointText, "x" + count, Color.red);
                }
                else
                {
                    Point(pointText, "x" + count, Color.red);
                }

                break;

            case 62: //Gain xa ATK for each "a" card hold in hand
                count = 1;

                foreach (GameObject card in GMInstance.cardsInHandfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count += args[0];
                            break;
                        }
                    }
                }

                GMInstance.totalAttack *= count;
                Point(pointText, "x" + count, Color.red);
                break;

            case 63: //Im Last Place
                try
                {
                    GMInstance.battlefieldTrans.GetChild(lastPos()).GetComponent<TroopCardDisplay>().Effect();
                }
                catch
                {
                }

                break;

            case 64: //xA DEF if DEF < ATK
                count = 1;

                if (GMInstance.totalDefense < GMInstance.totalAttack)
                {
                    count = args[0];
                    GMInstance.totalDefense *= args[0];
                }

                Point(pointText, "x" + count, Color.cyan);
                break;

            case 65:
                if (GMInstance.cardsInBattlefield.Count >= 3 && GMInstance.cardsInAltarfield.Count > 0)
                {
                    ran = Random.Range(0, GMInstance.cardsInAltarfield.Count);
                    GMInstance.CardFromAltarfieldToHandfield(GMInstance.cardsInAltarfield[ran]);
                    Point(pointText, "Effect!", Color.yellow);
                }

                Point(pointText, "Nope!", Color.yellow);
                break;

            case 66: //x(Number of current discard) ATK 
                count = GMInstance.discards;
                GMInstance.totalAttack *= count;
                Point(pointText, "x" + count, Color.red);
                break;

            case 67: //x(Number of current hand) ATK 
                count = GMInstance.hands;
                GMInstance.totalAttack *= count;
                Point(pointText, "x" + count, Color.red);
                break;

            case 68: //xA DEF if DEF > ATK
                count = 1;

                if (GMInstance.totalDefense > GMInstance.totalAttack)
                {
                    count = args[0];
                    GMInstance.totalDefense *= args[0];
                }

                Point(pointText, "x" + count, Color.cyan);
                break;

            case 69: //if you have "Sotft & Wet" in magicfield destroy it
                foreach (GameObject card in GMInstance.cardsInMagicfield.ToArray())
                {
                    if (card == softAndWet)
                    {
                        softAndWet.GetComponent<MagicCardDisplay>().card = magicCards[0];
                        softAndWet.GetComponent<MagicCardDisplay>().Init();
                        softAndWet.name = magicCards[0].name;
                    }
                }

                Point(pointText, "Effect!", Color.yellow);
                break;

            case 70: //if you have "Tusk" in magicfield, x(Number of card's Act) ATK
                count = 1;

                foreach (GameObject card in GMInstance.cardsInMagicfield.ToArray())
                {
                    if (card.GetComponent<MagicCardDisplay>().card.name == "Tusk Act II")
                    {
                        count = args[0];
                        GMInstance.totalAttack *= args[0];
                        break;
                    }
                    else if (card.GetComponent<MagicCardDisplay>().card.name == "Tusk Act III")
                    {
                        count = args[1];
                        GMInstance.totalAttack *= args[1];
                        break;
                    }
                    else if (card.GetComponent<MagicCardDisplay>().card.name == "Tusk Act IV")
                    {
                        count = args[2];
                        GMInstance.totalAttack *= args[2];
                        break;
                    }
                }

                Point(pointText, "x" + count, Color.red);
                break;

            case 71: //+a Hand & -a Discard
                GMInstance.ChangeHand(0, args[0]);
                GMInstance.ChangeDiscard(0, args[1]);
                Point(pointText, "Effect!", Color.yellow);
                break;

            case 72: //+a ATK if played this card alone in battlefield | +a ATK if played only one card in battlefield
                count = 0;

                if (GMInstance.cardsInBattlefield.Count == 1 || GMInstance.HasLonely())
                {
                    count += args[0];
                    GMInstance.totalAttack += count;
                }

                Point(pointText, "+" + count, Color.red);
                break;

            case 73: //xa ATK for each non-effect card play in battlefield
                count = 1;

                foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                {
                    if (card.GetComponent<TroopCardDisplay>().card.effect == "None")
                    {
                        count *= args[0];
                        GMInstance.totalAttack *= args[0];
                    }
                }

                Point(pointText, "x" + count, Color.red);
                break;

            case 74: //Balance DEF & ATK when calculating
                BigInteger balance = (GMInstance.totalAttack + GMInstance.totalDefense) / 2;
                GMInstance.totalAttack = balance;
                GMInstance.totalDefense = balance;
                Point(pointText, "Effect!", Color.yellow);
                break;

            case 75: //Set hand to a
                GMInstance.ChangeHand(1, 2);
                Point(pointText, "Effect!", Color.yellow);
                break;

            case 76: //Set discard to a
                GMInstance.ChangeDiscard(1, 2);
                Point(pointText, "Effect!", Color.yellow);
                break;

            case 77: //LOL
                ran = Random.Range(0, 2);
                if (ran == 0)
                {
                    GMInstance.totalAttack += args[0];
                    Point(pointText, "+" + args[0], Color.red);
                }
                else
                {
                    GMInstance.totalAttack = 0;
                    Point(pointText, "x0", Color.red);
                }
                break;


            case 79: //x(a -> a) DEF
                count = Random.Range(args[0], args[1] + 1);
                GMInstance.totalDefense *= count;
                Point(pointText, "x" + count, Color.cyan);
                break;

            case 80:
                count = GMInstance.maxHands;
                GMInstance.totalDefense *= count;
                Point(pointText, "x" + count, Color.cyan);
                break;

            case 81:
                count = GMInstance.maxDiscards;
                GMInstance.totalAttack *= count;
                Point(pointText, "x" + count, Color.red);
                break;

            case 82:
                ran = Random.Range(0, args[0]);
                if (ran == 1 && (GMInstance.cardsInBattlefield.Count == 1 || GMInstance.HasLonely()))
                {
                    GMInstance.isEnd = true;
                    Point(pointText, "Win!", Color.yellow);
                    return;
                }

                Point(pointText, "Nope!", Color.yellow);
                break;

            case 83:
                count = 1;

                if (GMInstance.turn == 1 || GMInstance.HasGoldExperience())
                {
                    count *= args[0];
                    GMInstance.totalAttack *= args[0];
                    Point(pointText, "x" + count, Color.red);
                    break;
                }
                else if (GMInstance.hands == 0 || GMInstance.HasKingCrimson())
                {
                    count *= args[1];
                    GMInstance.totalAttack *= args[1];
                    Point(pointText, "x" + count, Color.red);
                    break;
                }

                Point(pointText, "x" + count, Color.red);
                break;

            case 84: //x(Number of hand size you have)
                GMInstance.totalDefense *= GMInstance.handSize;
                Point(pointText, "x" + GMInstance.handSize, Color.cyan);
                break;

            case 85:
                GMInstance.totalAttack *= BMInstance.stage;
                Point(pointText, "x" + BMInstance.stage, Color.red);
                break;

            case 86:
                count = 1;

                if (GMInstance.discards > 1)
                {
                    GMInstance.totalAttack *= args[0];
                    count *= args[0];
                }

                Point(pointText, "x" + count, Color.red);
                break;

            case 87:
                count = 0;

                try
                {
                    if (GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalDefense += args[0];
                        count += args[0];
                    }

                    Point(pointText, "+" + count, Color.cyan);
                }
                catch
                {
                    Point(pointText, "+" + count, Color.cyan);
                }
                break;

            case 88:
                count = 0;
                foreach (GameObject card in GMInstance.cardsInAltarfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count++;
                            break;
                        }
                    }
                }

                GMInstance.totalAttack *= count * args[0];
                Point(pointText, "x" + count * args[0], Color.red);
                break;

            case 89:
                count = 0;
                foreach (GameObject card in GMInstance.cardsInAltarfield.ToArray())
                {
                    for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                    {
                        if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[0])
                        {
                            count++;
                            break;
                        }
                    }
                }
                GMInstance.totalAttack += count * args[0];
                Point(pointText, "+" + count * args[0], Color.red);
                break;

            case 90:
                GMInstance.totalDefense *= GMInstance.cardsInMagicfield.Count * args[0];

                Point(pointText, "x" + GMInstance.cardsInMagicfield.Count * args[0], Color.cyan);
                break;

            case 91:
                if (GMInstance.cardsInAltarfield.Count > 0)
                {
                    ran = Random.Range(0, GMInstance.cardsInAltarfield.Count);
                    GMInstance.CardFromAltarfieldToCampfield(GMInstance.cardsInAltarfield[ran]);
                    Point(pointText, "Effect!", Color.yellow);
                }
                else
                {
                    Point(pointText, "Nope!", Color.yellow);
                }
                break;

            case 92:
                if (GMInstance.cardsInShopfield.Contains(zedShadow))
                {
                    zedShadow.SetActive(true);
                    zedShadow.GetComponent<TroopCardDisplay>().BuyCardFromMagic();
                    Point(pointText, "Effect!", Color.yellow);
                    return;
                }

                Point(pointText, "Nope!", Color.yellow);
                break;

            case 93:
                count = GMInstance.skipTime * args[0];
                GMInstance.totalAttack *= count;

                Point(pointText, "x" + count, Color.red);
                break;

            case 94:
                GMInstance.totalAttack *= GMInstance.cardsInSuperfield.Count * args[0];

                Point(pointText, "x" + GMInstance.cardsInSuperfield.Count * args[0], Color.red);
                break;

            case 95:
                count = GMInstance.money / args[0];
                GMInstance.totalAttack *= count;

                Point(pointText, "x" + count, Color.red);
                break;

            case 96: //xatk first pos magic
                count = 1;

                try
                {
                    if (GMInstance.HasBraveHeart(args[1]) || GMInstance.magicfieldTrans.GetChild(args[1]).GetComponent<MagicCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalAttack *= args[0];
                        count *= args[0];
                    }

                    Point(pointText, "x" + count, Color.red);
                }
                catch
                {
                    Point(pointText, "x" + count, Color.red);
                }
                break;

            case 97: //+def first pos magic
                count = 0;

                try
                {
                    if (GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalDefense += args[0];
                        count += args[0];
                    }

                    Point(pointText, "+" + count, Color.cyan);
                }
                catch
                {
                    Point(pointText, "+" + count, Color.cyan);
                }
                break;

            case 98: //set atk first pos
                count = 0;

                try
                {
                    if (GMInstance.HasBraveHeart(lastPos()) || GMInstance.battlefieldTrans.GetChild(lastPos()).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalAttack = args[0];
                        count = args[0];
                        Point(pointText, "=" + count, Color.red);
                        return;
                    }

                    Point(pointText, "Nope!", Color.yellow);
                }
                catch
                {
                    Point(pointText, "Nope!", Color.yellow);
                }
                break;

            case 99: //xdef first pos for "a" type
                count = 1;

                try
                {
                    if (GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        foreach (GameObject card in GMInstance.cardsInBattlefield.ToArray())
                        {
                            for (int i = 2; i < card.GetComponent<TroopCardDisplay>().type.Length; i++)
                            {
                                if (card.GetComponent<TroopCardDisplay>().type[i] == sargs[1])
                                {
                                    count *= args[0];
                                    GMInstance.totalDefense *= args[0];
                                    break;
                                }
                            }
                        }
                    }

                    Point(pointText, "x" + count, Color.cyan);
                }
                catch
                {
                    Point(pointText, "x" + count, Color.cyan);
                }
                break;

            case 100: //xatk last pos
                count = 1;

                try
                {
                    if (GMInstance.HasBraveHeart(lastPos()) || GMInstance.battlefieldTrans.GetChild(lastPos()).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalAttack *= args[0];
                        count *= args[0];
                    }

                    Point(pointText, "x" + count, Color.red);
                }
                catch
                {
                    Point(pointText, "x" + count, Color.red);
                }
                break;

            case 101: //+atk first pos
                count = 0;

                try
                {
                    if (GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalAttack += args[0];
                        count += args[0];
                    }

                    Point(pointText, "+" + count, Color.red);
                }
                catch
                {
                    Point(pointText, "+" + count, Color.red);
                }
                break;

            case 102: //xatk first 3 pos
                count = 1;

                try
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        if (GMInstance.HasBraveHeart(args[i]) || GMInstance.battlefieldTrans.GetChild(args[i]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                        {
                            GMInstance.totalAttack *= args[0];
                            count *= args[0];
                            break;
                        }
                    }

                    Point(pointText, "x" + count, Color.red);
                }
                catch
                {
                    Point(pointText, "x" + count, Color.red);
                }
                break;

            case 103: //*atk last hand & last pos
                count = 1;

                try
                {
                    if (GMInstance.hands == 0 || GMInstance.HasKingCrimson())
                    {
                        if (GMInstance.HasBraveHeart(lastPos()) || GMInstance.battlefieldTrans.GetChild(lastPos()).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                        {
                            GMInstance.totalAttack *= args[0];
                            count *= args[0];
                        }
                    }

                    Point(pointText, "x" + count, Color.red);
                }
                catch
                {
                    Point(pointText, "x" + count, Color.red);
                }
                break;

            case 104: //+atk last pos
                count = 0;

                try
                {
                    if (GMInstance.HasBraveHeart(lastPos()) || GMInstance.battlefieldTrans.GetChild(lastPos()).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalAttack += args[0];
                        count += args[0];
                    }

                    Point(pointText, "+" + count, Color.red);
                }
                catch
                {
                    Point(pointText, "+" + count, Color.red);
                }
                break;

            case 105: //xatk & +b$ first 2 pos
                count = 1;

                try
                {
                    for (int i = 2; i <= 3; i++)
                    {
                        if (GMInstance.HasBraveHeart(args[i]) || GMInstance.battlefieldTrans.GetChild(args[i]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                        {
                            GMInstance.totalAttack *= args[0];
                            GMInstance.AddMoney(args[1]);
                            count *= args[0];
                            Point(pointText, "Effect!", Color.yellow);
                            return;
                        }
                    }

                    Point(pointText, "Nope!", Color.yellow);
                }
                catch
                {
                    Point(pointText, "Nope!", Color.yellow);
                }
                break;

            case 106: //Mountain
                try
                {
                    if (GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.mountainCounter += args[0];
                        mountain.GetComponent<TroopCardDisplay>().ChangeAttack(args[0]);
                        Point(pointText, "Effect!", Color.yellow);
                        return;
                    }

                    Point(pointText, "Nope!", Color.yellow);
                }
                catch
                {
                    Point(pointText, "Nope!", Color.yellow);
                }
                break;

            case 107:
                try
                {
                    if (GMInstance.hands == 0 || GMInstance.HasKingCrimson())
                    {
                        if (GMInstance.HasBraveHeart(lastPos()) || GMInstance.battlefieldTrans.GetChild(lastPos()).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                        {
                            GMInstance.AddMoney(args[0]);
                            Point(pointText, "+" + args[0] + "$", Color.yellow);
                            return;
                        }
                    }

                    Point(pointText, "Nope!", Color.yellow);
                }
                catch
                {
                    Point(pointText, "Nope!", Color.yellow);
                }
                break;

            case 108:
                try
                {
                    bool isFirstOrSecond = false;

                    for (int i = 1; i <= 2; i++)
                    {
                        if (GMInstance.HasBraveHeart(args[i]) || GMInstance.battlefieldTrans.GetChild(args[i]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                        {
                            isFirstOrSecond = true;
                            break;
                        }
                    }

                    if (!isFirstOrSecond)
                    {
                        GMInstance.totalAttack = args[0];
                        Point(pointText, "=" + args[0], Color.red);
                    }
                    else
                    {
                        Point(pointText, "Nope!", Color.yellow);
                    }
                }
                catch
                {
                    Point(pointText, "Nope!", Color.yellow);
                }
                break;

            case 109:
                Point(pointText, "Effect!", Color.yellow);
                break;

            case 110:
                try
                {
                    if (GMInstance.HasBraveHeart(args[0]) || GMInstance.battlefieldTrans.GetChild(args[0]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        if (GMInstance.cardsInShopfield.Contains(mon3tr))
                        {
                            mon3tr.SetActive(true);
                            mon3tr.GetComponent<TroopCardDisplay>().BuyCardFromMagic();
                            Point(pointText, "Effect!", Color.yellow);
                            return;
                        }

                        Point(pointText, "Nope!", Color.yellow);
                    }
                }
                catch
                {
                    Point(pointText, "Nope!", Color.yellow);
                }
                break;

            case 111:
                count = 0;

                try
                {
                    if (GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.battlefieldTrans.GetChild(args[0]).GetComponent<TroopCardDisplay>().Effect();
                    }
                }
                catch
                {
                }
                break;

            case 112: //xa atk first pos
                count = 1;

                try
                {
                    if (GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalAttack *= args[0];
                        count *= args[0];
                    }

                    Point(pointText, "x" + count, Color.red);
                }
                catch
                {
                    Point(pointText, "x" + count, Color.red);
                }
                break;

            case 113:
                count = 0;

                try
                {
                    if ((GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0]) && (GMInstance.HasBraveHeart(args[2]) || GMInstance.battlefieldTrans.GetChild(args[2]).GetComponent<TroopCardDisplay>().card.name == sargs[0]))
                    {
                        GMInstance.totalAttack += args[0];
                        GMInstance.totalAttack *= args[3];
                        Point(pointText, "Effect!", Color.yellow);
                        return;
                    }
                    else if (GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalAttack += args[0];
                        count += args[0];
                        Point(pointText, "+" + count, Color.red);
                        return;
                    }
                    else if (GMInstance.HasBraveHeart(args[2]) || GMInstance.battlefieldTrans.GetChild(args[2]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        count = 1;
                        GMInstance.totalAttack *= args[3];
                        count *= args[3];
                        Point(pointText, "x" + count, Color.red);
                        return;
                    }

                    Point(pointText, "Nope!", Color.yellow);
                }
                catch
                {
                    Point(pointText, "Nope!", Color.yellow);
                }
                break;

            case 114: //+def first pos
                count = 0;

                try
                {
                    if (GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalDefense += args[0];
                        count += args[0];
                    }

                    Point(pointText, "+" + count, Color.cyan);
                }
                catch
                {
                    Point(pointText, "+" + count, Color.cyan);
                }
                break;

            case 115:
                count = 0;

                try
                {
                    if ((GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0]) && (GMInstance.HasBraveHeart(args[2]) || GMInstance.battlefieldTrans.GetChild(args[2]).GetComponent<TroopCardDisplay>().card.name == sargs[0]))
                    {
                        GMInstance.totalAttack += args[0];
                        GMInstance.totalAttack += args[3];
                        Point(pointText, "Effect!", Color.yellow);
                        return;
                    }
                    else if (GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalAttack += args[0];
                        count += args[0];
                        Point(pointText, "+" + count, Color.red);
                        return;
                    }
                    else if (GMInstance.HasBraveHeart(args[2]) || GMInstance.battlefieldTrans.GetChild(args[2]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalAttack += args[3];
                        count += args[3];
                        Point(pointText, "+" + count, Color.red);
                        return;
                    }

                    Point(pointText, "Nope!", Color.yellow);
                }
                catch
                {
                    Point(pointText, "Nope!", Color.yellow);
                }
                break;

            case 116:
                count = 0;

                try
                {
                    if (GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalAttack = args[0];
                        count = args[0];
                        Point(pointText, "=" + count, Color.red);
                        return;
                    }
                    if (GMInstance.HasBraveHeart(lastPos()) || GMInstance.battlefieldTrans.GetChild(lastPos()).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        GMInstance.totalAttack = args[0];
                        count = args[0];
                        Point(pointText, "=" + count, Color.red);
                        return;
                    }

                    Point(pointText, "Nope!", Color.yellow);
                }
                catch
                {
                    Point(pointText, "Nope!", Color.yellow);
                }
                break;

            case 117:
                count = 1;

                try
                {
                    bool isCheck = true;

                    if (GMInstance.HasBraveHeart(args[1]) || GMInstance.battlefieldTrans.GetChild(args[1]).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        isCheck = false;
                    }
                    if (GMInstance.HasBraveHeart(lastPos()) || GMInstance.battlefieldTrans.GetChild(lastPos()).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                    {
                        isCheck = false;
                    }

                    if (isCheck)
                    {
                        GMInstance.totalAttack *= args[0];
                        count *= args[0];
                        Point(pointText, "x" + count, Color.red);
                        return;
                    }

                    Point(pointText, "Nope!", Color.yellow);
                }
                catch
                {
                    Point(pointText, "Nope!", Color.yellow);
                }
                break;

            case 118:
                count = 1;

                try
                {
                    for (int i = args[1]; i < GMInstance.battlefieldTrans.childCount; i++)
                    {
                        if (GMInstance.HasBraveHeart(i) || GMInstance.battlefieldTrans.GetChild(i).GetComponent<TroopCardDisplay>().card.name == sargs[0])
                        {
                            GMInstance.totalAttack *= args[0];
                            count *= args[0];
                            Point(pointText, "x" + count, Color.red);
                            return;
                        }
                    }

                    Point(pointText, "x" + count, Color.red);
                }
                catch
                {
                    Point(pointText, "x" + count, Color.red);
                }
                break;
        }
    }

    private int lastPos()
    {
        return GMInstance.battlefieldTrans.childCount - 1;
    }
}
