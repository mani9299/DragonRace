using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Dragonstats : MonoBehaviour
{
    public int Speed;
    void Start()
    {
        GetComponentInParent<SplineFollower>().followSpeed = Speed;
    }

    
}
