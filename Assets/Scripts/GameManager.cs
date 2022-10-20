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

    [Header("Value Variables")]
    [SerializeField] private float pixlAmount = 0;

    [Header("UI References")]
    [SerializeField] private TMP_Text pixlAmountText;
    [SerializeField] private TMP_Text pixlClickPowerText;
    [SerializeField] private TMP_Text pixlPerSecondText;
    [SerializeField] private Image pixlImage;
    [SerializeField] private GameObject clickShop;
    [SerializeField] private GameObject productionShop;
    [SerializeField] private Scrollbar clickScrollBar;
    [SerializeField] private Scrollbar productionScrollBar;


    private const string dataFileName = "Pixl_Clickr";
    void Start()
    {
        data = SaveSystem.SaveExists(dataFileName) 
            ? SaveSystem.LoadData<Data>(dataFileName) : new Data();
        UpgradeManager.instance.StartUpdateManager();
    }

    public float SaveTime;

    void Update()
    {
        data.pixlAmount += PixlPerSecond() * Time.deltaTime;
        pixlAmountText.text = $"{data.pixlAmount:F0} Pixl!";
        pixlClickPowerText.text = "+" + ClickPower() + " Pixl";
        pixlPerSecondText.text = $"{PixlPerSecond():F0} per sec";

        SaveTime += Time.deltaTime * (1 / Time.timeScale);
        if(SaveTime >= 3)
        {
            SaveSystem.SaveData(data, dataFileName);
            SaveTime = 0;
        }

    }

    public BigDouble ClickPower()
    {
        BigDouble total = 1;
        for(int i = 0; i < data.clickUpgradeLevel.Count; i++)
        {
            total += UpgradeManager.instance.clickUpgradesBasePower[i] * data.clickUpgradeLevel[i];
        }
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

        sequence.Insert(0, pixlImage.transform.DOScale(new Vector2(0.5f, 0.5f), 0.05f));
        sequence.Insert(0, pixlImage.transform.DOScale(new Vector2(2f, 2f), 0.25f));
        
        data.pixlAmount += ClickPower();
    }

    public void ClickShopController()
    {
        if(clickShop.activeSelf)
        {
            clickShop.SetActive(false);
        } 

        else
        {
            clickShop.SetActive(true);
            clickScrollBar.value = 1;
        }
    }

    public void ProductionShopController()
    {
        if(productionShop.activeSelf)
        {
            productionShop.SetActive(false);
        } 

        else
        {
            productionShop.SetActive(true);
            productionScrollBar.value = 1;
        }
    }
}
