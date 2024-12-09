using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "Scriptable Objects/ScriptableItem")]
public class ScriptableItem : ScriptableObject
{
    [Header("Building Block")]
    //[SerializeField] Mesh _buildingBlockMesh;
    //[SerializeField] Material _buildingBlockMaterial;
    [SerializeField] GameObject _itemObject;

    [Header("Base item info")]
    [SerializeField] Sprite _itemIcon;
    [SerializeField] int itemMaxStack;

    public int ItemMaxStack { get => itemMaxStack; }
    public Sprite ItemIcon { get => _itemIcon; set => _itemIcon = value; }
    public GameObject ItemObject { get => _itemObject; set => _itemObject = value; }
    //public Mesh BuildingBlockMesh { get => _buildingBlockMesh; set => _buildingBlockMesh = value; }
    //public Material BuildingBlockMaterial { get => _buildingBlockMaterial; set => _buildingBlockMaterial = value; }
}
