using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;


public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    private void Awake() => instance = this;

    public List<Upgrades> specialUpgrades;
    public List<Upgrades> productionUpgrades;
    public Upgrades productionUpgradePrefab;
    public Upgrades clickUpgradePrefab;
    public GameObject ppsItemsPrefab;

    public ScrollRect clickUpgradesScroll;
    public Transform clickUpgradesPanel;

    public ScrollRect productionUpgradesScroll;
    public Transform productionUpgradesPanel;

    public string[] clickUpgradeNames;
    public string[] productionUpgradeNames;
    public string[] clickUpgradePerks;
    public string[] productionUpgradePerks;
    

    public BigDouble[] clickUpgradesBaseCost;
    public BigDouble[] clickUpgradesCostMult;
    public BigDouble[] clickUpgradesBasePower;

    public BigDouble[] productionUpgradesBaseCost;
    public BigDouble[] productionUpgradesCostMult;
    public BigDouble[] productionUpgradesBasePower;

    public void StartUpdateManager()
    {
        //Methods.UpgradeCheck(GameManager.instance.data.clickUpgradeLevel, 2);

        clickUpgradeNames = new []{"Offline Progress", "Power Click"};
        clickUpgradePerks = new []{"+5%", "2x Per Click"};
        clickUpgradesBaseCost = new BigDouble[] {10000, 5};
        clickUpgradesCostMult = new BigDouble[] {2.5, 2.5};
        clickUpgradesBasePower = new BigDouble[] {5, 2};
        
        productionUpgradeNames = new []{"Sappy Seal", "Bronze Goat", "Pixl Pet", "Silver Goat", "Gold Goat", "Egg Incubator", "Pixel Palace", "Diety Pet", "1 of 1"};
        productionUpgradePerks = new []{"PPS   +0.1", "PPS   +0.3", "PPS   +1", "PPS   +3", "PPS   +6", "PPS   +20", "PPS   +40", "PPS   +70", "PPS   +300"};
        productionUpgradesBaseCost = new BigDouble[] {3, 100, 1000, 10000, 50000, 200000, 500000, 1000000, 5000000};
        //productionUpgradesCostMult = new BigDouble[] {1.5, 1.75, 2, 3};
        productionUpgradesBasePower = new BigDouble[] {0.1, 0.3, 1, 3, 6, 20, 40, 70, 300};

        for(int i = 0; i < GameManager.instance.data.clickUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(clickUpgradePrefab, productionUpgradesPanel);
            upgrade.UpgradeID = i; 
            specialUpgrades.Add(upgrade);
        }

        Instantiate(ppsItemsPrefab, productionUpgradesPanel);

        for(int i = 0; i < GameManager.instance.data.productionUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(productionUpgradePrefab, productionUpgradesPanel);
            upgrade.UpgradeID = i; 
            productionUpgrades.Add(upgrade);
        }

        //clickUpgradesScroll.normalizedPosition = new Vector2(0,0);
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
                    for(int i = 0; i < specialUpgrades.Count; i++) UpdateUI(specialUpgrades, data.clickUpgradeLevel, clickUpgradeNames, clickUpgradePerks, i);
                }

                else UpdateUI(specialUpgrades, data.clickUpgradeLevel, clickUpgradeNames, clickUpgradePerks, UpgradeID);
                break;

            case "production":
                if(UpgradeID == -1)
                {
                    for(int i = 0; i < productionUpgrades.Count; i++) UpdateUI(productionUpgrades, data.productionUpgradeLevel, productionUpgradeNames, productionUpgradePerks, i);
                }
                else UpdateUI(productionUpgrades, data.productionUpgradeLevel, productionUpgradeNames, productionUpgradePerks, UpgradeID);
                break;
        }

        void UpdateUI(List<Upgrades> upgrades,List<int> upgradeLevels, string[] upgradeNames, string[] upgradePerks, int ID)
        {
            upgrades[ID].LevelText.text = "Lvl: " + upgradeLevels[ID].ToString();          
            upgrades[ID].CostText.text = $"{UpgradeCost(type, ID):F0}";
            upgrades[ID].NameText.text = upgradeNames[ID];
            upgrades[ID].PerkText.text = upgradePerks[ID];
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
                * BigDouble.Pow(1.3, (BigDouble)data.productionUpgradeLevel[UpgradeID]);
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

