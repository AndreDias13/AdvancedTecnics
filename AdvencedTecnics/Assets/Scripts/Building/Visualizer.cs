using Unity.VisualScripting;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    [SerializeField] int _width; //x
    [SerializeField] int _height; //y
    [SerializeField] int _length; //z
    [SerializeField] float _cellSize;
    private void Start()
    {
        
    Grid grid = new Grid(_width, _height, _length, _cellSize);
    }

}
