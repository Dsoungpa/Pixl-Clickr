using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BreakInfinity;

public class OfflineManager : MonoBehaviour
{
    public TMP_Text timeAwayText;
    public TMP_Text pixlGainsText;
    public GameObject offlinePopUp;

    public static OfflineManager instance;
    private void Awake() => instance = this;

    public void LoadOfflineProduction(){
        var data = GameManager.instance.data;


        // if(data.offlineProgressCheck)
        // {
            // Offline Time Management
            var tempOfflineTime = Convert.ToInt64(PlayerPrefs.GetString("OfflineTime"));
            var oldTime = DateTime.FromBinary(tempOfflineTime);
            var currentTime = DateTime.Now;
            var difference = currentTime.Subtract(oldTime);
            var rawTime = (float)difference.TotalSeconds;

            // This can be used to change the ratio amount earned offline;
            var offlineTime = rawTime;

            offlinePopUp.gameObject.SetActive(true);
            TimeSpan timer = TimeSpan.FromSeconds(rawTime);
            timeAwayText.text = $"You were away for\n<color=#00FFFF>{timer:dd\\:hh\\:mm\\:ss}</color>";

            BigDouble pixlGains = (GameManager.instance.PixlPerSecond() * offlineTime) * GetOfflineRatio();
            data.pixlAmount += pixlGains;
            pixlGainsText.text = $"You earned \n+{pixlGains:F0} Pixl!";
        // }
    }

    public void CloseOffline()
    {
        offlinePopUp.gameObject.SetActive(false);
    }

    public BigDouble GetOfflineRatio()
    {
        return (UpgradeManager.instance.clickUpgradesBasePower[0] * GameManager.instance.data.clickUpgradeLevel[0]) / 100;
    }
}


