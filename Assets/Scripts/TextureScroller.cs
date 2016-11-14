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

        bgObj.position += Vector3.left * scrollSpeed;
    }
}