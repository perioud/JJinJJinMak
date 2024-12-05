using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public InputManager xrinput;
    public Transform player;
    private Transform handTransform;
    private Rigidbody handRigidbody;
    private Rigidbody attachedObj;
    private List<Rigidbody> contactRigidbodies;
    private ActiveState attachedObjActiveState; // 그랩 된 오브젝트의 활성화 상태 변경을 위해 참조
    private static GrabObject currentGrabbingHand; // 현재 그랩 중인 손을 추적하는 변수

    void Start()
    {
        handTransform = GetComponent<Transform>();
        handRigidbody = GetComponent<Rigidbody>();
        contactRigidbodies = new List<Rigidbody>();
    }

    void Update()
    {
        if (xrinput.IsGripPressed())
        {
            ObjectPickUp();
        }
        else
        {
            ObjectDrop();
        }
    }

    public void ObjectPickUp()
    {
        if (attachedObj != null)
        {
            return;  // 이미 오브젝트를 잡고 있다면, 더 이상 그랩 x
        }

        Rigidbody nearestObj = GetNearestRigidbody();

        if (nearestObj == null)
        {
            return;  // 근처에 그랩 가능 오브젝트가 없으면
        }

        // 다른 손이 이미 해당 오브젝트를 잡고 있다면 그 손의 그랩을 해제
        if (currentGrabbingHand != null && currentGrabbingHand != this)
        {
            currentGrabbingHand.ForceDrop();  // 이전 손의 그랩 강제로 해제
        }


        attachedObj = nearestObj;
        currentGrabbingHand = this;  // 현재 손을 그랩 중인 손으로 설정

        attachedObj.useGravity = false;
        attachedObj.isKinematic = true;
        attachedObj.transform.SetParent(handTransform);
        attachedObj.transform.position = handTransform.position;
        attachedObj.transform.rotation = handTransform.rotation;

        // Attached object의 활성화 상태 변경
        attachedObjActiveState = attachedObj.GetComponent<ActiveState>();
        if (attachedObjActiveState != null)
        {
            attachedObjActiveState.SetGrabbed(true);
        }
    }

    public void ObjectDrop()
    {
        if (attachedObj == null || currentGrabbingHand != this)
        {
            return;  // 오브젝트를 잡고 있지 않거나, 다른 손에서 잡고 있을 경우 해제하지 않음
        }

        ForceDrop();  // 드랍 분리
    }

    public void ForceDrop()
    {
        // 오브젝트 놓기
        if (attachedObj == null)
        {
            return;
        }

        attachedObj.transform.SetParent(null);
        attachedObj.useGravity = true;
        attachedObj.isKinematic = false;

        attachedObj.velocity += player.rotation * xrinput.VelocityInput();
        attachedObj.angularVelocity = player.rotation * xrinput.AngularVelocityInput();

        if (attachedObjActiveState != null)
        {
            attachedObjActiveState.SetGrabbed(false);
        }

        attachedObj = null;
        attachedObjActiveState = null;
        currentGrabbingHand = null;  // 현재 그랩 상태 초기화
    }

    private Rigidbody GetNearestRigidbody()
    {
        Rigidbody nearestRigidbody = null;
        float minDistance = float.MaxValue;

        foreach (Rigidbody rigidbody in contactRigidbodies)
        {
            float distance = Vector3.Distance(rigidbody.transform.position, handTransform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestRigidbody = rigidbody;
            }
        }

        return nearestRigidbody;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractionObject") || other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            contactRigidbodies.Add(other.gameObject.GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractionObject") || other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            contactRigidbodies.Remove(other.gameObject.GetComponent<Rigidbody>());
        }
    }
}
