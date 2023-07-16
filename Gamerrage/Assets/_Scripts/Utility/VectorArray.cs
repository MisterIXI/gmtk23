using System;
using UnityEngine;
public class VectorArray<T>
{
    private T[,] arr;

    public VectorArray(Vector2Int dimensions) : this(dimensions.x, dimensions.y)
    {
    }
    public VectorArray(int sizeX, int sizeY)
    {
        arr = new T[sizeX, sizeY];
    }
    public T this[Vector2Int coord]
    {
        get { return (T)arr[coord.x, coord.y]; }
        set { arr[coord.x, coord.y] = value; }
    }

    public T this[int x, int y]
    {
        get { return (T)arr[x, y]; }
        set { arr[x, y] = value; }
    }

    public T[,] GetArr() => arr;
    public int GetLength(int dimension) => arr.GetLength(dimension);
}