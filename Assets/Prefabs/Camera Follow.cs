using UnityEngine;
using Mirror;

public class CameraFollow : MonoBehaviour
{
    
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float minZOOM;
    [SerializeField] private float maxZOOM;
    [SerializeField] private float speedZOOM;
    [SerializeField] private float lerpSpeedZOOM;
    [SerializeField] private float cameraFollowDistance;

    private Transform target;

    private void LateUpdate()
    {
        if(target == null)
        {
            FindLocalPlayer();
            return;
        }
        float currentZOOM = ChangeZOOMByScroll(Camera.main.orthographicSize);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, currentZOOM, Time.deltaTime * lerpSpeedZOOM);

        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector2 screenPositionNormalized = new Vector2(mouseScreenPosition.x / Screen.width, mouseScreenPosition.y / Screen.height);
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x + (screenPositionNormalized.x - 0.5f) * (currentZOOM + cameraFollowDistance),
        target.position.y + (screenPositionNormalized.y - 0.5f) * (currentZOOM + cameraFollowDistance), -10f), cameraSpeed * Time.deltaTime);

    }

    private float ChangeZOOMByScroll(float ZOOM)
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if(scrollInput != 0)
        {
            ZOOM += scrollInput * speedZOOM;
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
