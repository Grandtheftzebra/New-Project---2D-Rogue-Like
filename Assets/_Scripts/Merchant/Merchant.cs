using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour, IInteractable
{
   public virtual void Interact()
    {
        // Handle Shop later
        Debug.Log("Hey there! Open Shop!");
    }
}
