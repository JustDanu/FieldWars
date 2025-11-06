using UnityEngine;
using Mirror;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float minZOOM;
    [SerializeField] private float maxZOOM;
    [SerializeField] private float speedZOOM;
    [SerializeField] private float lerpSpeedZOOM;
    [SerializeField] private float cameraFollowDistance;
    [SerializeField] private float rotateSpeed;

    private Transform target;

    private void LateUpdate()
    {
        if (target == null)
        {
            FindLocalPlayer();
            return;
        }

        // Rotation to match the player
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, target.up) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

        // Scrolling and following camera stuff
        float currentZOOM = ChangeZOOMByScroll(Camera.main.orthographicSize);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, currentZOOM, Time.deltaTime * lerpSpeedZOOM);

        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector2 screenPositionNormalized = new Vector2(mouseScreenPosition.x / Screen.width, mouseScreenPosition.y / Screen.height);
        // Its close but still off, have to decide if it will be by crosshair or by mouse position and or an implementation that kinda does both.
        Vector2 rotatedScreenPosition = targetRotation * screenPositionNormalized;

        // This makes it so when the mouse is on the middle the camera will be centered on the player (I am not completely sure but I somehow fixed the math using vector math with drawings)
        Vector2 offSetNormalized = new Vector2(rotatedScreenPosition.x + (0.5f * -(transform.up + transform.right).x), rotatedScreenPosition.y + (0.5f * -(transform.up + transform.right).y));
        // Scale it by zoom and camera distance and some transform magic
        Vector3 offset = offSetNormalized * (currentZOOM + cameraFollowDistance);
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x + offset.x, target.position.y + offset.y, -10f), cameraSpeed * Time.deltaTime);

        //transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x + (rotatedScreenPosition.x - (transform.right.x - 0.5f)) * (currentZOOM + cameraFollowDistance),
        //target.position.y + (rotatedScreenPosition.y - (transform.up.y - 0.5f)) * (currentZOOM + cameraFollowDistance), -10f), cameraSpeed * Time.deltaTime);

        
    }

    private float ChangeZOOMByScroll(float ZOOM)
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if(scrollInput != 0)
        {
            ZOOM += -scrollInput * speedZOOM;
            ZOOM = Mathf.Clamp(ZOOM, minZOOM, maxZOOM);
        }
        return ZOOM;
    }

    private void FindLocalPlayer()
    {
        
        if(NetworkClient.localPlayer != null)
        {
            target = NetworkClient.localPlayer.transform;
        }
        
    }
}
