using System;
using System.Drawing;
using GXPEngine;

//SufficientDungeon class inherits from the Dungeon class
class SufficientDungeon : Dungeon
{
    //Private field to store the minimum room size
    private int _minimumRoomSize;

    //Constructor for SufficientDungeon, calls base constructor with size parameter
    public SufficientDungeon(Size pSize) : base(pSize)
    {

    }

    //Override the generate method from the base class to create the dungeon layout
    protected override void generate(int pMinimumRoomSize)
    {
        //Set the minimum room size and create a main room from which we start creating the dungeon
        _minimumRoomSize = pMinimumRoomSize;
        Room mainRoom = new Room(new Rectangle(0, 0, size.Width, size.Height));

        //Split the main room into smaller rooms
        Split(mainRoom);
        Console.WriteLine(rooms.Count + " rooms created.");

        //Place doors to connect the rooms
        DoorPlacement();
        Console.WriteLine("Doors to connect the rooms created.");
    }

    //Split the main room into smaller rooms to create the dungeon layout
    void Split(Room pRoom)
    {
        //Choose randomly whether to split the room vertically or horizontally
        if (Utils.Random(0, 2) == 1) VerticalSplit(pRoom); else HorizontalSplit(pRoom);
    }

    //Method to handle vertical splits
    void VerticalSplit(Room pRoom)
    {
        Console.WriteLine("Vertical split");

        //Check if the room is big enough to split vertically
        if (pRoom.area.Width >= _minimumRoomSize * 2)
        {
            //Determine the x-coordinate for the vertical split
            int xSplit = Utils.Random(_minimumRoomSize, pRoom.area.Width - _minimumRoomSize);

            //Create two new rooms from the split (left and right rooms)
            Room room1 = new Room(new Rectangle(pRoom.area.X, pRoom.area.Y,
                pRoom.area.Width - (pRoom.area.Width - xSplit) + 1, pRoom.area.Height));

            Room room2 = new Room(new Rectangle(pRoom.area.X + xSplit, pRoom.area.Y,
                pRoom.area.Width - xSplit, pRoom.area.Height));

            //Recursively split the new rooms
            Split(room1);
            Split(room2);
        }
        else
        {
            //If the room is too small to split, add it to the list of rooms
            Console.WriteLine("Room too small for vertical split. Adding to the list of rooms.");
            rooms.Add(pRoom);
        }
    }

    //Method to handle horizontal splits
    void HorizontalSplit(Room pRoom)
    {
        Console.WriteLine("Horizontal split");

        //Check if the room is big enough to split horizontally
        if (pRoom.area.Height >= _minimumRoomSize * 2)
        {
            //Determine the y-coordinate for the horizontal split
            int ySplit = Utils.Random(_minimumRoomSize, pRoom.area.Height - _minimumRoomSize);

            //Create two new rooms from the split (top and bottom rooms)
            Room room1 = new Room(new Rectangle(pRoom.area.X, pRoom.area.Y,
                pRoom.area.Width, pRoom.area.Height - (pRoom.area.Height - ySplit) + 1));

            Room room2 = new Room(new Rectangle(pRoom.area.X, pRoom.area.Y + ySplit,
                pRoom.area.Width, pRoom.area.Height - ySplit));

            //Recursively split the new rooms
            Split(room1);
            Split(room2);
        }
        else
        {
            //If the room is too small to split, add it to the list of rooms
            Console.WriteLine("Room too small for horizontal split. Adding to the list of rooms.");
            rooms.Add(pRoom);
        }
    }

    //Create doors connecting the rooms
    private void DoorPlacement()
    {
        for (int index = 0; index < rooms.Count; index++)
        {
            Room currentRoom = rooms[index];

            for (int i = index + 1; i < rooms.Count; i++)
            {
                Room nextRoom = rooms[i];

                if (currentRoom != nextRoom)
                {
                    //Check if the rooms are adjacent
                    Rectangle rect = Rectangle.Intersect(currentRoom.area, nextRoom.area);
                    if (rect.IsEmpty)
                    {
                        Console.WriteLine("Rooms aren't neighbours. Try the next one...");
                        continue;
                    }

                    //Ensure the intersection area is large enough for a door
                    if (rect.Width * rect.Height < 5) continue;
                    Console.Write("Rooms are neighbours. ");

                    //Create a door in the appropriate location
                    if (rect.Width == 1)
                    {
                        Console.WriteLine("Creating door on vertical.");
                        Door doorToAdd = new Door(new Point(Utils.Random(rect.Left, rect.Right), Utils.Random(rect.Top + 1, rect.Bottom - 1)));
                        doors.Add(doorToAdd);
                    }
                    else
                    {
                        Console.WriteLine("Creating door on horizontal.");
                        Door doorToAdd = new Door(new Point(Utils.Random(rect.Left + 1, rect.Right - 1), Utils.Random(rect.Top, rect.Bottom)));
                        doors.Add(doorToAdd);
                    }
                }
            }
        }
    }
}
