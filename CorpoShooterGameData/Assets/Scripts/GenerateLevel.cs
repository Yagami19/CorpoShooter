using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{

    public bool showDebug;

    //materialy jakies costam beng XD
    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;
    [SerializeField] private Material treasureMat;


    //ilosc pokoi

    [SerializeField] private int roomCount;

    //start
    [SerializeField] private int startX;

    //end
    [SerializeField] private int endX;



    //zakres generowaina poziomu, w kodzie 

    //min
    [SerializeField] private int sizeRowsRandomMin;
    [SerializeField] private int sizeColsRandomMin;


    //max
    [SerializeField] private int sizeRowsRandomMax;
    [SerializeField] private int sizeColsRandomMax;




    //gameobjects z przeszkodami itp
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject wall;


    [SerializeField] private GameObject obstacle01;
    [SerializeField] private GameObject obstacle02;
    [SerializeField] private GameObject obstacle03;
    [SerializeField] private GameObject obstacle04;
    [SerializeField] private GameObject obstacle05;






    //[SerializeField] private int startingPointX;
    //[SerializeField] private int startingPointY;

    //[SerializeField] private int endingPointX;
    //[SerializeField] private int endingPointY;



    [SerializeField] private int obstaclePercent01;
    [SerializeField] private int obstaclePercent02;
    [SerializeField] private int obstaclePercent03;
    [SerializeField] private int obstaclePercent04;
    [SerializeField] private int obstaclePercent05;





    //pozycja generownaia poziomu
    private int startPosX;
   private int startPosY;

    //stworzenie obiektui tablicy z danymi poziomiu
    public int[,] data
    {
        get; private set;
    }


    //obiekt przechowujacy rozmiar wierszy tablicy
    public int sizeRows
    {
        get; private set;
    }

    //obiekt przechowujacy rozmiar kolumn tablicy
    public int sizeCols
    {
        get; private set;
    }

    private int move;

    // Start is called before the first frame update
    void Awake()



    {


        //randomize level size, odd numbers work better somehow

        startPosX = 0;
        startPosY = 0;






        // default to walls surrounding a single empty cell
        //generowanie nowego poziomu metodom XD


        for (int i = 0; i < roomCount; i++)
        {
            data = GenerateNewRoom();
            InstantiateRoom(data);
        }

    }






    //generuj nowy poziom metoda
    public int[,] GenerateNewRoom()
    {


        //losowanie rozmiaru pokoju
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

        //end



        //tablica sie domyslnie zeruje, ale mi sie nie chciala zerowac wiec jak nie bedzie dzialac to wiadomo o co chodzi, trzeba wyzerowac tablice XD

        //pobranie rozmiaru z metody i utworzenie tablicy
        int[,] room = new int[sizeRows, sizeCols];


      

        //wypelnienie pokoju scianami, najpierw wierszy a potem kolumn, albo na odwrot XD
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


        //zrobic assety i je kopiowac do sceny 

        //generowanie przeszkod
        for (int i = 1; i < sizeRows - 1; i++)
        {
            for (int j = 1; j < sizeCols - 1; j++)
            {

                int obstacle = Random.Range(0, 1000);
                if (room[i, j] != 9)
                {
                    //to cale zrobic else ifami
                    if (obstacle > obstaclePercent01)
                    {
                        room[i, j] = 2;
                    }


                    if (obstacle > obstaclePercent02)
                    {
                        room[i, j] = 3;




                    }


                    if (obstacle > obstaclePercent03)
                    {

                        room[i, j] = 4;

                    }


                    if (obstacle > obstaclePercent04)
                    {
                        if (j % 2 == 0)
                        {
                            room[i, j] = 5;
                            room[i, j + 1] = 9;


                        }
                    }


                    if (obstacle > obstaclePercent05)
                    {
                        if (i % 2 == 0)
                        {
                            room[i + 1, j] = 6;
                            room[i, j] = 9;


                        }
                    }



                }
                //dotad

            }


        }

        //generowanie drogi









        //  startX = Random.Range(2, sizeCols - 2);


        if (startX<=0)
        {
            startX = sizeRows / 2;

            

        }
        else
        {
            startX = endX;

        }

        if (endX >= room.GetUpperBound(1)-1)
        {

            startX = Random.Range(2, sizeCols - 2) ;
             
             startPosY +=(endX-startX);


;


            Debug.Log(endX);

           

          
                

        }

        //Debug.Log("endX =" + endX + "sizeCols = " + sizeCols + " startX=" + startX + " upperbound1 " + room.GetUpperBound(1) + " ");

       

        endX = Random.Range(2, sizeCols - 2);

        //Debug.Log(startX + " beng " + endX);




        room[0, startX] = 0;
        room[0, startX + 1] = 0;
        room[1, startX] = 0;
        room[1, startX + 1] = 0;

        room[sizeRows - 2, endX] = 0;
        room[sizeRows - 2, endX + 1] = 0;
        room[sizeRows - 1, endX] = 0;
        room[sizeRows - 1, endX + 1] = 0;













        //beng





        return room;
    }









    private void InstantiateRoom(int [,] levelArray)
    {
       

        for (int i = 0; i <= levelArray.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= levelArray.GetUpperBound(1); j++)

            {
                //generowanie podlogi
               

                Instantiate(floor, new Vector3(i+startPosX, 0 , j+startPosY), Quaternion.identity);

                //
              
             
                int value = levelArray[i, j];
                switch(value)
                {
                    case 0:
                        {

                            break;
                        }

                    case 1:

                        {
                            Instantiate(wall, new Vector3(i+startPosX, 1, j+ startPosY), Quaternion.identity);
                            break;
                        }

                    case 2:
                        {
                            Instantiate(obstacle01, new Vector3(i+startPosX, 1, j+ startPosY), Quaternion.identity);
                            break;
                        }
                    case 3:

                        {
                            Instantiate(obstacle02, new Vector3(i+startPosX, 1, j+ startPosY), Quaternion.identity);
                            break;
                        }
                    case 4:
                        {
                            Instantiate(obstacle03, new Vector3(i+startPosX, 1, j+ startPosY), Quaternion.identity);
                            break;
                        }


                    case 5:
                        {
                            Instantiate(obstacle04, new Vector3(i + startPosX, 1, j +0.5f + startPosY), Quaternion.identity);
                            break;
                        }


                    case 6:
                        {
                            Instantiate(obstacle05, new Vector3(i - 0.5f+ startPosX, 1, j + startPosY), Quaternion.identity);
                            break;
                        }








                }

              


            }
            

        }


        startPosX += sizeRows;
       
        //startPosY = endX;
        //Debug.Log(endX);


    }

    //jakas metoda z neta pokazujaca w okienku wygenerowana tablice XDDDDD
    void OnGUI()
    {
        //1
        if (!showDebug)
        {
            return;
        }

        //2
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        string msg = "";

        //3
        for (int i = rMax; i >= 0; i--)
        {
            for (int j = 0; j <= cMax; j++)
            {

                msg += maze[i, j];

                


                
            }
            msg += "\n";
        }

        //4
        GUI.Label(new Rect(50, 50, 1000, 1000), msg);
    }



}
