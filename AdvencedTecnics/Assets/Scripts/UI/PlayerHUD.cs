using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Image[] _hotbarBackground;
    [SerializeField] PlayerController _playerCharacter;

    public static PlayerHUD Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

        }
    }

    private void Update()
    {
        UpdateSlot();
    }
    public void SelecterSlotHotbar()
    {
        //loop that resets slot size
        for (int i = 0; i < _hotbarBackground.Length; i++)
        {
            Vector3 normalScale = new Vector3(1f, 1f, 1f);
            _hotbarBackground[i].rectTransform.localScale = normalScale;
        }
        //change current slot size
        Vector3 bigScale = new Vector3(1.2f, 1.2f, 1.2f);
        _hotbarBackground[_playerCharacter.CurrentSlot].rectTransform.localScale = bigScale;
    }

    public void UpdateSlot()
    {
        foreach (var item in _playerCharacter.Hotbar)
        {
            ItemSlot itemSlot = item.GetComponent<ItemSlot>();
            TextMeshProUGUI itemTextAmount = item.GetComponentInChildren<TextMeshProUGUI>();
            Image icon = item.GetComponent<Image>();

            //adds info to the item slot
            if(itemSlot.Item != null)
            {
            itemTextAmount.text = itemSlot.ItemSlotAmount.ToString();
            icon.sprite = itemSlot.Item.ItemIcon;
                Color color = icon.color;
                color.a = 1;
                icon.color = color;
           
                //if has no items left, resets the item slot info
                if(itemSlot.ItemSlotAmount <= 0)
                {
                    itemTextAmount.text = "";
                    icon.sprite = null;
                    color.a = 0;
                    icon.color = color;
                    itemSlot.Item = null;
                }
            }

        }
    }
}
