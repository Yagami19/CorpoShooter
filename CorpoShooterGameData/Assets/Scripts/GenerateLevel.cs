using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    //bool ktory pokazuje jak sie generuje pokoj, uzyty w metodzie on gui na dole 
    public bool showDebug;

    //materialy jakies costam beng XD
   // [SerializeField] private Material mazeMat1;
   // [SerializeField] private Material mazeMat2;
   // [SerializeField] private Material startMat;
   // [SerializeField] private Material treasureMat;


    //ilosc pokoi

    [SerializeField] private int roomCount;

    //miejsce w tabeli, w ktorym sa drzwi wchodzace do pokoju
    private int startDoorX;


    //miejsce w tabeli, w ktorym sa drzwi wychodzace z pokoju
   private int endDoorX;



    //zakres generowaina poziomu, w kodzie 

    //minimalny
    [SerializeField] private int sizeRowsRandomMin;
    [SerializeField] private int sizeColsRandomMin;


    //maksymalny
    [SerializeField] private int sizeRowsRandomMax;
    [SerializeField] private int sizeColsRandomMax;




    //gameobjects z przeszkodami, scianami itp.
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject wall;


    [SerializeField] private GameObject obstacle01;
    [SerializeField] private GameObject obstacle02;
    [SerializeField] private GameObject obstacle03;
    [SerializeField] private GameObject obstacle04;
    [SerializeField] private GameObject obstacle05;








    //procent danego obstacla, kazdy sie generuje niezaleznie od poprzedniego wiec jak damy 20% na kazdego to bedzie ich wiecej niz 20%
    [SerializeField] private int obstaclePercent01;
    [SerializeField] private int obstaclePercent02;
    [SerializeField] private int obstaclePercent03;
    [SerializeField] private int obstaclePercent04;
    [SerializeField] private int obstaclePercent05;

    //ilosc skretow w drodze
    [SerializeField] private int routeHandlesRangeMin;
    [SerializeField] private int routeHandlesRangeMax;

    



    //pozycja generownaia nowego pokoju
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


        //wyzerowanie pozycji, mozna zamiast 0  wstawic jakaspozycje gracza czy cos

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

                int obstacleRandom = Random.Range(0, 1000);
                if (room[i, j] != 9)
                {
                    //to cale zrobic else ifami
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

                    //j%2 dlatego, ze przeszkoda jest dwumodulowa
                    if (obstacleRandom > obstaclePercent04 & j % 2 == 0)
                    {

                            room[i, j] = 5;
                            room[i, j + 1] = 9;


                        
                    }


                


                    if (obstacleRandom > obstaclePercent05 & i % 2 == 0)
                    {
                        
                            room[i, j] = 6;
                            room[i+1, j] = 9;


                        




                    }





                }
                //dotad

            }


        }



   






        //  startX = Random.Range(2, sizeCols - 2);


        if (startDoorX<=0)
        {
            startDoorX = sizeCols / 2;

            

        }
        else
        {
            startDoorX = endDoorX;

        }

        if (endDoorX >= room.GetUpperBound(1)-1)
        {

            startDoorX = Random.Range(2, sizeCols - 2) ;
             
             startPosY +=(endDoorX-startDoorX);


;


           // Debug.Log(endDoorX);

           

          
                

        }

        //Debug.Log("endX =" + endX + "sizeCols = " + sizeCols + " startX=" + startX + " upperbound1 " + room.GetUpperBound(1) + " ");

       

        endDoorX = Random.Range(2, sizeCols - 2);





        //Debug.Log("Edgecase01: " + startDoorX + " jedynga: " + room.GetUpperBound(0) + " dwujka: " + room.GetUpperBound(1));
        room[0, startDoorX] = 0;
        //Debug.Log("Edgecase02" + startDoorX + " jedynga: " + room.GetUpperBound(0) + " dwujka: " + room.GetUpperBound(1));
        room[0, startDoorX + 1] = 0;
       

        //poczatek drzwi, czyli skad generuje trase
        room[1, startDoorX] = 0;
        room[1, startDoorX + 1] = 0;
        //koniec drzwi,czyli dokad generuje trase
        room[sizeRows - 2, endDoorX] = 0;


        //Debug.Log("start dźwi: " + startDoorX + "koniedz dźwi XDD: " + endDoorX);

        room[sizeRows - 2, endDoorX + 1] = 0;
        room[sizeRows - 1, endDoorX] = 0;
        room[sizeRows - 1, endDoorX + 1] = 0;




        //generowanie drogi
        int routeHandlesNumber = Random.Range(routeHandlesRangeMin, routeHandlesRangeMax+1);
        int[,] routeArray = new int[routeHandlesNumber,2];
        


        for (int i = 0; i < routeHandlesNumber; i++)
        {

            int _routeHandleX = Random.Range(1, sizeRows - 1); ;

            int _routeHandleY = Random.Range(1, sizeCols - 1);
            routeArray[i, 0] = _routeHandleX;
            routeArray[i, 1] = _routeHandleY;


            room[_routeHandleX, _routeHandleY] = 7;


        }



       //2(n+2)  - 2
       //i to jest indeks routehandlera, 0 to wspolrzedna X, 1 to wspolrzedna Y czyli po petli i=startX i przeszkodaX same 0, przeszkoda0, przeszkoda  y same 0

        for (int i = 1; i < room.GetUpperBound(0); i++)
        {

            for (int j = 1; j < room.GetUpperBound(1); j++)
            {

                int RouteX = 0;
                int RouteY = 0;




                room[0, startDoorX] = 0;
                room[sizeRows - 1, endDoorX] = 0;






            }


        }


        //Debug.Log("count: " + count);







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

                float _floorPositionY = -0.5f;
                Instantiate(floor, new Vector3(i+startPosX, _floorPositionY, j+startPosY), Quaternion.identity);

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
                            Instantiate(obstacle04, new Vector3(i + startPosX, _floorPositionY+1, j +0.5f + startPosY), Quaternion.identity);
                            break;
                        }


                    case 6:
                        {
                            Instantiate(obstacle05, new Vector3(i - 0.5f+ startPosX, _floorPositionY+1, j + startPosY), Quaternion.identity);
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
