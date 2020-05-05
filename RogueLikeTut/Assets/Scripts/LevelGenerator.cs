using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{

    public GameObject layoutRoom;
    public int distanceToEnd;
    public Color startColor, endColor;

    public Transform generatorPoint;

    public enum Direction { up, right, down, left};
    public Direction selectedDirection;

    public float xOffset = 18f, yOffset = 10f;
    public LayerMask whatIsRoom;
    private GameObject endRoom;
    private List<GameObject> layoutRoomObjects = new List<GameObject>();
    public RoomPrefabs rooms;
    private List<GameObject> roomOutlines = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;
        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for(int i = 0; i< distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            layoutRoomObjects.Add(newRoom);
            if (i+1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();
            while(Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }


        }

        //create room outlines
        createRoomOutline(Vector3.zero);
        foreach(GameObject room in layoutRoomObjects)
        {
            createRoomOutline(room.transform.position);
        }
        createRoomOutline(endRoom.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
        }
    }

    public void createRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), .2f, whatIsRoom);
        bool roombelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), .2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), .2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), .2f, whatIsRoom);

        int directionCount = 0;

        if(roomAbove)
        {
            directionCount++;
        }
        if (roombelow)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }

        switch (directionCount)
        {
            case 0:
                Debug.LogError("Found no room exists!!");
                break;
            case 1:
                if(roomAbove)
                {
                    roomOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }
                if (roombelow)
                {
                    roomOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }
                if (roomLeft)
                {
                    roomOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }
                if (roomRight)
                {
                    roomOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }
                break;
            case 2:
                if(roomAbove && roombelow)
                {
                    roomOutlines.Add(Instantiate(rooms.doubleUD, roomPosition, transform.rotation));
                }
                if (roomAbove && roomLeft)
                {
                    roomOutlines.Add(Instantiate(rooms.doubleUL, roomPosition, transform.rotation));
                }
                if (roomAbove && roomRight)
                {
                    roomOutlines.Add(Instantiate(rooms.doubleUR, roomPosition, transform.rotation));
                }
                if (roombelow && roomLeft)
                {
                    roomOutlines.Add(Instantiate(rooms.doubleDL, roomPosition, transform.rotation));
                }
                if (roombelow && roomRight)
                {
                    roomOutlines.Add(Instantiate(rooms.doubleRD, roomPosition, transform.rotation));
                }
                if (roomRight && roomLeft)
                {
                    roomOutlines.Add(Instantiate(rooms.doubleLR, roomPosition, transform.rotation));
                }
                break;
            case 3:
                if (roomRight && roomLeft && roomAbove)
                {
                    roomOutlines.Add(Instantiate(rooms.tripleULR, roomPosition, transform.rotation));
                }
                if (roomRight && roombelow && roomAbove)
                {
                    roomOutlines.Add(Instantiate(rooms.tripleURD, roomPosition, transform.rotation));
                }
                if (roomRight && roomLeft && roombelow)
                {
                    roomOutlines.Add(Instantiate(rooms.tripleRDL, roomPosition, transform.rotation));
                }
                if (roombelow && roomLeft && roomAbove)
                {
                    roomOutlines.Add(Instantiate(rooms.tripleULD, roomPosition, transform.rotation));
                }
                break;
            case 4:
                roomOutlines.Add(Instantiate(rooms.fourway, roomPosition, transform.rotation));
                break;
        }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft,
        doubleUD, doubleLR, doubleUR, doubleRD, doubleUL, doubleDL,
        tripleURD, tripleRDL, tripleULD, tripleULR,
        fourway;
}
