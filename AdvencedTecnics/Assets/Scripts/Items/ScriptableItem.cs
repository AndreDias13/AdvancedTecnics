using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Scriptable Objects/ScriptableItem")]


public class ScriptableItem : ScriptableObject
{
    [Header("Building Block")]
    [SerializeField] GameObject _itemObject;

    [Header("Base item info")]
    public ItemTypes myItemTypes;
    [SerializeField] Sprite _itemIcon;
    [SerializeField] int itemMaxStack;
    
  public enum ItemTypes
    {
        None,
        Center,
        Foundation,
        Wall
    } 

    public int ItemMaxStack { get => itemMaxStack; }
    public Sprite ItemIcon { get => _itemIcon; set => _itemIcon = value; }
    public GameObject ItemObject { get => _itemObject; set => _itemObject = value; }

}
