using UnityEngine;

public class Room
{
    public int xPos;                      // The x coordinate of the lower left tile of the room.
    public int yPos;                      // The y coordinate of the lower left tile of the room.
    public int roomWidth;                     // How many tiles wide the room is.
    public int roomHeight;                    // How many tiles high the room is.
    public Direction enteringCorridor;    // The direction of the corridor that is entering this room.


    // This is used for the first room.  It does not have a Corridor parameter since there are no corridors yet.
    public void SetupRoom(int _roomWidth, int _roomHeight, int columns, int rows)
    {
        roomWidth = _roomWidth;
        roomHeight = _roomHeight;
        // Set the x and y coordinates so the room is roughly in the middle of the board.
        xPos = Mathf.RoundToInt(columns / 2f - roomWidth / 2f);
        yPos = Mathf.RoundToInt(rows / 2f - roomHeight / 2f);
    }


    // This is an overload of the SetupRoom function and has a corridor parameter that represents the corridor entering the room.
    public void SetupRoom(int _roomWidth, int _roomHeight, int columns, int rows, int xcoord, int ycoord)
    {
        // Set the entering corridor direction.
        //enteringCorridor = corridor.direction;

        // Set random values for width and height.
        roomWidth = _roomWidth;
        roomHeight = _roomHeight;

        yPos = ycoord * (roomWidth + 5);
        xPos = xcoord * (roomHeight + 5);
    }
}