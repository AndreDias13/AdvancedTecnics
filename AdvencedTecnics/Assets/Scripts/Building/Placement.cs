using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class Placement : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] int _width; //x
    [SerializeField] int _height; //y
    [SerializeField] int _length; //z
    [SerializeField] float _cellSize;

    Grid _buildingGrid;

    [Header("Placement")]
    [SerializeField] GameObject _mouseInWorld;
    [SerializeField] GameObject _placementPlacehoulder; //temp
    [SerializeField] GameObject _placementPreview;
    [SerializeField] Material _greenMat; //temp
    [SerializeField] Material _redMat; //temp
    [SerializeField] GameObject _playerObject;
    Vector3 _mousePos;
    Vector3 cellCenterPos;
    int currentCell;

    Quaternion currentRotation;
    Vector3 _wallPos;

    [SerializeField] Rotations myRotations;

    ItemSlot _currentSlot;
   public enum Rotations
    {
        Front,
        Back,
        Right,
        Left
    }

    private void Start()
    {
        //initializes grid
        _buildingGrid = new Grid(_width, _height, _length, _cellSize);

    }

    private void Update()
    {
        PlaceStructure();

    }

    void PlaceStructure()
    {
        //uses the mouse position to find the pretended cell
        _mousePos = _mouseInWorld.transform.position;

        //gets positions in cell
        cellCenterPos = _buildingGrid.GetCellPlacePoint(_mousePos);
        _wallPos = _buildingGrid.GetCellWallPoint(_mousePos, myRotations);

        _buildingGrid.GetCellNumber(_mousePos, out currentCell); //gives the info of current cell

        PlacementVisualizer();

        PlacementRotation();

    }

    //Temp way to rotate Placehoulder Object
    public void PlacementRotation()
    {
        Quaternion newRotation = Quaternion.identity; // Default rotation
         currentRotation = _playerObject.transform.rotation;

        float normalizedY = currentRotation.eulerAngles.y % 360f;
        if (normalizedY < 0f) normalizedY += 360f; // 

        if (normalizedY >= 45f && normalizedY < 135f)
        {
            newRotation = Quaternion.Euler(0, 90, 0);
            myRotations = Rotations.Right;
        }
        else if (normalizedY >= 315f || normalizedY < 45f)
        {
            newRotation = Quaternion.Euler(0, 0, 0);
            myRotations = Rotations.Front;
        }
        else if (normalizedY >= 225f && normalizedY < 315f)
        {
            newRotation = Quaternion.Euler(0, -90, 0);
            myRotations = Rotations.Left;
        }
        else if (normalizedY >= 135f && normalizedY < 225f)
        {
            newRotation = Quaternion.Euler(0, 180, 0);
            myRotations = Rotations.Back;
        }
        //Can Smoothly interpolate the cell indicator's rotation towards the new rotation but with that hight value does it immediately
        //  _placementPreview.transform.rotation = Quaternion.Lerp(_placementPreview.transform.rotation, newRotation, Time.deltaTime * 10000);
        _placementPreview.transform.rotation = newRotation;
    }

    void PlacementVisualizer()
    {
        _placementPreview.transform.position = _wallPos;

        Transform child = _placementPreview.transform.GetChild(0);
        child.localScale = Vector3.one * _buildingGrid.CellSize;

        Renderer placementMat = _placementPreview.GetComponentInChildren<Renderer>();

        bool isNorthWall = _buildingGrid.CellStruct[currentCell].HasNorthWall;
        bool isSouthWall = _buildingGrid.CellStruct[currentCell].HasSouthWall;
        bool isEastWall = _buildingGrid.CellStruct[currentCell].HasEastWall;
        bool isWestWall = _buildingGrid.CellStruct[currentCell].HasWestWall;

        bool isCenter = _buildingGrid.CellStruct[currentCell].HasCenter;

        bool isFoundation = _buildingGrid.CellStruct[currentCell].HasFoundation;

        // NEEDS A COLOR CHANGE ON VISUALIZER DEPENDING OF CAN OR CANNOT BE PLACED
        placementMat.material = _greenMat;
    }

    public void PlaceItem(ItemSlot itemSlot)
    {
        _currentSlot = itemSlot;

        bool isNorthWall = _buildingGrid.CellStruct[currentCell].HasNorthWall;
        bool isSouthWall = _buildingGrid.CellStruct[currentCell].HasSouthWall;
        bool isEastWall = _buildingGrid.CellStruct[currentCell].HasEastWall;
        bool isWestWall = _buildingGrid.CellStruct[currentCell].HasWestWall;

        bool isCenter = _buildingGrid.CellStruct[currentCell].HasCenter;

        bool isFoundation = _buildingGrid.CellStruct[currentCell].HasFoundation;


        if (!isNorthWall && myRotations == Rotations.Front && itemSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall)
        {
            CreateBlock(_wallPos, itemSlot);
            _buildingGrid.CellStruct[currentCell].HasNorthWall = true;

        }
        if (!isSouthWall && myRotations == Rotations.Back && itemSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall)
        {
            CreateBlock(_wallPos, itemSlot);
            _buildingGrid.CellStruct[currentCell].HasSouthWall = true;

        }
        if (!isEastWall && myRotations == Rotations.Right && itemSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall)
        {
            CreateBlock(_wallPos, itemSlot);
            _buildingGrid.CellStruct[currentCell].HasEastWall = true;

        }
        if (!isWestWall && myRotations == Rotations.Left && itemSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall)
        {
            CreateBlock(_wallPos, itemSlot);
            _buildingGrid.CellStruct[currentCell].HasWestWall = true;
        }
         if (!isCenter && itemSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Center)
        {
            CreateBlock(cellCenterPos, itemSlot);
            _buildingGrid.CellStruct[currentCell].HasCenter = true;
        }
         if (!isFoundation && itemSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Foundation)
        {
            CreateBlock(cellCenterPos, itemSlot);
            _buildingGrid.CellStruct[currentCell].HasFoundation = true;

        }
        //   int offsetCell;
        //   _buildingGrid.GetCellNumber(-newBock.transform.right, out offsetCell);

    }

    void CreateBlock(Vector3 blockPos, ItemSlot itemSlot)
    {
        if (itemSlot.ItemSlotAmount > 0)
        {
            itemSlot.ItemSlotAmount--;
            GameObject newBock = Instantiate(itemSlot.Item.ItemObject, blockPos, Quaternion.identity);
            newBock.transform.localScale = Vector3.one * _buildingGrid.CellSize;
            newBock.transform.rotation = _placementPreview.transform.rotation;

            Collider blockCollider = newBock.GetComponentInChildren<Collider>();
            blockCollider.enabled = true;
        }
    }
}
