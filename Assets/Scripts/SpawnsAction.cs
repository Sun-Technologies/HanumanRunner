using UnityEngine;
using System.Collections;

public enum SpawnsType
{
    RotatingSpike,
    SpikeWall,
    GroundedRakhshas,
    FlyingRakhshas,
    Lightning,
    Snake,
    FlyingBeast,
    Boulder
};

public class SpawnsAction : MonoBehaviour
{
    public SpawnsType spawnsType;
    public GameObject FireballObj;

    public float interval = 0.5f;
    public float rotationSpeed = 0.0f;

    private float timeUntilNextToggle;

    public float objectsMinY = -2f;
    public float objectsMaxY = 1.6f;
    public float secondsForOneLength = 20f;

    public bool canFly = false;
    public bool alwaysGrounded = false;
    public bool alwaysSpin = false;
    public bool staticOnAir = false;


    void Start()
    {
        timeUntilNextToggle = interval;
        if (spawnsType == SpawnsType.FlyingRakhshas)
        {
            StartCoroutine(SpitFireball());
        }
    }

    IEnumerator SpitFireball()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 3));
        if (FireballObj != null)
        {
            Instantiate(FireballObj, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Fireball object not found.");
        }
    }

    void DoVerticalAnimation(Transform obj)
    {
        Vector3 minPos = new Vector3(obj.position.x, objectsMinY, 0);
        Vector3 maxPos = new Vector3(obj.position.x, objectsMaxY, 0);
        obj.transform.position = Vector3.Lerp(minPos, maxPos,
                                                   Mathf.SmoothStep(0f, 1f,
                                                   Mathf.PingPong(Time.time / secondsForOneLength, 1f)
                                                   ));
    }

    void FixedUpdate()
    {
        if (alwaysSpin)
        {
            transform.RotateAround(transform.position,
                              Vector3.forward,
                              rotationSpeed * Time.fixedDeltaTime);
        }

        if (canFly)
        {
            DoVerticalAnimation(transform);
        }
    }
}
