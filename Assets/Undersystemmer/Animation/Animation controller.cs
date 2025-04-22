using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Anim : MonoBehaviour
{
    public enum Animtype
    {
        Idle,
        Roam,
        Chase, 
        Attack,
        Die
    }

    public void setAnimation(Animtype State)
    {
        switch (State)
        {
            case Animtype.Idle:
                
        }
    }

}

