using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public Transform headBone;
    public Vector3 locationOffset;


    public void AddFlower(GameObject flowerPrefab)
    {
        GameObject flower = Instantiate(flowerPrefab);
        Vector3 scale = flower.transform.localScale;
        flower.transform.parent = headBone;
        flower.transform.localPosition = locationOffset;
        flower.transform.localScale = scale;
    }
}
