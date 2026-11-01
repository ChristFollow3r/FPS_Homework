using UnityEngine;
using UnityEngine.InputSystem;


public class PickUp : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private float hoverDistance;
    [SerializeField] private float speed;
    [SerializeField] private float strength;
    
    private bool isHoldingItem;
    private RaycastHit hit;
    
    void Update()
    {
        DetectItem();
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
        }
    }

    void PickUpItem()
    {
        if (isHoldingItem)
        {
            //var current = hit.rigidbody.position;
            var target = playerCamera.transform.position + (playerCamera.transform.forward * hoverDistance);
            
            //Vector3 newPosition = Vector3.MoveTowards(current, target, speed * Time.fixedDeltaTime);
            Vector3 desiredSpeed = (target - hit.rigidbody.position) * strength;
            
            hit.rigidbody.useGravity = false;
            hit.rigidbody.linearVelocity = Vector3.ClampMagnitude(desiredSpeed, speed); // Also interpolate is set to interpolate on the item
        }

        if (!Mouse.current.rightButton.isPressed)
        {
            isHoldingItem = false;
            if (hit.rigidbody is not null) hit.rigidbody.useGravity = true;
        }
    }
}
