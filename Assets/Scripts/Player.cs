using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player player;

    [SerializeField] MarkerManager markerManager;
    [SerializeField] TilemapReadController tileMapReadController;

    private Rigidbody2D rigidBody;
    public Vector3 position;
    public Vector2 direction;
    int lastDirection;
    public Vector2 inputVector;

    private SpriteRenderer spriteRenderer;
    public Image inventoryUI;
    public Image statsUI;
    public Image craftingUI;
    public Image questsUI;
    public QuestWindow questWindow;
    public Image settingsUI;
    public GameObject[] invBtns;
    public Image[] mainInteractionUI;
    public Sprite[] spriteArray;
    public Animator animator;

    CinemachineBrain currentCamera;

    Vector3Int selectedTilePosition;
    bool selectable;

    [SerializeField] private float offsetDistance = 1f;
    [SerializeField] private float speed;
    [SerializeField] private float sizeOfIA;
    [SerializeField] float maxDistance = 1.5f;
    private float normalSpeed;
    private float sprintSpeed;
    public bool isInteract = false;
    public bool useGrid = false;
    //public bool isBuild = false;

    bool isActiveUI = false;

    public Character character;

    public InventoryManager inventoryManager;
    public CollisionManager collisionManager;
    public Item selectedItem;
    public Item[] itemsToPickup;

    [SerializeField] ToolAction onTilePickUp;
    [SerializeField] ItemHighlight itemHighlight;

    void Awake()
    {
        player = this;
        currentCamera = Camera.main.GetComponent<CinemachineBrain>();
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
        //isInteract = Input.GetMouseButtonDown(0) ? true : false;
        //isBuild = Input.GetKeyDown(KeyCode.B) ? !isBuild : isBuild;
        selectedItem = inventoryManager.selectedItem;

        character.Rest(.01f);

        // Checks if grid needs to be displayed
        if (selectedItem == null || selectedItem.usesGrid)
        {
            useGrid = true;
            CanSelectCheck();
            SelectTile();
        }
        else
        {
            useGrid = false;
            markerManager.Show(false);
        }

        ProcessMovement();
        OpenMenus();

        // If player interacts
        if (Input.GetMouseButton(0))
        {
            Interact();
            if (!isInteract && !isActiveUI && useGrid)
            {
                UseToolGrid();
                character.GetTired(5);
                Debug.Log("Stamina used");
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!isInteract && !isActiveUI)
            {
                UseToolWorld();
                character.GetTired(5);
                Debug.Log("Stamina used");
            }
        }
    }

    #region UI

    public void OpenMenus()
    {
        var brain = currentCamera.GetComponent<CinemachineBrain>();
        CinemachineVirtualCamera vcam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;

        // Page up & Page down changes camera size
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            if (vcam.m_Lens.OrthographicSize <= 10)
            {
                vcam.m_Lens.OrthographicSize += 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            if(vcam.m_Lens.OrthographicSize >= 2)
            {
                vcam.m_Lens.OrthographicSize -= 1;
            }
        }

        // Check for open inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory();
        }
        // Checks for open crafting menu
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenCrafting();
        }
        // Checks for open quests menu
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OpenQuests();
        }
        // Checks for open settings menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenSettings();
        }
    }

    public void OpenInventory()
    {
        isActiveUI = !inventoryUI.gameObject.activeSelf;
        inventoryUI.gameObject.SetActive(isActiveUI);
        invBtns[0].gameObject.SetActive(false);
        invBtns[1].gameObject.SetActive(true);
        invBtns[2].gameObject.SetActive(true);
        invBtns[3].gameObject.SetActive(true);
        SetUIHidden(inventoryUI);
    }

    public void OpenCrafting()
    {
        isActiveUI = !craftingUI.gameObject.activeSelf;
        craftingUI.gameObject.SetActive(isActiveUI);
        invBtns[0].gameObject.SetActive(true);
        invBtns[1].gameObject.SetActive(false);
        invBtns[2].gameObject.SetActive(true);
        invBtns[3].gameObject.SetActive(true);
        SetUIHidden(craftingUI);
    }

    public void OpenQuests()
    {
        isActiveUI = !questsUI.gameObject.activeSelf;
        questsUI.gameObject.SetActive(isActiveUI);
        invBtns[0].gameObject.SetActive(true);
        invBtns[1].gameObject.SetActive(true);
        invBtns[2].gameObject.SetActive(false);
        invBtns[3].gameObject.SetActive(true);
        SetUIHidden(questsUI);
    }

    public void OpenSettings()
    {
        isActiveUI = !settingsUI.gameObject.activeSelf;
        settingsUI.gameObject.SetActive(isActiveUI);
        invBtns[0].gameObject.SetActive(true);
        invBtns[1].gameObject.SetActive(true);
        invBtns[2].gameObject.SetActive(true);
        invBtns[3].gameObject.SetActive(false);
        SetUIHidden(settingsUI);
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
    #endregion

    public void ProcessMovement()
    {
        if (true)
        {
            // Applies physics based movement to the Player RigidBody2D
            Vector2 currentPos = rigidBody.position;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            inputVector = new Vector2(horizontalInput, verticalInput);

            if(horizontalInput != 0 || verticalInput != 0)
            {
                direction = new Vector2(horizontalInput, verticalInput);
            }

            // Check for sprint
            speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : normalSpeed;

            // Prevents movement if menu is open
            for (int i = 0; i < mainInteractionUI.Length; i++)
            {
                if (mainInteractionUI[i].gameObject.activeSelf == true)
                {
                    speed = 0;
                    animator.SetFloat("Speed", 0.0f);
                    rigidBody.velocity = 2 * inputVector * speed * Time.fixedDeltaTime;

                    invBtns[0].gameObject.SetActive(true);
                    invBtns[1].gameObject.SetActive(true);
                    invBtns[2].gameObject.SetActive(true);
                    invBtns[3].gameObject.SetActive(true);
                    invBtns[i].gameObject.SetActive(false);

                    return;
                }
            }

            invBtns[0].gameObject.SetActive(false);
            invBtns[1].gameObject.SetActive(false);
            invBtns[2].gameObject.SetActive(false);
            invBtns[3].gameObject.SetActive(false);
            questWindow.CloseWindow();

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

            // If the player isn't pressing anything
            if (horizontalInput == 0 && verticalInput == 0)
            {
                animator.SetFloat("Speed", 0.0f);
                rigidBody.velocity = new Vector2(0, 0);
            }

            rigidBody.velocity = 2 * inputVector * speed * Time.fixedDeltaTime;
        }
    }

    private void SelectTile()
    {
        selectedTilePosition = tileMapReadController.GetGridPosition(Input.mousePosition, true);
        Marker();
    }

    // Method checks if it is possible for the user to select the tile 
    // based on its position and the camera's posiiton
    void CanSelectCheck()
    {
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
        markerManager.Show(selectable);
        itemHighlight.CanSelect = selectable;
    }

    private void Marker()
    {
        markerManager.markedCellPosition = selectedTilePosition;
        itemHighlight.cellPosition = selectedTilePosition;
    }

    private bool UseToolWorld()
    {
        Vector2 position = rigidBody.position + direction * offsetDistance;

        if (selectedItem == null) { return false; }
        if (selectedItem.onAction == null) { return false; }

        bool complete = selectedItem.onAction.OnApply(position);

        character.IncreaseXP(5);

        // Checks if item used can be removed from inventory
        if (complete)
        {
            if (selectedItem.onItemUsed != null)
            {
                selectedItem.onItemUsed.OnItemUsed(selectedItem, inventoryManager);
            }
        }

        return complete;
    }

    public void UseToolGrid()
    {

        if (selectable)
        {
            if(selectedItem == null) 
            { 
                PickUpTile();
                return;
            }
            if(selectedItem.onTileMapAction == null) { return; }

            bool complete = selectedItem.onTileMapAction.OnApplyToTileMap(selectedTilePosition, tileMapReadController, selectedItem);

            character.IncreaseXP(5);

            // Checks if item used can be removed from inventory
            if (complete)
            {
                if(selectedItem.onItemUsed != null)
                {
                    selectedItem.onItemUsed.OnItemUsed(selectedItem, inventoryManager);
                }
            }
        }
    }

    private void Interact()
    {
        /// Handles collision for key input instead of mouse
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 position = rigidBody.position + direction * offsetDistance;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfIA);

            foreach (Collider2D c in colliders)
            {
                Interactable obj = c.GetComponent<Interactable>();
                if (obj != null)
                {
                    obj.Interact(player);
                }
            }
        }
    }

    private void PickUpTile()
    {
        if(onTilePickUp == null) { return; }
        onTilePickUp.OnApplyToTileMap(selectedTilePosition, tileMapReadController, null);
    }
}
