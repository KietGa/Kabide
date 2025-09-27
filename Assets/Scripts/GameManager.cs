using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static BossManager;
using static EffectManager;

public class GameManager : MonoBehaviour
{
    public static GameManager GMInstance {  get; private set; }
    public List<GameObject> cardsInBattlefield;
    public List<GameObject> cardsInCampfield;
    public List<GameObject> cardsInAltarfield;
    public List<GameObject> cardsInMagicfield;
    public List<GameObject> cardsInSuperfield;
    public List<GameObject> cardsInHandfield;
    public List<GameObject> cardsInShopfield;
    public List<GameObject> cardsInPackfield;
    public List<GameObject> cardsInShopPackfield;
    public List<GameObject> cardsInIdolfield;
    public List<GameObject> cardsInShopIdolfield;
    [SerializeField] private GameObject troopCardPrefab;
    [SerializeField] private GameObject magicCardPrefab;
    [SerializeField] private GameObject superCardPrefab;
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private Image bg;
    [SerializeField] private Sprite[] bgSprites;
    public Transform battlefieldTrans;
    public Transform altarfieldTrans;
    public Transform handfieldTrans;
    public Transform magicfieldTrans;
    public Transform superfieldTrans;
    public Transform campfieldTrans;
    public Transform shopfieldTrans;
    public Transform blessfieldTrans;
    public Transform packfieldTrans;
    public Transform shopPackfieldTrans;
    public Transform shopIdolfieldTrans;
    public Transform idolfieldTrans;
    [SerializeField] private TextMeshProUGUI moneyText;
    public TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI totalDefenseText;
    [SerializeField] private TextMeshProUGUI totalAttackText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI bossScoreText;
    [SerializeField] private TextMeshProUGUI handText;
    [SerializeField] private TextMeshProUGUI discardText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private TextMeshProUGUI superCountText;
    [SerializeField] private TextMeshProUGUI magicCountText;
    [SerializeField] private TextMeshProUGUI handsizeCountText;
    [SerializeField] private TextMeshProUGUI battleCountText;
    [SerializeField] private TextMeshProUGUI blessCountText;
    [SerializeField] private TextMeshProUGUI seedText;
    [SerializeField] private TextMeshProUGUI infoText;
    public TextMeshProUGUI debugText;
    [SerializeField] private GameObject choosenObject;
    [SerializeField] private GameObject bossObject;
    [SerializeField] private GameObject shopObject;
    [SerializeField] private GameObject skipPackBtn;
    [SerializeField] private GameObject endMatchPanel;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject winGamePanel;
    [SerializeField] private GameObject battlePanel;
    [SerializeField] private GameObject otherfield;
    [SerializeField] private GameObject infofield;
    [SerializeField] private GameObject closeScoutBtn;
    [SerializeField] private Button battleBtn;
    [SerializeField] private Button discardBtn;
    public Button rerollBtn;
    public int state;
    public int money;
    public int maxRerollMoney = 5;
    public int maxHandSize = 5;
    public int maxHands = 5;
    public int maxDiscards = 5;
    public int maxTroopInBattlefield = 5;
    public int maxMagicInMagicfield = 5;
    public int maxSuperInSuperfield = 3;
    public int maxCardInShopPackfield = 2;
    public int maxCardInShop = 3;
    public int turn { get; set; }
    public int dturn { get; set; }

    public int hands { get; set; }
    public int discards { get; set; }

    public BigInteger totalAttack { get; set; }
    public BigInteger totalDefense { get; set; }
    public int baseDefense;
    public int baseAttack;
    public BigInteger totalScore { get; set; }
    public bool isBattle { get; set; }
    public bool isCount { get; set; }
    public int skipTime { get; set; }
    public int battleTime { get; set; }
    public int artifactSellValue { get; set; }
    public int tuskCounter { get; set; }
    public int heartSteelCounter { get; set; }
    public int soloLevelingCounter { get; set; }
    public int kaynCounter { get; set; }
    public int fireflyCounter { get; set; }
    public int ahriCounter { get; set; }
    public int xiaoCounter { get; set; }
    public int hutaoATKCounter { get; set; }
    public int hutaoDEFCounter { get; set; }
    public int veigarCounter { get; set; }
    public int qiqiCounter { get; set; }
    public int nasusCounter { get; set; }
    public int mountainCounter { get; set; }
    public int rerollMoney { get; set; }
    public int handSize { get; set; }
    public bool isEnd { get; set; }
    public long tempAttack { get; set; }
    public long tempDefense { get; set; }
    public int numberOfDiscard { get; set; }
    public int numberOfAdd { get; set; }
    public int numberOfDestroy { get; set; }
    public int currentBattlefield { get; set; }
    public int seed { get; set; }
    public int bless { get; set; }

    public bool isSellMagic { get; set; }
    public int battleBonus { get; set; }
    public int skipBonus { get; set; }

    private float playedTime;

    private void Awake()
    {
        GMInstance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("save") == 0)
        {
            seed = PlayerPrefs.GetInt("seed");
            Random.InitState(seed);
            seedText.text = "Seed: " + seed;
            if (PlayerPrefs.GetInt("cheat") == 1)
            {
                money = 9999;
                seedText.text = "Seed: CC" + seed;
            }

            foreach (Transform card in shopfieldTrans)
            {
                cardsInShopfield.Add(card.gameObject);
            }

            foreach (Transform card in shopIdolfieldTrans)
            {
                cardsInShopIdolfield.Add(card.gameObject);
            }

            foreach (Transform card in shopPackfieldTrans)
            {
                cardsInShopPackfield.Add(card.gameObject);
            }

            GMInstance.rerollBtn.onClick.AddListener(() =>
            {
                GMInstance.RerollShop();
            });
            ChangeHand(1, maxHands);
            ChangeDiscard(1, maxDiscards);
            handSize = maxHandSize;

            Blessing();
            FreeRerollShop();
            RerollPack();
            ToShop();
        }
        else if (PlayerPrefs.GetInt("save") == 1)
        {
            foreach (Transform c in shopfieldTrans)
            {
                Destroy(c.gameObject);
            }

            foreach (Transform c in shopIdolfieldTrans)
            {
                Destroy(c.gameObject);
            }

            foreach (Transform c in shopPackfieldTrans)
            {
                Destroy(c.gameObject);
            }

            GMInstance.rerollBtn.onClick.AddListener(() =>
            {
                GMInstance.RerollShop();
            });

            
            bless = PlayerPrefs.GetInt("bless");
            blessfieldTrans.GetChild(bless).gameObject.SetActive(true);
            isSellMagic = PlayerPrefs.GetInt("isSellMagic") == 1;
            money = PlayerPrefs.GetInt("money");
            AddMoney(0);
            BMInstance.stage = PlayerPrefs.GetInt("stage");
            AudioManager.AMInstance.PlayBGM();
            stageText.text = "Stage " + BMInstance.stage;
            maxHands = PlayerPrefs.GetInt("maxHands");
            maxDiscards = PlayerPrefs.GetInt("maxDiscards");
            maxCardInShop = PlayerPrefs.GetInt("maxCardInShop");
            maxHandSize = PlayerPrefs.GetInt("maxHandSize");
            maxMagicInMagicfield = PlayerPrefs.GetInt("maxMagicInMagicfield");
            maxRerollMoney = PlayerPrefs.GetInt("maxRerollMoney");
            maxSuperInSuperfield = PlayerPrefs.GetInt("maxSuperInSuperfield");
            maxTroopInBattlefield = PlayerPrefs.GetInt("maxTroopInBattlefield");
            turn = PlayerPrefs.GetInt("turn");
            dturn = PlayerPrefs.GetInt("dturn");
            hands = PlayerPrefs.GetInt("hands");
            ChangeHand(0, 0);
            discards = PlayerPrefs.GetInt("discards");
            battleBonus = PlayerPrefs.GetInt("battleBonus");
            skipBonus = PlayerPrefs.GetInt("skipBonus");
            ChangeDiscard(0, 0);
            isBattle = PlayerPrefs.GetInt("isBattle") == 1;
            isCount = PlayerPrefs.GetInt("isCount") == 1; 
            skipTime = PlayerPrefs.GetInt("skipTime");
            battleTime = PlayerPrefs.GetInt("battleTime");
            artifactSellValue = PlayerPrefs.GetInt("artifactSellValue");
            rerollMoney = PlayerPrefs.GetInt("rerollMoney");
            rerollBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Reroll - " + rerollMoney + "$";
            handSize = PlayerPrefs.GetInt("handSize");
            isEnd = PlayerPrefs.GetInt("isEnd") == 1;
            numberOfDiscard = PlayerPrefs.GetInt("numberOfDiscard");
            numberOfAdd = PlayerPrefs.GetInt("numberOfAdd");
            numberOfDestroy = PlayerPrefs.GetInt("numberOfDestroy");
            totalAttack = BigInteger.Parse(PlayerPrefs.GetString("totalAttack"));
            totalDefense = BigInteger.Parse(PlayerPrefs.GetString("totalDefense"));
            totalScore = BigInteger.Parse(PlayerPrefs.GetString("totalScore"));
            tempAttack = long.Parse(PlayerPrefs.GetString("tempAttack"));
            tempDefense = long.Parse(PlayerPrefs.GetString("tempDefense"));
            baseAttack = PlayerPrefs.GetInt("baseAttack");
            baseDefense = PlayerPrefs.GetInt("baseDefense");
            state = PlayerPrefs.GetInt("state");
            kaynCounter = PlayerPrefs.GetInt("kaynCounter");
            fireflyCounter = PlayerPrefs.GetInt("fireflyCounter");
            tuskCounter = PlayerPrefs.GetInt("tuskCounter");
            ahriCounter = PlayerPrefs.GetInt("ahriCounter");
            xiaoCounter = PlayerPrefs.GetInt("xiaoCounter");
            qiqiCounter = PlayerPrefs.GetInt("qiqiCounter");
            nasusCounter = PlayerPrefs.GetInt("nasusCounter");
            veigarCounter = PlayerPrefs.GetInt("veigarCounter");
            hutaoDEFCounter = PlayerPrefs.GetInt("hutaoDEFCounter");
            hutaoATKCounter = PlayerPrefs.GetInt("hutaoATKCounter");
            mountainCounter = PlayerPrefs.GetInt("mountainCounter");
            soloLevelingCounter = PlayerPrefs.GetInt("soloLevelingCounter");
            heartSteelCounter = PlayerPrefs.GetInt("heartSteelCounter");
            currentBattlefield = PlayerPrefs.GetInt("currentBattlefield");

            switch (state)
            {
                case 0:
                    ToShop();
                    break;

                case 1:
                    ToChoosen();
                    break;

                case 2:
                    ToChoosen();
                    ToBossSave();
                    if (!isBattle)
                    {
                        endMatchPanel.SetActive(true);
                        battlePanel.SetActive(false);
                        rewardText.text = PlayerPrefs.GetInt("currentBossReward") + "$";
                    }
                    break;

                case 3:
                    ToScout();
                    break;
            }

            int counting = 0;
            GameObject card = null;

            while (PlayerPrefs.GetString(counting + "", "NONE") != "NONE")
            {
                string cardInfo = PlayerPrefs.GetString(counting + "");
                string cardName = cardInfo.Remove(0, 3);

                switch (cardInfo[0])
                {
                    case 'T':
                        card = Instantiate(Resources.Load<GameObject>("TroopCard"), shopfieldTrans);
                        card.name = cardName;
                        card.GetComponent<TroopCardDisplay>().card = Resources.Load<TroopCard>("Cards/Troops/Old/" + cardName);
                        break;

                    case 'M':
                        card = Instantiate(Resources.Load<GameObject>("MagicCard"), shopfieldTrans);
                        card.name = cardName;
                        card.GetComponent<MagicCardDisplay>().card = Resources.Load<MagicCard>("Cards/Magic/Old/" + cardName);
                        break;

                    case 'S':
                        card = Instantiate(Resources.Load<GameObject>("SuperCard"), shopfieldTrans);
                        card.name = cardName;
                        card.GetComponent<SuperCardDisplay>().card = Resources.Load<SuperCard>("Cards/Super/Old/" + cardName);
                        break;

                    case 'G':
                        card = Instantiate(Resources.Load<GameObject>("GachaCard"), shopPackfieldTrans);
                        card.name = cardName;
                        card.GetComponent<GachaCardDisplay>().card = Resources.Load<GachaCard>("Cards/Gacha/Old/" + cardName);
                        break;

                    case 'I':
                        card = Instantiate(Resources.Load<GameObject>("IdolCard"), shopIdolfieldTrans);
                        card.name = cardName;
                        card.GetComponent<IdolCardDisplay>().card = Resources.Load<IdolCard>("Cards/Idol/Old/" + cardName);
                        break;
                }

                switch (cardInfo[1])
                {
                    case '1':
                        card.transform.SetParent(altarfieldTrans);
                        cardsInAltarfield.Add(card.gameObject);
                        break;

                    case '2':
                        card.transform.SetParent(battlefieldTrans);
                        cardsInBattlefield.Add(card.gameObject);
                        break;

                    case '3':
                        card.transform.SetParent(campfieldTrans);
                        cardsInCampfield.Add(card.gameObject);
                        break;

                    case '4':
                        card.transform.SetParent(handfieldTrans);
                        cardsInHandfield.Add(card.gameObject);
                        break;

                    case '5':
                        card.transform.SetParent(idolfieldTrans);
                        cardsInIdolfield.Add(card.gameObject);
                        break;

                    case '6':
                        card.transform.SetParent(magicfieldTrans);
                        cardsInMagicfield.Add(card.gameObject);
                        break;

                    case '7':
                        card.transform.SetParent(packfieldTrans);
                        cardsInPackfield.Add(card.gameObject);
                        break;

                    case '8':
                        card.transform.SetParent(shopfieldTrans);
                        cardsInShopfield.Add(card.gameObject);
                        break;

                    case '9':
                        card.transform.SetParent(shopIdolfieldTrans);
                        cardsInShopIdolfield.Add(card.gameObject);
                        break;

                    case 'A':
                        card.transform.SetParent(shopPackfieldTrans);
                        cardsInShopPackfield.Add(card.gameObject);
                        break;

                    case 'B':
                        card.transform.SetParent(superfieldTrans);
                        cardsInSuperfield.Add(card.gameObject);
                        break;
                }

                switch (cardInfo[2])
                {
                    case 'A':
                        card.SetActive(true);
                        break;

                    case 'U':
                        card.SetActive(false);
                        break;
                }

                card.GetComponent<CardDisplay>().Init();
                SetObject(cardName, card);
                counting++;
            }

            EMInstance.heartSteel.GetComponent<MagicCardDisplay>().effectText.text = "+" + EMInstance.heartSteel.GetComponent<MagicCardDisplay>().card.args[0] + " DEF per played\n(Currently: +" + GMInstance.heartSteelCounter + " DEF)";
            EMInstance.soloLeveling.GetComponent<MagicCardDisplay>().effectText.text = "+" + EMInstance.soloLeveling.GetComponent<MagicCardDisplay>().card.args[0] + " ATK per played\n(Currently: +" + GMInstance.soloLevelingCounter + " ATK)";
            int count = GMInstance.numberOfDestroy * EMInstance.deathNote.GetComponent<MagicCardDisplay>().card.args[0] + 1;
            EMInstance.deathNote.GetComponent<MagicCardDisplay>().effectText.text = "Gain x" + EMInstance.deathNote.GetComponent<MagicCardDisplay>().card.args[0] + " ATK after Destroy a troop card\n(Currently: x" + count + " ATK)";
            count = GMInstance.numberOfAdd / EMInstance.friend.GetComponent<MagicCardDisplay>().card.args[0] + 1;
            EMInstance.friend.GetComponent<MagicCardDisplay>().effectText.text = "Gain x1 ATK after Add " + EMInstance.friend.GetComponent<MagicCardDisplay>().card.args[0] + " cards\n(Currently: x" + count + " ATK)";
            count = GMInstance.numberOfDiscard / EMInstance.anihilate.GetComponent<MagicCardDisplay>().card.args[0] + 1;
            EMInstance.anihilate.GetComponent<MagicCardDisplay>().effectText.text = "Gain x1 ATK after Discard " + EMInstance.anihilate.GetComponent<MagicCardDisplay>().card.args[0] + " troop cards\n (Currently: x" + count + " ATK)";
            count = GMInstance.battleTime / EMInstance.challenger.GetComponent<MagicCardDisplay>().card.args[0] + 1;
            EMInstance.challenger.GetComponent<MagicCardDisplay>().effectText.text = "Gain x" + EMInstance.challenger.GetComponent<MagicCardDisplay>().card.args[0] + " ATK after challenge\n (Currently: x" + count + " ATK)";
            count = GMInstance.skipTime / EMInstance.coward.GetComponent<MagicCardDisplay>().card.args[0] + 1;
            EMInstance.coward.GetComponent<MagicCardDisplay>().effectText.text = "Gain x" + EMInstance.coward.GetComponent<MagicCardDisplay>().card.args[0] + " ATK after challenge\n (Currently: x" + count + " ATK)";
            EMInstance.yoshikageKira.GetComponent<TroopCardDisplay>().SetAttack(EMInstance.yoshikageKira.GetComponent<TroopCardDisplay>().card.args[0] * numberOfDestroy);
            EMInstance.ahri.GetComponent<TroopCardDisplay>().SetAttack(ahriCounter);
            EMInstance.xiao.GetComponent<TroopCardDisplay>().SetDefense(xiaoCounter);
            EMInstance.hutao.GetComponent<TroopCardDisplay>().SetAttack(hutaoATKCounter);
            EMInstance.hutao.GetComponent<TroopCardDisplay>().SetAttack(hutaoDEFCounter);
            EMInstance.mountain.GetComponent<TroopCardDisplay>().SetAttack(mountainCounter);
            EMInstance.nasus.GetComponent<TroopCardDisplay>().SetAttack(nasusCounter);
            EMInstance.veigar.GetComponent<TroopCardDisplay>().SetAttack(veigarCounter);
            EMInstance.qiqi.GetComponent<TroopCardDisplay>().SetAttack(qiqiCounter);
            CalculateSpacingAltarSize();
            CalculateSpacingBattlefield();
            CalculateSpacingCampSize();
            CalculateSpacingHandSize();
            CalculateSpacingIdolSize();
            CalculateSpacingMagicfield();
            CalculateSpacingSuperSize();

            if (cardsInPackfield.Count != 0)
            {
                packfieldTrans.gameObject.SetActive(true);
                shopfieldTrans.gameObject.SetActive(false);
                shopIdolfieldTrans.gameObject.SetActive(false);
                shopPackfieldTrans.gameObject.SetActive(false);
                skipPackBtn.SetActive(true);
            }

            seed = PlayerPrefs.GetInt("seed");
            SaveState.SSInstance.LoadSeed();
            seedText.text = "Seed: " + seed;
            if (PlayerPrefs.GetInt("cheat") == 1)
            {
                seedText.text = "Seed: CC" + seed;
            }
        }
    }

    private void SetObject(string objectName, GameObject theObject)
    {
        switch (objectName)
        {
            case "Soft And Wet":
                EMInstance.softAndWet = theObject;
                break;

            case "Soft And Wet Go Beyond":
                EMInstance.softAndWet = theObject;
                break;

            case "Annihilate":
                EMInstance.anihilate = theObject;
                break;

            case "Artifact":
                EMInstance.artifact = theObject;
                break;

            case "Blue Mirror":
                EMInstance.blueMirror = theObject;
                break;

            case "Qiqi":
                EMInstance.qiqi = theObject;
                break;

            case "Convenience Store":
                EMInstance.convenienceStore = theObject;
                break;

            case "Heartsteel":
                EMInstance.heartSteel = theObject;
                break;

            case "Credit Card":
                EMInstance.creditCard = theObject;
                break;

            case "Death Note":
                EMInstance.deathNote = theObject;
                break;

            case "Friend":
                EMInstance.friend = theObject;
                break;

            case "God Bless":
                EMInstance.godBless = theObject;
                break;

            case "Gold Experience":
                EMInstance.goldExperience = theObject;
                break;

            case "King Crimson":
                EMInstance.kingCrimson = theObject;
                break;

            case "Herta":
                EMInstance.herta = theObject;
                break;

            case "Magic Mirror":
                EMInstance.magicMirror = theObject;
                break;

            case "Solo Leveling":
                EMInstance.soloLeveling = theObject;
                break;

            case "Yoshikage Kira":
                EMInstance.yoshikageKira = theObject;
                break;

            case "Yone":
                EMInstance.yone = theObject;
                break;

            case "Yone After Death":
                EMInstance.yone = theObject;
                break;

            case "Nope":
                EMInstance.nope = theObject;
                break;

            case "Ahri":
                if (theObject.GetComponent<TroopCardDisplay>())
                {
                    EMInstance.ahri = theObject;
                }
                break;

            case "Xiao":
                EMInstance.xiao = theObject;
                break;

            case "Hu Tao":
                EMInstance.hutao = theObject;
                break;

            case "Veigar":
                EMInstance.veigar = theObject;
                break;

            case "Coward":
                EMInstance.coward = theObject;
                break;

            case "Challenger":
                EMInstance.challenger = theObject;
                break;

            case "Negative":
                EMInstance.negative = theObject;
                break;

            case "Tusk":
                EMInstance.tusk = theObject;
                break;

            case "Tusk Act II":
                EMInstance.tusk = theObject;
                break;

            case "Tusk Act III":
                EMInstance.tusk = theObject;
                break;

            case "Tusk Act IV":
                EMInstance.tusk = theObject;
                break;

            case "Nasus":
                EMInstance.nasus = theObject;
                break;

            case "Zed Shadow":
                EMInstance.zedShadow = theObject;
                break;

            case "Brave Heart":
                EMInstance.braveHeart = theObject;
                break;

            case "Lonely":
                EMInstance.lonely = theObject;
                break;

            case "VPN":
                EMInstance.vpn = theObject;
                break;

            case "Emergency":
                EMInstance.emergency = theObject;
                break;

            case "Mountain":
                EMInstance.mountain = theObject;
                break;

            case "Mon3tr":
                EMInstance.mon3tr = theObject;
                break;

            case "Crownslayer":
                EMInstance.crownslayer = theObject;
                break;
        }
    }

    private void Update()
    {
        playedTime += Time.deltaTime;
        magicCountText.text = cardsInMagicfield.Count + "/" + maxMagicInMagicfield;
        superCountText.text = cardsInSuperfield.Count + "/" + maxSuperInSuperfield;
        if (isBattle)
        {
            battleCountText.text = cardsInBattlefield.Count + "/" + currentBattlefield;
            handsizeCountText.text = cardsInHandfield.Count + "/" + handSize;
        }

        /*
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
        }
        */
    }

    private void Blessing()
    {
        bless = PlayerPrefs.GetInt("bless");
        blessfieldTrans.GetChild(bless).gameObject.SetActive(true);
        switch (bless)
        {
            case 0:
                baseDefense += 100;
                baseAttack += 100;
                break; 

            case 1:
                AddMoney(15);
                break;

            case 2:
                baseDefense += 400;
                break;

            case 3:
                baseAttack += 400;
                break;

            case 4:
                maxHandSize += 2;
                break;

            case 5:
                maxHands += 2;
                break;

            case 6:
                maxDiscards += 2;
                break;

            case 7:
                maxMagicInMagicfield += 1;
                break;

            case 8:
                battleBonus++;
                break;

            case 9:
                maxTroopInBattlefield += 1;
                currentBattlefield = maxTroopInBattlefield;
                break;

            case 10:
                GMInstance.maxCardInShop++;
                GMInstance.shopfieldTrans.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(GMInstance.shopfieldTrans.GetComponent<RectTransform>().rect.width + 160, 220);
                break;

            case 11:
                maxHands++;
                maxDiscards++;
                break;

            case 12:
                GMInstance.maxRerollMoney -= 3;
                break;

            case 13:
                skipBonus += 2;
                break;

            case 14:
                GMInstance.maxSuperInSuperfield++;
                EffectManager.EMInstance.vpn.GetComponent<SuperCardDisplay>().BuyCardFromMagic();
                EffectManager.EMInstance.emergency.GetComponent<SuperCardDisplay>().BuyCardFromMagic();
                break;

            case 15:
                break;

            default:
                break;
        }
    }

    public void CardFromHandfieldToBattlefield(GameObject status)
    {
        status.transform.SetParent(battlefieldTrans);
        cardsInBattlefield.Add(status);
        cardsInHandfield.Remove(status);
        CoutingStatus();
        CalculateSpacingBattlefield();
    }

    public void CardFromBattlefieldToHandfield(GameObject status)
    {
        status.transform.SetParent(handfieldTrans);
        cardsInHandfield.Add(status);
        cardsInBattlefield.Remove(status);
        CoutingStatus();
        CalculateSpacingBattlefield();
    }

    public void CardFromBattlefieldToAltarfield(GameObject status)
    {
        status.transform.SetParent(altarfieldTrans);
        cardsInAltarfield.Add(status);
        cardsInBattlefield.Remove(status);
        status.GetComponent<TroopCardDisplay>().AltarEffect();
        EMInstance.SpecialEffect(3, EMInstance.anihilate.GetComponent<MagicCardDisplay>().card.args, null);
        CalculateSpacingAltarSize();
        CalculateSpacingBattlefield();
    }

    public void CardFromBattlefieldToShopfield(GameObject status)
    {
        GMInstance.numberOfDestroy++;
        status.GetComponent<TroopCardDisplay>().DestroyCard();
        status.transform.SetParent(shopfieldTrans);
        cardsInShopfield.Add(status);
        cardsInBattlefield.Remove(status);
        CalculateSpacingBattlefield();
    }

    public void CardFromAltarfieldToCampfield(GameObject status)
    {
        status.transform.SetParent(campfieldTrans);
        cardsInCampfield.Add(status);
        cardsInAltarfield.Remove(status);
        CalculateSpacingCampSize();
        CalculateSpacingAltarSize();
    }

    public void CardFromAltarfieldToShopfield(GameObject status)
    {
        GMInstance.numberOfDestroy++;
        status.GetComponent<TroopCardDisplay>().DestroyCard();
        status.transform.SetParent(shopfieldTrans);
        cardsInShopfield.Add(status);
        cardsInAltarfield.Remove(status);
        CalculateSpacingAltarSize();
    }

    public void CardFromAltarfieldToHandfield(GameObject status)
    {
        status.transform.SetParent(handfieldTrans);
        cardsInHandfield.Add(status);
        cardsInAltarfield.Remove(status);
        CalculateSpacingHandSize();
        CalculateSpacingAltarSize();
    }

    public void CardFromCampfieldToHandfield(GameObject status)
    {
        status.transform.SetParent(handfieldTrans);
        cardsInHandfield.Add(status);
        cardsInCampfield.Remove(status);
        CalculateSpacingHandSize();
        CalculateSpacingCampSize();
    }

    public void CardFromHandfieldToAltarfield(GameObject status)
    {
        status.transform.SetParent(altarfieldTrans);
        cardsInAltarfield.Add(status);
        cardsInHandfield.Remove(status);
        CalculateSpacingHandSize();
        status.GetComponent<TroopCardDisplay>().AltarEffect();
        CalculateSpacingAltarSize();
    }

    public void CardFromHandfieldToCampfield(GameObject status)
    {
        status.transform.SetParent(campfieldTrans);
        cardsInCampfield.Add(status);
        cardsInHandfield.Remove(status);
        CalculateSpacingHandSize();
        CalculateSpacingCampSize();
    }

    public void CardFromShopfieldToCampfield(GameObject status)
    {
        status.transform.SetParent(campfieldTrans);
        cardsInCampfield.Add(status);
        cardsInShopfield.Remove(status);
        CalculateSpacingCampSize();
        numberOfAdd++;
        EMInstance.SpecialEffect(1, EMInstance.friend.GetComponent<MagicCardDisplay>().card.args, null);
    }

    public void CardFromPackfieldToCampfield(GameObject status)
    {
        status.transform.SetParent(campfieldTrans);
        cardsInCampfield.Add(status);
        cardsInPackfield.Remove(status);
        CalculateSpacingCampSize();
    }

    public void CardFromPackfieldToIdolfield(GameObject status)
    {
        status.transform.SetParent(idolfieldTrans);
        cardsInIdolfield.Add(status);
        cardsInPackfield.Remove(status);
        numberOfAdd++;
        EMInstance.SpecialEffect(1, EMInstance.friend.GetComponent<MagicCardDisplay>().card.args, null);
        CalculateSpacingIdolSize();
    }

    public void CardFromPackfieldToShopIdolfield(GameObject status)
    {
        status.SetActive(false);
        status.transform.SetParent(shopIdolfieldTrans);
        cardsInShopIdolfield.Add(status);
        cardsInPackfield.Remove(status);
    }

    public void CardFromShopIdolfieldToIdolfield(GameObject status)
    {
        status.transform.SetParent(idolfieldTrans);
        cardsInIdolfield.Add(status);
        cardsInShopIdolfield.Remove(status);
        CalculateSpacingIdolSize();
        numberOfAdd++;
        EMInstance.SpecialEffect(1, EMInstance.friend.GetComponent<MagicCardDisplay>().card.args, null);
    }

    public void CardFromCampfieldToShopfield(GameObject status)
    {
        GMInstance.numberOfDestroy++;
        status.GetComponent<TroopCardDisplay>().DestroyCard();
        status.transform.SetParent(shopfieldTrans);
        cardsInShopfield.Add(status);
        cardsInCampfield.Remove(status);
        CalculateSpacingCampSize();
    }

    public void CardFromShopfieldToMagicfield(GameObject status)
    {
        status.transform.SetParent(magicfieldTrans);
        cardsInMagicfield.Add(status);
        cardsInShopfield.Remove(status);
        CalculateSpacingMagicfield();
        numberOfAdd++;
        EMInstance.SpecialEffect(1, EMInstance.friend.GetComponent<MagicCardDisplay>().card.args, null);
    }

    public void CardFromPackfieldToMagicfield(GameObject status)
    {
        status.transform.SetParent(magicfieldTrans);
        cardsInMagicfield.Add(status);
        cardsInPackfield.Remove(status);
        CalculateSpacingMagicfield();
        numberOfAdd++;
        EMInstance.SpecialEffect(1, EMInstance.friend.GetComponent<MagicCardDisplay>().card.args, null);
    }

    public void CardFromMagicfieldToShopfield(GameObject status)
    {
        status.SetActive(false);
        status.transform.SetParent(shopfieldTrans);
        cardsInShopfield.Add(status);
        cardsInMagicfield.Remove(status);
        CalculateSpacingMagicfield();
    }

    public void CardFromShopfieldToPackfield(GameObject status)
    {
        status.transform.SetParent(packfieldTrans);
        cardsInPackfield.Add(status);
        cardsInShopfield.Remove(status);
        numberOfAdd++;
        EMInstance.SpecialEffect(1, EMInstance.friend.GetComponent<MagicCardDisplay>().card.args, null);
    }

    public void CardFromShopIdolfieldToPackfield(GameObject status)
    {
        status.transform.SetParent(packfieldTrans);
        cardsInPackfield.Add(status);
        cardsInShopIdolfield.Remove(status);
    }

    public void CardFromPackfieldToShopfield(GameObject status)
    {
        status.SetActive(false);
        status.transform.SetParent(shopfieldTrans);
        cardsInShopfield.Add(status);
        cardsInPackfield.Remove(status);
    }

    public void CardFromShopfieldToSuperfield(GameObject status)
    {
        status.transform.SetParent(superfieldTrans);
        cardsInSuperfield.Add(status);
        cardsInShopfield.Remove(status);
        CalculateSpacingSuperSize();
        numberOfAdd++;
        EMInstance.SpecialEffect(1, EMInstance.friend.GetComponent<MagicCardDisplay>().card.args, null);
    }

    public void CardFromPackfieldToSuperfield(GameObject status)
    {
        status.transform.SetParent(superfieldTrans);
        cardsInSuperfield.Add(status);
        cardsInPackfield.Remove(status);
        CalculateSpacingSuperSize();
        numberOfAdd++;
        EMInstance.SpecialEffect(1, EMInstance.friend.GetComponent<MagicCardDisplay>().card.args, null);
    }

    public void CardFromSuperfieldToShopfield(GameObject status)
    {
        status.SetActive(false);
        status.transform.SetParent(shopfieldTrans);
        cardsInShopfield.Add(status);
        cardsInSuperfield.Remove(status);
        CalculateSpacingSuperSize();
    }

    public void CoutingStatus()
    {
        if (BMInstance.bossEffect == 2 && !HasNope() && BMInstance.currentBossScore == BMInstance.mainBossScore)
        {
            totalAttack = 0;
            totalDefense = 0;
        }
        else
        {
            totalAttack = baseAttack + tempAttack;
            totalDefense = baseDefense + tempDefense;
        }

        totalAttackText.text = ConvertScore(totalAttack + "");
        totalDefenseText.text = ConvertScore(totalDefense + "");
    }

    public IEnumerator BattleCountingStatus()
    {
        isCount = true;
        turn++; 

        if (BMInstance.bossEffect == 2 && !HasNope() && BMInstance.currentBossScore == BMInstance.mainBossScore)
        {
            totalAttack = 0;
            totalDefense = 0;
        }
        else
        {
            totalAttack = baseAttack + tempAttack;
            totalDefense = baseDefense + tempDefense;
        }

        totalAttackText.text = ConvertScore(totalAttack + "");
        totalDefenseText.text = ConvertScore(totalDefense + "");
        float delay = 0;

        foreach (Transform troop in battlefieldTrans)
        {
            if (BMInstance.bossEffect == 8 && BMInstance.currentBossScore == BMInstance.mainBossScore && !HasNope())
            {
                AddMoney(-2);
            }
            StartCoroutine(BattleDelay(delay, troop.gameObject, 0));
            delay += 0.3f;
            StartCoroutine(BattleDelay(delay, troop.gameObject, 1));
            delay += 0.3f;
            if (troop.GetComponent<TroopCardDisplay>().card.effectType[0] == 0 && troop.GetComponent<TroopCardDisplay>().card.effectCode[0] != -1)
            {
                StartCoroutine(BattleDelay(delay, troop.gameObject, 2));
                delay += 0.3f;
            }
        }

        foreach (Transform troop in handfieldTrans)
        {
            foreach (int i in troop.GetComponent<TroopCardDisplay>().card.effectType)
            {
                if (i == 6)
                {
                    StartCoroutine(BattleDelay(delay, troop.gameObject, 4));
                    delay += 0.3f;
                    break;
                }
            }
        }

        foreach (Transform magic in magicfieldTrans)
        {
            if (magic.GetComponent<MagicCardDisplay>().card.effectType[0] == 0 && magic.GetComponent<MagicCardDisplay>().card.effectCode[0] != -1)
            {
                StartCoroutine(BattleDelay(delay, magic.gameObject, 3));
                delay += 0.3f;
            }
        }

        totalAttackText.text = ConvertScore(totalAttack + "");
        totalDefenseText.text = ConvertScore(totalDefense + "");

        yield return new WaitForSeconds(delay);

        if (BMInstance.bossEffect == 7 && BMInstance.currentBossScore == BMInstance.mainBossScore && !HasNope() && !isSellMagic)
        {
            totalScore = 0;
            totalScoreText.text = "0 (Boss Effect!)";
        }
        else
        {
            totalScore += totalAttack * totalDefense;
            if (totalScore > BigInteger.Parse(PlayerPrefs.GetString("highScore", "0")))
            {
                PlayerPrefs.SetString("highScore", totalScore.ToString());
            }
            totalScoreText.text = ConvertScore(totalScore + "");
        }
        isCount = false;

        BlockDragDrop(true);

        foreach (GameObject card in cardsInBattlefield.ToArray())
        {
            card.GetComponent<TroopCardDisplay>().EndEffect();
        }

        foreach (GameObject card in cardsInBattlefield.ToArray())
        {
            CardFromBattlefieldToAltarfield(card);
        }

        battleBtn.interactable = true;
        discardBtn.interactable = true;
        
        if (isEnd)
        {
            WinMatch();
        }
        else if (totalScore >= 2 * BMInstance.currentBossScore && BMInstance.bossEffect == 1 && BMInstance.currentBossScore == BMInstance.mainBossScore && !HasNope())
        {
            EndGame();
        }
        else if (totalScore >= BMInstance.currentBossScore)
        {
            WinMatch();
        }
        else if (hands <= 0 && totalScore < BMInstance.currentBossScore)
        {
            EndGame();
        }
        else
        {
            GetRandomTroop();
        }
    }

    public void WinMatch()
    {
        tempDefense = 0;
        tempAttack = 0;
        FreeRerollShop();
        RerollPack();

        foreach (Transform magic in magicfieldTrans)
        {
            magic.GetComponent<MagicCardDisplay>().LateEffect();
        }

        foreach (GameObject card in cardsInAltarfield.ToArray())
        {
            CardFromAltarfieldToCampfield(card);
        }

        foreach (GameObject card in cardsInHandfield.ToArray())
        {
            CardFromHandfieldToCampfield(card);
        }

        foreach (GameObject card in cardsInCampfield.ToArray())
        {
            card.GetComponent<TroopCardDisplay>().LateEffect();
        }

        if (BMInstance.currentBossScore == BMInstance.mainBossScore && BMInstance.stage >= 40)
        {
            WinGame();
        }
        else
        {
            isBattle = false;
            endMatchPanel.SetActive(true);
            battlePanel.SetActive(false);
            rewardText.text = BMInstance.currentBossReward + "$";
        }
    }

    private void WinGame()
    {
        float time = float.Parse(PlayerPrefs.GetString("playedTime", "0"));
        string newTime = Mathf.CeilToInt(time + playedTime).ToString();
        PlayerPrefs.SetString("playedTime", newTime);
        PlayerPrefs.SetInt("save", 0);
        PlayerPrefs.SetInt("prevSeed", PlayerPrefs.GetInt("seed"));
        AudioManager.AMInstance.bgm.enabled = false;
        AudioManager.AMInstance.PlayAudioRandom(11);
        winGamePanel.SetActive(true);
        battlePanel.SetActive(false);
    }

    private void EndGame()
    {
        if (cardsInMagicfield.Contains(EffectManager.EMInstance.godBless))
        {
            WinMatch();
            CardFromMagicfieldToShopfield(EffectManager.EMInstance.godBless);
            EMInstance.godBless.GetComponent<MagicCardDisplay>().priceText.transform.parent.gameObject.SetActive(true);
        }
        else if (EffectManager.EMInstance.aine.activeSelf)
        {
            WinMatch();
            EffectManager.EMInstance.aine.SetActive(false);
            blessCountText.text = "0/1";
        }
        else
        {
            float time = float.Parse(PlayerPrefs.GetString("playedTime", "0"));
            string newTime = Mathf.CeilToInt(time + playedTime).ToString();
            PlayerPrefs.SetString("playedTime", newTime);
            PlayerPrefs.SetInt("save", 0);
            PlayerPrefs.SetInt("prevSeed", PlayerPrefs.GetInt("seed"));
            AudioManager.AMInstance.bgm.enabled = false;
            AudioManager.AMInstance.PlayAudioRandom(9);
            endGamePanel.SetActive(true);
            battlePanel.SetActive(false);
        }
    }

    public void Reward()
    {
        AudioManager.AMInstance.PlayAudio(4);
        battlePanel.SetActive(true);
        endMatchPanel.SetActive(false);

        dturn = 0;
        turn = 0;
        isEnd = false;
        totalScore = 0;
        currentBattlefield = maxTroopInBattlefield;
        ChangeHand(1, maxHands);
        ChangeDiscard(1, maxDiscards);
        handSize = maxHandSize;
        totalScoreText.text = ConvertScore(totalScore + "");
        AddMoney(BMInstance.currentBossReward);

        if (BMInstance.currentBossScore == BMInstance.smallBossScore)
        {
            BMInstance.BigMinion();
        }
        else if (BMInstance.currentBossScore == BMInstance.bigBossScore)
        {
            BMInstance.MainBoss();
        }
        else
        {
            BMInstance.NewStage();
        }

        ToShop();
    }

    private IEnumerator BattleDelay(float time, GameObject troop, int type)
    {
        yield return new WaitForSeconds(time);

        switch (type)
        {
            case 0:
                troop.GetComponent<TroopCardDisplay>().Point(0);
                totalAttack += troop.GetComponent<TroopCardDisplay>().attack;
                break;

            case 1:
                troop.GetComponent<TroopCardDisplay>().Point(1);
                totalDefense += troop.GetComponent<TroopCardDisplay>().defense;
                break;

            case 2:
                troop.GetComponent<TroopCardDisplay>().Effect();
                break;

            case 3:
                troop.GetComponent<MagicCardDisplay>().Effect();
                break;

            case 4:
                troop.GetComponent<TroopCardDisplay>().HandEffect();
                break;
        }

        totalAttackText.text = ConvertScore(totalAttack + "");
        totalDefenseText.text = ConvertScore(totalDefense + "") ;
        AudioManager.AMInstance.PlayAudioRandom(1, 0.5f);
        yield return null;
    }

    public void ChangeTotalAttack(int bonus)
    {
        tempAttack += bonus;
        CoutingStatus();
    }

    public void ChangeTotalDefense(int bonus)
    {
        tempDefense += bonus;
        CoutingStatus();
    }

    public void SpendMoney(int cost)
    {
        money -= cost;
        moneyText.text = money + "$";
    }

    public void AddMoney(int cost)
    {
        money += cost;
        moneyText.text = money + "$";
    }

    public void FreeRerollShop()
    {
        SpendMoney(0);
        Reroll();
        rerollMoney = maxRerollMoney;
        rerollBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Reroll - " + rerollMoney + "$";
    }

    public void FreeRerollShopEffect()
    {
        SpendMoney(0);
        Reroll();
        rerollMoney = maxRerollMoney;
        rerollBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Reroll - " + rerollMoney + "$";
        GMInstance.rerollBtn.onClick.RemoveAllListeners();
        GMInstance.rerollBtn.onClick.AddListener(() =>
        {
            GMInstance.RerollShop();
        });
    }

    private void Reroll()
    {
        foreach (Transform card in shopfieldTrans)
        {
            card.gameObject.SetActive(false);
        }

        int reset = shopfieldTrans.childCount;
        if (shopfieldTrans.childCount > maxCardInShop)
        {
            reset = maxCardInShop;
        }

        for (int i = 0; i < reset; i++)
        {
            List<GameObject> tempList = new List<GameObject>();

            while (tempList.Count == 0)
            {
                if (tempList.Count != 0)
                {
                    tempList.RemoveRange(0, tempList.Count);
                }

                string rarity = GachaRate();

                foreach (Transform card in shopfieldTrans)
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
    }

    public void RerollPack()
    {
        foreach (Transform card in shopPackfieldTrans)
        {
            card.gameObject.SetActive(false);
        }

        int reset = shopPackfieldTrans.childCount;
        if (shopPackfieldTrans.childCount > maxCardInShopPackfield)
        {
            reset = maxCardInShopPackfield;
        }

        for (int i = 0; i < reset; i++)
        {
            List<GameObject> tempList = new List<GameObject>();

            while (tempList.Count == 0)
            {
                if (tempList.Count != 0)
                {
                    tempList.RemoveRange(0, tempList.Count);
                }

                string rarity = GachaRate();

                foreach (Transform card in shopPackfieldTrans)
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
    }

    public void RerollIdol()
    {
        foreach (Transform card in shopIdolfieldTrans)
        {
            card.gameObject.SetActive(false);
        }

        int reset = shopIdolfieldTrans.childCount;
        if (reset == 0)
        {
            return;
        }
        else if (reset > 1)
        {
            reset = 1;
        }

        for (int i = 0; i < reset; i++)
        {
            List<GameObject> tempList = new List<GameObject>();

            while (tempList.Count == 0)
            {
                if (tempList.Count != 0)
                {
                    tempList.RemoveRange(0, tempList.Count);
                }

                string rarity = GachaRate();

                foreach (Transform card in shopIdolfieldTrans)
                {
                    if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf)
                    {
                        tempList.Add(card.gameObject);
                    }
                }

                if (tempList.Count != 0)
                {
                    int ran = Random.Range(0, tempList.Count);
                    tempList[ran].SetActive(true);
                }
            }
        }
    }

    public void RerollShop()
    {
        if ((money >= rerollMoney && !HasCreditCard()) || (money >= (-20 + rerollMoney) && HasCreditCard()))
        {
            AudioManager.AMInstance.PlayAudio(2);
            SpendMoney(rerollMoney);

            Reroll();

            rerollMoney++;
            rerollBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Reroll - " + rerollMoney + "$";
        }
    }

    private void Packing(int code, string sarg, int arg)
    {
        int reset = shopfieldTrans.childCount;
        int counter = 0;

        if (shopfieldTrans.childCount > arg)
        {
            reset = arg;
        }

        if (reset <= 2)
        {
            packfieldTrans.transform.localPosition = new UnityEngine.Vector3(300, -250, 0);
        }
        else if (reset > 2)
        {
            packfieldTrans.transform.localPosition = new UnityEngine.Vector3(200, -250, 0);
        }

        for (int i = 0; i < reset; i++)
        {
            List<GameObject> tempList = new List<GameObject>();

            while (tempList.Count == 0 && counter < 200)
            {
                counter++;

                if (tempList.Count != 0)
                {
                    tempList.RemoveRange(0, tempList.Count);
                }

                string rarity = GachaRate();

                switch (code)
                {
                    case 0:
                        foreach (Transform card in shopfieldTrans)
                        {
                            if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf)
                            {
                                tempList.Add(card.gameObject);
                            }
                        }
                        break;

                    case 1:
                        foreach (Transform card in shopfieldTrans)
                        {
                            if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf && card.GetComponent<CardDisplay>().type[0] == sarg)
                            {
                                tempList.Add(card.gameObject);
                            }
                        }
                        break;

                    case 2:
                        foreach (Transform card in shopfieldTrans)
                        {
                            if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf)
                            {
                                for (int j = 2; j < card.GetComponent<CardDisplay>().type.Length; j++)
                                {
                                    if (card.GetComponent<CardDisplay>().type[j] == sarg)
                                    {
                                        tempList.Add(card.gameObject);
                                        break;
                                    }
                                }
                            }
                        }
                        break;

                    case 3:
                        rarity = sarg;

                        foreach (Transform card in shopfieldTrans)
                        {
                            if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf)
                            {
                                tempList.Add(card.gameObject);
                            }
                        }
                        break;

                    case 4:
                        foreach (Transform card in shopIdolfieldTrans)
                        {
                            if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeSelf && card.GetComponent<CardDisplay>().type[0] == sarg)
                            {
                                tempList.Add(card.gameObject);
                            }
                        }
                        break;
                }

                if (tempList.Count != 0 && code != 4)
                {
                    int ran = Random.Range(0, tempList.Count);
                    CardFromShopfieldToPackfield(tempList[ran]);
                    tempList[ran].GetComponent<CardDisplay>().priceText.transform.parent.gameObject.SetActive(false);
                    tempList[ran].SetActive(true);
                }
                else if (tempList.Count != 0 && code == 4)
                {
                    int ran = Random.Range(0, tempList.Count);
                    CardFromShopIdolfieldToPackfield(tempList[ran]);
                    tempList[ran].GetComponent<CardDisplay>().priceText.transform.parent.gameObject.SetActive(false);
                    tempList[ran].SetActive(true);
                }
            }
        }
    }

    public void OpenPack(int cost, int code, string sarg, int arg = 2)
    {
        if ((money >= cost && !HasCreditCard()) || (money >= (-20 + cost) && HasCreditCard()))
        {
            packfieldTrans.gameObject.SetActive(true);
            shopfieldTrans.gameObject.SetActive(false);
            shopIdolfieldTrans.gameObject.SetActive(false);
            shopPackfieldTrans.gameObject.SetActive(false);
            skipPackBtn.SetActive(true);
            SpendMoney(cost);

            Packing(code, sarg, arg);
        }
    }

    public void BuyIdol()
    {
        if ((money >= 10 && !HasCreditCard()) || (money >= (-20 + 10) && HasCreditCard()))
        {
            SpendMoney(10);
        }
    }

    public void DonePack()
    {
        foreach (GameObject card in cardsInPackfield.ToArray())
        {
            card.GetComponent<CardDisplay>().priceText.transform.parent.gameObject.SetActive(true);
            CardFromPackfieldToShopfield(card);
        }

        packfieldTrans.gameObject.SetActive(false);
        shopfieldTrans.gameObject.SetActive(true);
        shopPackfieldTrans.gameObject.SetActive(true);
        shopIdolfieldTrans.gameObject.SetActive(true);
        skipPackBtn.SetActive(false);
    }

    public void DoneIdolPack()
    {
        foreach (GameObject card in cardsInPackfield.ToArray())
        {
            card.GetComponent<CardDisplay>().priceText.transform.parent.gameObject.SetActive(true);
            CardFromPackfieldToShopIdolfield(card);
        }

        packfieldTrans.gameObject.SetActive(false);
        shopfieldTrans.gameObject.SetActive(true);
        shopPackfieldTrans.gameObject.SetActive(true);
        shopIdolfieldTrans.gameObject.SetActive(true);
        skipPackBtn.SetActive(false);
    }

    public void ToShop()
    {
        state = 0;
        bg.sprite = bgSprites[0];
        bossObject.SetActive(false);
        shopObject.SetActive(true);
    }

    public void ToShopFromScout()
    {
        state = 0; 
        bg.sprite = bgSprites[0];
        choosenObject.SetActive(false);
        otherfield.SetActive(true);
        shopObject.SetActive(true);
        closeScoutBtn.SetActive(false);
    }

    public void ToShopFromSkip()
    {
        state = 0;
        bg.sprite = bgSprites[0];
        FreeRerollShop();
        RerollPack();
        choosenObject.SetActive(false);
        shopObject.SetActive(true);
    }

    public void ToChoosen()
    {
        state = 1;
        bg.sprite = bgSprites[1];
        shopObject.SetActive(false);
        choosenObject.SetActive(true);
        BMInstance.BattleMode();
    }

    public void ToScout()
    {
        state = 3;
        bg.sprite = bgSprites[1];
        shopObject.SetActive(false);
        otherfield.SetActive(false);
        choosenObject.SetActive(true);
        closeScoutBtn.SetActive(true);
        BMInstance.ScoutMode();
    }

    public void ToBossSave()
    {
        state = 2;
        ChangeHand(1, hands);
        ChangeDiscard(1, discards);
        int ran = Random.Range(2, bgSprites.Length);
        bg.sprite = bgSprites[ran];
        choosenObject.SetActive(false);
        bossObject.SetActive(true);
        bossScoreText.text = "TCPR: " + ConvertScore(BigInteger.Parse(PlayerPrefs.GetString("currentBossScore")) + "");
        CoutingStatus();
    }

    public void ToBoss()
    {
        state = 2;
        int ran = Random.Range(2, bgSprites.Length);
        isSellMagic = false;
        currentBattlefield = maxTroopInBattlefield;
        ChangeHand(1, maxHands);
        ChangeDiscard(1, maxDiscards);
        handSize = maxHandSize;
        bg.sprite = bgSprites[ran];
        isBattle = true;
        choosenObject.SetActive(false);
        bossObject.SetActive(true);
        bossScoreText.text = "TCPR: " + ConvertScore(BMInstance.currentBossScore + "");

        if (!HasNope())
        {
            if (BMInstance.bossEffect == 3 && BMInstance.currentBossScore == BMInstance.mainBossScore)
            {
                ChangeDiscard(1, 0);
            }
            else if (BMInstance.bossEffect == 4 && BMInstance.currentBossScore == BMInstance.mainBossScore)
            {
                ChangeHand(1, 1);
            }
            else if (BMInstance.bossEffect == 5 && BMInstance.currentBossScore == BMInstance.mainBossScore)
            {
                handSize -= 2;
            }
            else if (BMInstance.bossEffect == 9 && BMInstance.currentBossScore == BMInstance.mainBossScore)
            {
                GMInstance.currentBattlefield--;
            }
            else if (BMInstance.bossEffect == 9 && BMInstance.currentBossScore == BMInstance.mainBossScore)
            {
                GMInstance.currentBattlefield--;
            }
            else if (BMInstance.bossEffect == 10 && BMInstance.currentBossScore == BMInstance.mainBossScore && GMInstance.money > BMInstance.stage * 3)
            {
                GMInstance.money = BMInstance.stage * 3;
                GMInstance.AddMoney(0);
            }
        }

        battleTime++;
        EMInstance.SpecialEffect(5, EMInstance.challenger.GetComponent<MagicCardDisplay>().card.args, null);
        foreach (Transform magic in magicfieldTrans)
        {
            magic.GetComponent<MagicCardDisplay>().StartEffect();
        }

        GetRandomTroop();
    }

    private void GetRandomTroop()
    {
        CoutingStatus();

        for (int i = 0; i < handSize; i++)
        {
            if (cardsInHandfield.Count < handSize && cardsInCampfield.Count > 0)
            {
                int ran = Random.Range(0, cardsInCampfield.Count);
                CardFromCampfieldToHandfield(cardsInCampfield[ran]);
            }
            else
            {
                break;
            }
        }
    }

    public void Battle()
    {
        ChangeHand();
        battleBtn.interactable = false;
        discardBtn.interactable = false;
        BlockDragDrop(false);

        StartCoroutine(BattleCountingStatus());
    }    

    private void BlockDragDrop(bool isNotBlock)
    {
        foreach (Transform card in battlefieldTrans)
        {
            card.GetComponent<CanvasGroup>().blocksRaycasts = isNotBlock;
        }

        foreach (Transform card in handfieldTrans)
        {
            card.GetComponent<CanvasGroup>().blocksRaycasts = isNotBlock;
        }

        foreach (Transform card in magicfieldTrans)
        {
            card.GetComponent<CanvasGroup>().blocksRaycasts = isNotBlock;
        }
    }

    public void Discard()
    {
        if (discards > 0)
        {
            dturn++;
            ChangeDiscard();

            foreach (GameObject card in cardsInBattlefield.ToArray())
            {
                card.GetComponent<TroopCardDisplay>().DiscardEffect();
            }

            foreach (GameObject card in cardsInMagicfield.ToArray())
            {
                card.GetComponent<MagicCardDisplay>().DiscardEffect();
            }

            foreach (GameObject card in cardsInBattlefield.ToArray())
            {
                CardFromBattlefieldToAltarfield(card);
                numberOfDiscard++;
            }

            GetRandomTroop();
        }
    }

    public void OpenCamp()
    {
        campfieldTrans.parent.gameObject.SetActive(true);
        CloseIdol();
    }

    public void CloseCamp()
    {
        campfieldTrans.parent.gameObject.SetActive(false);
    }

    public void OpenIdol()
    {
        idolfieldTrans.parent.gameObject.SetActive(true);
        CloseCamp();
    }

    public void CloseIdol()
    {
        idolfieldTrans.parent.gameObject.SetActive(false);
    }

    public void OpenAltar()
    {
        altarfieldTrans.parent.gameObject.SetActive(true);
    }

    public void CloseAltar()
    {
        altarfieldTrans.parent.gameObject.SetActive(false);
    }

    private void CalculateSpacingMagicfield()
    {
        if (cardsInMagicfield.Count >= 5)
        {
            float space = (magicfieldTrans.GetComponent<RectTransform>().rect.width - 145 * cardsInMagicfield.Count) / (cardsInMagicfield.Count - 1);
            magicfieldTrans.GetComponent<HorizontalLayoutGroup>().spacing = space;
        }
        else
        {
            magicfieldTrans.GetComponent<HorizontalLayoutGroup>().spacing = 20;
        }
    }

    private void CalculateSpacingBattlefield()
    {
        if (cardsInBattlefield.Count >= 5)
        {
            float space = (battlefieldTrans.GetComponent<RectTransform>().rect.width - 145 * cardsInBattlefield.Count) / (cardsInBattlefield.Count - 1);
            battlefieldTrans.GetComponent<HorizontalLayoutGroup>().spacing = space;
        }
        else
        {
            battlefieldTrans.GetComponent<HorizontalLayoutGroup>().spacing = 20;
        }
    }

    private void CalculateSpacingHandSize()
    {
        if (cardsInHandfield.Count >= 5)
        {
            float space = (handfieldTrans.GetComponent<RectTransform>().rect.width - 145 * cardsInHandfield.Count) / (cardsInHandfield.Count - 1);
            handfieldTrans.GetComponent<HorizontalLayoutGroup>().spacing = space;
        }
        else
        {
            handfieldTrans.GetComponent<HorizontalLayoutGroup>().spacing = 20;
        }
    }

    private void CalculateSpacingSuperSize()
    {
        if (cardsInSuperfield.Count > 1)
        {
            float space = (superfieldTrans.GetComponent<RectTransform>().rect.width - 145 * cardsInSuperfield.Count) / (cardsInSuperfield.Count - 1);
            superfieldTrans.GetComponent<HorizontalLayoutGroup>().spacing = space;
        }
    }

    private void CalculateSpacingCampSize()
    {
        if (cardsInCampfield.Count >= 30)
        {
            float space = (campfieldTrans.GetComponent<RectTransform>().rect.width * 2.8f - 145 * cardsInCampfield.Count) / (cardsInCampfield.Count - 1);
            campfieldTrans.GetComponent<GridLayoutGroup>().spacing = new UnityEngine.Vector2(space, 10);
        }
        else
        {
            campfieldTrans.GetComponent<GridLayoutGroup>().spacing = new UnityEngine.Vector2(10, 10);
        }
    }

    private void CalculateSpacingAltarSize()
    {
        if (cardsInAltarfield.Count >= 30)
        {
            float space = (altarfieldTrans.GetComponent<RectTransform>().rect.width * 2.8f - 145 * cardsInAltarfield.Count) / (cardsInAltarfield.Count - 1);
            altarfieldTrans.GetComponent<GridLayoutGroup>().spacing = new UnityEngine.Vector2(space, 10);
        }
        else
        {
            altarfieldTrans.GetComponent<GridLayoutGroup>().spacing = new UnityEngine.Vector2(10, 10);
        }
    }

    private void CalculateSpacingIdolSize()
    {
        if (cardsInIdolfield.Count >= 30)
        {
            float space = (idolfieldTrans.GetComponent<RectTransform>().rect.width * 2.8f - 145 * cardsInIdolfield.Count) / (cardsInIdolfield.Count - 1);
            idolfieldTrans.GetComponent<GridLayoutGroup>().spacing = new UnityEngine.Vector2(space, 10);
        }
    }

    public void Menu()
    {
        loadPanel.SetActive(true);
        SceneManager.LoadScene(0);
    }

    public bool HasCreditCard()
    {
        foreach (Transform card in magicfieldTrans)
        {
            if (card.gameObject == EMInstance.creditCard)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasNope()
    {
        foreach (Transform card in magicfieldTrans)
        {
            if (card.gameObject == EMInstance.nope)
            {
                return true;
            }
        }
        return false;
    }

    public void ChangeHand(int mode = 0, int change = -1)
    {
        if (mode == 0)
        {
            hands += change;
        }
        else
        {
            hands = change;
        }

        handText.text = "Hand: " + hands + "";
    }

    public void ChangeDiscard(int mode = 0, int change = -1)
    {
        if (mode == 0)
        {
            discards += change;
        }
        else
        {
            discards = change;
        }
        discardText.text = "Discard: " + discards + "";
    }

    private string ConvertScore(string str)
    {
        string strm = "";
        int count = 0;
        int startPoint = 0;
        if (str[0] == '-')
        {
            startPoint = 1;
        }

        for (int i = str.Length - 1; i >= startPoint; i--)
        {
            count++;
            strm = str[i] + strm;
            if (count == 3 && (i - 1) >= startPoint)
            {
                count = 0;
                strm = "." + strm;
            }
        }

        if (str[0] == '-')
        {
            strm = "-" + strm;
        }
        return strm;
    }

    public bool HasGoldExperience()
    {
        foreach (Transform card in magicfieldTrans)
        {
            if (card.gameObject == EMInstance.goldExperience)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasKingCrimson()
    {
        foreach (Transform card in magicfieldTrans)
        {
            if (card.gameObject == EMInstance.kingCrimson)
            {
                return true;
            }
        }
        return false;
    }

    public string GachaRate()
    {
        int rate = Random.Range(1, 1001);

        if (rate >= 1 && rate <= 520)
        {
            return "Common"; //52.0%
        }
        else if (rate > 520 && rate <= 794)
        {
            return "Uncommon"; //27.4%
        }
        else if (rate > 794 && rate <= 925)
        {
            return "Rare"; //13.1%
        }
        else if (rate > 925 && rate <= 979)
        {
            return "Epic"; //5.4%
        }
        else if (rate > 979 && rate <= 995)
        {
            return "Mythic"; //1.6%
        }
        else if (rate > 995 && rate <= 1000)
        {
            return "Legendary"; //0.5%
        }

        return "Legendary";
    }

    public void SaveGame()
    {
        if (!endGamePanel.gameObject.activeSelf)
        {
            PlayerPrefs.DeleteAll();

            float time = float.Parse(PlayerPrefs.GetString("playedTime", "0"));
            string newTime = Mathf.CeilToInt(time + playedTime).ToString();
            PlayerPrefs.SetString("playedTime", newTime);

            SaveState.SSInstance.SaveSeed();
            PlayerPrefs.SetInt("seed", seed);
            PlayerPrefs.SetInt("bless", bless);
            PlayerPrefs.SetInt("isSellMagic", isSellMagic ? 1 : 0);
            PlayerPrefs.SetInt("save", 1);
            PlayerPrefs.SetInt("money", money);
            PlayerPrefs.SetInt("stage", BMInstance.stage);
            PlayerPrefs.SetInt("maxHands", maxHands);
            PlayerPrefs.SetInt("maxDiscards", maxDiscards);
            PlayerPrefs.SetInt("maxCardInShop", maxCardInShop);
            PlayerPrefs.SetInt("maxHandSize", maxHandSize);
            PlayerPrefs.SetInt("maxMagicInMagicfield", maxMagicInMagicfield);
            PlayerPrefs.SetInt("maxRerollMoney", maxRerollMoney);
            PlayerPrefs.SetInt("maxSuperInSuperfield", maxSuperInSuperfield);
            PlayerPrefs.SetInt("maxTroopInBattlefield", maxTroopInBattlefield);
            PlayerPrefs.SetInt("maxCardInShopPackfield", maxCardInShopPackfield);
            PlayerPrefs.SetInt("currentBattlefield", currentBattlefield);
            PlayerPrefs.SetInt("turn", turn);
            PlayerPrefs.SetInt("dturn", dturn);
            PlayerPrefs.SetInt("hands", hands);
            PlayerPrefs.SetInt("battleBonus", battleBonus);
            PlayerPrefs.SetInt("skipBonus", skipBonus);
            PlayerPrefs.SetInt("discards", discards);
            PlayerPrefs.SetInt("isBattle", isBattle ? 1 : 0);
            PlayerPrefs.SetInt("isCount", isCount ? 1 : 0);
            PlayerPrefs.SetInt("skipTime", skipTime);
            PlayerPrefs.SetInt("battleTime", battleTime);
            PlayerPrefs.SetInt("artifactSellValue", artifactSellValue);
            PlayerPrefs.SetInt("rerollMoney", rerollMoney);
            PlayerPrefs.SetInt("handSize", handSize);
            PlayerPrefs.SetInt("isEnd", isEnd ? 1 : 0);
            PlayerPrefs.SetInt("numberOfDiscard", numberOfDiscard);
            PlayerPrefs.SetInt("numberOfAdd", numberOfAdd);
            PlayerPrefs.SetInt("numberOfDestroy", numberOfDestroy);
            PlayerPrefs.SetString("totalAttack", totalAttack + "");
            PlayerPrefs.SetString("totalDefense", totalDefense + "");
            PlayerPrefs.SetString("totalScore", totalScore + "");
            PlayerPrefs.SetString("tempAttack", tempAttack + "");
            PlayerPrefs.SetString("tempDefense", tempDefense + "");
            PlayerPrefs.SetInt("baseAttack", baseAttack);
            PlayerPrefs.SetInt("baseDefense", baseDefense);
            PlayerPrefs.SetInt("tuskCounter", tuskCounter);
            PlayerPrefs.SetInt("heartSteelCounter", heartSteelCounter);
            PlayerPrefs.SetInt("soloLevelingCounter", soloLevelingCounter);
            PlayerPrefs.SetInt("kaynCounter", kaynCounter);
            PlayerPrefs.SetInt("state", state);
            PlayerPrefs.SetInt("fireflyCounter", fireflyCounter);
            PlayerPrefs.SetInt("ahriCounter", ahriCounter);
            PlayerPrefs.SetInt("xiaoCounter", xiaoCounter);
            PlayerPrefs.SetInt("qiqiCounter", qiqiCounter);
            PlayerPrefs.SetInt("veigarCounter", veigarCounter);
            PlayerPrefs.SetInt("hutaoDEFCounter", hutaoDEFCounter);
            PlayerPrefs.SetInt("hutaoATKCounter", hutaoATKCounter);
            PlayerPrefs.SetInt("nasusCounter", nasusCounter);
            PlayerPrefs.SetInt("mountainCounter", mountainCounter);

            PlayerPrefs.SetInt("smallBossReward", BMInstance.smallBossReward);
            PlayerPrefs.SetInt("bigBossReward", BMInstance.bigBossReward);
            PlayerPrefs.SetInt("mainBossReward", BMInstance.mainBossReward);
            PlayerPrefs.SetInt("currentBossReward", BMInstance.currentBossReward);
            PlayerPrefs.SetInt("bossEffect", BMInstance.bossEffect);
            PlayerPrefs.SetString("smallBossScore", BMInstance.smallBossScore + "");
            PlayerPrefs.SetString("bigBossScore", BMInstance.bigBossScore + "");
            PlayerPrefs.SetString("mainBossScore", BMInstance.mainBossScore + "");
            PlayerPrefs.SetString("currentBossScore", BMInstance.currentBossScore + "");
            PlayerPrefs.SetString("bossScore", BMInstance.bossScore + "");
            PlayerPrefs.SetInt("smallBossSkipBtn", BMInstance.smallBossSkipBtn.interactable ? 1 : 0);
            PlayerPrefs.SetInt("bigBossSkipBtn", BMInstance.bigBossSkipBtn.interactable ? 1 : 0);
            PlayerPrefs.SetInt("smallBossBtn", BMInstance.smallBossBtn.interactable ? 1 : 0);
            PlayerPrefs.SetInt("bigBossBtn", BMInstance.bigBossBtn.interactable ? 1 : 0);
            PlayerPrefs.SetInt("mainBossBtn", BMInstance.mainBossBtn.interactable ? 1 : 0);
            PlayerPrefs.SetInt("smallBossSkipEffect", BMInstance.smallBossSkipEffect);
            PlayerPrefs.SetInt("bigBossSkipEffect", BMInstance.bigBossSkipEffect);
            int counting = 0;

            foreach (GameObject card in cardsInAltarfield)
            {
                if (card.GetComponent<TroopCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "T1A" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "T1U" + card.name);
                    }
                }

                counting++;
            }

            foreach (GameObject card in cardsInBattlefield)
            {
                if (card.GetComponent<TroopCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "T2A" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "T2U" + card.name);
                    }
                }

                counting++;
            }

            foreach (GameObject card in cardsInCampfield)
            {
                if (card.GetComponent<TroopCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "T3A" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "T3U" + card.name);
                    }
                }

                counting++;
            }

            foreach (GameObject card in cardsInHandfield)
            {
                if (card.GetComponent<TroopCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "T4A" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "T4U" + card.name);
                    }
                }

                counting++;
            }

            foreach (GameObject card in cardsInIdolfield)
            {
                if (card.GetComponent<IdolCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "I5A" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "I5U" + card.name);
                    }
                }

                counting++;
            }

            foreach (GameObject card in cardsInMagicfield)
            {
                if (card.GetComponent<MagicCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "M6A" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "M6U" + card.name);
                    }
                }

                counting++;
            }

            foreach (GameObject card in cardsInPackfield)
            {
                if (card.GetComponent<TroopCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "T7A" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "T7U" + card.name);
                    }
                }
                else if (card.GetComponent<MagicCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "M7A" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "M7U" + card.name);
                    }
                }
                else if (card.GetComponent<SuperCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "S7A" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "S7U" + card.name);
                    }
                }

                counting++;
            }

            foreach (GameObject card in cardsInShopfield)
            {
                if (card.GetComponent<TroopCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "T8A" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "T8U" + card.name);
                    }
                }
                else if (card.GetComponent<MagicCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "M8A" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "M8U" + card.name);
                    }
                }
                else if (card.GetComponent<SuperCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "S8A" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "S8U" + card.name);
                    }
                }

                counting++;
            }

            foreach (GameObject card in cardsInShopIdolfield)
            {
                if (card.GetComponent<IdolCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "I9A" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "I9U" + card.name);
                    }
                }

                counting++;
            }

            foreach (GameObject card in cardsInShopPackfield)
            {
                if (card.GetComponent<GachaCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "GAA" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "GAU" + card.name);
                    }
                }

                counting++;
            }

            foreach (GameObject card in cardsInSuperfield)
            {
                if (card.GetComponent<SuperCardDisplay>())
                {
                    if (card.activeSelf)
                    {
                        PlayerPrefs.SetString(counting + "", "SBA" + card.name);
                    }
                    else
                    {
                        PlayerPrefs.SetString(counting + "", "SBU" + card.name);
                    }
                }

                counting++;
            }
        }
    }

    public bool HasBraveHeart(int arg)
    {
        return arg == 0 && cardsInMagicfield.Contains(EMInstance.braveHeart);
    }

    public bool HasLonely()
    {
        return cardsInMagicfield.Contains(EMInstance.lonely);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveGame();
    }

    public void MenuSave()
    {
        SaveGame();
        Menu();
    }

    public void OpenInfoField()
    {
        infoText.text = "Base Attack: " + baseAttack + "\r\nBase Defense: " + baseDefense + "\r\nMax Base Hand: " + maxHands + "\r\nMax Base Discard: " + maxDiscards + "\r\nBattlefield Size: " + maxTroopInBattlefield + "\r\nHandsize: " + maxHandSize + "\r\nBattle Bonus: " + battleBonus + "\r\nSkip Bonus: " + skipBonus;
        infofield.SetActive(true);
    }

    public void CloseInfoField()
    {
        infofield.SetActive(false);
    }
}
