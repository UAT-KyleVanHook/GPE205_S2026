using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;


public enum RandomType { Random, Seeded, MapOfTheDay };
public class MapGenerator : MonoBehaviour
{
    [Header("Random Data")]
    public RandomType randomType;
    public int seed = 27;

    [Header("TileData")]
    public List<Tile> avialableTiles;
    public float tileWidth;
    public float tileLength;
    public int mapCols;
    public int mapRows;

    public Tile[,] grid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set the seed value
        InitializeRandom();

        //TODO: Remove tihs. For testing
        //GenerateMap(); //<-- Remove THIS
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeRandom()
    {
        if(randomType == RandomType.Seeded)
        {
            UnityEngine.Random.InitState(seed);
        }
        else if (randomType == RandomType.Random)
        {
            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        }
        else if (randomType == RandomType.MapOfTheDay)
        {
            UnityEngine.Random.InitState(DateToInt(DateTime.Now.Date));
        }
        
    }

    public int DateToInt(DateTime date)
    {
        return date.Year + date.Month + date.Day + date.Hour + date.Minute + date.Second;
    }

    public void GenerateMap()
    {
        // Create grid array to hold our map
        grid = new Tile[mapCols, mapRows];

        // Iterate through and generate all the map tiles
        for(int currentRow = 0; currentRow < mapRows; currentRow++)
        {
            for (int currentCol = 0; currentCol < mapCols; currentCol++)
            {
                // Create map tiles
                Tile tempTile = Instantiate<Tile>(GetRandomTile()) as Tile;

                // Put it in the right position
                Vector3 correctPosition = Vector3.zero;

                correctPosition.z = currentRow * tileWidth;
                correctPosition.x = currentCol * tileLength;

                tempTile.transform.position = correctPosition;

                // Name the tile, so that we can easily see if it is in the right spot.
                tempTile.name = "Tile(" + currentCol + "," + currentRow + ")";

                // Open the correct doors
                //If in the southmost row, turn of the North door
                if (currentRow == 0)
                {
                    tempTile.DoorNorth.SetActive(false);
                }
                //Otherwise, if in the Northmost door, turn off the south door
                else if (currentRow == mapRows-1)
                {
                    tempTile.DoorSouth.SetActive(false);
                }
                //otherwise, we are in the middle and we turn off both sides
                else 
                {
                    tempTile.DoorNorth.SetActive(false);
                    tempTile.DoorSouth.SetActive(false);
                }

                //If eastmost door, open west door.
                if(currentCol == mapCols - 1)
                {
                    tempTile.DoorWest.SetActive(false);
                }
                //Otherise, if Westmost door, open east door.
                else if (currentCol == 0)
                {
                    tempTile.DoorEast.SetActive(false);
                }
                //Otherwise, opne both.
                else
                {
                    tempTile.DoorWest.SetActive(false);
                    tempTile.DoorEast.SetActive(false);
                }


                    // Save it to the grid
                    grid[currentCol, currentRow] = tempTile;
            }
        }

    }

    public Tile GetRandomTile()
    {

        int tileNumber = UnityEngine.Random.Range(0, avialableTiles.Count);
        return avialableTiles[tileNumber];

    }
}
