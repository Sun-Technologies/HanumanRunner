using UnityEngine;
using System.Collections;

public class TextureScroller : MonoBehaviour
{

    public float scrollSpeed = 1.5f;
    public Vector3 endPosition;
    public Vector3 startPosition;

    Transform bgObj;

    void Start()
    {
        bgObj = transform;
    }

    void Update()
    {
        if (bgObj.position.x <= endPosition.x)
        {
            bgObj.position = startPosition;
        }

        if (Time.timeScale == 1)
        {
            bgObj.position += Vector3.left * Time.deltaTime * scrollSpeed;
        }
        else
        {
            scrollSpeed = 0;
        }
    }
}