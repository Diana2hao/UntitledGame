using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public Texture[] eyeTexArray;
    Renderer rd;
    Material eyeMat;
    float timer = 0f;
    float blinkTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Renderer>();
        eyeMat = rd.materials[1];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > blinkTime)
        {
            timer = timer - blinkTime;

            StartCoroutine("blink");
        }
    }

    IEnumerator blink()
    {
        for(int idx = 0; idx <= 4; idx += 1)
        {
            eyeMat.SetTexture("_Texture2D", eyeTexArray[idx]);
            yield return new WaitForSeconds(.05f);
        }
    }
}
