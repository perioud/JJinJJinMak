using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OutlineHand"))
        {
            outline.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OutlineHand"))
        {
            outline.enabled = false;
        }
    }
}
