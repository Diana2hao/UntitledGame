using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;
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

        //fill grid array with empty grids
        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int z = 0; z < gridArray.GetLength(1); z++)
            {
                gridArray[x, z] = new Grid((int)GridType.EMPTY);
            }
        }
    }

    //get the world position of the center of a grid, based on the scale wanted
    //scale = 1 -> object occupies 1*1 grid, scale = 2 -> object occupies 2*2 grid
    private Vector3 GetGridCenterWorldPosition(int x, int z, int scale = 1)
    {
        return new Vector3(x, 0, z) * cellSize + new Vector3(cellSize * scale / 2, 0, cellSize * scale / 2);
    }

    //get the xz index of grid array from a world position
    private void GetXZ(Vector3 worldPostion, out int x, out int z, int scale = 1)
    {
        x = Mathf.FloorToInt(worldPostion.x / (cellSize * scale)) * scale;
        z = Mathf.FloorToInt(worldPostion.z / (cellSize * scale)) * scale;
    }

    //get the grid this position is on and return the center of that grid
    public Vector3 GetGridCenterOfPosition(Vector3 position, int scale = 1)
    {
        GetXZ(position, out int x, out int z, scale);
        return GetGridCenterWorldPosition(x, z, scale);
    }

    //whether the grid at this position is empty
    public bool isEmptyAtPosition(Vector3 position)
    {
        GetXZ(position, out int x, out int z);
        return gridArray[x, z].isEmpty();
    } 

    //add a game object to grid[x,z]
    public bool AddGameObject(int x, int z, GameObject go)
    {
        if(x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridArray[x, z].AddGameObject(go);
        }

        return false;
    }

    //add a game object to grid at this world position
    public bool AddGameObject(Vector3 worldPosition, GameObject go)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return AddGameObject(x, z, go);
    }

    public void AddGameObjectOfScale(Vector3 worldPosition, GameObject go, int scale)
    {
        GetXZ(worldPosition - new Vector3(scale / 2, 0, scale / 2), out int x, out int z);
        for (int xOffset = 0; xOffset < scale; xOffset++)
        {
            for (int zOffset = 0; zOffset < scale; zOffset++)
            {
                gridArray[x + xOffset, z + zOffset].AddGameObject(go);
            }
        }
    }

    public void RemoveGameObjectOfScale(GameObject go, int scale)
    {
        GetXZ(go.transform.position - new Vector3(scale / 2, 0, scale / 2), out int x, out int z);
        for (int xOffset = 0; xOffset < scale; xOffset++)
        {
            for (int zOffset = 0; zOffset < scale; zOffset++)
            {
                gridArray[x + xOffset, z + zOffset].RemoveGameObject();
            }
        }
    }

    //block out the grids from 'from' to 'to'
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

    public void BlockOutGrids(Vector3 gridPos)
    {
        GetXZ(gridPos, out int x, out int z);
        gridArray[x, z].SetGridType((int)GridType.BLOCKED);
    }

    //for each grid that is not blocked, generate a mosaic map for player to see
    public GameObject GenerateMosaicMap(GameObject mosaic)
    {
        GameObject ms = new GameObject("MosaicMap");

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                if(!gridArray[x, z].isBlocked())
                {
                    Vector3 position = new Vector3(x, 0.06f, z);
                    GameObject m = Object.Instantiate(mosaic, position, Quaternion.Euler(0, 180, 0));
                    m.transform.parent = ms.transform;
                }
            }
        }

        return ms;
    }
    
    public bool FindPlantPosition(Vector3 playerPosition, Vector3 playerForwardDirection, int scale, out Vector3 plantPosition)
    {
        Vector3 hitPoint;
        int xOrig;
        int zOrig;
        if (scale % 2 == 0)
        {
            GetXZ(playerPosition, out xOrig, out zOrig);
            hitPoint = new Vector3(xOrig + 1, 0, zOrig + 1) + playerForwardDirection.normalized * 0.5f;
        }
        else
        {
            hitPoint = playerPosition + playerForwardDirection.normalized;
        }

        GetXZ(hitPoint, out xOrig, out zOrig);
        xOrig -= Mathf.FloorToInt(scale / 2);
        zOrig -= Mathf.FloorToInt(scale / 2);
        for (int xOffset = 0; xOffset < scale; xOffset++)
        {
            for (int zOffset = 0; zOffset < scale; zOffset++)
            {
                int x = xOrig + xOffset;
                int z = zOrig + zOffset;
                if (x >= width || z >= height)
                {
                    plantPosition = new Vector3(0, 0, 0);
                    return false;
                }

                if (!gridArray[x, z].isEmpty())
                {
                    plantPosition = new Vector3(0, 0, 0);
                    return false;
                }
            }
        }
        float xp = xOrig + scale / 2f;
        float zp = zOrig + scale / 2f;
        plantPosition = new Vector3(xp, 0, zp);
        return true;
    }

    public bool FindTrapPosition(GameObject tree, int scale, out Vector3 trapPosition, out Vector3 poacherPosition, out Vector3 hidePosition)
    {
        GetXZ(tree.transform.position - new Vector3((scale + 2) / 2, 0, (scale + 2) / 2), out int x, out int z);
        List<Vector2Int> emptyGrids = new List<Vector2Int>();

        //find all empty grids around the tree for trap placement
        for (int xOffset = 0; xOffset < scale + 2; xOffset++)
        {
            for (int zOffset = 0; zOffset < scale + 2; zOffset++)
            {
                if(gridArray[x + xOffset, z + zOffset].isEmpty())
                {
                    emptyGrids.Add(new Vector2Int(x + xOffset, z + zOffset));
                }
            }
        }

        //find a random grid (trap) with 2 empty grids (poachr) along a line
        Random rnd = new Random();
        List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left }.OrderBy(a => rnd.Next()).ToList();

        foreach (Vector2Int trap in emptyGrids.OrderBy(a => rnd.Next()).ToList())
        {
            foreach(Vector2Int dir in directions)
            {
                if(gridArray[(trap +dir).x, (trap + dir).y].isEmpty() && gridArray[(trap + dir*2).x, (trap + dir*2).y].isEmpty())
                {
                    trapPosition = GetGridCenterWorldPosition(trap.x, trap.y);
                    poacherPosition = new Vector3(trapPosition.x + dir.x, 0, trapPosition.z + dir.y);
                    hidePosition = new Vector3(trapPosition.x + dir.x * 2, 0, trapPosition.z + dir.y * 2);
                    return true;
                }
            }
        }

        trapPosition = poacherPosition = hidePosition = Vector3.zero;
        return false;
    }

    public HashSet<GameObject> FindTreeNextToTrap(TrapController trap)
    {
        HashSet<GameObject> trees = new HashSet<GameObject>();
        GetXZ(trap.transform.position, out int x, out int z);
        for (int xi = x - 1; xi <= x + 1; xi++)
        {
            for (int zi = z - 1; zi <= z + 1; zi++)
            {
                if(xi != x || zi != z)
                {
                    GameObject go = gridArray[xi, zi].GetGridObject();
                    if(go != null)
                    {
                        trees.Add(go);
                    }
                }
            }
        }
        return trees;
    }
}
