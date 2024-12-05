using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockOutline : MonoBehaviour
{
    private Outline outline;
    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
