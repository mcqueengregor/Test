using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDestination : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Working!");
    }
}
