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
    [SerializeField] private GameObject clickTextPrefab;
    [SerializeField] private GameObject clickButton;
    [SerializeField] private Scrollbar clickScrollBar;
    [SerializeField] private Scrollbar shopScrollBar;
    [SerializeField] private Vector2 shopUITransform;
    //[SerializeField] private BigDouble clickPowertotal = 1;


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
        pixlClickPowerText.text = "+" + ((long)ClickPower()).ToString("n0") + " Pixl";
        pixlPerSecondText.text = ((long)PixlPerSecond()).ToString("n0") + " per sec";

        SaveTime += Time.deltaTime * (1 / Time.timeScale);
        if(SaveTime >= 0.5f)
        {
            SaveSystem.SaveData(data, dataFileName);
            SaveTime = 0;
        }

    }

    public BigDouble ClickPower()
    {
        BigDouble clickPowertotal = 1;
        if(data.clickUpgradeLevel[1] == 0)
            return clickPowertotal;


        print((float)UpgradeManager.instance.clickUpgradesBasePower[1]);
        print((float)data.clickUpgradeLevel[1]);
        clickPowertotal += Mathf.Pow((float)UpgradeManager.instance.clickUpgradesBasePower[1], (float)data.clickUpgradeLevel[1]);
        clickPowertotal -= 1;
        return clickPowertotal;
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

        GameObject clickText = Instantiate(clickTextPrefab, clickButton.transform);
        
        clickText.transform.position += new Vector3(Random.Range(-20, 100), Random.Range(-60, 25), 0);
        clickText.GetComponent<TMP_Text>().text = "+" + ((long)ClickPower()).ToString("n0");

        sequence.Insert(0, clickText.transform.DOMove(new Vector2(clickText.transform.position.x, clickText.transform.position.y + 1000), 5f));
        sequence.Insert(0, clickText.GetComponent<TextMeshProUGUI>().DOFade(0f, 0.8f));
        


        
        data.pixlAmount += ClickPower();
    }

    
}
