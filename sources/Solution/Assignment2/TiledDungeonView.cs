using GXPEngine;

//TiledDungeonView class extends TiledView
class TiledDungeonView : TiledView
{
    //Reference to the dungeon object
    private Dungeon _dungeon;

    //Constructor for TiledDungeonView
    public TiledDungeonView(Dungeon pDungeon, TileType pDefaultTileType) : base(pDungeon.size.Width, pDungeon.size.Height, (int)pDungeon.scale, pDefaultTileType)
    {
        _dungeon = pDungeon;
    }

    //Override the generate method to generate the tiled dungeon view
    protected override void generate()
    {
        //First, set all tiles to the default (assuming the default is a wall)
        resetAllTilesToDefault();

        //Iterate through all rooms
        foreach (Room room in _dungeon.rooms)
        {
            for (int x = room.area.Left; x < room.area.Right; x++)
            {
                for (int y = room.area.Top; y < room.area.Bottom; y++)
                {
                    //Check if the tile is at the boundary of the room
                    bool isBoundary = x == room.area.Left || x == room.area.Right - 1 || y == room.area.Top || y == room.area.Bottom - 1;

                    //Set the tile type based on whether it's a boundary or not
                    SetTileType(x, y, isBoundary ? TileType.WALL : TileType.GROUND);
                }
            }
        }

        //Iterate through all doors and set their tiles to GROUND
        foreach (Door door in _dungeon.doors)
        {
            SetTileType(door.location.X, door.location.Y, TileType.GROUND);
        }
    }
}