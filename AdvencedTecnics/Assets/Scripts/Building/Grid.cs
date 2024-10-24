using UnityEngine;

public class Grid
{
    int _width; //x
    int _height; //y
    int _length; //z

    float _cellSize;

    int[,,] _gridArray;

    public Grid(int width, int height,int length, float cellSize)
    {
        _width = width;
        _height = height;
        _length = length;

        _cellSize = cellSize;


        _gridArray = new int[width, height, length];

        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= height; y++)
            {
                for (int z = 0; z <= length; z++)
                {
                    if (x < width)
                    {
                        Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x + 1, y, z), Color.red, 200f);  
                    }
                    if (y < height)
                    {
                        Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y + 1, z), Color.green, 200f);  
                    }
                    if (z < length)
                    {
                        Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y, z + 1), Color.blue, 200f);  
                    }
                }
            }
        }

        Vector3 GetWorldPosition(int x, int y, int z)
        {
            return new Vector3(x, y, z) * _cellSize;
        }
    }


}
