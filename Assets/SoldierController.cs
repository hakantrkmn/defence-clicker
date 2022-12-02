using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    public Animator animator;
    public SoldierStates state;
    public List<EnemyController> allEnemies;
    public EnemyController currentEnemy;
    private float timer;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public int level;
    public Transform attackPoint;
    private void OnEnable()
    {
        EventManager.EnemySpawned += EnemySpawned;
        EventManager.EnemyDestroyed += EnemyDestroyed;
    }

    private void EnemyDestroyed(EnemyController enemy)
    {
        allEnemies.Remove(enemy);
        currentEnemy = null;
    }

    private void OnDisable()
    {
        EventManager.EnemyDestroyed -= EnemyDestroyed;
        EventManager.EnemySpawned -= EnemySpawned;
    }

    private void EnemySpawned(EnemyController enemy)
    {
        allEnemies.Add(enemy);
    }

    private void Start()
    {
        state = SoldierStates.Run;
    }

    public void Attack()
    {
        animator.SetBool("gun", true);
        state = SoldierStates.Attack;
        EventManager.SoldierOnAttack(this);
    }
    public void Merge(Vector3 mergePos)
    {
        state = SoldierStates.Merge;
        transform.DOMove(mergePos, 1f);
        transform.LookAt(mergePos);
    }

    public void RunToAttackPoint(Vector3 pos)
    {
        transform.DOMove(pos, 2).OnComplete(() =>
        {
            Attack();
        });
        transform.DOLookAt(pos, .2f);
    }

    public void CheckForClosestEnemy()
    {
        foreach (var enemy in allEnemies)
        {
            if (Mathf.Abs((enemy.transform.position - transform.position).magnitude) < 50)
            {
                currentEnemy = enemy;
                break;
            }
        }
    }

    private void Update()
    {
        if (state == SoldierStates.Attack)
        {
            if (currentEnemy == null)
            {
                CheckForClosestEnemy();
            }
            else
            {
                AttackToEnemy();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position,30);
    }
    public void AttackToEnemy()
    {
        transform.LookAt(currentEnemy.transform.position);
        timer += Time.deltaTime;
        if (timer > .5f)
        {
            timer = 0;
            var temp = Instantiate(bulletPrefab, bulletSpawnPoint.position, quaternion.identity);
            temp.GetComponent<Bullet>().direction = (currentEnemy.transform.position - transform.position).normalized;
            Destroy(temp.gameObject,4);
        }
    }
}