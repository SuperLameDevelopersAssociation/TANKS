﻿using UnityEngine;
using System.Collections;

public class TankSpawn : MonoBehaviour
{
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Spawn")
        {
            this.GetComponent<Health>().inSpawn = false;
            print(this.GetComponent<Health>().inSpawn + " : for we have left spawn");
        }
    }
}