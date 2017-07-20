using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Animator _hanumanAnimator;
    AnimatorStateInfo animStateInfo;
    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            Debug.Log("Hit Enemy.");
            animStateInfo = _hanumanAnimator.GetCurrentAnimatorStateInfo(0);
        
            if (AttackBehaviour.inRunAttackState)
            {
                collider.gameObject.GetComponent<SpawnsAction>().KillEnemy();
            }

            AttackBehaviour.inRunAttackState = false;
        }
    }
}
