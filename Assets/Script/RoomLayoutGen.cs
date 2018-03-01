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

    public float height_of_blocks = 1;
    public int TextureQuality = 1;
    public int columns = 100;                                 // The number of columns on the board (how wide it will be).
    public int rows = 70;                                    // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange(7, 10);         // The range of the number of rooms there can be.
    public IntRange numRooms_simple = new IntRange(5, 8);         // The range of the number of rooms there can be.
    public int roomWidth = 30;         // The range of widths rooms can have.
    public int roomHeight = 20;        // The range of heights rooms can have.
    public int corridorLength = 6;    // The range of lengths corridors between rooms can have.
    public GameObject[] floorTiles;                           // An array of floor tile prefabs.
    public GameObject[] wallTiles;                            // An array of wall tile prefabs.
    public GameObject[] outerWallTiles;                       // An array of outer wall tile prefabs.

    public GameObject player_obj;
    public GameObject spawner_block;
    public GameObject door_blocks;
    public GameObject obstacle_blocks;
    public GameObject exit_block;
    public GameObject quad_block;
    public GameObject[] statue;
    public GameObject AI_Controller;
    public GameObject canvas_layout;
    

    private Player player_obj_script;
    private Portal portal_obj_script;
    private Grid AI_Controller_Grid;
    //private GameObject AI_Controller_Obj;

    private int[][] room_layout;
    private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private Room[] rooms;                                     // All the rooms that are created for this board.
    private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.
    private bool spawned_exit;
    private int player_prev_room;

    private Stack<Vector2> prev_steps = new Stack<Vector2>();
    private List<Corridor> total_corridor = new List<Corridor>();

    private GameObject[] total_spawners;
    private GameObject[] total_quads;
    private List<GameObject> total_blocks = new List<GameObject>();

    private void Awake()
    {
        SpawnerManager.Instance.SetUpSpawnerManager();
    }

    private void Start()
    {
        // Create the board holder.
        boardHolder = new GameObject("BoardHolder");
        player_obj = GameObject.FindGameObjectWithTag("Player");
        
        GameObject controllerInstance = Instantiate(AI_Controller, AI_Controller.transform.position, Quaternion.identity) as GameObject;
        //AI_Controller_Obj = controllerInstance;
        AI_Controller_Grid = controllerInstance.GetComponent<Grid>();

        player_obj_script = player_obj.GetComponent<Player>();

        Random.InitState(System.DateTime.Now.Millisecond);

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            QualitySettings.masterTextureLimit = TextureQuality;
        else
            QualitySettings.masterTextureLimit = 5;

        total_spawners = new GameObject[numRooms.m_Max - 1];

        for(int i = 0; i < total_spawners.Length; ++i)
        {
            Vector3 temp_spawner_vec = new Vector3(0, 0, 0);
            GameObject tileInstance = Instantiate(spawner_block, temp_spawner_vec, Quaternion.identity) as GameObject;          

            tileInstance.SetActive(false);

            total_spawners[i] = tileInstance;
            total_spawners[i].GetComponent<SpawnerBlock>().SetSpawnerRoomID(i);
        }

        total_quads = new GameObject[numRooms.m_Max - 1];

        for (int i = 0; i < total_quads.Length; ++i)
        {
            Vector3 temp_spawner_vec = new Vector3(0, 0, 0);
            GameObject tileInstance = Instantiate(quad_block, temp_spawner_vec, quad_block.transform.rotation) as GameObject;

            tileInstance.SetActive(true);

            total_quads[i] = tileInstance;
        }

        Vector3 temp_exit_vec = new Vector3(0, 0, 0);
        GameObject exitPortalInstance = Instantiate(exit_block, temp_exit_vec, exit_block.transform.rotation) as GameObject;

        portal_obj_script = exitPortalInstance.GetComponent<Portal>();
        portal_obj_script.gameObject.SetActive(false);

        SetUpMap();
       
    }
    int count = 0;
    void Update()
    {
        if (Input.GetKeyDown("space"))
            SetUpMap();

        if (player_obj_script.GetPlayerCurrentRoom() != player_prev_room)
        {
            if (count == 1 )
            {
               canvas_layout.GetComponent<UIScript>().SetRoomObjective("Defeat Monsters");
            }
            player_prev_room = player_obj_script.GetPlayerCurrentRoom();
            total_quads[player_prev_room].SetActive(false);
            count++; 
        }

        if (!spawned_exit)
        {
            bool is_all_spawner_dead = false;
            for (int i = 0; i < total_spawners.Length; ++i)
            {
                if (total_spawners[i].activeSelf)
                {
                    is_all_spawner_dead = false;
                    break;
                }

                if (!is_all_spawner_dead)
                    is_all_spawner_dead = true;
            }

            if (total_spawners[player_prev_room].GetComponent<SpawnerBlock>().IsSpawningBoss())
            {
                if (!total_spawners[player_prev_room].activeSelf)
                {
                    is_all_spawner_dead = true;
                }
            }

            if (is_all_spawner_dead)
            {
                portal_obj_script.transform.position = new Vector3(total_spawners[player_obj_script.GetPlayerCurrentRoom()].GetComponent<SpawnerBlock>().transform.position.x + ((Random.Range(0, 100) < 50) ? 5 : -5),
                -5,
                total_spawners[player_obj_script.GetPlayerCurrentRoom()].GetComponent<SpawnerBlock>().transform.position.z);

                portal_obj_script.Reset();
                portal_obj_script.gameObject.SetActive(true);
                spawned_exit = true;
              
            }
        }
        else
        {
            if(portal_obj_script.GetIsDone())
            {
                SetUpMap();
                //count = 0;
            }
        }

    }

    void SetupTilesArray()
    {
        foreach (GameObject i in total_blocks)
        {
            i.SetActive(false);
        }
        foreach (GameObject i in total_blocks)
        {
            Destroy(i);
        }

        total_blocks.Clear();

        total_corridor.Clear();
        prev_steps.Clear();

        if(player_obj_script.GetpStats().level % 3 == 0)
            rooms = new Room[numRooms.Random];
        else
            rooms = new Room[numRooms_simple.Random];

      //  Debug.Log("Room No: " + rooms.Length);

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

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
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

        int temp_no_of_room = 0, player_spawn_point_y = 0;
        bool set_player_spawn = false;
        bool set_boss_spawn = false;
        bool set_statue_spawn = false;

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                if (room_layout[i][j] != 5)
                {
                    rooms[temp_no_of_room] = new Room();
                    rooms[temp_no_of_room].SetupRoom(roomWidth, roomHeight, columns, rows, j, i);

                    if (!set_player_spawn)
                    {
                        player_spawn_point_y = i;
                        player_obj_script.transform.position = new Vector3(rooms[temp_no_of_room].xPos * 2 + columns * 0.28f, 0, rooms[temp_no_of_room].yPos * 2 + rows * 0.24f);
                        set_player_spawn = true;
                        player_obj_script.SetPlayerCurrentRoom(total_spawners[temp_no_of_room].GetComponent<SpawnerBlock>().GetSpawnerRoomID());

                        Vector3 temp_spawner_vec = new Vector3(rooms[temp_no_of_room].xPos * 2 + columns * 0.28f, 0, rooms[temp_no_of_room].yPos * 2 + rows * 0.24f);
                        total_spawners[temp_no_of_room].GetComponent<SpawnerBlock>().transform.position = temp_spawner_vec;
                        total_spawners[temp_no_of_room].SetActive(false);

                        Vector3 temp_quad_vec = new Vector3(rooms[temp_no_of_room].xPos * 2 + columns * 0.28f, 22, rooms[temp_no_of_room].yPos * 2 + rows * 0.24f);
                        total_quads[temp_no_of_room].transform.position = temp_quad_vec;
                        total_quads[temp_no_of_room].SetActive(true);

                    }
                    else
                    {
                        // Creating the spawner blocks
                        Vector3 temp_spawner_vec = new Vector3(rooms[temp_no_of_room].xPos * 2 + columns * 0.28f, 0, rooms[temp_no_of_room].yPos * 2 + rows * 0.24f);
                        total_spawners[temp_no_of_room].GetComponent<SpawnerBlock>().transform.position = temp_spawner_vec;
                        total_spawners[temp_no_of_room].GetComponent<SpawnerBlock>().SetDistanceDetect(new Vector2(rooms[temp_no_of_room].roomWidth, rooms[temp_no_of_room].roomHeight));
                        total_spawners[temp_no_of_room].SetActive(true);

                        Vector3 temp_quad_vec = new Vector3(rooms[temp_no_of_room].xPos * 2 + columns * 0.28f, 22, rooms[temp_no_of_room].yPos * 2 + rows * 0.24f);
                        total_quads[temp_no_of_room].transform.position = temp_quad_vec;
                        total_quads[temp_no_of_room].transform.localScale = new Vector3(rooms[temp_no_of_room].roomWidth * 2.1f, rooms[temp_no_of_room].roomHeight * 2.2f, 1);
                        total_quads[temp_no_of_room].SetActive(true);

                        if (player_obj_script.GetpStats().level % 3 == 0)
                        {
                            if (!set_boss_spawn)
                            {
                                if (player_spawn_point_y > 0 && i == 0)
                                {
                                    canvas_layout.GetComponent<UIScript>().SetRoomObjective("BOSS!!!");
                                    total_spawners[temp_no_of_room].GetComponent<SpawnerBlock>().SpawnBoss(true);
                                    set_boss_spawn = true;
                                }
                                else if (player_spawn_point_y <= 0 && i == 2)
                                {
                                    total_spawners[temp_no_of_room].GetComponent<SpawnerBlock>().SpawnBoss(true);
                                    set_boss_spawn = true;
                                }
                            }
                        }

                        if(!set_statue_spawn)
                        {                          
                            if(player_obj_script.GetpStats().level == 1)
                            {
                                int rand_statue = 0;
                                Vector3 temp_statue_vec = new Vector3(rooms[temp_no_of_room].xPos * 2 + columns * 0.28f, -10, rooms[temp_no_of_room].yPos * 2 + rows * 0.24f);
                                GameObject tileInstance = Instantiate(statue[rand_statue], temp_statue_vec, statue[rand_statue].transform.rotation) as GameObject;
                                tileInstance.SetActive(false);

                                tileInstance.GetComponent<Statue>().SetType(rand_statue);
                                tileInstance.GetComponent<Statue>().SetCostRange(new IntRange(10, 100));

                                total_spawners[temp_no_of_room].GetComponent<SpawnerBlock>().SetStatue(tileInstance);
                                total_blocks.Add(tileInstance);

                                set_statue_spawn = true;
                            }
                            else if (Random.Range(0, 100) < 30)
                            {
                                int rand_statue = Random.Range(0, statue.Length);
                                Vector3 temp_statue_vec = new Vector3(rooms[temp_no_of_room].xPos * 2 + columns * 0.28f, -10, rooms[temp_no_of_room].yPos * 2 + rows * 0.24f);
                                GameObject tileInstance = Instantiate(statue[rand_statue], temp_statue_vec, statue[rand_statue].transform.rotation) as GameObject;
                                tileInstance.SetActive(false);

                                tileInstance.GetComponent<Statue>().SetType(rand_statue);

                                total_spawners[temp_no_of_room].GetComponent<SpawnerBlock>().SetStatue(tileInstance);
                                total_blocks.Add(tileInstance);

                                set_statue_spawn = true;
                            }
                        }
                    }
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
        int[][] temp_array;

        int half_room_height = ((int)((roomHeight * 0.5) * 0.8));
        int half_room_width = ((int)((roomWidth * 0.5) * 0.8));
       
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

        for (int room_no = 1; room_no < rooms.Length; room_no++)
        {
            if (!total_spawners[room_no].GetComponent<SpawnerBlock>().IsSpawningBoss())
            {
                Room currentRoom = rooms[room_no];

                temp_array = new int[half_room_height][];

                for (int i = 0; i < half_room_height; ++i)
                {
                    temp_array[i] = new int[half_room_width];
                }

                for (int i = 0; i < half_room_height; ++i)
                {
                    for (int j = 0; j < half_room_width; ++j)
                    {
                        int rand = UnityEngine.Random.Range(0, 100);

                        if (rand < 50)
                        {
                            temp_array[i][j] = 1;
                        }
                        else
                        {
                            temp_array[i][j] = 0;
                        }
                    }
                }

                //BOTTOM LEFT
                for (int i = 1; i < half_room_height; ++i)
                {
                    for (int j = 0; j < half_room_width; ++j)
                    {
                        if (temp_array[i][j] == 1)
                        {
                            Vector3 temp_vec = new Vector3(((currentRoom.xPos + j) + half_room_width) - 7, height_of_blocks, ((currentRoom.yPos + i) + half_room_height) - 3.5f);
                            //  tiles[((currentRoom.xPos + j) + half_room_width) - 2][((currentRoom.yPos + i) + half_room_height) - 1] = TileType.Wall;

                            GameObject tileInstance = Instantiate(obstacle_blocks, temp_vec * 2, Quaternion.identity) as GameObject;
                            total_blocks.Add(tileInstance);

                        }
                    }
                }

                //BOTTOM RIGHT
                for (int i = 1; i < half_room_height; ++i)
                {
                    int x = half_room_width - 1;
                    for (int j = 0; j < half_room_width; ++j)
                    {
                        if (temp_array[i][x] == 1)
                        {
                            Vector3 temp_vec = new Vector3(((currentRoom.xPos + j) + (half_room_width * 2)) - 5, height_of_blocks, ((currentRoom.yPos + i) + half_room_height) - 3.5f);

                            GameObject tileInstance = Instantiate(obstacle_blocks, temp_vec * 2, Quaternion.identity) as GameObject;
                            total_blocks.Add(tileInstance);
                            //tiles[((currentRoom.xPos + j) + (half_room_width * 2)) + 2][((currentRoom.yPos + i) + half_room_height) - 1] = TileType.Wall;
                        }

                        --x;
                    }
                }

                //TOP RIGHT
                int y = half_room_height - 1;
                for (int i = 0; i < half_room_height - 1; ++i)
                {
                    int x = half_room_width - 1;
                    for (int j = 0; j < half_room_width; ++j)
                    {
                        if (temp_array[y][x] == 1)
                        {
                            Vector3 temp_vec = new Vector3(((currentRoom.xPos + j) + (half_room_width * 2)) - 5, height_of_blocks, ((currentRoom.yPos + i) + (half_room_height * 2)) - 2.5f);

                            GameObject tileInstance = Instantiate(obstacle_blocks, temp_vec * 2, Quaternion.identity) as GameObject;
                            total_blocks.Add(tileInstance);

                            // tiles[((currentRoom.xPos + j) + (half_room_width * 2)) + 2][((currentRoom.yPos + i) + (half_room_height * 2)) + 2] = TileType.Wall;
                        }

                        --x;
                    }
                    --y;
                }

                //TOP LEFT
                y = half_room_height - 1;
                for (int i = 0; i < half_room_height - 1; ++i)
                {
                    for (int j = 0; j < half_room_width; ++j)
                    {
                        if (temp_array[y][j] == 1)
                        {
                            Vector3 temp_vec = new Vector3(((currentRoom.xPos + j) + half_room_width) - 7, height_of_blocks, ((currentRoom.yPos + i) + (half_room_height * 2)) - 2.5f);

                            GameObject tileInstance = Instantiate(obstacle_blocks, temp_vec * 2, Quaternion.identity) as GameObject;
                            total_blocks.Add(tileInstance);

                            // tiles[((currentRoom.xPos + j) + half_room_width) - 2][((currentRoom.yPos + i) + (half_room_height * 2)) + 2] = TileType.Wall;
                        }
                    }
                    --y;
                }

                //for (int i = 0; i < half_room_height; ++i)
                //{
                //    string temp_string = "";
                //    for (int j = 0; j < half_room_width; ++j)
                //    {
                //        temp_string += temp_array[i][j];
                //    }

                //    Debug.Log(temp_string);
                //}
            }
        }
    }


    void SetTilesValuesForCorridors()
    {
        // Go through every corridor...
        for (int loop = 0; loop < 3; ++loop)
        {
           foreach(Corridor currentCorridor in total_corridor)
            {
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
                            xCoord += loop;

                            if (j == 0 || j == corridorLength - 2)
                            {
                                Vector3 temp_vec = new Vector3(xCoord * 2, height_of_blocks, yCoord * ((yCoord < 20) ? 1.865f : 1.93f));

                                GameObject tileInstance = Instantiate(door_blocks, temp_vec, Quaternion.identity) as GameObject;
                                total_blocks.Add(tileInstance);
                            }
                            break;
                        case Direction.Left:
                            xCoord -= j;
                            yCoord += loop;

                            if (j == 0 || j == corridorLength - 2)
                            {
                                Vector3 temp_vec = new Vector3(xCoord * 2, height_of_blocks, yCoord * 2);

                                GameObject tileInstance = Instantiate(door_blocks, temp_vec, Quaternion.identity) as GameObject;
                                total_blocks.Add(tileInstance);
                            }
                            break;
                        case Direction.Down:
                            yCoord -= j;
                            xCoord -= loop;

                            if (j == 0 || j == corridorLength - 2)
                            {
                                Vector3 temp_vec = new Vector3(xCoord * 2, height_of_blocks, yCoord * ((yCoord < 20) ? 1.865f : 1.93f));

                                GameObject tileInstance = Instantiate(door_blocks, temp_vec, Quaternion.identity) as GameObject;
                                total_blocks.Add(tileInstance);
                            }
                            break;
                        case Direction.Right:
                            xCoord += j;
                            yCoord -= loop;

                            if (j == 0 || j == corridorLength - 2)
                            {
                                Vector3 temp_vec = new Vector3(xCoord * 2, height_of_blocks, yCoord * 2);

                                GameObject tileInstance = Instantiate(door_blocks, temp_vec, Quaternion.identity) as GameObject;
                                total_blocks.Add(tileInstance);
                            }
                            break;
                    }

                    // Set the tile at these coordinates to Floor.
                    tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }

        foreach (GameObject obj in total_blocks)
        {
            if (obj.CompareTag("Door"))
            {
                for (int i = 0; i < total_spawners.Length; ++i)
                {
                    total_spawners[i].GetComponent<SpawnerBlock>().AddDoors(obj);
                }
            }
        }
    }


    void InstantiateTiles()
    {
        // ... and instantiate a floor tile for it.
        // Create an instance of the prefab from the random index of the array.
        Vector3 position = new Vector3(roomWidth * 3.45f, 0, roomHeight * 3.9f);
        GameObject tileInstance = Instantiate(floorTiles[0], position, Quaternion.identity) as GameObject;
        
        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
        tileInstance.transform.localScale = new Vector3(columns * 0.2f, tileInstance.transform.localScale.y, rows * 0.2f);

        total_blocks.Add(tileInstance);

        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
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
        Vector3 position = new Vector3(xCoord * 2, height_of_blocks, yCoord * 2);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;
        total_blocks.Add(tileInstance);

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }

    void SetUpMap()
    {
        SpawnerManager.Instance.ResetSpawnerManager();
        player_obj_script.SetLevel(player_obj_script.GetpStats().level + 1);
        player_prev_room = -1;
        spawned_exit = false;
        portal_obj_script.gameObject.SetActive(false);

        for (int i = 0; i < total_spawners.Length; ++i)
        {
            total_spawners[i].GetComponent<SpawnerBlock>().Reset();
            total_spawners[i].SetActive(false);
        }

        for (int i = 0; i < total_quads.Length; ++i)
        {
            total_quads[i].SetActive(false);
        }

        GameObject []delete_array = GameObject.FindGameObjectsWithTag("Coin");

        for(int i = 0; i < delete_array.Length; ++i)
        {
            Destroy(delete_array[i]);
        }

        delete_array = GameObject.FindGameObjectsWithTag("Blessing");

        for (int i = 0; i < delete_array.Length; ++i)
        {
            Destroy(delete_array[i]);
        }

        SetupTilesArray();

        CreateRoomsAndCorridors();

        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        InstantiateTiles();
        InstantiateOuterWalls();

        AI_Controller_Grid.CreateGrid();

        canvas_layout.GetComponent<UIScript>().SetRoomLevelLayout();
        //canvas_layout.GetComponent<UIScript>().SetRoomObjective("Explore");
    }
}
