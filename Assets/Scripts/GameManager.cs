using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
    private string[,] possibleMoves = new string[7,7];
    private string[,] _boardState = new string[7, 7];

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    internal bool ProcessDrop(int newX, int newY, int oldX, int oldY)
    {
        Debug.Log("Drop at " + newX.ToString() + "," + newY.ToString());
        if (!String.IsNullOrEmpty(possibleMoves[newX, newY]))
        {
            var _bs = BoardManager.boardState;
            _bs[oldX, oldY] = null; //clear up where the checker came from

            _bs[newX, newY] = "RC"; //log where the checker is now
            //legal move
            return true;
        }

        return false;
    }

    //Initializes the game for each level.
    void InitGame()
    {
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        boardScript.SetupScene();

    }
    public void RemoveIndications()
    {
        instance.boardScript.RemoveIndications();
    }
    public void ProcessPickUp(int X, int Y)
    {
        Debug.Log("Pickup at " + X.ToString() + "," + Y.ToString());
        possibleMoves = new string[7, 7];
        var x = X;
        var y = Y;
        var toprightX = x + 1;
        var toprightY = y + 1;
        var topleftX = x + 1;
        var topleftY = y - 1;
        var bs = BoardManager.boardState;
        //Check top left diagonal for piece
        if (toprightX < 8 && toprightY < 8)
        {
            if (String.IsNullOrEmpty(bs[toprightX, toprightY]))
            {

                //possible move
                instance.boardScript.IndicatePossibleMoves(toprightX, toprightY);
                possibleMoves[toprightX, toprightY] = "PM";
                Debug.Log("Indicator at " + toprightX.ToString() + ","+ toprightY.ToString());
            }
        }
        if (topleftX < 8 && topleftY < 8)
        {
            //Check top right diagonal for piece
            if (String.IsNullOrEmpty(bs[topleftX, topleftY]))
            {

                //possible move
                instance.boardScript.IndicatePossibleMoves(topleftX, topleftY);
                possibleMoves[topleftX, topleftY] = "PM";
                Debug.Log("Indicator at " + topleftX.ToString() + "," + topleftY.ToString());
            }
        }
    }

    //Update is called every frame.
    void Update()
    {

    }
}
