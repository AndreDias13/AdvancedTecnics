using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class Node
{
    // Graph Representation Of Surrounding Nodes
    public List<Node> Neighbours = new List<Node>();

    // Physical Representation In Unity Scene
    public Transform Object;

    public void RemoveNeighbour(Transform NeighbourToRemove)
    {
        for (int i = 0; i < Neighbours.Count; ++i)
        {
            if (Neighbours[i].Object == NeighbourToRemove)
            {
                Neighbours.RemoveAt(i);
                return;
            }
        }
    }
}

public class Graph
{
    public List<Node> GraphNodes = new List<Node>();

    public void AddNewNode(GameObject ObjectToAdd, List<Transform> ExpectedNeighbours)
    {
        Node NewNode = new Node();
        NewNode.Object = ObjectToAdd.transform;

        for (int i = 0; i < ExpectedNeighbours.Count; ++i)
        {
            Node NodeFound = FindNode(ExpectedNeighbours[i]);
            if (NodeFound != null)
                NewNode.Neighbours.Add(NodeFound);
        }

        GraphNodes.Add(NewNode);
    }

    private Node FindNode(Transform NeightbourToFind)
    {
        for (int i = 0; i < GraphNodes.Count; ++i)
        {
            if (GraphNodes[i].Object == NeightbourToFind)
            {
                return GraphNodes[i];
            }
        }

        return null;
    }

    public void RemoveNode(Transform ObjectToRemove)
    {
        for (int i = 0; i < GraphNodes.Count; ++i)
        {
            if (GraphNodes[i].Object == ObjectToRemove)
            {
                // Remove Yourself From All Neightbours First
                for (int NeightbourIndex = 0; NeightbourIndex < GraphNodes[i].Neighbours.Count; ++NeightbourIndex)
                {
                    // Could Make This Easier With An Overload Operator TO Compare THe Internal Transform
                    if (GraphNodes[i].Neighbours[i].Object == ObjectToRemove)
                    {
                        GraphNodes[i].Neighbours[i].RemoveNeighbour(ObjectToRemove);
                        GraphNodes[i].Neighbours.RemoveAt(i);
                    }
                }

                GraphNodes.RemoveAt(i);

                return;
            }
        }
    }

}
