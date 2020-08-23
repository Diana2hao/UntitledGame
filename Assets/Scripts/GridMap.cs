using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    private int width;
    private int height;
    private float cellSize = 1;
    private Grid[,] gridArray;

    public GridMap(int width, int height)
    {
        this.width = width;
        this.height = height;

        gridArray = new Grid[width, height];

        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int z = 0; z < gridArray.GetLength(1); z++)
            {
                gridArray[x, z] = new Grid((int)GridType.EMPTY);
            }
        }
    }

    //scale = 1 -> object occupies 1*1 grid, scale = 2 -> object occupies 2*2 grid
    private Vector3 GetGridCenterWorldPosition(int x, int z, int scale = 1)
    {
        return new Vector3(x, 0, z) * cellSize + new Vector3(cellSize * scale / 2, 0, cellSize * scale / 2);
    }

    private void GetXZ(Vector3 worldPostion, out int x, out int z, int scale = 1)
    {
        x = Mathf.FloorToInt(worldPostion.x / (cellSize * scale)) * scale;
        z = Mathf.FloorToInt(worldPostion.z / (cellSize * scale)) * scale;
    }

    public Vector3 GetGridCenterOfPosition(Vector3 position, int scale = 1)
    {
        GetXZ(position, out int x, out int z, scale);
        return GetGridCenterWorldPosition(x, z, scale);
    }



    public bool AddGameObject(int x, int z, GameObject go)
    {
        if(x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridArray[x, z].AddGameObject(go);
        }

        return false;
    }

    public bool AddGameObject(Vector3 worldPosition, GameObject go)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return AddGameObject(x, z, go);
    }

    public void BlockOutGrids(Vector3 from, Vector3 to)
    {
        int x_from, z_from, x_to, z_to;
        GetXZ(from, out x_from, out z_from);
        GetXZ(to, out x_to, out z_to);
        for (int x = x_from; x <= x_to; x++)
        {
            for (int z = z_from; z <= z_to; z++)
            {
                gridArray[x, z].SetGridType((int)GridType.BLOCKED);
            }
        }
    }
    
}
