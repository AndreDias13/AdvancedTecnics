using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Scriptable Objects/ScriptableItem")]
public class ScriptableItem : ScriptableObject
{
    [Header("Building Block")]
    [SerializeField] GameObject _buildingToPlace;
    [SerializeField] GameObject _preViewBuilding;

    [Header("Base item info")]
    [SerializeField] GameObject _floorItem;
    [SerializeField] GameObject _handItem;
    [SerializeField] Sprite _itemIcon;
    [SerializeField] int itemMaxStack;

    public GameObject PreViewBuilding { get => _preViewBuilding; set => _preViewBuilding = value; }
    public GameObject FloorItem { get => _floorItem; set => _floorItem = value; }
    public int ItemMaxStack { get => itemMaxStack; }
    public Sprite ItemIcon { get => _itemIcon; set => _itemIcon = value; }
}
