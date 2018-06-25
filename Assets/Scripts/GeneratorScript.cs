using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratorScript : MonoBehaviour
{

    public GameObject[] availableRooms;

    public List<GameObject> currentRooms;

    private float screenWidthInPoints;

    public GameObject[] availableObjects;
    public List<GameObject> objects;

    public float objectsMinDistance = 5.0f;
    public float objectsMaxDistance = 10.0f;

    public float objectsMinY = -1.4f;
    public float objectsMaxY = 1.4f;

    public float objectsMinRotation = -45.0f;
    public float objectsMaxRotation = 45.0f;

    private Vector3 laserPos = new Vector3(0, -0.99f, 0);
    HanumanGearInfo _hanumanGearInfo;

    // Use this for initialization
    void Start()
    {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
        _hanumanGearInfo = GetComponent<HanumanGearInfo>();
    }

    void FixedUpdate()
    {

        GenerateRoomIfRequred();

        GenerateObjectsIfRequired();
    }


    void AddRoom(float farhtestRoomEndX)
    {
        //1
        int randomRoomIndex = Random.Range(0, availableRooms.Length);

        //2
        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);

        //3
        float roomWidth = room.transform.Find("floor").localScale.x;

        //4
        float roomCenter = farhtestRoomEndX + roomWidth * 0.5f;

        //5
        room.transform.position = new Vector3(roomCenter, 0, 0);

        //6
        currentRooms.Add(room);
    }

    void GenerateRoomIfRequred()
    {
        //1
        List<GameObject> roomsToRemove = new List<GameObject>();

        //2
        bool addRooms = true;

        //3
        float playerX = transform.position.x;

        //4
        float removeRoomX = playerX - screenWidthInPoints;

        //5
        float addRoomX = playerX + screenWidthInPoints;

        //6
        float farthestRoomEndX = 0;

        foreach (var room in currentRooms)
        {
            try
            {
                //7
                float roomWidth = room.transform.Find("floor").localScale.x;
                float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
                float roomEndX = roomStartX + roomWidth;

                //8
                if (roomStartX > addRoomX)
                    addRooms = false;

                //9
                if (roomEndX < removeRoomX)
                    roomsToRemove.Add(room);

                //10
                farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
            }
            catch (System.Exception ex)
            {
                Debug.Log("ERROR: " + ex.Message);
            }
        }

        //11
        foreach (var room in roomsToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }

        //12
        if (addRooms)
            AddRoom(farthestRoomEndX);
    }

    void AddObject(float lastObjectX)
    {
        int randomIndex;
        if (GetComponent<HanumanController>().hasEquippedGada || GetComponent<HanumanController>().isTempGadaOn)  //Temp hack to spawn gada only when it's not equipped
        {
            randomIndex = Random.Range(0, availableObjects.Length - 1);
        }
        else
        {
            if (HanumanGearInfo.levelIndex >= 1)    //temp hack to not span gada in default character
            {
                randomIndex = Random.Range(0, availableObjects.Length);
            }
            else
            {
                randomIndex = Random.Range(0, availableObjects.Length - 1);
            }
        }

        //2
        GameObject obj = (GameObject)Instantiate(availableObjects[randomIndex]);

        //3
        float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
        float rotation = Random.Range(objectsMinRotation, objectsMaxRotation);

        float randomY = Random.Range(objectsMinY, objectsMaxY);
        obj.transform.position = new Vector3(objectPositionX, randomY, 0);
        if (!obj.name.Equals("Lightning(Clone)") && !obj.name.Equals("SpikeWall(Clone)") && !obj.name.Equals("Rakshas_Ground(Clone)") && !obj.name.Equals("Winged_Rakshas(Clone)") && !obj.name.Equals("Demon_Ground(Clone)") && !obj.name.Equals("Flying_Beast(Clone)") && !obj.name.Equals("Winged_Rakshas(Clone)") && !obj.name.Equals("Boulder(Clone)") && !obj.name.Equals("Snake(Clone)") && !obj.name.Equals("LavaPatch(Clone)"))
        {
            obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);

        }
        if (obj.name.Equals("Lightning(Clone)"))
        {
            obj.transform.position = new Vector3(objectPositionX, -1.3f, 0);
        }

        if (obj.name.Equals("Rakshas_Ground(Clone)") || obj.name.Equals("Boulder(Clone)") || obj.name.Equals("Snake(Clone)") || obj.name.Equals("LavaPatch(Clone)"))
        {
            obj.transform.position = new Vector3(objectPositionX, -1.96f, 0);
        }

        if (obj.name.Equals("Demon_Ground(Clone)"))
        {
            obj.transform.position = new Vector3(objectPositionX, -1.35f, 0);
        }

        if (obj.name.Equals("LavaPatch(Clone)"))
        {
            obj.transform.position = new Vector3(objectPositionX, -2.32f, 0);
        }
        if (obj.name.Equals("Croc(Clone)"))
        {
            obj.transform.position = new Vector3(objectPositionX, -1.85f, 0);
            obj.transform.rotation = Quaternion.identity;
        }
        if (obj.name.Equals("OctoCrab(Clone)"))
        {
            obj.transform.position = new Vector3(objectPositionX, -1.85f, 0);
            obj.transform.rotation = Quaternion.identity;
        }
       

        objects.Add(obj);
    }

    void GenerateObjectsIfRequired()
    {
        //1
        float playerX = transform.position.x;
        float removeObjectsX = playerX - screenWidthInPoints;
        float addObjectX = playerX + screenWidthInPoints;
        float farthestObjectX = 0;

        //2
        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (var obj in objects)
        {
            //3
            float objX = obj.transform.position.x;

            //4
            farthestObjectX = Mathf.Max(farthestObjectX, objX);

            //5
            if (objX < removeObjectsX)
                objectsToRemove.Add(obj);
        }

        //6
        foreach (var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }

        //7
        if (farthestObjectX < addObjectX)
            AddObject(farthestObjectX);
    }
}
