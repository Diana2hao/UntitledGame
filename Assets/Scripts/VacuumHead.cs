using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VacuumHead : MonoBehaviour
{
    public VacuumController vacuumControl;
    public BoxCollider box;

    List<GameObject> poopSplatters;
    Vector3 headPosition;

    GameObject lastPoop;
    GameObject currPoop;

    // Start is called before the first frame update
    void Start()
    {
        poopSplatters = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //finds the closest poopsplatter
        if(poopSplatters.Count != 0)
        {
            headPosition = transform.TransformPoint(box.center);
            List<float> distances = new List<float>();
            foreach (GameObject poop in poopSplatters)
            {
                float distance = Vector3.Distance(poop.transform.position, headPosition);
                distances.Add(distance);
            }
            int index = distances.IndexOf(distances.Min());
            lastPoop = currPoop;
            currPoop = poopSplatters[index];
            if(lastPoop != currPoop || !vacuumControl.IsOnPoop)
            {
                vacuumControl.IsOnPoop = (currPoop.GetComponent<PoopSplatter>().PoopCount > 0);
                vacuumControl.SwitchPoop(currPoop);
            }
        }
        else
        {
            vacuumControl.IsOnPoop = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Poop"))
        {
            poopSplatters.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Poop"))
        {
            poopSplatters.Remove(other.gameObject);
        }
    }
}
