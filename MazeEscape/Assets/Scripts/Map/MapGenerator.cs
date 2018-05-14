using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public int x;
    public int y;
    public List<Neighbour> neighbours;
    public bool isVisited;

    public Node(int x, int y) {
        this.x = x;
        this.y = y;
        this.neighbours = new List<Neighbour>();
        this.isVisited = false;
    }
}

public class Neighbour {
    public bool hasWall;
    public GameObject wall;
    public Node node;
    public Vector2 direction;

    public Neighbour(Node node, Vector2 direction) {
        this.node = node;
        this.direction = direction;
        this.hasWall = false;
        this.wall = null;
    }
}

public class MapGenerator : MonoBehaviour {

    static int MAP_SIZE = 10;
    static float SQUARE_SIZE = 1f;

    public GameObject wallPrefab;

    public Node[] nodes = new Node[MAP_SIZE * MAP_SIZE];

    Transform wallsParent;

    // Use this for initialization
    void Start() {
        wallsParent = transform.Find("Walls");
        InitialiseMap();
    }

    // Update is called once per frame
    void Update() {

    }

    void InitialiseMap() {
        float gridOffset = MAP_SIZE * SQUARE_SIZE / 2;
        wallsParent.position -= new Vector3(gridOffset, 0, gridOffset);

        for (int i = 0; i < nodes.Length; ++i) {
            int x = i % MAP_SIZE;
            int y = i / MAP_SIZE;

            Node newNode = new Node(x, y);

            nodes[i] = newNode;
        }

        FindNeighbours();
        GenerateWalls();
        Update();
    }

    void ResetNodes() {

    }

    void FindNeighbours() {
        foreach (Node n in nodes) {
            if (n.x > 0) {
                n.neighbours.Add(new Neighbour(GetNode(n.x - 1, n.y), new Vector2(-1, 0)));
            }

            if (n.x < MAP_SIZE - 1) {
                n.neighbours.Add(new Neighbour(GetNode(n.x + 1, n.y), new Vector2(1, 0)));
            }

            if (n.y > 0) {
                n.neighbours.Add(new Neighbour(GetNode(n.x, n.y - 1), new Vector2(0, -1)));
            }

            if (n.y < MAP_SIZE - 1) {
                n.neighbours.Add(new Neighbour(GetNode(n.x, n.y + 1), new Vector2(0, 1)));
            }
        }
    }

    public Node GetNode(int x, int y) {
        return nodes[(y * MAP_SIZE) + x];
    }

    public Vector3 GetNodePosition(Node node) {
        return new Vector3(node.x * SQUARE_SIZE, 0, node.y * SQUARE_SIZE);
    }

    void CreateWall(Node node, Vector2 direction) {
        Neighbour neighbour = node.neighbours.Find(n => n.direction == direction);
        Neighbour oppositeNeighbour = neighbour.node.neighbours.Find(n => n.direction == -direction);

        //TODO check that we dont still have these values after foreach's
        /*Neighbour neighbour = null;
        Neighbour oppositeNeighbour = null;

        foreach(Neighbour n in node.neighbours) {
            if (n.direction == direction) {
                neighbour = n;
            }
        }

        foreach (Neighbour n in neighbour.node.neighbours) {
            if (n.direction == -direction) {
                oppositeNeighbour = n;
            }
        }*/

        if (!neighbour.hasWall && !oppositeNeighbour.hasWall) {

            GameObject newWall = Instantiate(wallPrefab, wallsParent);
            newWall.transform.localPosition = GetNodePosition(node) + (new Vector3(direction.x, 0, direction.y) * (SQUARE_SIZE / 2));

            if (Mathf.Abs(direction.y) > 0) {
                newWall.transform.Rotate(new Vector3(0, 90, 0));
            }

            neighbour.hasWall = true;
            neighbour.wall = newWall;
            oppositeNeighbour.hasWall = true;
            oppositeNeighbour.wall = newWall;
        } else {
            Debug.LogError("We tried to create a wall where there already was one, oops");
            Debug.LogError("X: " + node.x);
            Debug.LogError("Y: " + node.y);
            Debug.LogError("dirX: " + direction.x);
            Debug.LogError("dirY: " + direction.y);
        }
    }

    public void RemoveWall(Neighbour neighbour) {
        //TODO: possible bug where opposite neighbour still thinks wall exists
        if (neighbour.hasWall) {
            Destroy(neighbour.wall);
            neighbour.hasWall = false;
            neighbour.wall = null;
        }

    }

    void GenerateWalls() {
        foreach (Node node in nodes) {
            node.neighbours.ForEach(neighbour => {
                if (neighbour.wall == null) {
                    CreateWall(node, neighbour.direction);
                }
            });
        }
    }
}

