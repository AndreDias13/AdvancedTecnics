using UnityEngine;

[ExecuteInEditMode]

public class BuildingBlock : MonoBehaviour, IDamageable
{
    public BlockTypes myBuildingBlocks;

    private StructureManager _structureManager = null;

    public enum BlockTypes
    {
        None,
        Center,
        Foundation,
        Wall
    }

    public void Destroy(PlayerController player)
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        GetOrFindStructureManager()?.AddPieceToHouse(gameObject);
    }

    private void OnDisable()
    {
        GetOrFindStructureManager()?.RemovePieceFromHouse(gameObject);
    }

    private StructureManager GetOrFindStructureManager()
    {
        if (!_structureManager)
        {
            _structureManager = FindObjectOfType<StructureManager>();
        }

        return _structureManager;
    }
}
