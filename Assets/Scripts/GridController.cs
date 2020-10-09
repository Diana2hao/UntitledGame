using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int GridSize;
    public FromTO[] BlockedAreas;
    public Vector3[] BlockedGrids;
    public GameObject mosaic;

    GridMap gridMap;
    HashSet<GameObject> players;
    GameObject mosaicMap;

    ////test
    //public GameObject prefabBlock;
    //public GameObject prefabEmpty;

    [System.Serializable]
    public struct FromTO
    {
        public Vector3 from;
        public Vector3 to;
    }

    // Start is called before the first frame update
    void Start()
    {
        gridMap = new GridMap(GridSize.x, GridSize.y);

        foreach(FromTO ft in BlockedAreas)
        {
            gridMap.BlockOutGrids(ft.from, ft.to);
        }

        foreach (Vector3 g in BlockedGrids)
        {
            gridMap.BlockOutGrids(g);
        }

        mosaicMap = gridMap.GenerateMosaicMap(mosaic);
        mosaicMap.SetActive(false);

        players = new HashSet<GameObject>();

        ////test
        //for (int x = 0; x < gridMap.gridArray.GetLength(0); x++)
        //{
        //    for (int z = 0; z < gridMap.gridArray.GetLength(1); z++)
        //    {
        //        Vector3 pos = gridMap.GetGridCenterWorldPosition(x, z);
        //        if (gridMap.gridArray[x, z].GetGridType() == (int)GridType.BLOCKED)
        //        {
        //            Instantiate(prefabBlock, pos, Quaternion.identity);
        //        }
        //        if (gridMap.gridArray[x, z].GetGridType() == (int)GridType.EMPTY)
        //        {
        //            Instantiate(prefabEmpty, pos, Quaternion.identity);
        //        }
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMosaic(bool show, GameObject player)
    {
        if (show)
        {
            players.Add(player);
            mosaicMap.SetActive(show);
        }
        else
        {
            players.Remove(player);
            if (players.Count == 0)
            {
                mosaicMap.SetActive(show);
            }
        }
    }

    public bool isEmptyAtPosition(Vector3 position)
    {
        return gridMap.isEmptyAtPosition(position);
    }

    public Vector3 GetGridCenterOfPosition(Vector3 position, int scale = 1)
    {
        return gridMap.GetGridCenterOfPosition(position, scale);
    }

    public bool FindPlantPosition(Vector3 playerPosition, Vector3 playerForwardDirection, int scale, out Vector3 plantPosition)
    {
        return gridMap.FindPlantPosition(playerPosition, playerForwardDirection, scale, out plantPosition);
    }

    public void AddGameObjectOfScale(Vector3 worldPosition, GameObject go, int scale)
    {
        gridMap.AddGameObjectOfScale(worldPosition, go, scale);
    }

    public void RemoveGameObjectOfScale(GameObject go, int scale)
    {
        gridMap.RemoveGameObjectOfScale(go, scale);
    }

    public bool FindTrapPosition(GameObject tree, int scale, out Vector3 trapPosition, out Vector3 poacherPosition, out Vector3 hidePosition)
    {
        return gridMap.FindTrapPosition(tree, scale, out trapPosition, out poacherPosition, out hidePosition);
    }

    public HashSet<GameObject> FindTreeNextToTrap(TrapController trap)
    {
        return gridMap.FindTreeNextToTrap(trap);
    }
}
