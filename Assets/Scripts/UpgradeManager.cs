using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;


public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    private void Awake() => instance = this;

    public List<Upgrades> clickUpgrades;
    public Upgrades clickUpgradePrefab;

    public List<Upgrades> productionUpgrades;
    public Upgrades productionUpgradePrefab;

    public ScrollRect clickUpgradesScroll;
    public Transform clickUpgradesPanel;

    public ScrollRect productionUpgradesScroll;
    public Transform productionUpgradesPanel;

    public string[] clickUpgradeNames;
    public string[] productionUpgradeNames;

    public BigDouble[] clickUpgradesBaseCost;
    public BigDouble[] clickUpgradesCostMult;
    public BigDouble[] clickUpgradesBasePower;

    public BigDouble[] productionUpgradesBaseCost;
    public BigDouble[] productionUpgradesCostMult;
    public BigDouble[] productionUpgradesBasePower;

    public void StartUpdateManager()
    {
        Methods.UpgradeCheck(GameManager.instance.data.clickUpgradeLevel, 3);

        clickUpgradeNames = new []{"Click Power + 1", "Click Power +5", "Click Power +10", "Click Power +25"};
        clickUpgradesBaseCost = new BigDouble[] {10, 50, 100, 1000};
        clickUpgradesCostMult = new BigDouble[] {1.25, 1.35, 1.55, 2};
        clickUpgradesBasePower = new BigDouble[] {1, 5, 10, 25};
        
        productionUpgradeNames = new []{"+1 Pixl/s", "+2 Pixl/s", "+10 Pixl/s", "+100 Pixl/s"};
        productionUpgradesBaseCost = new BigDouble[] {25, 100, 1000, 10000};
        productionUpgradesCostMult = new BigDouble[] {1.5, 1.75, 2, 3};
        productionUpgradesBasePower = new BigDouble[] {1, 2, 10, 100};

        for(int i = 0; i < GameManager.instance.data.clickUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(clickUpgradePrefab, clickUpgradesPanel);
            upgrade.UpgradeID = i; 
            clickUpgrades.Add(upgrade);
        }

        for(int i = 0; i < GameManager.instance.data.productionUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(productionUpgradePrefab, productionUpgradesPanel);
            upgrade.UpgradeID = i; 
            productionUpgrades.Add(upgrade);
        }

        clickUpgradesScroll.normalizedPosition = new Vector2(0,0);
        productionUpgradesScroll.normalizedPosition = new Vector2(0,0);

        UpdateUpgradeUI("click");
        UpdateUpgradeUI("production");
    }


    public void UpdateUpgradeUI(string type, int UpgradeID = -1)
    {
        var data = GameManager.instance.data;

        switch(type)
        {
            case "click":
                if(UpgradeID == -1)
                {
                    for(int i = 0; i < clickUpgrades.Count; i++) UpdateUI(clickUpgrades, data.clickUpgradeLevel, clickUpgradeNames, i);
                }
                else UpdateUI(clickUpgrades, data.clickUpgradeLevel, clickUpgradeNames, UpgradeID);
                break;

            case "production":
                if(UpgradeID == -1)
                {
                    for(int i = 0; i < productionUpgrades.Count; i++) UpdateUI(productionUpgrades, data.productionUpgradeLevel, productionUpgradeNames, i);
                }
                else UpdateUI(productionUpgrades, data.productionUpgradeLevel, productionUpgradeNames, UpgradeID);
                break;
        }

        void UpdateUI(List<Upgrades> upgrades,List<int> upgradeLevels, string[] upgradeNames, int ID)
        {
            upgrades[ID].LevelText.text = "Lvl: " + upgradeLevels[ID].ToString();
            upgrades[ID].CostText.text = $"Cost: {UpgradeCost(type, ID):F0} Pixl!";
            upgrades[ID].NameText.text = upgradeNames[ID];
        }
        
    }

    public BigDouble UpgradeCost(string type, int UpgradeID){

        var data = GameManager.instance.data;

        switch(type)
        {
            case "click":
                return clickUpgradesBaseCost[UpgradeID] 
                * BigDouble.Pow(clickUpgradesCostMult[UpgradeID], (BigDouble)data.clickUpgradeLevel[UpgradeID]);
            break;

            case "production":
                return productionUpgradesBaseCost[UpgradeID] 
                * BigDouble.Pow(productionUpgradesCostMult[UpgradeID], (BigDouble)data.productionUpgradeLevel[UpgradeID]);
            break;
        }

        return 0;
        
    } 

    public void BuyUpgrade(string type, int UpgradeID)
    {
        var data = GameManager.instance.data;

        switch(type)
        {
            case "click": Buy(data.clickUpgradeLevel);
                break;
            case "production": Buy(data.productionUpgradeLevel);
                break;
        }

        void Buy(List<int> upgradeLevels)
        {
            if(data.pixlAmount >= UpgradeCost(type, UpgradeID))
            {
                data.pixlAmount -= UpgradeCost(type, UpgradeID);
                upgradeLevels[UpgradeID] += 1;
            }
            UpdateUpgradeUI(type, UpgradeID);
        }
    }
}

