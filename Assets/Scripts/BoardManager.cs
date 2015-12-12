using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public class BoardManager : MonoBehaviour
{

    public int columns = 7;                                         //Number of columns in our game board.
    public int rows = 7;                                            //Number of rows in our game board.
    public GameObject blackTiles;                                 //Array of floor prefabs.
    public GameObject whiteTiles;                                  //Array of wall prefabs.

    public GameObject redChecker;
    public GameObject blackChecker;

    public GameObject indicator;

    public static string[,] boardState = new string[8,8];
    private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
    private Transform checkerHolder;                                  //A variable to store a reference to the transform of our Board object.
    private Transform indicationHolder;



    //Clears our list gridPositions and prepares it to generate a new board.
    void PiecesSetup()
    {

        //Instantiate Board and set boardHolder to its transform.
        checkerHolder = new GameObject("Checkers").transform;
        indicationHolder = new GameObject("Indicators").transform;

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = 0; x <= columns; x++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int y = 0; y <= rows; y++)
            {

                GameObject toInstantiate;
                GameObject instance;

                if (x % 2 != 0 && y % 2 == 0 || x % 2 == 0 && y % 2 != 0)
                {
                    if (x < 3)
                    {
                        toInstantiate = redChecker;
                        instance = Instantiate(toInstantiate, new Vector3(x, y, -1f), Quaternion.Euler(0, 0, -90)) as GameObject; //1f on z so that the checkers onmousedown doesn't get caught by the board.
                        NetworkServer.Spawn(instance);
                        
                        instance.transform.SetParent(checkerHolder);

                        boardState[x, y] = "RC";
                    }
                    if (x > 4)
                    {
                        toInstantiate = blackChecker;
                        instance = Instantiate(toInstantiate, new Vector3(x, y, -1f), Quaternion.Euler(0, 0, -90)) as GameObject; //1f on z so that the checkers onmousedown doesn't get caught by the board.
                        NetworkServer.Spawn(instance);
                        instance.transform.SetParent(checkerHolder);

                        boardState[x, y] = "BC";
                    }
                }
                
            }
        }
    }

    internal void IndicatePossibleMoves(int x, int y)
    {
        GameObject instance = Instantiate(indicator, new Vector3(x, y, -2f), Quaternion.identity) as GameObject; //1f on z so that the checkers onmousedown doesn't get caught by the board.
        instance.transform.SetParent(indicationHolder);
    }
    internal void RemoveIndications()
    {
        foreach (Transform child in indicationHolder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    //Sets up the outer walls and floor (background) of the game board.
    void BoardSetup()
    {

        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject("Board").transform;

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = 0; x <= columns; x++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int y = 0; y <= rows; y++)
            {
                //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                GameObject toInstantiate = blackTiles;

                //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                if (x % 2 == 0 && y % 2 == 0 || x % 2 != 0 && y % 2 != 0)
                    toInstantiate = whiteTiles;

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance =
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(boardHolder);
            }
        }

    }





    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene()
    {

        try
        {
            //Creates the outer walls and floor.
            BoardSetup();

            //Reset our list of gridpositions.
            PiecesSetup();

        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}

