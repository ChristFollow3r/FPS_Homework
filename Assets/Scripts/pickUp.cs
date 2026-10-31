using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
        [SerializeField] private Camera playerCamera;
        [SerializeField] private LayerMask pickupLayer;
        [SerializeField] private float pickupRange = 2f;
        [SerializeField] private float holdDistance = 1.5f;
        [SerializeField] private float moveSpeed = 15f;

        private Rigidbody heldItem;

        
}