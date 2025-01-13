using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class StructureManager : MonoBehaviour
{
    private Graph StructureGraph = new Graph();

    public void AddPieceToHouse(GameObject NewObject)
    {
        StructureGraph.AddNewNode(NewObject, GetSurroundingStructureFoundations(NewObject.transform));
    }

    public void RemovePieceFromHouse(GameObject ObjectToRemove)
    {
        StructureGraph.RemoveNode(ObjectToRemove.transform);
    }

    private void OnDrawGizmos()
    {
        List<Node> GraphNodes = StructureGraph.GraphNodes;
        for (int i = 0; i < StructureGraph.GraphNodes.Count; ++i)
        {
            // Draw A Sphere To Showcase The Object
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GraphNodes[i].Object.position, .2f);

            for (int neighbourIndex = 0; neighbourIndex < GraphNodes[i].Neighbours.Count; ++neighbourIndex)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(GraphNodes[i].Object.position, GraphNodes[i].Neighbours[neighbourIndex].Object.position);
            }
        }
    }

 
    //This would have to be changed so instead of finding the near nodes by colision, should use the grid for it, to do that on placing an object on the grid he should be saved on a dictionary with the object itself and his position on the grid
    private List<Transform> GetSurroundingStructureFoundations(Transform newObjectTransform)
    {
        Vector3 ObjectPosition = newObjectTransform.position;

        float SphereCheckSize = 1.5f;
        Collider[] LeftSideCheck = Physics.OverlapSphere
        (
            ObjectPosition - newObjectTransform.right * newObjectTransform.localScale.x,
            SphereCheckSize
        );
        Collider[] RightSideCheck = Physics.OverlapSphere
        (
            ObjectPosition + newObjectTransform.right * newObjectTransform.localScale.x,
            SphereCheckSize
        );
        Collider[] TopSideCheck = Physics.OverlapSphere
        (
            ObjectPosition + newObjectTransform.up * newObjectTransform.localScale.y,
            SphereCheckSize
        );
        Collider[] BottomSideCheck = Physics.OverlapSphere
        (
            ObjectPosition - newObjectTransform.up * newObjectTransform.localScale.y,
            SphereCheckSize
        );

        // Easy Debug To Check The Sides
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = ObjectPosition - newObjectTransform.right * newObjectTransform.localScale.x * 0.5f;
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = ObjectPosition + newObjectTransform.right * newObjectTransform.localScale.x * 0.5f;
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = ObjectPosition + newObjectTransform.up * newObjectTransform.localScale.y * 0.5f;
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = ObjectPosition - newObjectTransform.up * newObjectTransform.localScale.y * 0.5f;

        List<Transform> NeighbourList = new List<Transform>();

        // Now Parse Through All The Data
        // Note yes we can generate garbage to make life easier, but where is the fun in that :p
        for (int i = 0; i < LeftSideCheck.Length; ++i)
        {
            if (LeftSideCheck[i].transform == newObjectTransform)
                continue;

            NeighbourList.Add(LeftSideCheck[i].transform);
        }

        for (int i = 0; i < RightSideCheck.Length; ++i)
        {
            if (RightSideCheck[i].transform == newObjectTransform)
                continue;

            NeighbourList.Add(RightSideCheck[i].transform);
        }

        for (int i = 0; i < TopSideCheck.Length; ++i)
        {
            if (TopSideCheck[i].transform == newObjectTransform)
                continue;

            NeighbourList.Add(TopSideCheck[i].transform);
        }

        for (int i = 0; i < BottomSideCheck.Length; ++i)
        {
            if (BottomSideCheck[i].transform == newObjectTransform)
                continue;

            NeighbourList.Add(BottomSideCheck[i].transform);
        }

        return NeighbourList;
    }

}

