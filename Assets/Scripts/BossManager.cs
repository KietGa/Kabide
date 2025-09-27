using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class BossManager : MonoBehaviour
{
    public static BossManager BMInstance {  get; private set; }
    [SerializeField] private TextMeshProUGUI tcpSmallBoss;
    [SerializeField] private TextMeshProUGUI tcpBigBoss;
    [SerializeField] private TextMeshProUGUI tcpMainBoss;
    [SerializeField] private TextMeshProUGUI rewardSmallBoss;
    [SerializeField] private TextMeshProUGUI rewardBigBoss;
    [SerializeField] private TextMeshProUGUI rewardMainBoss;
    [SerializeField] private TextMeshProUGUI mainBossName;
    [SerializeField] private TextMeshProUGUI mainBossEffect;
    [SerializeField] private TextMeshProUGUI skipSmallBossText;
    [SerializeField] private TextMeshProUGUI skipBigBossText;
    public Button smallBossBtn;
    public Button bigBossBtn;
    public Button mainBossBtn;
    public Button smallBossSkipBtn;
    public Button bigBossSkipBtn;
    public BigInteger currentBossScore { get; set; }
    public int currentBossReward { get; set; }
    public BigInteger bossScore = 0;
    public int stage = 0;
    public BigInteger smallBossScore;
    public BigInteger bigBossScore;
    public BigInteger mainBossScore;
    public int smallBossReward;
    public int bigBossReward;
    public int mainBossReward;
    public int bossEffect;
    public int smallBossSkipEffect;
    public int bigBossSkipEffect;

    private void Awake()
    {
        BMInstance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("save") == 0)
        {
            NewStage();
        }
        else
        {
            smallBossReward = PlayerPrefs.GetInt("smallBossReward");
            bigBossReward = PlayerPrefs.GetInt("bigBossReward");
            mainBossReward = PlayerPrefs.GetInt("mainBossReward");
            currentBossReward = PlayerPrefs.GetInt("currentBossReward");
            bossEffect = PlayerPrefs.GetInt("bossEffect");
            smallBossScore = BigInteger.Parse(PlayerPrefs.GetString("smallBossScore"));
            bigBossScore = BigInteger.Parse(PlayerPrefs.GetString("bigBossScore"));
            mainBossScore = BigInteger.Parse(PlayerPrefs.GetString("mainBossScore"));
            currentBossScore = BigInteger.Parse(PlayerPrefs.GetString("currentBossScore"));
            bossScore = BigInteger.Parse(PlayerPrefs.GetString("bossScore")); 
            smallBossSkipBtn.interactable = PlayerPrefs.GetInt("smallBossSkipBtn") == 1;
            bigBossSkipBtn.interactable = PlayerPrefs.GetInt("bigBossSkipBtn") == 1;
            smallBossBtn.interactable = PlayerPrefs.GetInt("smallBossBtn") == 1;
            bigBossBtn.interactable = PlayerPrefs.GetInt("bigBossBtn") == 1;
            mainBossBtn.interactable = PlayerPrefs.GetInt("mainBossBtn") == 1;
            smallBossSkipEffect = PlayerPrefs.GetInt("smallBossSkipEffect");
            bigBossSkipEffect = PlayerPrefs.GetInt("bigBossSkipEffect");

            smallBossBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Challenge";
            bigBossBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Challenge";
            mainBossBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Challenge";
            currentBossScore = smallBossScore;
            currentBossReward = smallBossReward;

            tcpSmallBoss.text = ConvertScore(smallBossScore + "");
            tcpBigBoss.text = ConvertScore(bigBossScore + "");
            tcpMainBoss.text = ConvertScore(mainBossScore + "");
            rewardSmallBoss.text = "Reward: " + smallBossReward + "$+";
            rewardBigBoss.text = "Reward: " + bigBossReward + "$+";
            rewardMainBoss.text = "Reward: " + mainBossReward + "$+";
            switch (bossEffect)
            {
                case 0:
                    mainBossName.text = "The Strong";
                    mainBossEffect.text = "Effect: x3 TCP";
                    break;

                case 1:
                    mainBossName.text = "The Weak";
                    mainBossEffect.text = "Effect: TCP >= 2x TCP required = Lost";
                    break;

                case 2:
                    mainBossName.text = "The Canon";
                    mainBossEffect.text = "Effect: Base ATK & Base DEF set to 0";
                    break;

                case 3:
                    mainBossName.text = "The Friend";
                    mainBossEffect.text = "Effect: 0 Discard";
                    break;

                case 4:
                    mainBossName.text = "The Thread";
                    mainBossEffect.text = "Effect: Only 1 Hand";
                    break;

                case 5:
                    mainBossName.text = "The Night";
                    mainBossEffect.text = "Effect: -2 Handsize";
                    break;

                case 6:
                    mainBossName.text = "The Poor";
                    mainBossEffect.text = "Effect: Only get half of boss reward";
                    break;

                case 7:
                    mainBossName.text = "The Death";
                    mainBossEffect.text = "Effect: Set TCP = 0 if not sell one magic";
                    break;

                case 8:
                    mainBossName.text = "The Harvest";
                    mainBossEffect.text = "Effect: -$1 per troop card played";
                    break;

                case 9:
                    mainBossName.text = "The Titan";
                    mainBossEffect.text = "Effect: -1 Battlefield";
                    break;

                case 10:
                    mainBossName.text = "The Hunger";
                    mainBossEffect.text = "Effect: Set your $ = 3x Number of stage if your $ > it";
                    break;
            }

            SkipEffect(smallBossSkipEffect, smallBossSkipBtn, skipSmallBossText);
            SkipEffect(bigBossSkipEffect, bigBossSkipBtn, skipBigBossText);

            if (smallBossBtn.interactable)
            {
                bigBossSkipBtn.interactable = false;
                bigBossBtn.interactable = false;
                mainBossBtn.interactable = false;
                smallBossSkipBtn.interactable = true;
                smallBossBtn.interactable = true;
            }
            else if (bigBossBtn.interactable)
            {
                BigMinion();
            }
            else if (mainBossBtn.interactable)
            {
                BigMinion();
                MainBoss();
            }
        }
    }

    public void NewStage()
    {
        if (stage <= 0)
        {
            bossEffect = Random.Range(0, 11);
            while (bossEffect == 1)
            {
                bossEffect = Random.Range(0, 11);
            }
        }
        else
        {
            bossEffect = Random.Range(0, 11);
        }

        stage++;

        if (stage > PlayerPrefs.GetInt("highestStage", 0))
        {
            PlayerPrefs.SetInt("highestStage", stage);
        }

        if (stage == 1 || stage == 5 || stage == 10 || stage == 15 || stage == 20)
        {
            AudioManager.AMInstance.PlayBGM();
        }

        CalculateBossScore(stage);
        GameManager.GMInstance.RerollIdol();
        bigBossSkipBtn.interactable = false;
        bigBossBtn.interactable = false;
        mainBossBtn.interactable = false;
        smallBossSkipBtn.interactable = true;
        smallBossBtn.interactable = true;
        smallBossBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Challenge";
        bigBossBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Challenge";
        mainBossBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Challenge";
        currentBossScore = smallBossScore;
        currentBossReward = smallBossReward;

        tcpSmallBoss.text = ConvertScore(smallBossScore + "");
        tcpBigBoss.text = ConvertScore(bigBossScore + "");
        tcpMainBoss.text = ConvertScore(mainBossScore + "");
        rewardSmallBoss.text = "Reward: " + smallBossReward + "$+";
        rewardBigBoss.text = "Reward: " + bigBossReward + "$+";
        rewardMainBoss.text = "Reward: " + mainBossReward + "$+";
        switch (bossEffect)
        {
            case 0:
                mainBossName.text = "The Strong";
                mainBossEffect.text = "Effect: x3 TCP";
                break;

            case 1:
                mainBossName.text = "The Weak";
                mainBossEffect.text = "Effect: TCP >= 2x TCP required = Lost";
                break;

            case 2:
                mainBossName.text = "The Canon";
                mainBossEffect.text = "Effect: Base ATK & Base DEF set to 0";
                break;

            case 3:
                mainBossName.text = "The Friend";
                mainBossEffect.text = "Effect: 0 Discard";
                break;

            case 4:
                mainBossName.text = "The Thread";
                mainBossEffect.text = "Effect: Only 1 Hand";
                break;

            case 5:
                mainBossName.text = "The Night";
                mainBossEffect.text = "Effect: -2 Handsize";
                break;

            case 6:
                mainBossName.text = "The Poor";
                mainBossEffect.text = "Effect: Only get quarter of boss reward";
                break;

            case 7:
                mainBossName.text = "The Death";
                mainBossEffect.text = "Effect: Set TCP = 0 if not sell one magic";
                break;

            case 8:
                mainBossName.text = "The Harvest";
                mainBossEffect.text = "Effect: -$2 per troop card played";
                break;

            case 9:
                mainBossName.text = "The Titan";
                mainBossEffect.text = "Effect: -1 Battlefield";
                break;

            case 10:
                mainBossName.text = "The Hunger";
                mainBossEffect.text = "Effect: Set your $ = 3x Number of stage if your $ higher than it";
                break;
        }

        smallBossSkipEffect = Random.Range(0, 100);
        bigBossSkipEffect = Random.Range(0, 100);
        SkipEffect(smallBossSkipEffect, smallBossSkipBtn, skipSmallBossText);
        SkipEffect(bigBossSkipEffect, bigBossSkipBtn, skipBigBossText);
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
            if (count == 3 && (i-1) >= startPoint)
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

    public void BigMinion()
    {
        BMInstance.currentBossReward = BMInstance.bigBossReward;
        BMInstance.currentBossScore = BMInstance.bigBossScore;
        bigBossSkipBtn.interactable = true;
        bigBossBtn.interactable = true;
        mainBossBtn.interactable = false;
        smallBossSkipBtn.interactable = false;
        smallBossBtn.interactable = false;
        smallBossBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Defeated";
        bigBossBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Challenge";
        mainBossBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Challenge";
    }

    public void MainBoss()
    {
        BMInstance.currentBossReward = BMInstance.mainBossReward;
        BMInstance.currentBossScore = BMInstance.mainBossScore;
        bigBossSkipBtn.interactable = false;
        bigBossBtn.interactable = false;
        mainBossBtn.interactable = true;
        smallBossSkipBtn.interactable = false;
        smallBossBtn.interactable = false;
        smallBossBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Defeated";
        bigBossBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Defeated";
        mainBossBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Challenge";
    }

    public void BackStage()
    {
        stage--;
        CalculateBossScore(stage);
        if (smallBossBtn.interactable)
        {
            currentBossScore = smallBossScore;
            currentBossReward = smallBossReward;
        }
        else if (bigBossBtn.interactable)
        {
            currentBossScore = bigBossScore;
            currentBossReward = bigBossReward;
        }
        else if (mainBossBtn.interactable)
        {
            currentBossScore = mainBossScore;
            currentBossReward = mainBossReward;
        }
        tcpSmallBoss.text = ConvertScore(smallBossScore + "");
        tcpBigBoss.text = ConvertScore(bigBossScore + "");
        tcpMainBoss.text = ConvertScore(mainBossScore + "");
        rewardSmallBoss.text = "Reward: " + smallBossReward + "$+";
        rewardBigBoss.text = "Reward: " + bigBossReward + "$+";
        rewardMainBoss.text = "Reward: " + mainBossReward + "$+";
    }

    public void CalculateBossScore(int stage)
    {
        GameManager.GMInstance.stageText.text = "Stage " + stage;
        BigInteger mul = 0;
        bossScore = 0;

        if (stage == 0)
        {
            smallBossScore = -200;
            smallBossReward = 3 / 2 + GameManager.GMInstance.battleBonus;

            bigBossScore = -100;
            bigBossReward = 4 / 2 + GameManager.GMInstance.battleBonus;

            mainBossScore = 0;
            if (bossEffect == 0 && !GameManager.GMInstance.HasNope())
            {
                mainBossScore *= 3;
            }

            if (bossEffect == 6)
            {
                mainBossReward = (5 + GameManager.GMInstance.battleBonus) / 4;
            }
            else
            {
                mainBossReward = 5 + GameManager.GMInstance.battleBonus;
            }
        }
    
        for (int i = 1; i <= stage; i++) 
        {
            if (i >= 20)
            {
                mul *= i;
            }
            else if (i >= 15)
            {
                mul += i * 10 * i * i * i;
            }
            else if (i >= 10)
            {
                mul += i * 10 * i * i;
            }
            else if (i >= 5)
            {
                mul += i * 10 * i;
            }
            else
            {
                mul += i * 10;
            }

            bossScore += mul;
            smallBossScore = bossScore * bossScore;

            smallBossReward = 3 + i / 2 + GameManager.GMInstance.battleBonus;

            bossScore += mul;
            bigBossScore = bossScore * bossScore;
            bigBossReward = 4 + i / 2 + GameManager.GMInstance.battleBonus;

            bossScore += mul;
            mainBossScore = bossScore * bossScore;
            if (bossEffect == 0 && !GameManager.GMInstance.HasNope())
            {
                mainBossScore *= 3;
            }

            if (bossEffect == 6)
            {
                mainBossReward = (5 + i + GameManager.GMInstance.battleBonus) / 2;
            }
            else
            {
                mainBossReward = 5 + i + GameManager.GMInstance.battleBonus;
            }

            mul += 10;
        }
    }

    private void SkipEffect(int ran, Button btn, TextMeshProUGUI skipText)
    {
        btn.onClick.RemoveAllListeners();

        switch (ran)
        {
            case < 21:
                btn.onClick.AddListener(() =>
                {
                    Skip();
                    GameManager.GMInstance.AddMoney(20 + GameManager.GMInstance.skipBonus);
                    AudioManager.AMInstance.PlayAudio(4);
                });
                int total = 20 + GameManager.GMInstance.skipBonus;
                skipText.text = "Reward: " + total + "$+"; 

                return;

            case < 43:
                btn.onClick.AddListener(() =>
                {
                    AudioManager.AMInstance.PlayAudio(4);
                    Skip();
                    GameManager.GMInstance.AddMoney(GameManager.GMInstance.battleTime + GameManager.GMInstance.skipBonus);
                });
                total = GameManager.GMInstance.battleTime + GameManager.GMInstance.skipBonus;

                skipText.text = "Reward: " + total + "$+\n(Number of Battle Previous Stage)";

                return;

            case < 65:
                btn.onClick.AddListener(() =>
                {
                    AudioManager.AMInstance.PlayAudio(4);
                    Skip();
                    GameManager.GMInstance.AddMoney(GameManager.GMInstance.skipTime * 2 + GameManager.GMInstance.skipBonus);
                });
                total = GameManager.GMInstance.skipTime * 2 + GameManager.GMInstance.skipBonus;
                skipText.text = "Reward: " + total + "$+\n(2x Number of Skip Previous Stage)";


                return;

            case < 87:
                btn.onClick.AddListener(() =>
                {
                    AudioManager.AMInstance.PlayAudio(4);
                    Skip();
                    GameManager.GMInstance.AddMoney(stage * 2 + GameManager.GMInstance.skipBonus);
                });
                total = stage * 2 + GameManager.GMInstance.skipBonus;
                skipText.text = "Reward: " + total + "$+\n(2x Current Stage)";


                return;

            case < 96:
                btn.onClick.AddListener(() =>
                {
                    AudioManager.AMInstance.PlayAudio(5);
                    Skip();
                    AddCard("Epic");
                });
                skipText.text = "Reward: x1 Epic Card";


                return;

            case < 99:
                btn.onClick.AddListener(() =>
                {
                    AudioManager.AMInstance.PlayAudio(5);
                    Skip();
                    AddCard("Mythic");
                });
                skipText.text = "Reward: x1 Mythic Card";
                return;

            case < 100:
                btn.onClick.AddListener(() =>
                {
                    AudioManager.AMInstance.PlayAudio(5);
                    Skip();
                    AddCard("Legendary");
                });
                skipText.text = "Reward: x1 Legendary Card";
                return;
        }
    }

    private void AddCard(string rate)
    {
        for (int i = 0; i < 1; i++)
        {
            List<GameObject> tempList = new List<GameObject>();

            if (tempList.Count != 0)
            {
                tempList.RemoveRange(0, tempList.Count);
            }

            string rarity = rate;

            foreach (Transform card in GMInstance.shopfieldTrans)
            {
                if (card.GetComponent<CardDisplay>().type[1] == rarity && !card.gameObject.activeInHierarchy)
                {
                    tempList.Add(card.gameObject);
                }
            }
            
            bool isGet = false;

            do
            {
                int ran1 = Random.Range(0, tempList.Count);
                if (tempList[ran1].GetComponent<MagicCardDisplay>())
                {
                    if (GMInstance.cardsInMagicfield.Count < GMInstance.maxMagicInMagicfield)
                    {
                        tempList[ran1].GetComponent<MagicCardDisplay>().BuyCardFromMagic();
                        isGet = true;
                    }
                }
                else if (tempList[ran1].GetComponent<TroopCardDisplay>())
                {
                    tempList[ran1].GetComponent<TroopCardDisplay>().BuyCardFromMagic();
                    isGet = true;
                }
                else if (tempList[ran1].GetComponent<SuperCardDisplay>())
                {
                    if (GMInstance.cardsInSuperfield.Count < GMInstance.maxSuperInSuperfield)
                    {
                        tempList[ran1].GetComponent<SuperCardDisplay>().BuyCardFromMagic();
                        isGet = true;
                    }
                }
                tempList[ran1].SetActive(true);
                tempList.Remove(tempList[ran1]);
                AudioManager.AMInstance.PlayAudio(5);
            }
            while (!isGet && tempList.Count > 0);
        }
    }

    public void ScoutMode()
    {
        smallBossBtn.enabled = false;
        bigBossBtn.enabled = false;
        mainBossBtn.enabled = false;
        smallBossSkipBtn.enabled = false;
        bigBossSkipBtn.enabled = false;
    }

    public void BattleMode()
    {
        smallBossBtn.enabled = true;
        bigBossBtn.enabled = true;
        mainBossBtn.enabled = true;
        smallBossSkipBtn.enabled = true;
        bigBossSkipBtn.enabled = true;
    }

    public void Skip()
    {
        GameManager.GMInstance.isBattle = false;
        GameManager.GMInstance.skipTime++;

        foreach (Transform troop in GameManager.GMInstance.campfieldTrans)
        {
            troop.GetComponent<TroopCardDisplay>().SkipEffect();
        }

        foreach (Transform magic in GameManager.GMInstance.magicfieldTrans)
        {
            magic.GetComponent<MagicCardDisplay>().SkipEffect();
        }

        EffectManager.EMInstance.SpecialEffect(6, EffectManager.EMInstance.coward.GetComponent<MagicCardDisplay>().card.args, null);

        if (BMInstance.currentBossScore == BMInstance.smallBossScore)
        {
            BMInstance.BigMinion();
        }
        else if (BMInstance.currentBossScore == BMInstance.bigBossScore)
        {
            BMInstance.MainBoss();
        }

        if (GameManager.GMInstance.cardsInMagicfield.Contains(EffectManager.EMInstance.convenienceStore))
        {
            GameManager.GMInstance.ToShopFromSkip();
        }
    }
}
