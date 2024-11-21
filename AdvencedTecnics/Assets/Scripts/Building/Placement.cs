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
   [SerializeField] GameObject _placementIndicator;
    Vector3 _mousePos;

    [SerializeField] GameObject _placementPlacehoulder; //temp
    [SerializeField] GameObject _placementIndicatorObjectPlacehoulder; //temp
    [SerializeField] Material _greenMat; //temp
    [SerializeField] Material _redMat; //temp
    [SerializeField] GameObject _playerObject; //temp



    private void Start()
    {
        //initializes grid
        _buildingGrid = new Grid(_width, _height, _length, _cellSize);

    }

    private void Update()
    {
        
            //uses the mouse position to find the pretended cell
            _mousePos = _placementIndicator.transform.position;
            Vector3 cellPos = _buildingGrid.GetCellPlacePoint(_mousePos);

            int cellNumber;
            _buildingGrid.GetCellNumber(_mousePos, out cellNumber);


        //test placement visulizer WIP |
        //                             V

        _placementIndicatorObjectPlacehoulder.transform.position = cellPos;
        _placementIndicatorObjectPlacehoulder.transform.localScale = Vector3.one * _buildingGrid.CellSize;
      
     

          Renderer placementMat = _placementIndicatorObjectPlacehoulder.GetComponentInChildren<Renderer>();


        if (_buildingGrid.CellStruct[cellNumber].HasRamp == false)
        {
            placementMat.material = _greenMat;
        }
        else
        {
            placementMat.material = _redMat;
        }

        PlacementRotation();

        //place 
        if ( Input.GetKeyDown(KeyCode.Mouse0))
        {

            //test placement - WIP |
            //                     v

            if (_buildingGrid.CellStruct[cellNumber].HasRamp == false)
            {
            //changed prefab size acording to the cell size
            _placementPlacehoulder.transform.localScale = Vector3.one * _buildingGrid.CellSize; 

          GameObject newBock =  Instantiate(_placementPlacehoulder, cellPos, Quaternion.identity); //place building
                _buildingGrid.CellStruct[cellNumber].HasRamp = true;

                newBock.transform.rotation = _placementIndicatorObjectPlacehoulder.transform.rotation;
            }
         
        }
    }

    //Temp way to rotate Placehoulder Object
    public void PlacementRotation()
    {
        Quaternion newRotation = Quaternion.identity; // Default rotation
        Quaternion currentRotation = _playerObject.transform.rotation;

        // Determine the new rotation based on the player's rotation
        if (currentRotation.eulerAngles.y >= 45f && currentRotation.eulerAngles.y < 135f)
        {
            newRotation = Quaternion.Euler(0, 90, 0); // Rotate 90 degrees
            //Right
        }
        else if (currentRotation.eulerAngles.y >= 315 && currentRotation.eulerAngles.y < 45f)
        {
            newRotation = Quaternion.Euler(0, 0, 0); // No rotation
            //Front
        }
        else if (currentRotation.eulerAngles.y >= 225 && currentRotation.eulerAngles.y < 315)
        {
            newRotation = Quaternion.Euler(0, -90, 0); // Rotate -90 degrees
            //Left
        }
        else if (currentRotation.eulerAngles.y >= 135f && currentRotation.eulerAngles.y < 225)
        {
            newRotation = Quaternion.Euler(0, 180, 0); // Rotate 180 degrees
            //Back
        }
        //Can Smoothly interpolate the cell indicator's rotation towards the new rotation but with that hight value does it immediately

        _placementIndicatorObjectPlacehoulder.transform.rotation = Quaternion.Lerp(_placementIndicatorObjectPlacehoulder.transform.rotation, newRotation, Time.deltaTime * 10000);
    }
}
