using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopSplatter : MonoBehaviour
{
    public Texture[] poopTexArray;
    public Renderer rd;
    Material poopMat;
    int poopCount = 0;
    int maxPoop;
    int poopHit = 0;

    bool isVacuuming = false;
    Color transDiff = new Color(0f, 0f, 0f, 0.05f);

    BirdAI bAI;
    VacuumController vc;
    bool hasVacuum;
    bool vacuumStarted = false;

    public BirdAI BAI { get => bAI; set => bAI = value; }
    public int PoopCount { get => poopCount; set => poopCount = value; }

    // Start is called before the first frame update
    void Start()
    {
        //rd = GetComponent<Renderer>();
        poopMat = rd.materials[0];
        maxPoop = poopTexArray.Length * 100;
    }

    // Update is called once per frame
    void Update()
    {
        //if (hasVacuum && isVacuuming && !vacuumStarted)
        //{
        //    //isVacuuming = !isVacuuming;
        //    StartCoroutine("Vacuum");
        //}
    }

    public void StartVacuum()
    {
        isVacuuming = true;
        if (!vacuumStarted)
        {
            StartCoroutine("Vacuum");
        }

    }

    public void StopVacuum()
    {
        isVacuuming = false;
    }

    public void AddPoop()
    {
        if(poopCount < maxPoop)
        {
            poopCount += 100;

            if (poopCount < 150f)
            {
                poopMat.SetTexture("_BaseMap", poopTexArray[0]);
            }
            else if (poopCount >= 150f && poopCount < 250f)
            {
                poopMat.SetTexture("_BaseMap", poopTexArray[1]);
            }
            else if (poopCount >= 250f)
            {
                poopMat.SetTexture("_BaseMap", poopTexArray[2]);
                poopCount = maxPoop;
                bAI.CanPoop = false;
            }

            poopMat.SetColor("_BaseColor", new Color(1f, 1f, 1f, 1f));
        }
    }

    IEnumerator Vacuum()
    {
        vacuumStarted = true;

        while (isVacuuming)
        {
            //transparent material one by one
            float trans = poopMat.GetColor("_BaseColor").a;
            if(poopCount <= 100)
            {
                if(trans > 0)
                {
                    poopMat.SetColor("_BaseColor", poopMat.GetColor("_BaseColor") - transDiff*2f);
                    poopCount -= 10;
                }
                else
                {
                    isVacuuming = false;
                    poopCount = 0;
                    bAI.CanPoop = true;
                    //vacuum stop filling
                    vc.IsOnPoop = false;
                }
            }
            else
            {
                poopMat.SetColor("_BaseColor", poopMat.GetColor("_BaseColor") - transDiff / 2f);
                poopCount -= 5;
                if (poopCount % 100 == 0)
                {
                    Debug.Log(poopCount);
                    poopMat.SetTexture("_BaseMap", poopTexArray[poopCount / 100 - 1]);
                    poopMat.SetColor("_BaseColor", new Color(1f, 1f, 1f, 1f));
                }
            }

            yield return new WaitForSeconds(0.1f);
        }

        vacuumStarted = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        poopHit++;

        if (poopHit == 1)
        {
            AddPoop();
        }

        if (poopHit == 3)
        {
            poopHit = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Vacuum"))
        {
            vc = other.transform.parent.GetComponent<VacuumController>();
            hasVacuum = true;
        }
    }

}
