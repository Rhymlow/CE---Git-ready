using UnityEngine;

public class PlayerMovement : GameSystem
{
    [Header("Item Equipped")]
    public string itemEquipped;

    
    [Header("Settings")]
    public int animationSelector;
    bool animationSelectorLock;
    Animator anim;
    GameObject thisGO;
    public bool playerMovementActivated = false;
    public int haveSomethingEquiped = 0;
    public GameObject temporaryRecoverThing;
    public string temporaryThingInfo;
    public bool playerColliderActivated = true;
    private Vector2 initialTouchPosition;
    bool isJoystickActive;
    public Transform cameraRotation;
    float frontConstant;
    public float moveSpeed = 3.0f;

    private Vector3 playerVelocity;
    private CharacterController controller;
    public float speed = 5.0f; // Velocidad de movimiento del jugador
    bool isJoysticActive;

    void Start()
    {
        itemEquipped = "null";
        thisGO = transform.gameObject;
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = this.transform.GetComponent<CharacterController>();
    }

    void Update()
    {
        CustomGravity();
        PlayerMovementFunction();
        frontConstant = cameraRotation.rotation.eulerAngles.y;
    }

    void PlayerMovementFunction()
    {
        if (playerMovementActivated)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            { }
            if (Input.touchCount == 0)
            {
                float forwardSpeed = ConvertTouchToAxis(0f, 0f) * speed;
                float sideSpeed = ConvertTouchToAxis(0f, 0f) * speed;
                Vector3 speedVector = new Vector3(sideSpeed, 0, forwardSpeed);
                speedVector = cameraRotation.rotation * speedVector;
                speedVector.y = playerVelocity.y;
                controller.Move(speedVector * Time.deltaTime);
            }
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.position.x < Screen.width / 2)
                {
                    float forwardSpeed = ConvertTouchToAxis(touch.position.y, initialTouchPosition.y) * speed;
                    float sideSpeed = ConvertTouchToAxis(touch.position.x, initialTouchPosition.x) * speed;
                    Vector3 speedVector = new Vector3(sideSpeed, 0, forwardSpeed);
                    speedVector = cameraRotation.rotation * speedVector;
                    speedVector.y = playerVelocity.y;
                    controller.Move(speedVector * Time.deltaTime);
                    ChangeAnimSelector(1);
                    animationSelectorLock = false;
                    thisGO.transform.eulerAngles = new Vector3(0.0f, 90.0f + frontConstant, 0.0f);
                    if (touch.phase == TouchPhase.Began && isJoysticActive == false)
                    {
                        isJoysticActive = true;
                        initialTouchPosition = touch.position;
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        isJoysticActive = false;
                    }
                }
                else
                {
                    float forwardSpeed = ConvertTouchToAxis(0f, 0f) * speed;
                    float sideSpeed = ConvertTouchToAxis(0f, 0f) * speed;
                    Vector3 speedVector = new Vector3(sideSpeed, 0, forwardSpeed);
                    speedVector = cameraRotation.rotation * speedVector;
                    speedVector.y = playerVelocity.y;
                    controller.Move(speedVector * Time.deltaTime);
                }
            }
        }
        #region Animations
        if (animationSelectorLock == false && playerMovementActivated == true)
        {
            animationSelectorLock = true;
        }
        else if (haveSomethingEquiped == 0 && playerMovementActivated == true)
        {
            ChangeAnimSelector(0);
        }
        else if (haveSomethingEquiped == 1 && playerMovementActivated == true)
        {
            ChangeAnimSelector(3);
        }
        #endregion
    }

    public float gravityAcceleration = 20f;
    public float maxGravitySpeed = 10f;
    [SerializeField] [Unity.Collections.ReadOnly] private float playerGravitySpeed = 0f;

    public void UpdatePlayerGravitySpeed(float tPlayerGravitySpeed)
    {
        playerGravitySpeed = tPlayerGravitySpeed;
    }

    void CustomGravity()
    {
        playerGravitySpeed -= gravityAcceleration * Time.deltaTime;
        playerGravitySpeed = Mathf.Max(playerGravitySpeed, -maxGravitySpeed);
        controller.Move(new Vector3(0, playerGravitySpeed, 0) * Time.deltaTime);

        if (controller.isGrounded)
        {
            playerGravitySpeed = 0f;
        }
    }


    public void ChangeAnimSelector(int animationPlayerSelector)
    {
        anim.SetInteger("AnimationSelector", animationPlayerSelector);
    }


    Vector2 GetJoysticInput()
    {
        Vector2 outputInput = new Vector2(0, 0);
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.position.x < Screen.width / 2)
            {
                outputInput.y = ConvertTouchToAxis(touch.position.y, initialTouchPosition.y);
                outputInput.x = ConvertTouchToAxis(touch.position.x, initialTouchPosition.x);
            }
            if (touch.phase == TouchPhase.Began && isJoystickActive == false)
            {
                isJoystickActive = true;
                initialTouchPosition = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                isJoystickActive = false;
            }
        }
        return outputInput;
    }

    float ConvertTouchToAxis(float tTouch, float initialPosition)
    {
        float realTouch = tTouch - initialPosition;
        if (realTouch >= 100)
        {
            return 1f;
        }
        else if (realTouch <= -100)
        {
            return -1f;
        }
        else
        {
            return realTouch / 100;
        }
    }
}