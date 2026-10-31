using UnityEngine;
using UnityEngine.InputSystem;


public class PickUp : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private float hoverDistance;
    [SerializeField] private float speed;
    
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
            var current = hit.rigidbody.transform.position;
            var target = playerCamera.transform.position + (playerCamera.transform.forward * hoverDistance);
            
            Vector3 newPosition = Vector3.MoveTowards(current, target, speed * Time.fixedDeltaTime);
            
            hit.rigidbody.useGravity = false;
            hit.rigidbody.MovePosition(newPosition);
        }

        if (!Mouse.current.rightButton.isPressed)
        {
            isHoldingItem = false;
            if (hit.rigidbody is not null) hit.rigidbody.useGravity = true;
        }
    }
}
