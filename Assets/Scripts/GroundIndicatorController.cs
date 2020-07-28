using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundIndicatorController : MonoBehaviour
{
    public GameObject mosaic;
    public List<Vector2Int> corners;
    public int size;

    HashSet<GameObject> players;
    Dictionary<int, List<Vector3>> Positions;
    GameObject ms;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMosaic();
        ms.SetActive(false);

        Positions = new Dictionary<int, List<Vector3>>();
        players = new HashSet<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMosaic()
    {
        ms = new GameObject("MosaicsOfSize" + size);
        for (int i = 0; i < corners.Count; i += 2)
        {
            Vector2Int left = corners[i];
            Vector2Int right = corners[i + 1];

            for (int x = left.x; x <= right.x - size; x += size)
            {
                for (int y = left.y; y <= right.y - size; y += size)
                {
                    Vector3 position = new Vector3(x, 0.001f, y);
                    GameObject m = Instantiate(mosaic, position, Quaternion.Euler(0, 180, 0));
                    m.transform.parent = ms.transform;
                }
            }
        }
    }

    public void ShowMosaic(bool show, GameObject player)
    {
        if (show)
        {
            players.Add(player);
            ms.SetActive(show);
        }
        else
        {
            players.Remove(player);
            if (players.Count == 0)
            {
                ms.SetActive(show);
            }
        }
    }

    public Vector3 GetPlantPosition(Vector3 playerPosition, int finalSize)
    {
        if (!Positions.ContainsKey(finalSize))
        {
            List<Vector3> ps = new List<Vector3>();

            decimal d = (decimal)finalSize / 2;
            int half = (int)Math.Ceiling(d);

            for (int i = 0; i < corners.Count; i += 2)
            {
                Vector2Int left = corners[i];
                Vector2Int right = corners[i + 1];

                for (int x = left.x + half; x <= right.x - half; x += half)
                {
                    for (int y = left.y + half; y <= right.y - half; y += half)
                    {
                        Vector3 p = new Vector3(x, 0.001f, y);
                        ps.Add(p);
                    }
                }
            }

            Positions.Add(finalSize, ps);
        }

        Vector3 intendedPosition = getClosestPosition(playerPosition, Positions[finalSize]);

        return intendedPosition;
    }

    private Vector3 getClosestPosition(Vector3 playerP, List<Vector3> ps)
    {
        Vector3 pr = Vector3.zero;
        float minDist = -1.0f;
        foreach (Vector3 p in ps)
        {
            float dist = FlatDistance(playerP, p);
            if (minDist < 0 || dist < minDist)
            {
                minDist = dist;
                pr = p;
            }
        }

        return pr;
    }

    private float FlatDistance(Vector3 p1, Vector3 p2)
    {
        Vector3 p12 = p1 - p2;
        p12.y = 0;
        float dist = p12.magnitude;
        return dist;
    }
}
