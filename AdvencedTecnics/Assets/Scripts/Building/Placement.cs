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

    Vector3Int _cellPos;

    public ItemSlot CurrentSlot { get => _currentSlot; set => _currentSlot = value; }

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

        _cellPos = _buildingGrid.GetCellPos(_mousePos);

        //gets positions in cell
        cellCenterPos = _buildingGrid.GetCellPlacePoint(_mousePos);
        _wallPos = _buildingGrid.GetCellWallPoint(_mousePos, myRotations);

      //  _buildingGrid.GetCellNumber(_mousePos, out currentCell); //gives the info of current cell

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
        //checks if theres an visualizer to show and puts it in the correct position
        if (_placementPreview.transform.childCount <= 0) return;

        if (CurrentSlot == null) return;

        if (CurrentSlot.Item == null) return;

        if (CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall)
        {
        _placementPreview.transform.position = _wallPos;
        }
        else
        {
        _placementPreview.transform.position = cellCenterPos;
        }

        
        Transform child = _placementPreview.transform.GetChild(0);
        child.localScale = Vector3.one * _buildingGrid.CellSize;

        Renderer placementMat = _placementPreview.GetComponentInChildren<Renderer>();

        bool isNorthWall = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasNorthWall;
        bool isSouthWall = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasSouthWall;
        bool isEastWall = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasEastWall;
        bool isWestWall = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasWestWall;

        bool isCenter = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasCenter;

        bool isFoundation = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasFoundation;

        //verify if theres a block on the cell of the same type, if it has change the visualizer material to red, if it doesnt change it to green
        placementMat.material = _greenMat;

        bool isInvalidPlacement =
            (isNorthWall && myRotations == Rotations.Front && CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall) ||
            (isSouthWall && myRotations == Rotations.Back && CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall) ||
            (isEastWall && myRotations == Rotations.Right && CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall) ||
            (isWestWall && myRotations == Rotations.Left && CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall) ||
            (isCenter && CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Center) ||
            (isFoundation && CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Foundation);

        if (isInvalidPlacement)
        {
            placementMat.material = _redMat;
        }
    }

    public void PlaceItem()
    { 
        bool isNorthWall = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasNorthWall;
        bool isSouthWall = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasSouthWall;
        bool isEastWall = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasEastWall;
        bool isWestWall = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasWestWall;

        bool isCenter = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasCenter;

        bool isFoundation = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasFoundation;

        if (!isNorthWall && myRotations == Rotations.Front && CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall)
        {
            CreateBlock(_wallPos, _currentSlot);
            _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasNorthWall = true;
        }
        if (!isSouthWall && myRotations == Rotations.Back && CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall)
        {
            CreateBlock(_wallPos, CurrentSlot);
            _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasSouthWall = true;
        }
        if (!isEastWall && myRotations == Rotations.Right && CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall)
        {
            CreateBlock(_wallPos, CurrentSlot);
            _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasEastWall = true;
        }
        if (!isWestWall && myRotations == Rotations.Left && CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall)
        {
            CreateBlock(_wallPos, CurrentSlot);
            _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasWestWall = true;
        }
         if (!isCenter && CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Center)
        {
            CreateBlock(cellCenterPos, CurrentSlot);
            _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasCenter = true;
        }
         if (!isFoundation && CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Foundation)
        {
            CreateBlock(cellCenterPos, CurrentSlot);
            _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasFoundation = true;
        }

      //  _buildingGrid.GetXYZ(_mousePos, out int x, out int y, out int z);
      if(CurrentSlot.Item.myItemTypes == ScriptableItem.ItemTypes.Wall)
        {
        _buildingGrid.HandleCloseCellsForWalls(_cellPos, myRotations, true);
        }
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

    public void CellRemoveInfo(GameObject newBlock)
    {
        //this will verify the type of building block that we are removing, and pass the iformation that we are removing it to the cell so we can place something after it
        BuildingBlock thisBlock = newBlock.GetComponent<BuildingBlock>();

        bool isNorthWall = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasNorthWall;
        bool isSouthWall = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasSouthWall;
        bool isEastWall = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasEastWall;
        bool isWestWall = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasWestWall;

        bool isCenter = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasCenter;

        bool isFoundation = _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasFoundation;

        if (isNorthWall &&  thisBlock.myBuildingBlocks == BuildingBlock.BlockTypes.Wall)
        {
            _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasNorthWall = false;
        }
        if (isSouthWall && thisBlock.myBuildingBlocks == BuildingBlock.BlockTypes.Wall)
        {
            _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasSouthWall = false;
        }
        if (isEastWall && thisBlock.myBuildingBlocks == BuildingBlock.BlockTypes.Wall)
        {
            _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasEastWall = false;
        }
        if (isWestWall && thisBlock.myBuildingBlocks == BuildingBlock.BlockTypes.Wall)
        {
            _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasWestWall = false;
        }
        if (isCenter && thisBlock.myBuildingBlocks == BuildingBlock.BlockTypes.Center)
        {
            _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasCenter = false;
        }
        if (isFoundation && thisBlock.myBuildingBlocks == BuildingBlock.BlockTypes.Foundation)
        {
            _buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasFoundation = false;
            Debug.Log(_buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasFoundation);

        }

        if (thisBlock.myBuildingBlocks == BuildingBlock.BlockTypes.Wall)
        {
            _buildingGrid.HandleCloseCellsForWalls(_cellPos, myRotations, false);
        }

    }

    public void CheckPlacement()
    {
        Debug.Log(_buildingGrid.GridArray[_cellPos.x, _cellPos.y, _cellPos.z].HasFoundation);

    }

}
