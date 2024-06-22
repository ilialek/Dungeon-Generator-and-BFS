using System;
using System.Collections.Generic;
using System.Drawing;

//HighLevelDungeonNodeGraph class extends SampleDungeonNodeGraph
internal class HighLevelDungeonNodeGraph : SampleDungeonNodeGraph
{
    //Dictionary to store nodes corresponding to doors
    private readonly Dictionary<Point, Node> _doorDict = new Dictionary<Point, Node>();

    //Dictionary to store nodes corresponding to rooms
    private readonly Dictionary<Point, Node> _roomDict = new Dictionary<Point, Node>();

    //Constructor for HighLevelDungeonNodeGraph
    public HighLevelDungeonNodeGraph(Dungeon pDungeon) : base(pDungeon)
    {

    }

    //Override the generate method from the base class to create the node graph
    protected override void generate()
    {
        //Get the center of the room and put a node there for each room
        Console.WriteLine("Putting nodes in the center of the rooms...");
        foreach (Room dungeonRoom in _dungeon.rooms)
        {
            Point roomCenter = getRoomCenter(dungeonRoom);
            Node node = new Node(roomCenter);
            nodes.Add(node);
            _roomDict.Add(roomCenter, node);
        }
        Console.WriteLine("Room nodes added.");

        //Get the center/position of the door and put a node there for each door
        Console.WriteLine("Putting nodes on the doors...");
        foreach (Door dungeonDoor in _dungeon.doors)
        {
            Point doorCenter = getDoorCenter(dungeonDoor);
            Node node = new Node(doorCenter);
            nodes.Add(node);
            _doorDict.Add(doorCenter, node);
        }
        Console.WriteLine("Door nodes added.");
        Console.WriteLine(_roomDict.Count + _doorDict.Count + " nodes created.");

        //Create connections between a room node and the door nodes the room contains
        Console.WriteLine("Connecting all nodes...");

        foreach (Room room in _dungeon.rooms)
        {
            foreach (Door door in _dungeon.doors)
            {
                //Find the doors for every room and create connections
                if (room.area.Contains(door.location))
                {
                    AddConnection(_roomDict[getRoomCenter(room)], _doorDict[getDoorCenter(door)]);
                }
            }
        }
        Console.WriteLine("Nodes connected!");
    }
}

