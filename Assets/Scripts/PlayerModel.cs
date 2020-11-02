using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public Transform headBone;
    public Vector3 locationOffset;
    public Gradient gradient;

    float maxPlayer = 4f;

    public void AddFlower(GameObject flowerPrefab, int playerIndex)
    {
        GameObject flower = Instantiate(flowerPrefab);
        Vector3 scale = flower.transform.localScale;
        flower.transform.parent = headBone;
        flower.transform.localPosition = locationOffset;
        flower.transform.localScale = scale;
        
        float gColor = ((float)playerIndex)/maxPlayer;
        flower.GetComponent<MeshRenderer>().materials[2].SetColor("_BaseColor", gradient.Evaluate(gColor));
    }
}
