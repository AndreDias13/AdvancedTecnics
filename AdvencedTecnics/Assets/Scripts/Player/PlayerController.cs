using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _movementSpeed;

    [Header("References")]
    Rigidbody _rigidBody;

    [Header("Hotbar")]
    [SerializeField] GameObject[] _hotbar;
    public int CurrentSlot { get; set; } = 0;

    [Header("Items")]
    BaseItem _collectedItem;

    [Header("Building")]
    List<GameObject> _buildingBlockViewer = new List<GameObject>();
    public BaseItem CollectedItem { get => _collectedItem; set => _collectedItem = value; }
    public GameObject[] Hotbar { get => _hotbar; set => _hotbar = value; }

    #region MonoBehaviour
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        MoveInDirection(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }

    private void Update()
    {
        SlotChange();
    }

    private void OnCollisionEnter(Collision collision)
    {
        IColectable colected = collision.transform.GetComponent<IColectable>();

        if (colected != null)
        {
            //Collects the item and adds it to the player 
             colected.Collect(this);

            if(CollectedItem != null)
            {
                ItemToHotbar();
            }
        }
    }
    #endregion

    #region Player Actions
    private void MoveInDirection(Vector2 direction)
    {
        Vector3 finalVelocity = (direction.x * transform.right + direction.y * transform.forward).normalized * _movementSpeed;

        finalVelocity.y = _rigidBody.linearVelocity.y;
        _rigidBody.linearVelocity = finalVelocity;
    }

    #endregion

    #region Hotbar
    void SlotChange()
    {
     //   InicializeItemOnSlot();

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInput < 0f)
            {
                //scroll up
                SwitchSlot(true);
            }
            else if (scrollInput > 0f)
            {
                //scroll down
                SwitchSlot(false);
            }
        
        // Check if any number key (1 to 5) is pressed
        if (Input.anyKeyDown && Input.inputString.Length == 1 && Input.inputString[0] >= '1' && Input.inputString[0] <= '5')
        {
            //Get the pressed number as an integer
            int keyPressed = int.Parse(Input.inputString);

            //Changes item slot
            CurrentSlot = keyPressed - 1;

        }
        PlayerHUD.Instance.SelecterSlotHotbar();
    }

    void SwitchSlot(bool scrollUp)
    {
        //changes to the next slot using scroll

        if (scrollUp == false)
        {

            if (CurrentSlot < Hotbar.Length - 1)
            {
                CurrentSlot++;
            }
            else
            {
                CurrentSlot = 0;
            }
        }
        else
        {
            if (CurrentSlot > 0)
            {
                CurrentSlot--;
            }
            else
            {
                CurrentSlot = Hotbar.Length - 1;
            }
        }
    }
    #endregion
    void ItemToHotbar()
    {

        for (int i = 0; i < Hotbar.Length; i++)
        {

            ItemSlot itemSlot = Hotbar[i].GetComponent<ItemSlot>();
            // Ensure the hotbar slot is not empty
            if (itemSlot != null)
            {
                itemSlot.Item = CollectedItem.ItemInfo;

                // Check if the item IDs match and the hotbar item can take more stacks
                if (itemSlot.Item == CollectedItem.ItemInfo && itemSlot.ItemSlotAmount < itemSlot.Item.ItemMaxStack)
                {
                    int totalStack = itemSlot.ItemSlotAmount + CollectedItem.ItemAmount;

                    if (totalStack > itemSlot.Item.ItemMaxStack) // Handle overflow scenario
                    {
                        int leftoverStack = totalStack - itemSlot.Item.ItemMaxStack;

                        itemSlot.ItemSlotAmount = itemSlot.Item.ItemMaxStack;  // Fill the hotbar slot
                   
                        //returns that amount value to be checked and added again 
                        CollectedItem.ItemAmount = leftoverStack; 
                        ItemToHotbar();

                        //if theres no more amount inside the item, will remove it from the world
                        if(CollectedItem.ItemAmount <= 0)
                        {
                            Destroy(CollectedItem.gameObject);
                        }
                    }
                    else // If there’s no overflow, just add to the stack
                    {
                        itemSlot.ItemSlotAmount += CollectedItem.ItemAmount;
                        CollectedItem.ItemAmount = 0;
                        Destroy(CollectedItem.gameObject);
                        
                    }
                    break;  // Exit once the item is handled
                }
            }
            else
            {
                // If hotbar slot is empty, add the item to it

                return;
            }
        }

    }

}
