using UnityEngine;

public class playerRotation : MonoBehaviour
{
    
    private Transform mouseTransform;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        mouseTransform = this.transform;
    }

    private void LookAtMouse()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        mouseTransform.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
    }
}