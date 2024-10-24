using UnityEngine;

public class CameraMov : MonoBehaviour
{
    [Header("Camera Movement Settings")]
    [SerializeField] float _mouseSensitivity;
    [SerializeField] float _cameraCurrentX = 0;
    Vector2 mouseDelta;
    bool _isMouseLocked = false;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _isMouseLocked = false;
    }
    private void LateUpdate()
    {
        if (_isMouseLocked == false)
        {
            UpdateMouseLook();
        }
    }


    void UpdateMouseLook()
    {
        mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * _mouseSensitivity;

        _cameraCurrentX -= mouseDelta.y;

        _cameraCurrentX = Mathf.Clamp(_cameraCurrentX, -90, 90);

        Camera.main.transform.localEulerAngles = Vector3.right * _cameraCurrentX;
        transform.Rotate(Vector3.up * mouseDelta.x);



    }
}
