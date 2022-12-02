using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public EnemyStates state;
    public Animator animator;
    private void Update()
    {
        if (state==EnemyStates.Run)
        {
            transform.position += new Vector3(0, 0, -1) * speed * Time.deltaTime;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            state = EnemyStates.AttackTheWall;
            animator.SetBool("gun",true);
        }

        if (other.GetComponent<Bullet>())
        {
            EventManager.EnemyDestroyed(this);
            Destroy(gameObject);
        }
    }
}
