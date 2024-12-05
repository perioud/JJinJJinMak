using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveState : MonoBehaviour
{
    public bool isGrabbed = false; // 그랩되었는지

    public void SetGrabbed(bool grabbed)
    {
        isGrabbed = grabbed;
    }
}
