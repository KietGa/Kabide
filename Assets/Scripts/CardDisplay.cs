using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Image artImage;
    public GameObject info;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI rarityText;
    public Button button;
    public int cost { get; set; }
    public string[] type { get; set; }

    public virtual void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GetComponent<Button>().enabled = false;
            try
            {
                priceText.transform.parent.gameObject.SetActive(false);
            }
            catch
            {

            }

            Init();
        }
    }

    public virtual void Init()
    {

    }
}
