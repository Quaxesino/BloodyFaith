using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;

    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float distance = 5;

    [SerializeField] float minVerticalAngle = -45;
    [SerializeField] float maxVerticalAngle = -45;

    [SerializeField] Vector2 framingOffset;

    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    float rotationX;
    float rotationY;

    float InvertXVal;
    float InvertYVal;

    private void Start()
    {
       //Cursor.visible = false;
       //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        InvertXVal = (invertX) ? -1 : 1;
        InvertYVal = (invertY) ? -1 : 1;

        rotationX += Input.GetAxis("Camera Y") * InvertYVal * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
        rotationY += Input.GetAxis("Camera X") * InvertXVal * rotationSpeed;

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y); 

        transform.position = focusPosition - targetRotation  * new Vector3(0, 0, distance);
        transform.rotation = targetRotation;
    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);

}
