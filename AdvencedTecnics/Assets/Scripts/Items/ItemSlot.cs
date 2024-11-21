using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]  ScriptableItem m_Item;
    [SerializeField] int _itemSlotAmount;
    public ScriptableItem Item { get => m_Item; set => m_Item = value; }
    public int ItemSlotAmount { get => _itemSlotAmount; set => _itemSlotAmount = value; }
}
