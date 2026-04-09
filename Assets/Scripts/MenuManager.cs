using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager MMInstance {  get; private set; }
    public List<GameObject> cardsInBlessingfield;
    public List<GameObject> albumfiled;
    public List<GameObject> infofiled;
    [SerializeField] private GameObject chooseObject;
    [SerializeField] private GameObject loadingObject;
    [SerializeField] private GameObject albumObject;
    [SerializeField] private GameObject infoObject;
    [SerializeField] private GameObject banObject;
    [SerializeField] private GameObject filterObject;
    [SerializeField] private TextMeshProUGUI pressStart;
    [SerializeField] private TextMeshProUGUI staticsText;
    [SerializeField] private Image pressStartPanel;
    [SerializeField] private Transform chooseTrans;
    [SerializeField] private Transform blessingTrans;
    [SerializeField] private Transform troopTrans;
    [SerializeField] private Transform magicTrans;
    [SerializeField] private Transform superTrans;
    [SerializeField] private Transform gachaTrans;
    [SerializeField] private Transform idolTrans;
    [SerializeField] private Sprite[] mbgs;
    [SerializeField] private Toggle[] rarityToggles;
    [SerializeField] private Toggle[] typeToggles;
    [SerializeField] private Image bg;
    [SerializeField] private Button loadBtn;
    [SerializeField] private TMP_InputField seedInput;
    [SerializeField] private TMP_InputField cardNameInput;
    [SerializeField] private ScrollRect[] rects;
    private int currentIndex = 0;
    private int currentAlbumIndex = 0;
    private int currentInfoIndex = 0;
    private float currentTime;
    private int currentbgIndex;
    [SerializeField] private float changebgTime = 6f;
    private bool isChoosen;

    private void Awake()
    {
        MMInstance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;

        foreach (Transform card in chooseTrans)
        {
            cardsInBlessingfield.Add(card.gameObject);
        }

        cardsInBlessingfield[currentIndex].SetActive(true);
        int ran = Random.Range(0, mbgs.Length);
        while (currentbgIndex == ran)
        {
            ran = Random.Range(0, mbgs.Length);
        }
        bg.sprite = mbgs[ran];
        currentbgIndex = ran;

        if (PlayerPrefs.GetInt("save") != 1)
        {
            loadBtn.interactable = false;
        }

        troopTrans.parent.parent.gameObject.SetActive(false);
        magicTrans.parent.parent.gameObject.SetActive(false);
        superTrans.parent.parent.gameObject.SetActive(false);
        gachaTrans.parent.parent.gameObject.SetActive(false);
        idolTrans.parent.parent.gameObject.SetActive(false);

        albumObject.SetActive(false);
        StaticsText();
    }

    private void StaticsText()
    {
        int playedNumber = PlayerPrefs.GetInt("playedNumber", 0);
        string playedTime = PlayerPrefs.GetString("playedTime", "0");
        string highestScore = PlayerPrefs.GetString("highScore", "0");
        int highestStage = PlayerPrefs.GetInt("highestStage", 0);
        int prevSeed = PlayerPrefs.GetInt("prevSeed", 0);
        staticsText.text = "STATICS:\r\n\r\nPlayed Number: " + playedNumber + "\r\nPlayed Time: " + playedTime + "s\r\nHighest Stage: " + highestStage + "\r\nHighest Score: " + highestScore + "\r\nPrevious Seed: " + prevSeed + "\r\n";
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > changebgTime)
        {
            currentTime = 0;
            int ran = Random.Range(0, mbgs.Length);
            while (currentbgIndex == ran)
            {
                ran = Random.Range(0, mbgs.Length);
            }
            bg.sprite = mbgs[ran];
            currentbgIndex = ran;
        }
    }

    public void Play()
    {
        if (!isChoosen)
        {
            pressStart.enabled = false;
            pressStartPanel.enabled = false;
            isChoosen = true;
            chooseObject.SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayInChoose()
    {
        PlayerPrefs.SetInt("prevSeed", PlayerPrefs.GetInt("seed"));
        PlayerPrefs.SetInt("playedNumber", PlayerPrefs.GetInt("playedNumber", 0) + 1);
        PlayerPrefs.SetInt("bless", currentIndex);
        loadingObject.SetActive(true);
        chooseObject.SetActive(false);
        PlayerPrefs.SetInt("save", 0);
        PlayerPrefs.SetInt("cheat", 0);
        int result;
        if (string.IsNullOrEmpty(seedInput.text))
        {
            PlayerPrefs.SetInt("seed", RandomSeedGenerate());
        }
        else if (seedInput.text == "CHEATCODE")
        {
            PlayerPrefs.SetInt("cheat", 1);
            PlayerPrefs.SetInt("seed", RandomSeedGenerate());
        }
        else if (int.TryParse(seedInput.text, out result))
        {
            if (seedInput.text.Length > 0 && result < 2147483647)
            {
                PlayerPrefs.SetInt("seed", result);
            }
            else
            {
                PlayerPrefs.SetInt("seed", RandomSeedGenerate());
            }
        }
        else
        {
            PlayerPrefs.SetInt("seed", RandomSeedGenerate());
        }
        SceneManager.LoadScene(1);
    }

    private int RandomSeedGenerate()
    {
        return Random.Range(0, 2147483647);
    }

    public void Load()
    {
        if (PlayerPrefs.GetInt("save") == 1)
        {
            loadingObject.SetActive(true);
            chooseObject.SetActive(false);
            SceneManager.LoadScene(1);
        }
    }

    public void Back()
    {
        pressStart.enabled = true;
        pressStartPanel.enabled = true;
        isChoosen = false;
        chooseObject.SetActive(false);
    }

    public void Left()
    {
        cardsInBlessingfield[currentIndex].SetActive(false);

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = cardsInBlessingfield.Count - 1;
        }

        cardsInBlessingfield[currentIndex].SetActive(true);
    }

    public void AlbumLeft()
    {
        albumfiled[currentAlbumIndex].SetActive(false);

        currentAlbumIndex--;
        if (currentAlbumIndex < 0)
        {
            currentAlbumIndex = albumfiled.Count - 1;
        }

        albumfiled[currentAlbumIndex].SetActive(true);
    }

    public void InfoLeft()
    {
        infofiled[currentInfoIndex].SetActive(false);

        currentInfoIndex--;
        if (currentInfoIndex < 0)
        {
            currentInfoIndex = infofiled.Count - 1;
        }

        infofiled[currentInfoIndex].SetActive(true);
    }

    public void Right()
    {
        cardsInBlessingfield[currentIndex].SetActive(false);

        currentIndex++;
        if (currentIndex > cardsInBlessingfield.Count - 1)
        {
            currentIndex = 0;
        }

        cardsInBlessingfield[currentIndex].SetActive(true);
    }

    public void AlbumRight()
    {
        albumfiled[currentAlbumIndex].SetActive(false);

        currentAlbumIndex++;
        if (currentAlbumIndex > albumfiled.Count - 1)
        {
            currentAlbumIndex = 0;
        }

        albumfiled[currentAlbumIndex].SetActive(true);
    }

    public void InfoRight()
    {
        infofiled[currentInfoIndex].SetActive(false);

        currentInfoIndex++;
        if (currentInfoIndex > infofiled.Count - 1)
        {
            currentInfoIndex = 0;
        }

        infofiled[currentInfoIndex].SetActive(true);
    }

    public void AlbumBanList()
    {
        albumObject.SetActive(true);
        banObject.SetActive(false);
        filterObject.SetActive(false);
    }

    public void AlbumFilter()
    {
        foreach (ScrollRect rect in rects)
        {
            rect.verticalNormalizedPosition = 1f;
        }

        CheckFilter();
        albumObject.SetActive(true);
        banObject.SetActive(false);
        filterObject.SetActive(false);
    }

    public void CloseAlbum()
    {
        albumObject.SetActive(false);
        banObject.SetActive(false);
        filterObject.SetActive(false);
    }

    public void Info()
    {
        infoObject.SetActive(true);
    }

    public void CloseInfo()
    {
        infoObject.SetActive(false);
    }

    public void OpenBanfield()
    {
        albumObject.SetActive(false);
        banObject.SetActive(true);
        filterObject.SetActive(false);
    }

    public void OpenFilterfield()
    {
        albumObject.SetActive(false);
        banObject.SetActive(false);
        filterObject.SetActive(true);
    }

    public void CheckFilter()
    {
        List<string> rarityOn = new List<string>();
        List<string> typeOn = new List<string>();

        foreach (Toggle rarity in rarityToggles)
        {
            if (rarity.isOn)
            {
                rarityOn.Add(rarity.transform.GetChild(1).GetComponent<Text>().text);
            } 
        }

        foreach (Toggle type in typeToggles)
        {
            if (type.isOn)
            {
                typeOn.Add(type.transform.GetChild(1).GetComponent<Text>().text);
            }
        }

        bool isAll = typeOn.Contains("All");

        foreach (Transform card in blessingTrans)
        {
            Filter(card, rarityOn, typeOn, isAll);
        }

        foreach (Transform card in troopTrans)
        {
            Filter(card, rarityOn, typeOn, isAll);
        }

        foreach (Transform card in magicTrans)
        {
            Filter(card, rarityOn, typeOn, isAll);
        }

        foreach (Transform card in superTrans)
        {
            Filter(card, rarityOn, typeOn, isAll);
        }

        foreach (Transform card in gachaTrans)
        {
            Filter(card, rarityOn, typeOn, isAll);
        }

        foreach (Transform card in idolTrans)
        {
            Filter(card, rarityOn, typeOn, isAll);
        }
    }

    private void Filter(Transform card, List<string> rarityOn, List<string> typeOn, bool isAll)
    {
        card.gameObject.SetActive(false);

        if (card.GetComponent<CardDisplay>().nameText.text.StartsWith(cardNameInput.text))
        {
            if (rarityOn.Contains(card.GetComponent<CardDisplay>().rarityText.text))
            {
                if (isAll)
                {
                    card.gameObject.SetActive(true);
                }
                else 
                {
                    foreach (string type in card.GetComponent<CardDisplay>().type)
                    {
                        if (typeOn.Contains(type)) 
                        {
                            card.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}
