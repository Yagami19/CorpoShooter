using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    //Bool to manage debug on GUI
    public bool showDebug;

    //Legacy code to manage materials
    // [SerializeField] private Material mazeMat1;
    // [SerializeField] private Material mazeMat2;
    // [SerializeField] private Material startMat;
    // [SerializeField] private Material treasureMat;

    //number of rooms to generate
    [SerializeField] private int roomCount;

    //location of door in array to enter the room
    private int startDoorX;

    //location of door in array to exit the room
    private int endDoorX;

    //floor position on the map
    float _floorPositionY = -0.5f;

    //level generation range
    //minimum
    [SerializeField] private int sizeRowsRandomMin;
    [SerializeField] private int sizeColsRandomMin;
    //maximum
    [SerializeField] private int sizeRowsRandomMax;
    [SerializeField] private int sizeColsRandomMax;
    //% of given obstacle to appear, each is generating regardless of the previous one so total % of obstacles is sum of the whole
    [SerializeField] private int obstaclePercent01;
    [SerializeField] private int obstaclePercent02;
    [SerializeField] private int obstaclePercent03;
    [SerializeField] private int obstaclePercent04;
    [SerializeField] private int obstaclePercent05;

    //variables gameobjects of obstacles, floors, walls etc.
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject wall;
    //obstacles
    [SerializeField] private GameObject obstacle01;
    [SerializeField] private GameObject obstacle02;
    [SerializeField] private GameObject obstacle03;
    [SerializeField] private GameObject obstacle04;
    [SerializeField] private GameObject obstacle05;

    //postions to generate new room
    private int startPosX;
    private int startPosY;

    //array that contains the data of the level
    public int[,] data
    {
        get; private set;
    }

    //variable storing size of the rows
    public int sizeRows
    {
        get; private set;
    }

    //variable storing size of the columns
    public int sizeCols
    {
        get; private set;
    }

    // Awake as the method is needed to be called more than once
    void Awake()

    {
        if (!showDebug)
        {
            roomCount = LevelNumber.roomCount;
        }

       //legacy code to randomize count of the rooms
       // roomCount = Random.Range(5, 10);

       //setting position to 0, can be changed to player position if needed

        startPosX = 0;
        startPosY = 0;

        //generating a new level

        for (int i = 0; i < roomCount; i++)
        {
            data = GenerateNewRoom();
            InstantiateRoom(data);
        }

        Instantiate(obstacle01, new Vector3( startPosX-1, 2.0f, startPosY+endDoorX), Quaternion.identity);
        Instantiate(obstacle01, new Vector3(startPosX-1, 2.0f, startPosY + endDoorX+1), Quaternion.identity);

    }

    //method for generating rooms
    public int[,] GenerateNewRoom()
    {
        //randomizing the size of the room based of parameters, adding 1 to ensure compatibility 
        sizeRows = Random.Range(sizeRowsRandomMin, sizeRowsRandomMax);
        if (sizeRows % 2 == 0)
        {
            sizeRows++;
        }
        sizeCols = Random.Range(sizeColsRandomMin, sizeColsRandomMax);
        if (sizeCols % 2 == 0)
        {
            sizeCols++;
        }

        //!Reminder - arrays are clean by default, but it didn't always work, so if the method is not working correctly there is need to clean the array before proceeding
        //creating room array based on previous size parameters
        int[,] room = new int[sizeRows, sizeCols];

        //placing wall objects in arrays, Rows first, then columns
        for (int i = 0; i < sizeRows; i++)
        {
            room[i, 0] = 1;
            room[i, room.GetUpperBound(1)] = 1;
        }
        for (int i = 0; i < sizeCols; i++)
        {
            room[0, i] = 1;
            room[room.GetUpperBound(0), i] = 1;
        }
        //generating obstacles
        for (int i = 1; i < sizeRows - 1; i++)
        {
            for (int j = 1; j < sizeCols - 1; j++)
            {

                int obstacleRandom = Random.Range(0, 1000);
                if (room[i, j] != 9)
                {
                    //this part can be cleaned
                    if (obstacleRandom > obstaclePercent01)
                    {
                        room[i, j] = 2;
                    }
                    if (obstacleRandom > obstaclePercent02)
                    {
                        room[i, j] = 3;
                    }
                    if (obstacleRandom > obstaclePercent03)
                    {
                        room[i, j] = 4;
                    }
                    //placeholder for generating double sized obstacle
                    if (obstacleRandom > obstaclePercent04 & j % 2 == 0)
                    {

                        room[i, j] = 5;
                        room[i, j + 1] = 9;
                    }
                    if (obstacleRandom > obstaclePercent05 & i % 2 == 0)
                    {
                        room[i + 1, j] = 6;
                        room[i, j] = 9;
                    }
                }
            }
        }

        //legacy code for randomizing the geneation
        //startX = Random.Range(2, sizeCols - 2);

        if (startDoorX <= 0)
        {
            startDoorX = sizeCols / 2;
        }
        else
        {
            startDoorX = endDoorX;
        }

        if (endDoorX >= room.GetUpperBound(1) - 1)
        {
            startDoorX = Random.Range(2, sizeCols - 2);
            startPosY += (endDoorX - startDoorX); 
            //Debug.Log(endDoorX);
        }
        endDoorX = Random.Range(2, sizeCols - 2);
        
        //placeholder method for cleaning rooms to ensure that route from door to the exit can be achived
        for (int i = 1; i < 7; i++)
        {
            room[sizeRows - i, endDoorX] = 0;
            room[sizeRows - i, endDoorX + 1] = 0;
            room[i - 1, startDoorX] = 0;
            room[i - 1, startDoorX + 1] = 0;
        }
        return room;


        //legacy code for testing purposes of generating route through the level, didn't proceed due to the time pressure
        //Debug.Log("endX =" + endX + "sizeCols = " + sizeCols + " startX=" + startX + " upperbound1 " + room.GetUpperBound(1) + " ");
        //Debug.Log("Edgecase01: " + startDoorX + " jedynga: " + room.GetUpperBound(0) + " dwujka: " + room.GetUpperBound(1));
        //room[0, startDoorX] = 0;
        //Debug.Log("Edgecase02" + startDoorX + " jedynga: " + room.GetUpperBound(0) + " dwujka: " + room.GetUpperBound(1));
        // room[0, startDoorX + 1] = 0;
        //start door to generate the route
        // room[1, startDoorX] = 0;
        //room[1, startDoorX + 1] = 0;
        //end door position, to stop generating route through the level
        //Debug.Log("start dźwi: " + startDoorX + "koniedz dźwi XDD: " + endDoorX);
        //room[sizeRows - 2, endDoorX] = 0;
        //room[sizeRows - 2, endDoorX + 1] = 0;
        //room[sizeRows - 1, endDoorX] = 0;
        //room[sizeRows - 1, endDoorX + 1] = 0;
        //2(n+2)  - 2
        //i to jest indeks routehandlera, 0 to wspolrzedna X, 1 to wspolrzedna Y czyli po petli i=startX i przeszkodaX same 0, przeszkoda0, przeszkoda  y same 0

    }

    //method to instantiate generated level - can be cleaned
    private void InstantiateRoom(int [,] levelArray)
    {
        for (int i = 0; i <= levelArray.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= levelArray.GetUpperBound(1); j++)
            {
                //instantiating floor
                Instantiate(floor, new Vector3(i+startPosX, _floorPositionY, j+startPosY), Quaternion.identity);
                //array to store the data
                int value = levelArray[i, j];
                //switch is not optimal used due to the time pressure
                switch(value)
                {
                    case 0:
                        {
                            break;
                        }
                    case 1:
                        {
                            Instantiate(wall, new Vector3(i+startPosX, 2, j+ startPosY), Quaternion.identity);
                            break;
                        }
                    case 2:
                        {
                            Instantiate(obstacle01, new Vector3(i+startPosX, _floorPositionY+ 1, j+ startPosY), Quaternion.identity);
                            break;
                        }
                    case 3:
                        {
                            Instantiate(obstacle02, new Vector3(i+startPosX, _floorPositionY+ 1, j+ startPosY), Quaternion.identity);
                            break;
                        }
                    case 4:
                        {
                            Instantiate(obstacle03, new Vector3(i+startPosX, _floorPositionY+1, j+ startPosY), Quaternion.identity);
                            break;
                        }

                    case 5:
                        {
                            Instantiate(obstacle04, new Vector3(i  + startPosX, _floorPositionY+1, j  + 0.5f + startPosY), Quaternion.identity);
                            break;
                        }

                    case 6:
                        {
                            Instantiate(obstacle05, new Vector3(i -0.5f + startPosX, _floorPositionY+1, j + startPosY ), Quaternion.identity);
                            break;
                        }
                }           
            }         
        }
        //code to generate new level
        startPosX += sizeRows;     

        //legacy code for testing purposes
        //startPosY = endX;
        //Debug.Log(endX);
    }
    //method from the internet showing on screen room generation
    void OnGUI()
    {
        if (!showDebug)
        {
            return;
        }
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);
        string msg = "";
        for (int i = rMax; i >= 0; i--)
        {
            for (int j = 0; j <= cMax; j++)
            {
                msg += maze[i, j];
            }
            msg += "\n";
        }
        GUI.Label(new Rect(50, 50, 1000, 1000), msg);
    }

}
