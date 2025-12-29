using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 cameraPosition = new Vector3(0.1f, 0.7f, -0.18f);
    public Vector3 cameraRotation = new Vector3(22f, -90f, 0f);
    float mouseSensitivity = 2f;
    private float rotationX = 0f;
    private float rotationY = 0f;

    public Transform cameraTransform;
    public float cameraSpeed = 10f;
    LayerMask mask;
    LayerMask mask2;
    public Vector3 adjustRaycast;

    Camera camera1;
    public Ray ray;
    Ray rayo;
    int layerMask = (1 << 5) | (1 << 3);
    bool isTouchingCameraMovement = false;
    bool isTouchingUI = false;


    private void Start()
    {
        GameSystem.cameraOrbit.transform.localRotation = Quaternion.Euler(cameraRotation);
        rotationX = GameSystem.cameraOrbit.transform.localRotation.eulerAngles.y;
        rotationY = GameSystem.cameraOrbit.transform.localRotation.eulerAngles.x;
        cameraTransform = this.transform;
        int excludedLayerUI = LayerMask.NameToLayer("UI");
        int excludedLayerPlayer = LayerMask.NameToLayer("Player");
        int excludedFloor = LayerMask.NameToLayer("Floor");
        mask = ~(1 << excludedLayerUI | 1 << excludedLayerPlayer);
        mask2 = ~(1 << excludedLayerUI | 1 << excludedLayerUI | 1 << excludedFloor);
        camera1 = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        OrbitCamera();
        FollowPlayer();
    }

    void LateUpdate()
    {
        
    }

    void OrbitCamera()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationX += mouseX;
            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);
            GameSystem.cameraOrbit.transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
        }
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.position.x > Screen.width / 2)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    rayo = camera1.ScreenPointToRay(touch.position);
                    RaycastHit hito;
                    if (Physics.Raycast(rayo, out hito, Mathf.Infinity, layerMask))
                    {
                        isTouchingUI = true;
                    }
                    else
                    {
                        isTouchingCameraMovement = true;
                    }
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isTouchingCameraMovement = false;
                    isTouchingUI = false;
                    GameSystem.ExecuteInputBuffer();
                    GameSystem.CleanInputBuffer();
                }
                if (isTouchingCameraMovement)
                {
                    float mouseX = touch.deltaPosition.x * (mouseSensitivity / 6);
                    float mouseY = touch.deltaPosition.y * (mouseSensitivity / 6);
                    rotationX += mouseX;
                    rotationY -= mouseY;
                    rotationY = Mathf.Clamp(rotationY, -90f, 90f);
                    GameSystem.cameraOrbit.transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
                }
                else if (isTouchingUI)
                {
                    ray = camera1.ScreenPointToRay(touch.position);
                    Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                    {
                        if (hit.transform.gameObject.GetComponent<SmothRotation>())
                        {
                            GameSystem.AddInputToBuffer(hit.transform.gameObject.GetComponent<SmothRotation>().buttonID, hit.transform.gameObject);
                        }
                    }
                }
            }
        }
    }

    void FollowPlayer()
    {
        Vector3 desiredPosition = GameSystem.player.transform.position + cameraPosition;
        GameSystem.cameraOrbit.transform.position = Vector3.Lerp(GameSystem.cameraOrbit.transform.position, desiredPosition, 1.0f);
    }

    #region RECORRER CAMARA DEPENDIENDO SI HAY UN OBJETO ENTRE LA CAMARA Y EL JUGADOR.
    /*void AdjustCameraPosition()
    {
        // Calcular la dirección de la cámara desde el jugador
        Vector3 directionToPlayer = (cameraTransform.position - player.position).normalized;

        

        // Lanza un raycast desde la posición de la cámara hacia el jugador
        RaycastHit hit;
        if (Physics.Raycast(player.position + adjustRaycast, directionToPlayer, out hit, distanceToPlayer + 0f, mask))
        {
            // Si el raycast detecta un obstáculo, retraemos la cámara hasta el punto de colisión
            Debug.Log(hit.transform.gameObject.name);
            currentDistance = Mathf.Max(hit.distance + 0f, minDistance); // Retroceder un poco para evitar el contacto

            // Si no detecta nada, la distancia no cambia, manteniendo la cámara a la distancia actual
            // No restablecemos a la distancia inicial aquí

            // Mover la cámara hacia la nueva posición, manteniendo la distancia ajustada
            Vector3 desiredPosition = player.position + directionToPlayer * currentDistance;

            // Mover la cámara suavemente con una interpolación
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSpeed * Time.deltaTime);
        }
        

        // Dibujar el raycast visible en el Editor de Unity
        Debug.DrawRay(player.position, directionToPlayer * currentDistance, Color.yellow);
    }

    // Esta función será llamada cuando el Collider del jugador esté en contacto con el trigger de la cámara
    void OnTriggerStay(Collider other)
    {
        // Calcular la dirección de la cámara desde el jugador
        Vector3 directionToPlayer = (cameraTransform.position - player.position).normalized;

        // Verificar si el objeto que está en el área del trigger no es el jugador ni la UI
        if ((mask2 & (1 << other.gameObject.layer)) != 0 && other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            // Si el objeto es un obstáculo, la cámara se retrae
            currentDistance = Mathf.Max(Vector3.Distance(cameraTransform.position, player.position) + 0f, minDistance);
        }

        // Mover la cámara hacia la nueva posición, manteniendo la distancia ajustada
        Vector3 desiredPosition = player.position + directionToPlayer * currentDistance;

        // Mover la cámara suavemente con una interpolación
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSpeed * Time.deltaTime);
    }*/
    #endregion
}
