using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsControl : MonoBehaviour
{
    public Animator[] animators;
    
    public void DisplayAccordingToScheme(int inputScheme)
    {
        foreach(Animator anim in animators)
        {
            if (inputScheme == (int)MyInputScheme.KEYBOARDALL)
            {
                anim.SetBool("keyboardAll", true);
                anim.SetBool("keyboardLeft", false);
                anim.SetBool("keyboardRight", false);
            }
            else if (inputScheme == (int)MyInputScheme.KEYBOARDLEFT)
            {
                anim.SetBool("keyboardLeft", true);
            }
            else if (inputScheme == (int)MyInputScheme.KEYBOARDRIGHT)
            {
                anim.SetBool("keyboardRight", true);
            }
            else if (inputScheme == (int)MyInputScheme.GAMEPAD)
            {
                anim.SetBool("gamepad", true);
            }
        }
    }

    //public void SplitLeft()
    //{
    //    anim.SetBool("keyboardLeft", true);
    //    anim.SetBool("keyboardRight", false);
    //}

    //public void SplitRight()
    //{
    //    anim.SetBool("keyboardLeft", false);
    //    anim.SetBool("keyboardRight", true);
    //}

    //public void UnsplitKeyboard()
    //{
    //    anim.SetBool("keyboardLeft", false);
    //    anim.SetBool("keyboardRight", false);
    //}

    //public void UseGamepad()
    //{
    //    anim.SetBool("gamepad", true);
    //}
}
