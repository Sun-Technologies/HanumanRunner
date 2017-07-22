using UnityEngine;

public class BirdsAnimation : MonoBehaviour
{

    public float objectsMinY = -2f;
    public float objectsMaxY = 1.6f;
    public float secondsForOneLength = 20f;
    Transform myTransform;
    void Start()
    {
        myTransform = transform;
    }

    void Update()
    {
        DoVerticalAnimation(myTransform);
    }

    void DoVerticalAnimation(Transform obj)
    {
        Vector3 minPos = new Vector3(obj.position.x, objectsMinY, 41);
        Vector3 maxPos = new Vector3(obj.position.x, objectsMaxY, 41);
        obj.transform.position = Vector3.Lerp(minPos, maxPos,
                                                   Mathf.SmoothStep(0f, 1f,
                                                   Mathf.PingPong(Time.time / secondsForOneLength, 1f)
                                                   ));
    }
}
