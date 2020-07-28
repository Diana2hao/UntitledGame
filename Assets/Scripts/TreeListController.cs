using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeListController : MonoBehaviour
{
    public List<GameObject> treeList;

    // Start is called before the first frame update
    void Start()
    {
        treeList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(treeList.Count);
    }
}
