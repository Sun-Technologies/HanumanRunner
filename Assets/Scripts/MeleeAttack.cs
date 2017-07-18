using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Animator _hanumanAnimator;
    AnimatorStateInfo animStateInfo;
    void Start()
    {
        Debug.Log("Layer count = " + _hanumanAnimator.layerCount);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            Debug.Log("Hit Enemy.");
            animStateInfo = _hanumanAnimator.GetCurrentAnimatorStateInfo(0);
            Debug.Log("Current state tag = " + animStateInfo.tagHash);
            if (animStateInfo.IsName("run"))
            {
                Debug.Break();
            } 
            if (animStateInfo.IsTag("Attack"))
            {
                collider.gameObject.GetComponent<SpawnsAction>().KillEnemy();
            }
            //if (GetComponentInParent<HanumanController>().CanKillEnemies)
            //{
                
            //}
        }
    }
}
