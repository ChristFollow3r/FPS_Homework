using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;


public class PickUp : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask itemLayer;
    
    [SerializeField] private float hoverDistance;
    [SerializeField] private float speed;
    [SerializeField] private float strength;
    [SerializeField] private float wheelSpeed;
    [SerializeField] private float wheelMinDistance;
    [SerializeField] private float wheelMaxDistance;
    
    private bool isHoldingItem;
    private float currentDistance;
    private RaycastHit hit;
    
    void Update()
    {
        DetectItem();
        MoveItemWithMouseWheel();
    }

    private void FixedUpdate()
    {
        PickUpItem();
    }

    void DetectItem()
    {
        if (!isHoldingItem)
        {
            bool itemIsInFront = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 2f, itemLayer) && hit.rigidbody is not null;
            
            isHoldingItem = itemIsInFront && Mouse.current.rightButton.isPressed;
            currentDistance = Mathf.Clamp(hoverDistance, wheelMinDistance, wheelMaxDistance);
        }
    }

    void PickUpItem()
    {
        if (isHoldingItem)
        {
            var target = playerCamera.transform.position + (playerCamera.transform.forward * currentDistance);
            Vector3 desiredSpeed = (target - hit.rigidbody.position) * strength;
            
            hit.rigidbody.useGravity = false;
            hit.rigidbody.linearVelocity = Vector3.ClampMagnitude(desiredSpeed, speed); 
            
        }

        if (!Mouse.current.rightButton.isPressed)
        {
            isHoldingItem = false;
            if (hit.rigidbody != null) hit.rigidbody.useGravity = true;
        }
    }

    void MoveItemWithMouseWheel()
    {
        float scrollValue = Mouse.current.scroll.value.y;
        
        if (!isHoldingItem) return;

        if (Mathf.Abs(scrollValue) > 0.1f)
        {
            currentDistance = Mathf.Clamp(currentDistance + Mathf.Sign(scrollValue) * wheelSpeed, wheelMinDistance, wheelMaxDistance);
        }
    }
}
