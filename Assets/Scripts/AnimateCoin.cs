using UnityEngine;
using System.Collections;

public class AnimateCoin : MonoBehaviour
{
    Vector3 HanumanPos;
    void Start()
    {
        HanumanPos = GameObject.Find("Hanuman").transform.position;
    }

    void Update()
    {
        Vector3.MoveTowards(transform.position,new Vector3(HanumanPos.x - 2, 2.8f, -10), Time.deltaTime * 5);
    }
}
