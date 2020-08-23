using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridType
{
    EMPTY,
    OCCUPIED,
    BLOCKED
}

public class Grid
{
    private int type;
    private GameObject go;

    public Grid(int type)
    {
        this.type = type;
    }

    public void SetGridType(int newType)
    {
        this.type = newType;
    }

    public bool AddGameObject(GameObject go)
    {
        if(this.type == (int)GridType.EMPTY)
        {
            this.go = go;
            this.type = (int)GridType.OCCUPIED;
            return true;
        }

        return false;
    }

    public void RemoveGameObject()
    {
        this.type = (int)GridType.EMPTY;
    }

    public bool isEmpty()
    {
        if(this.type == (int)GridType.EMPTY)
        {
            return true;
        }
        return false;
    }

    public bool isOccupied()
    {
        if (this.type == (int)GridType.OCCUPIED)
        {
            return true;
        }
        return false;
    }

    public int GetGridType()
    {
        return this.type;
    }

    public GameObject GetGridObject()
    {
        if(this.type == (int)GridType.OCCUPIED)
        {
            return this.go;
        }

        return null;
    }
}
