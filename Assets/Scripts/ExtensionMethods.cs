using System;
using Model;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2 ToVector2(this Cell cell)
    {
        return new Vector2(cell.X+0.5f, cell.Y+0.5f);
    }

    public static Cell ToCell(this Vector2 point)
    {
        var ret = new Cell();
        ret.X = Mathf.FloorToInt(point.x);
        ret.Y = Mathf.FloorToInt(point.y);
        return ret;
    }
}