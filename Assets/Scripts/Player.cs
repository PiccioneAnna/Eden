using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player player;

    [SerializeField] MarkerManager markerManager;
    [SerializeField] TilemapReadController tileMapReadController;

    private Rigidbody2D rigidBody;
    public Vector3 position;
    private Vector2 direction;
    int lastDirection;
    public Vector2 inputVector;

    private SpriteRenderer spriteRenderer;
    public Image inventoryUI;
    public Image statsUI;
    public Image craftingUI;
    public Image settingsUI;
    public Image[] mainInteractionUI;
    public Sprite[] spriteArray;
    public Animator animator;

    [SerializeField] private float speed;
    private float normalSpeed;
    private float sprintSpeed;
    public bool isInteract = false;
    public bool isBuild = false;

    public InventoryManager inventoryManager;
    public CollisionManager collisionManager;
    public Item selectedItem;
    public Item[] itemsToPickup;

    void Awake()
    {
        DontDestroyOnLoad(this);
        mainInteractionUI = new Image[] { inventoryUI, craftingUI, settingsUI };

        if (player == null)
        {
            player = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PickupItem(int id)
    {
        inventoryManager.AddItem(itemsToPickup[id]);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get all of the game object components and set defaults
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        position = transform.position;
        sprintSpeed = speed * 1.5f;
        normalSpeed = speed;
        selectedItem = inventoryManager.selectedItem;

        // Items player starts out with
        for (int i = 0; i < itemsToPickup.Length; i++)
        {
            PickupItem(i);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        isInteract = Input.GetKey(KeyCode.E) ? true : false;
        isBuild = Input.GetKeyDown(KeyCode.B) ? !isBuild : isBuild;

        ProcessMovement();
        OpenMenus();

        selectedItem = inventoryManager.selectedItem;
    }

    public void OpenMenus()
    {
        if (isBuild)
        {
            Marker();
        }

        // Check for open inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isActive = !inventoryUI.gameObject.activeSelf;
            inventoryUI.gameObject.SetActive(isActive);
            SetUIHidden(inventoryUI);
        }
        // Checks for open crafting menu
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool isActive = !craftingUI.gameObject.activeSelf;
            craftingUI.gameObject.SetActive(isActive);
            SetUIHidden(craftingUI);
        }
        // Checks for open settings menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = !settingsUI.gameObject.activeSelf;
            settingsUI.gameObject.SetActive(isActive);
            SetUIHidden(settingsUI);
        }
        // Checks for open stats menu
        if (Input.GetKeyDown(KeyCode.P))
        {
            bool isActive = !statsUI.gameObject.activeSelf;
            statsUI.gameObject.SetActive(isActive);
        }
    }

    public void SetUIHidden(Image openUI)
    {
        foreach (Image img in mainInteractionUI)
        {
            if (img != openUI)
            {
                img.gameObject.SetActive(false);
            }
        }
    }

    public void ProcessMovement()
    {
        if (true)
        {
            // Applies physics based movement to the Player RigidBody2D
            Vector2 currentPos = rigidBody.position;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            inputVector = new Vector2(horizontalInput, verticalInput);

            // Check for sprint
            speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : normalSpeed;

            // Prevents movement if inventory is open
            if (inventoryUI.gameObject.activeSelf)
            {
                speed = 0;
                animator.SetFloat("Speed", 0.0f);
            }
            else
            {
                speed = normalSpeed;
                // Checking for bottom diagonals first (WD & AS)
                if ((horizontalInput > 0 && verticalInput > 0) || (horizontalInput < 0 && verticalInput < 0))
                {
                    inputVector = new Vector2(inputVector.x, inputVector.y / 4);
                    speed = speed * 1.2f;
                }

                // Player presses w
                if (verticalInput > 0)
                {
                    animator.SetFloat("Speed", 1.0f);
                    animator.SetInteger("Direction", 0);
                }
                // Player presses s
                if (verticalInput < 0)
                {
                    animator.SetFloat("Speed", 1.0f);
                    animator.SetInteger("Direction", 4);
                }
                // Player presses d
                if (horizontalInput > 0)
                {
                    animator.SetFloat("Speed", 1.0f);
                    animator.SetInteger("Direction", 2);
                }
                // Player presses a
                if (horizontalInput < 0)
                {
                    animator.SetFloat("Speed", 1.0f);
                    animator.SetInteger("Direction", 6);
                }

                // Setting Animator settings for diagonals
                // WD Animation
                if (horizontalInput > 0 && verticalInput > 0)
                {
                    animator.SetFloat("Speed", 1.0f);
                    animator.SetInteger("Direction", 1);
                }

                // SD Animation
                if (horizontalInput > 0 && verticalInput < 0)
                {
                    animator.SetFloat("Speed", 1.0f);
                    animator.SetInteger("Direction", 3);
                }

                // SA Animation
                if (horizontalInput < 0 && verticalInput < 0)
                {
                    animator.SetFloat("Speed", 1.0f);
                    animator.SetInteger("Direction", 5);
                }

                // WA Animation
                if (horizontalInput < 0 && verticalInput > 0)
                {
                    animator.SetFloat("Speed", 1.0f);
                    animator.SetInteger("Direction", 7);
                }

                // If the player isn't pressing anythingw
                if (horizontalInput == 0 && verticalInput == 0)
                {
                    animator.SetFloat("Speed", 0.0f);
                    rigidBody.velocity = new Vector2(0, 0);
                }
            }

            rigidBody.velocity = inputVector * speed * Time.fixedDeltaTime;
        }
    }

    private void Marker()
    {
        if (isBuild)
        {
            Vector3Int gridPositon = tileMapReadController.GetGridPosition(Input.mousePosition, true);
            markerManager.markedCellPosition = gridPositon;
        }
        else
        {
            markerManager.ClearMarker();
        }
    }
}
