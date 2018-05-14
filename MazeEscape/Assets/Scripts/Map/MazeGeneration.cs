using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeGeneration : MonoBehaviour {
    MapGenerator mapGenerator;
    Node startNode;
    //Node endNode;
    Node currentNode;
    Stack<Node> visitedList;


    private void Start() {
        mapGenerator = GetComponent<MapGenerator>();
        visitedList = new Stack<Node>();
        Initialise();
        RecursiveMazeGeneration();
    }

    void Initialise() {
        startNode = mapGenerator.GetNode(0, 0);
        //endNode = mapGenerator.GetNode(1, 1);
        currentNode = startNode;
    }

    void RecursiveMazeGeneration() {
        currentNode.isVisited = true;
        Neighbour currentNeighbour;

        while (!NodesVisited(mapGenerator.nodes)){
            if (!NeighboursVisited(currentNode.neighbours)) {
                visitedList.Push(currentNode);

                currentNeighbour = GetUnvisitedNeighbour();

                mapGenerator.RemoveWall(currentNeighbour);
                currentNode = currentNeighbour.node;
                currentNeighbour.node.isVisited = true;
            }
            else if (visitedList.Count != 0) {
                currentNode = visitedList.Pop();
            }
        }
    }

    bool NodesVisited(Node[] nodeList) {
        foreach (Node n in nodeList) {
            if (!n.isVisited) {
                return false;
            }
        }
        return true;
    }

    bool NeighboursVisited(List<Neighbour> neighbourList) {
        foreach (Neighbour n in neighbourList) {
            if (!n.node.isVisited) {
                return false;
            }
        }
        return true;
    }

    Neighbour GetUnvisitedNeighbour() {
        
        List<Neighbour> FilteredNeighbours = currentNode.neighbours.FindAll(neighbour => !neighbour.node.isVisited);
        int index = Random.Range(0, FilteredNeighbours.Count);
        return FilteredNeighbours[index];
    }


}
