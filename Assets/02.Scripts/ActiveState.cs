using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveState : MonoBehaviour
{
    public bool isGrabbed = false; // �׷��Ǿ�����

    public void SetGrabbed(bool grabbed)
    {
        isGrabbed = grabbed;
    }
}
