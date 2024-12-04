using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDestination : MonoBehaviour
{
    public void DisableSelf()
    {
        transform.gameObject.SetActive(false);
    }
}
