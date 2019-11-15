using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public bool showDebug;

    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;
    [SerializeField] private Material treasureMat;

    [SerializeField] private int sizeRowsRandomRange;
    [SerializeField] private int sizeColsRandomRange;





    public int[,] data
    {
        get; private set;
    }

    public int sizeRows
    {
        get; private set;
    }

    public int sizeCols
    {
        get; private set;
    }



    // Start is called before the first frame update
    void Awake()
        


    {
        

        //randomize level size, odd numbers work better somehow

        sizeRows = Random.Range(50, sizeRowsRandomRange);
        if (sizeRows % 2 == 0)
        {
            sizeRows++;
        }

        sizeCols = Random.Range(5, sizeColsRandomRange);

        if (sizeCols % 2 == 0)
        {
            sizeCols++;
        }



        // default to walls surrounding a single empty cell
        data = GenerateNewLevel(sizeCols, sizeRows);


       
    }

    public int[,] GenerateNewLevel(int sizeRows, int sizeCols)
    {
        int[,] level = new int[sizeRows, sizeCols];
        // stub to fill in
        return level; 

    }


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
                if (maze[i, j] == 0)
                {
                    msg += "....";
                }
                else
                {
                    msg += "==";
                }
            }
            msg += "\n";
        }

        //4
        GUI.Label(new Rect(20, 20, 500, 500), msg);
    }



}
