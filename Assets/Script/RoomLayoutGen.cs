using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGen : MonoBehaviour
{
    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        Wall, Floor,
    }


    public int columns = 100;                                 // The number of columns on the board (how wide it will be).
    public int rows = 100;                                    // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange(7, 10);         // The range of the number of rooms there can be.
    public int roomWidth = 10;         // The range of widths rooms can have.
    public int roomHeight = 10;        // The range of heights rooms can have.
    public int corridorLength = 5;    // The range of lengths corridors between rooms can have.
    public GameObject[] floorTiles;                           // An array of floor tile prefabs.
    public GameObject[] wallTiles;                            // An array of wall tile prefabs.
    public GameObject[] outerWallTiles;                       // An array of outer wall tile prefabs.
    public GameObject player;

    private int[][] room_layout;
   // private int[] room_layout_total;
    private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private Room[] rooms;                                     // All the rooms that are created for this board.
    private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.

    public Stack<Vector2> prev_steps = new Stack<Vector2>();
    public List<Corridor> total_corridor = new List<Corridor>();


    private void Start()
    {
        // Create the board holder.
        boardHolder = new GameObject("BoardHolder");

        SetupTilesArray();

        CreateRoomsAndCorridors();

        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        InstantiateTiles();
        InstantiateOuterWalls();
    }


    void SetupTilesArray()
    {
        rooms = new Room[numRooms.Random];
        Debug.Log("Room No: " + rooms.Length);

        // Set the tiles jagged array to the correct width.
        tiles = new TileType[columns][];

        // Go through all the tile arrays...
        for (int i = 0; i < tiles.Length; i++)
        {
            // ... and set each tile array is the correct height.
            tiles[i] = new TileType[rows];
        }

      //  room_layout_total = new int[rooms.Length];

        room_layout = new int[3][];

        // Go through all the tile arrays...
        for (int i = 0; i < room_layout.Length; i++)
        {
            // ... and set each tile array is the correct height.
            room_layout[i] = new int[3];
        }

        for(int i = 0; i < 3; ++i)
        {
            for(int j = 0; j < 3; ++j)
            {
                room_layout[j][i] = 5;
            }
        }
    }


    void CreateRoomsAndCorridors()
    {
        int temp_x = 0, temp_y = 2;
        int temp_room_layout_adder = 0;
        Vector2 temp_vec;

        for (int i = 0; i < rooms.Length;)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            int temp = UnityEngine.Random.Range(0, 4);

            //0 - UP, 1 - DOWN, 2 - LEFT, 3 - RIGHT
            switch (temp)
            {
                case 0:     //UP
                    ++temp_y;
                    if (temp_y > 2)
                    {
                        --temp_y;
                        continue;
                    }
                    else
                    {
                        if (room_layout[temp_y][temp_x] == 5)
                        {
                            room_layout[temp_y][temp_x] = temp;

                            temp_vec = new Vector2(temp_x, temp_y);
                            prev_steps.Push(temp_vec);
                        }
                        else
                        {
                            if (prev_steps.Count > 0)
                            {
                                temp_vec = prev_steps.Pop();
                                temp_y = (int)temp_vec.y;
                                temp_x = (int)temp_vec.x;

                            }
                            continue;
                        }
                    }
                    break;

                case 1:     //DOWN
                    --temp_y;
                    if (temp_y < 0)
                    {
                        ++temp_y;
                        continue;
                    }
                    else
                    {
                        if (room_layout[temp_y][temp_x] == 5)
                        {
                            room_layout[temp_y][temp_x] = temp;
                            temp_vec = new Vector2(temp_x, temp_y);
                            prev_steps.Push(temp_vec);
                        }
                        else
                        {
                            if (prev_steps.Count > 0)
                            {
                                temp_vec = prev_steps.Pop();
                                temp_y = (int)temp_vec.y;
                                temp_x = (int)temp_vec.x;

                            }
                            continue;
                        }
                    }
                    break;

                case 2:     //LEFT
                    --temp_x;
                    if (temp_x < 0)
                    {
                        ++temp_x;
                        continue;
                    }
                    else
                    {
                        if (room_layout[temp_y][temp_x] == 5)
                        {
                            room_layout[temp_y][temp_x] = temp;
                            temp_vec = new Vector2(temp_x, temp_y);
                            prev_steps.Push(temp_vec);
                        }
                        else
                        {
                            if (prev_steps.Count > 0)
                            {
                                temp_vec = prev_steps.Pop();
                                temp_y = (int)temp_vec.y;
                                temp_x = (int)temp_vec.x;
                            }
                            continue;
                        }
                    }
                    break;

                case 3:     //RIGHT
                    ++temp_x;
                    if (temp_x > 2)
                    {
                        --temp_x;
                        continue;
                    }
                    else
                    {
                        if (room_layout[temp_y][temp_x] == 5)
                        {
                            room_layout[temp_y][temp_x] = temp;
                            temp_vec = new Vector2(temp_x, temp_y);
                            prev_steps.Push(temp_vec);
                        }
                        else
                        {
                            if (prev_steps.Count > 0)
                            {
                                temp_vec = prev_steps.Pop();
                                temp_y = (int)temp_vec.y;
                                temp_x = (int)temp_vec.x;
                            }
                            continue;
                        }
                    }
                    break;
            }


            ++i;
            ++temp_room_layout_adder;
        }

        // Create the first room and corridor.
        rooms[0] = new Room();

        // Setup the first room, there is no previous corridor so we do not use one.
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);
        // Setup the first corridor using the first room.

        int temp_no_of_room = 0;

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                if (room_layout[i][j] != 5)
                {
                    rooms[temp_no_of_room] = new Room();
                    rooms[temp_no_of_room].SetupRoom(roomWidth, roomHeight, columns, rows, j, i);
                 
                    if (i - 1 >= 0)
                    {
                        if (room_layout[i - 1][j] != 5)
                        {
                            Corridor temp_corr = new Corridor();
                            total_corridor.Add(temp_corr);

                            // Setup the corridor based on the room that was just created.
                            total_corridor[total_corridor.Count - 1].SetupCorridor(rooms[temp_no_of_room], corridorLength, roomWidth, roomHeight, columns, rows, false, 1);
                        }
                    }

                    if (j + 1 < 3)
                    {

                        if (room_layout[i][j + 1] != 5)
                        {
                            Corridor temp_corr = new Corridor();
                            total_corridor.Add(temp_corr);

                            // Setup the corridor based on the room that was just created.
                            total_corridor[total_corridor.Count - 1].SetupCorridor(rooms[temp_no_of_room], corridorLength, roomWidth, roomHeight, columns, rows, false, 3);
                        }
                    }
                    ++temp_no_of_room;
                }
            }
        }
    }


    void SetTilesValuesForRooms()
    {
        // Go through all the rooms...
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;
                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }
    }


    void SetTilesValuesForCorridors()
    {
        // Go through every corridor...
        for (int i = 0; i < total_corridor.Count; i++)
        {
            Corridor currentCorridor = total_corridor[i];

            // and go through it's length.
            for (int j = 0; j < corridorLength; j++)
            {
                // Start the coordinates at the start of the corridor.
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                // Depending on the direction, add or subtract from the appropriate
                // coordinate based on how far through the length the loop is.
                switch (currentCorridor.direction)
                {
                    case Direction.Up:
                        yCoord += j;
                        break;
                    case Direction.Left:
                        xCoord -= j;
                        break;
                    case Direction.Down:
                        yCoord -= j;
                        break;
                    case Direction.Right:
                        xCoord += j;
                        break;
                }

                // Set the tile at these coordinates to Floor.
                tiles[xCoord][yCoord] = TileType.Floor;
            }
        }
    }


    void InstantiateTiles()
    {
        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                // ... and instantiate a floor tile for it.
                InstantiateFromArray(floorTiles, i, j);

                // If the tile type is Wall...
                if (tiles[i][j] == TileType.Wall)
                {
                    // ... instantiate a wall over the top.
                    InstantiateFromArray(wallTiles, i, j);
                }
            }
        }
    }


    void InstantiateOuterWalls()
    {
        // The outer walls are one unit left, right, up and down from the board.
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        // Instantiate both vertical walls (one on each side).
        InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        // Instantiate both horizontal walls, these are one in left and right from the outer walls.
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }


    void InstantiateVerticalOuterWall(float xCoord, float startingY, float endingY)
    {
        // Start the loop at the starting value for Y.
        float currentY = startingY;

        // While the value for Y is less than the end value...
        while (currentY <= endingY)
        {
            // ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
            InstantiateFromArray(outerWallTiles, xCoord, currentY);

            currentY++;
        }
    }


    void InstantiateHorizontalOuterWall(float startingX, float endingX, float yCoord)
    {
        // Start the loop at the starting value for X.
        float currentX = startingX;

        // While the value for X is less than the end value...
        while (currentX <= endingX)
        {
            // ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
            InstantiateFromArray(outerWallTiles, currentX, yCoord);

            currentX++;
        }
    }


    void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord * 2, 0.0f, yCoord * 2);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }
}