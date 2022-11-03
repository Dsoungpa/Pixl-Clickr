using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrades : MonoBehaviour
{
    public int UpgradeID;
    public Image UpgradeButton;
    public Image UpgradeIcon;
    public TMP_Text LevelText;
    public TMP_Text NameText;
    public TMP_Text CostText;
    public TMP_Text PerkText;

    public void BuyClickUpgrade() => UpgradeManager.instance.BuyUpgrade("click", UpgradeID);
    public void BuyProductionUpgrade() => UpgradeManager.instance.BuyUpgrade("production", UpgradeID);

}
