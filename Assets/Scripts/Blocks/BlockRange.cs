using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRange
{

    public List<Tile> GetInRangeTiles(Tile t, BlockType b)
    {
        /// Cool distance 1.
        List<Tile> tList = new List<Tile>();
        switch (b)
        {

            case BlockType.Magma:
                tList = GetMagmaBlockTiles(t);
                break;
            case BlockType.BlueIce:
                tList = GetIceBlockTiles(t);
                break;
            case BlockType.TNT:
                tList = GetTNTBlockTiles(t);
                break;
            case BlockType.Evaporator:
                tList = GetEvaporatorBlockTiles(t);
                break;
            case BlockType.Condensation:
                tList = GetCondensorBlockTiles(t);
                break;
            default:
                //Otherwise No Range
                break;
        }

        return tList;



    }


    public List<Tile> GetIceBlockTiles(Tile t)
    {

        List<Tile> tList = new List<Tile>();
        tList.Add(t.upTile);
        tList.Add(t.downTile);
        tList.Add(t.leftTile);
        tList.Add(t.rightTile);

        /// Cool distance 2.
        tList.Add(t.getRelativeTile(new Vector2(+2, +0)));
        tList.Add(t.getRelativeTile(new Vector2(-2, +0)));
        tList.Add(t.getRelativeTile(new Vector2(+0, +2)));
        tList.Add(t.getRelativeTile(new Vector2(+0, -2)));

        tList.Add(t.getRelativeTile(new Vector2(+1, +1)));
        tList.Add(t.getRelativeTile(new Vector2(+1, -1)));
        tList.Add(t.getRelativeTile(new Vector2(-1, +1)));
        tList.Add(t.getRelativeTile(new Vector2(-1, -1)));

        /// Cool distance 3.
        tList.Add(t.getRelativeTile(new Vector2(+3, +0)));
        tList.Add(t.getRelativeTile(new Vector2(-3, +0)));
        tList.Add(t.getRelativeTile(new Vector2(+0, +3)));
        tList.Add(t.getRelativeTile(new Vector2(+0, -3)));

        tList.Add(t.getRelativeTile(new Vector2(+2, +1)));
        tList.Add(t.getRelativeTile(new Vector2(+2, -1)));
        tList.Add(t.getRelativeTile(new Vector2(-2, +1)));
        tList.Add(t.getRelativeTile(new Vector2(-2, -1)));

        tList.Add(t.getRelativeTile(new Vector2(+1, +2)));
        tList.Add(t.getRelativeTile(new Vector2(+1, -2)));
        tList.Add(t.getRelativeTile(new Vector2(-1, +2)));
        tList.Add(t.getRelativeTile(new Vector2(-1, -2)));
        return tList;
    }


    public List<Tile> GetMagmaBlockTiles(Tile t)
    {

        List<Tile> tList = new List<Tile>();
        tList.Add(t.upTile);
        tList.Add(t.downTile);
        tList.Add(t.leftTile);
        tList.Add(t.rightTile);

        /// Heat corners.
        tList.Add(t.getRelativeTile(new Vector2(+1, +1)));
        tList.Add(t.getRelativeTile(new Vector2(+1, -1)));
        tList.Add(t.getRelativeTile(new Vector2(-1, +1)));
        tList.Add(t.getRelativeTile(new Vector2(-1, -1)));

        /// Heat further water.
        tList.Add(t.getRelativeTile(new Vector2(+2, +2)));
        tList.Add(t.getRelativeTile(new Vector2(+2, -2)));
        tList.Add(t.getRelativeTile(new Vector2(-2, +2)));
        tList.Add(t.getRelativeTile(new Vector2(-2, -2)));

        tList.Add(t.getRelativeTile(new Vector2(+2, +1)));
        tList.Add(t.getRelativeTile(new Vector2(+2, -1)));
        tList.Add(t.getRelativeTile(new Vector2(-2, +1)));
        tList.Add(t.getRelativeTile(new Vector2(-2, -1)));

        tList.Add(t.getRelativeTile(new Vector2(+1, +2)));
        tList.Add(t.getRelativeTile(new Vector2(+1, -2)));
        tList.Add(t.getRelativeTile(new Vector2(-1, +2)));
        tList.Add(t.getRelativeTile(new Vector2(-1, -2)));

        tList.Add(t.getRelativeTile(new Vector2(+2, 0)));
        tList.Add(t.getRelativeTile(new Vector2(-2, 0)));
        tList.Add(t.getRelativeTile(new Vector2(0, +2)));
        tList.Add(t.getRelativeTile(new Vector2(0, -2)));


        return tList;
    }


    public List<Tile> GetCondensorBlockTiles(Tile t)
    {

        List<Tile> tList = new List<Tile>();
        tList.Add(t.upTile);
        tList.Add(t.downTile);
        tList.Add(t.leftTile);
        tList.Add(t.rightTile);
/*
        tList.Add(t.getRelativeTile(new Vector2(+1, +1)));
        tList.Add(t.getRelativeTile(new Vector2(+1, -1)));
        tList.Add(t.getRelativeTile(new Vector2(-1, +1)));
        tList.Add(t.getRelativeTile(new Vector2(-1, -1)));*/

        return tList;
    }

    public List<Tile> GetEvaporatorBlockTiles(Tile t)
    {

        List<Tile> tList = new List<Tile>();
        tList.Add(t.upTile);
        tList.Add(t.downTile);
        tList.Add(t.leftTile);
        tList.Add(t.rightTile);

       /* tList.Add(t.getRelativeTile(new Vector2(+1, +1)));
        tList.Add(t.getRelativeTile(new Vector2(+1, -1)));
        tList.Add(t.getRelativeTile(new Vector2(-1, +1)));
        tList.Add(t.getRelativeTile(new Vector2(-1, -1)));*/

        return tList;
    }

    public List<Tile> GetTNTBlockTiles(Tile t)
    {

        List<Tile> tList = new List<Tile>();
        tList.Add(t.upTile);
        tList.Add(t.downTile);
        tList.Add(t.leftTile);
        tList.Add(t.rightTile);

        /// Heat corners.
        tList.Add(t.getRelativeTile(new Vector2(+1, +1)));
        tList.Add(t.getRelativeTile(new Vector2(+1, -1)));
        tList.Add(t.getRelativeTile(new Vector2(-1, +1)));
        tList.Add(t.getRelativeTile(new Vector2(-1, -1)));

        /// Heat further water.
        tList.Add(t.getRelativeTile(new Vector2(+0, +2)));
        tList.Add(t.getRelativeTile(new Vector2(+0, -2)));
        tList.Add(t.getRelativeTile(new Vector2(+2, +0)));
        tList.Add(t.getRelativeTile(new Vector2(-2, +0)));

        return tList;
    }


}
