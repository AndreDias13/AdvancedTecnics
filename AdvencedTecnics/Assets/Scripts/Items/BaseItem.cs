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
        GameObject newObject = Instantiate(ItemInfo.ItemObject, transform.position, transform.rotation); //create the item visualizer
        newObject.transform.localScale = newObject.transform.localScale / 2; //scale it to fit item form
        newObject.transform.SetParent(this.gameObject.transform); //add to item object

    }
}
