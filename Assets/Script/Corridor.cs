using UnityEngine;

// Enum to specify the direction is heading.
public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}

public class Corridor
{
    public int startXPos;         // The x coordinate for the start of the corridor.
    public int startYPos;         // The y coordinate for the start of the corridor.
    public int corridorLength;            // How many units long the corridor is.
    public Direction direction;   // Which direction the corridor is heading from it's room.


    // Get the end position of the corridor based on it's start position and which direction it's heading.
    public int EndPositionX
    {
        get
        {
            if (direction == Direction.Up || direction == Direction.Down)
                return startXPos;
            if (direction == Direction.Right)
                return startXPos + corridorLength;
            return startXPos - corridorLength;
        }
    }


    public int EndPositionY
    {
        get
        {
            if (direction == Direction.Left || direction == Direction.Right)
                return startYPos;
            if (direction == Direction.Up)
                return startYPos + corridorLength;
            return startYPos - corridorLength;
        }
    }


    public void SetupCorridor(Room room, int length, int roomWidth, int roomHeight, int columns, int rows, bool firstCorridor, int dir)
    {
        // Set a random direction (a random index from 0 to 3, cast to Direction).
        direction = (Direction)dir;

        switch (direction)
        {
            // If the choosen direction is North (up)...
            case Direction.Up:
                // ... the starting position in the x axis can be random but within the width of the room.
                startXPos = room.xPos + ((int)(room.roomWidth * 0.5));

                // The starting position in the y axis must be the top of the room.
                startYPos = room.yPos + room.roomHeight;

                break;
            case Direction.Right:
                startXPos = room.xPos + room.roomWidth;
                startYPos = room.yPos + ((int)(room.roomHeight * 0.5));
             
                break;
            case Direction.Down:
                startXPos = room.xPos + ((int)(room.roomWidth * 0.5));
                startYPos = room.yPos;
               
                break;
            case Direction.Left:
                startXPos = room.xPos;
                startYPos = room.yPos + ((int)(room.roomHeight * 0.5));

                break;
        }

        // We clamp the length of the corridor to make sure it doesn't go off the board.
        corridorLength = length;
    }
}