using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float displacement = 0.15f;
    private float zPos = -10;

    // Update is called once per frame
    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 cameraDisplacement = (mousePos - playerTransform.position) * displacement;
        
        Vector3 finalCamPos = playerTransform.position + cameraDisplacement;
        finalCamPos.z = zPos;
        transform.position = finalCamPos;
    }
}
