using UnityEngine;
using UnityEngine.InputSystem;

public class newCameraScript : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private Camera camera;
    
    [SerializeField] private float horizontalSens;
    [SerializeField] private float verticalSens;
    
    [SerializeField] private float topCap;
    [SerializeField] private float bottomCap;

    private float xRotation = 0f;
    
    void Update()
    {
        cameraRotation();   
    }
 
    void cameraRotation()
    {
        float x = Mouse.current.delta.value.x * horizontalSens;
        float y = Mouse.current.delta.value.y * verticalSens;
        
        pivot.Rotate(Vector3.up * x);

        xRotation -= y;
        xRotation = Mathf.Clamp(xRotation, -topCap, bottomCap);
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

    }
}
