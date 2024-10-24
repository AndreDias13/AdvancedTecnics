using Unity.VisualScripting;
using UnityEngine;

public class Placement : MonoBehaviour
{
   [SerializeField] GameObject _placementIndicator;
    Grid _grid;

    private void Update()
    {
        Vector3 mousePos = _placementIndicator.transform.position;


    }
}
