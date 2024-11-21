using Unity.VisualScripting;
using UnityEngine;

public class BaseItem : MonoBehaviour, IColectable
{
    [SerializeField] ScriptableItem _itemInfo;
    [SerializeField] int _itemAmount;

    public ScriptableItem ItemInfo { get => _itemInfo; set => _itemInfo = value; }
    public int ItemAmount { get => _itemAmount; set => _itemAmount = value; }

    public void Collect(PlayerController player)
    {
        player.CollectedItem = this;       
    }

    private void Awake()
    {
        //Add the item visual repesentation to the world
        GameObject floorItem = Instantiate(ItemInfo.FloorItem, transform.position, transform.rotation);
        floorItem.transform.SetParent(this.transform);
    }
}
