using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    Animator anim;
    PoacherAI pAI;
    public ParticleSystem dustEffect;
    
    List<GameObject> birdsInTrap;
    bool isTriggered;

    public PoacherAI PAI { get => pAI; set => pAI = value; }
    public List<GameObject> BirdsInTrap { get => birdsInTrap; set => birdsInTrap = value; }
    public bool IsTriggered { get => isTriggered; set => isTriggered = value; }

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        birdsInTrap = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (birdsInTrap.Count >= 4)
        {
            pAI.CanCapture = true;
        }
    }

    public void TriggerTrap()
    {
        isTriggered = true;
        anim.SetBool("isTriggered", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TargetAnimal"))
        {
            if (!birdsInTrap.Contains(other.gameObject))
            {
                birdsInTrap.Add(other.gameObject);
            }
        }
    }
}
