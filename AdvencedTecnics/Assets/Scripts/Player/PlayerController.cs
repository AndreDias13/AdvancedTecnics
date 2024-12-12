using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _movementSpeed;

    [Header("References")]
    Rigidbody _rigidBody;
   [SerializeField] Placement _placementSystem;

    [Header("Hotbar")]
    [SerializeField] ItemSlot[] _hotbar;
    [SerializeField] int _currentSlot;

    [Header("Items")]
    BaseItem _collectedItem;
    ScriptableItem _currentItem;

    [SerializeField] GameObject _previewPlacement;


    [Header("Building")]
    List<GameObject> _buildingBlockViewer = new List<GameObject>();
    public BaseItem CollectedItem { get => _collectedItem; set => _collectedItem = value; }
    public ItemSlot[] Hotbar { get => _hotbar; set => _hotbar = value; }
    public int CurrentSlot { get => _currentSlot; set => _currentSlot = value; }

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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Hotbar[CurrentSlot].Item != null)
            {
            _placementSystem.PlaceItem(Hotbar[CurrentSlot]);
            }
        }
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
                //collect item
                ItemToHotbar();
                OnHandItem();
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
            OnHandItem();

        }
        PlayerHUD.Instance.SelecterSlotHotbar(); //changes hotbar background UI

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
        OnHandItem();
    }
    #endregion
    void ItemToHotbar()
    {
        for (int i = 0; i < Hotbar.Length; i++)
        {
            ItemSlot itemSlot = Hotbar[i];
        
            //if theres already an item on that slot
            if (itemSlot.Item != null)
            {
                //checks if theres an item as the same as the collected one already and if its not full
                if (itemSlot.Item == CollectedItem.ItemInfo && itemSlot.ItemSlotAmount < itemSlot.Item.ItemMaxStack)
                {
                    int totalStack = itemSlot.ItemSlotAmount + CollectedItem.ItemAmount; //total of stack value 

                    if (totalStack > itemSlot.Item.ItemMaxStack) //verifies if theres more on stack than it can
                    {
                        int leftoverStack = totalStack - itemSlot.Item.ItemMaxStack; //calculates the leftovers

                        itemSlot.ItemSlotAmount = itemSlot.Item.ItemMaxStack; //gives the max stack to the slot

                        CollectedItem.ItemAmount = leftoverStack; //returns the leftover amount to the item

                        ItemToHotbar(); //redo the check to deliver the leftover to other slot

                        if (CollectedItem.ItemAmount <= 0) //checks if collected item is empty
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
            else //if theres no item on that slot
            {
                itemSlot.Item = CollectedItem.ItemInfo;
                itemSlot.ItemSlotAmount = CollectedItem.ItemAmount;
                Destroy(CollectedItem.gameObject);

                return;
            }
                
        }
    }

    void OnHandItem()
    {
        ItemSlot currentSlot = Hotbar[CurrentSlot];
        _currentItem = currentSlot.Item;

        if (_currentItem == null)
        {           
            return;
        }

        for (int i = _previewPlacement.transform.childCount - 1; i >= 0; i--)
        {
            // Get the child at the current index
            Transform child = _previewPlacement.transform.GetChild(i);

            // Destroy the child GameObject
            Destroy(child.gameObject);
        }
        GameObject newObject = Instantiate(_currentItem.ItemObject, _previewPlacement.transform.position, _previewPlacement.transform.rotation); //create the item visualizer
    
        newObject.transform.SetParent(_previewPlacement.gameObject.transform); //add to item object

 
    }


}
