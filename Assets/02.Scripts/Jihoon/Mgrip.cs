using System.Collections.Generic;
using UnityEngine;

public class Mgrip : MonoBehaviour
{
    public InputManager xrinput;
    public Transform player;
    public Transform grabAnchor;
    private Transform handTransform;
    private Rigidbody attachedObj;
    private List<Rigidbody> contactRigidbodies;
    private ActiveState attachedObjActiveState;
    private static Mgrip currentGrabbingHand;

    [Header("Physics Settings")]
    public string grabbedObjectLayer = "GrabbedObject";
    private string originalLayer;

    void Start()
    {
        handTransform = GetComponent<Transform>();
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
        if (attachedObj != null) return;

        Rigidbody nearestObj = GetNearestRigidbody();
        if (nearestObj == null) return;

        if (currentGrabbingHand != null && currentGrabbingHand != this)
        {
            currentGrabbingHand.ForceDrop();
        }

        attachedObj = nearestObj;
        currentGrabbingHand = this;

        // 원래 Layer 저장 및 변경
        originalLayer = LayerMask.LayerToName(attachedObj.gameObject.layer);
        attachedObj.gameObject.layer = LayerMask.NameToLayer(grabbedObjectLayer);

        attachedObj.useGravity = false;
        attachedObj.isKinematic = true;

        attachedObj.transform.SetParent(handTransform);
        attachedObj.transform.position = grabAnchor.position;
        attachedObj.transform.rotation = grabAnchor.rotation;

        // 활성 상태 관리
        attachedObjActiveState = attachedObj.GetComponent<ActiveState>();
        if (attachedObjActiveState != null)
        {
            attachedObjActiveState.SetGrabbed(true);
        }
    }

    public void ObjectDrop()
    {
        if (attachedObj == null || currentGrabbingHand != this) return;

        ForceDrop();
    }

    public void ForceDrop()
    {
        if (attachedObj == null) return;

        attachedObj.transform.SetParent(null);
        attachedObj.useGravity = true;
        attachedObj.isKinematic = false;

        // Layer 복구
        attachedObj.gameObject.layer = LayerMask.NameToLayer(originalLayer);

        // 물리 속도 설정
        attachedObj.velocity += player.rotation * xrinput.VelocityInput();
        attachedObj.angularVelocity = player.rotation * xrinput.AngularVelocityInput();

        // 활성 상태 초기화
        if (attachedObjActiveState != null)
        {
            attachedObjActiveState.SetGrabbed(false);
        }

        attachedObj = null;
        attachedObjActiveState = null;
        currentGrabbingHand = null;
    }

    private Rigidbody GetNearestRigidbody()
    {
        Rigidbody nearestRigidbody = null;
        float minDistance = float.MaxValue;

        contactRigidbodies.RemoveAll(item => item == null || item.Equals(null));

        foreach (Rigidbody rb in contactRigidbodies)
        {
            float distance = Vector3.Distance(rb.transform.position, handTransform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestRigidbody = rb;
            }
        }

        return nearestRigidbody;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractionObject") || other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null && !contactRigidbodies.Contains(rb))
            {
                contactRigidbodies.Add(rb);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractionObject") || other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                contactRigidbodies.Remove(rb);
            }
        }
    }
}
