using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoldierManager : MonoBehaviour
{
    public GameObject level_1_Soldier;
    public GameObject level_2_Soldier;
    public GameObject level_3_Soldier;

    public Transform spawnPoint;

    public List<Transform> emptyAttackPoint;
    public List<Transform> takenAttackPoint;
    public List<EnemyController> allEnemies;
    public List<SoldierController> level_1_Soldiers;
    public List<SoldierController> level_2_Soldiers;
    public List<SoldierController> level_3_Soldiers;
    private void OnEnable()
    {
        EventManager.UpgradeButtonClicked += UpgradeButtonClicked;
        EventManager.EnemySpawned += EnemySpawned;
        EventManager.EnemyDestroyed += EnemyDestroyed;
        EventManager.SoldierOnAttack += SoldierOnAttack;
    }

    private void SoldierOnAttack(SoldierController soldier)
    {
        if (soldier.level==1)
        {
            if (!level_1_Soldiers.Contains(soldier))
            {
                level_1_Soldiers.Add(soldier);

            }
        }
        else if (soldier.level==2)
        {
            if (!level_2_Soldiers.Contains(soldier))
            {
                level_2_Soldiers.Add(soldier);

            }
        }
        else if (soldier.level==3)
        {
            if (!level_3_Soldiers.Contains(soldier))
            {
                level_3_Soldiers.Add(soldier);

            }
        }

        if (level_1_Soldiers.Count>=3)
        {
            EventManager.PlayerCanMerge(true);
        }
        else if (level_2_Soldiers.Count>=3)
        {
            EventManager.PlayerCanMerge(true);
        }
        else if (level_3_Soldiers.Count>=3)
        {
            EventManager.PlayerCanMerge(true);
        }
        else
        {
            EventManager.PlayerCanMerge(false);
        }
    }

    private void EnemyDestroyed(EnemyController enemy)
    {
        allEnemies.Remove(enemy);
    }

    private void EnemySpawned(EnemyController enemy)
    {
        allEnemies.Add(enemy);
    }

    private void OnDisable()
    {
        EventManager.SoldierOnAttack -= SoldierOnAttack;
        EventManager.EnemyDestroyed -= EnemyDestroyed;
        EventManager.EnemySpawned -= EnemySpawned;
        EventManager.UpgradeButtonClicked -= UpgradeButtonClicked;
    }

    private void UpgradeButtonClicked(int index)
    {
        if (index==0)
        {
            SpawnSoldier();
        }
        else if (index==1)
        {
            MergeSoldiers();
        }
    }

    public void MergeSoldiers()
    {
        if (level_1_Soldiers.Count>=3)
        {
            var firstSoldier = level_1_Soldiers[0];
            var secondSoldier = level_1_Soldiers[1];
            var thirdSoldier = level_1_Soldiers[2];
            takenAttackPoint.Remove(firstSoldier.attackPoint);
            takenAttackPoint.Remove(secondSoldier.attackPoint);
            emptyAttackPoint.Add(secondSoldier.attackPoint);
            emptyAttackPoint.Add(firstSoldier.attackPoint);

            level_1_Soldiers.RemoveRange(0,3);
            firstSoldier.Merge(firstSoldier.transform.position);
            secondSoldier.Merge(firstSoldier.transform.position);
            thirdSoldier.Merge(firstSoldier.transform.position);
            DOVirtual.Float(0, 1, 1, (x) => { x = x; }).OnComplete(() =>
            {
                var temp = Instantiate(level_2_Soldier, firstSoldier.transform.position, quaternion.identity);
                temp.GetComponent<SoldierController>().Attack();
                level_2_Soldiers.Add(temp.GetComponent<SoldierController>());
                Destroy(firstSoldier.gameObject);
                Destroy(secondSoldier.gameObject);
                Destroy(thirdSoldier.gameObject);
            });
        }
    }

    

    public void SpawnSoldier()
    {
        if (emptyAttackPoint.Count==0)
        {
            foreach (var point in takenAttackPoint)
            {
                point.position += new Vector3(0, 0, -1);
                emptyAttackPoint.Add(point);
            }
            takenAttackPoint.Clear();
        }
        var rand = Random.Range(0, emptyAttackPoint.Count);
        var temp = Instantiate(level_1_Soldier, spawnPoint.position, quaternion.identity);
        temp.GetComponent<SoldierController>().RunToAttackPoint(emptyAttackPoint[rand].position);
        temp.GetComponent<SoldierController>().allEnemies = allEnemies;
        temp.GetComponent<SoldierController>().attackPoint = emptyAttackPoint[rand];
        takenAttackPoint.Add(emptyAttackPoint[rand]);
        emptyAttackPoint.RemoveAt(rand);
    }
}
