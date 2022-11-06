using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BreakInfinity;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake() => instance = this;

    public Data data;

    [Header("UI References")]
    [SerializeField] private TMP_Text pixlAmountText;
    [SerializeField] private TMP_Text pixlClickPowerText;
    [SerializeField] private TMP_Text pixlPerSecondText;
    [SerializeField] private Image pixlImage;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject leaderboardUI;
    [SerializeField] private Scrollbar clickScrollBar;
    [SerializeField] private Scrollbar productionScrollBar;


    private const string dataFileName = "Pixl_Clickr";
    void Start()
    {

        data = SaveSystem.SaveExists(dataFileName) 
            ? SaveSystem.LoadData<Data>(dataFileName) : new Data();
        
        UpgradeManager.instance.StartUpdateManager();
        OfflineManager.instance.LoadOfflineProduction();

        //StartCoroutine(AddHighScore());
    }

    public float SaveTime;

    void Update()
    {
        data.pixlAmount += PixlPerSecond() * Time.deltaTime;

        pixlAmountText.text = ((long)data.pixlAmount).ToString("n0") + " Pixl!";
        pixlClickPowerText.text = "+" + ClickPower() + " Pixl";
        pixlPerSecondText.text = $"{PixlPerSecond():F2} per sec";

        SaveTime += Time.deltaTime * (1 / Time.timeScale);
        if(SaveTime >= 0.5f)
        {
            SaveSystem.SaveData(data, dataFileName);
            SaveTime = 0;
        }

    }

    public BigDouble ClickPower()
    {
        BigDouble total = 1;
        if(data.clickUpgradeLevel[1] == 0)
            return total;
        total += UpgradeManager.instance.clickUpgradesBasePower[1] * data.clickUpgradeLevel[1];
        total -= 1;
        return total;
    }

    public BigDouble PixlPerSecond()
    {
        BigDouble total = 0;
        for(int i = 0; i < data.productionUpgradeLevel.Count; i++)
        {
            total += UpgradeManager.instance.productionUpgradesBasePower[i] * data.productionUpgradeLevel[i];
        }
        return total;
    }

    public void GeneratePixl()
    {
        var sequence = DOTween.Sequence();

        sequence.Insert(0, pixlImage.transform.DOScale(new Vector2(1f, 1f), 0.05f));
        sequence.Insert(0, pixlImage.transform.DOScale(new Vector2(4f, 4f), 0.25f));
        
        data.pixlAmount += ClickPower();
    }

    public void ShopController()
    {
        if(shopUI.activeSelf)
        {
            shopUI.SetActive(false);
        } 

        else
        {
            leaderboardUI.SetActive(false);
            shopUI.SetActive(true);
        }
    }

    
}
