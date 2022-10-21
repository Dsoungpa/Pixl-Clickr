using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;
using System.Linq;
using System;

[Serializable]
public class Data
{
    public BigDouble pixlAmount;
    public List<int> clickUpgradeLevel;
    public List<int> productionUpgradeLevel;
    public bool offlineProgressCheck = false;

    public Data()
    {
        pixlAmount = 0;
        clickUpgradeLevel = new int[4].ToList();
        productionUpgradeLevel = new int[4].ToList();
    }
}
