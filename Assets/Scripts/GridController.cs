using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public FromTO[] BlockedAreas;

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
        GridMap gridMap = new GridMap(41,20);

        foreach(FromTO ft in BlockedAreas)
        {
            gridMap.BlockOutGrids(ft.from, ft.to);
        }

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
}
