using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{

    public static Action<int> UpgradeButtonClicked;
    public static Action<EnemyController> EnemySpawned;
    public static Action<EnemyController> EnemyDestroyed;
    public static Action<SoldierController> SoldierOnAttack;
    public static Action<SoldierController> SoldierMerged;
    public static Action<bool> PlayerCanMerge;
}