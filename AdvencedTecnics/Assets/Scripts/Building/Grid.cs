using JetBrains.Annotations;
using UnityEngine;

public class Grid
{
    [Header("Grid")]
    int _width; //x
    int _height; //y
    int _length; //z
    int[,,] _gridArray;

    [Header("Cell")]
    public float CellSize { get; set; }
    public myCell[] CellStruct { get; set; }

    public struct myCell
    {
      public  int CellNumber { get; set; }
       public bool HasRamp { get; set; }
    }

    public Grid(int width, int height,int length, float cellSize)
    {
        _width = width;
        _height = height;
        _length = length;

        CellSize = cellSize;

        _gridArray = new int[width, height, length];

        AddCellData();

       //draw Grid Gizmos
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
            return new Vector3(x, y, z) * CellSize;
        }
    }

    public void GetXYZ(Vector3 worldPos, out int x, out int y, out int z)
    {
        //returns the cell values based on the world position
        x = Mathf.FloorToInt(worldPos.x / CellSize);
        y = Mathf.FloorToInt(worldPos.y / CellSize);
        z = Mathf.FloorToInt(worldPos.z / CellSize);

        //dont allow to get cell values out of defined grid size
        if ( x <= 0 || x >= _width)
        {
            x = 0;
        }
        if (y <= 0 || y >= _height)
        {
            y = 0;
        }
        if (z <= 0 || z >= _length)
        {
            z = 0;
        }
    }

    public void GetCellNumber(Vector3 worldPosition, out int cellNumber)
    {
        //by reciving the world position we will find whats that cell number
        cellNumber = -1; 

        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);

            int index = x + (y * _gridArray.GetLength(0)) + (z * _gridArray.GetLength(0) * _gridArray.GetLength(1));

            if (index < CellStruct.Length)
            {
                cellNumber = CellStruct[index].CellNumber;
            }

    }
    public Vector3 GetCellPlacePoint(Vector3 worldPosition)
    {
        //gets cell´s values and turns into a vector3 
        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);

        Vector3 cellPos = GetCellCenter(x, y, z);

       return cellPos;
    }


    Vector3 GetCellCenter(int x, int y, int z)
    {
        return new Vector3(x, y, z) * CellSize + Vector3.one * CellSize / 2;
    }

    public void AddCellData()
    {
        //adds info to each cell on inicializing the grid
        CellStruct = new myCell[_gridArray.Length];

        for (int i = 0; i < _gridArray.Length; i++)
        {
            CellStruct[i].CellNumber = i;
            CellStruct[i].HasRamp = false;
        }
    }
}
