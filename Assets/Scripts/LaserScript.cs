using UnityEngine;
using System.Collections;

public enum ObstacleType { RotatingSpike, SpikeWall, GroundedRakhshas, FlyingRakhshas, Laser };


public class LaserScript : MonoBehaviour
{
    public ObstacleType obstacleType;
    public GameObject FireballObj;
    public Sprite laserOnSprite;
    public Sprite laserOffSprite;

    public float interval = 0.5f;
    public float rotationSpeed = 0.0f;

    private bool isLaserOn = true;
    private float timeUntilNextToggle;

    public float objectsMinY = -2f;
    public float objectsMaxY = 1.6f;
    public float secondsForOneLength = 20f;

    public bool isSpikeBall = false;

    void Start()
    {
        timeUntilNextToggle = interval;
        StartCoroutine(SpitFireball());
    }

    IEnumerator SpitFireball()
    {
        if (obstacleType == ObstacleType.FlyingRakhshas)
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
        if (obstacleType == ObstacleType.RotatingSpike)
        {
            transform.RotateAround(transform.position,
                              Vector3.forward,
                              rotationSpeed * Time.fixedDeltaTime);
        }

        if (obstacleType == ObstacleType.RotatingSpike || obstacleType == ObstacleType.FlyingRakhshas)
        {
            DoVerticalAnimation(transform);
        }
    }
}
