using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyType
{
    FARMER,
    POACHER
}

public class RaftController : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public GameObject PoacherPrefab;
    public GameObject FarmerPrefab;
    public int maxEnemyPerRaft = 2;

    int poacherReady = 0;
    int farmerReady = 0;

    GameObject[] enemyPositionHolders;
    Queue<int> enemiesToBeDeployed;

    int currentReadyEnemyCount = 0;
    bool isSailing;

    public bool IsSailing { get => isSailing; set => isSailing = value; }

    // Start is called before the first frame update
    void Start()
    {
        enemyPositionHolders = new GameObject[maxEnemyPerRaft];
        for(int i = 0; i < maxEnemyPerRaft; i++)
        {
            enemyPositionHolders[i] = transform.GetChild(i).gameObject;
        }

        enemiesToBeDeployed = new Queue<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (poacherReady > 0)
        {
            //add poacher to queue
            enemiesToBeDeployed.Enqueue((int)EnemyType.POACHER);
            poacherReady--;
        }

        if (farmerReady > 0)
        {
            //add farmer to queue
            enemiesToBeDeployed.Enqueue((int)EnemyType.FARMER);
            farmerReady--;
        }

        if (!isSailing && enemiesToBeDeployed.Count > 0)
        {
            for (int i = 0; i < maxEnemyPerRaft; i++)
            {
                if (enemiesToBeDeployed.Count > 0)
                {
                    GameObject enemyPrefab = enemiesToBeDeployed.Dequeue() == (int)EnemyType.POACHER
                                         ? PoacherPrefab : FarmerPrefab;
                    Instantiate(enemyPrefab, enemyPositionHolders[i].transform);
                }
            }
            isSailing = true;
            this.GetComponent<Animator>().SetBool("isSailing", isSailing);
        }
    }

    public void AddPoacher()
    {
        poacherReady++;
    }

    public void AddFarmer()
    {
        farmerReady++;
    }
}
