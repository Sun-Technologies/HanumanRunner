using UnityEngine;

public class FireballScript : MonoBehaviour
{
    Transform player;
    Transform myTransform;
    public float speed = 2.0f;

    void Start()
    {
        player = GameObject.Find("Hanuman").transform;
        myTransform = transform;
        if (gameObject != null)
        {
            Destroy(gameObject, 3);
        }
    }

    void Update()
    {
        myTransform.Translate(Vector3.left * Time.deltaTime * speed);
    }
}
