using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VacuumHead : MonoBehaviour
{
    public VacuumController vacuumControl;
    public BoxCollider box;

    List<PoopSplatter> poopSplatters;
    Vector3 headPosition;

    PoopSplatter lastPoop;
    PoopSplatter currPoop;

    // Start is called before the first frame update
    void Start()
    {
        poopSplatters = new List<PoopSplatter>();
    }

    // Update is called once per frame
    void Update()
    {
        //finds the closest poopsplatter
        if(poopSplatters.Count != 0)
        {
            headPosition = transform.TransformPoint(box.center);
            List<float> distances = new List<float>();
            foreach (PoopSplatter poop in poopSplatters)
            {
                float distance = Vector3.Distance(poop.transform.position, headPosition);
                distances.Add(distance);
            }
            int index = distances.IndexOf(distances.Min());
            lastPoop = currPoop;
            currPoop = poopSplatters[index];
            if(lastPoop != currPoop)
            {
                vacuumControl.IsOnPoop = (currPoop.PoopCount > 0);
                vacuumControl.SwitchPoop(currPoop);
            }
        }
        else
        {
            lastPoop = currPoop;
            currPoop = null;
            vacuumControl.IsOnPoop = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Poop"))
        {
            poopSplatters.Add(other.GetComponent<PoopSplatter>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Poop"))
        {
            poopSplatters.Remove(other.GetComponent<PoopSplatter>());
        }
    }
}
