using System;
using System.Collections.Generic;

//BreadthFirstPathFinder class inherits from the PathFinder class.
class BreadthFirstPathFinder : PathFinder
{
    //Flag for debugging purposes
    private bool _debug = true;

    //Constructor calls the base constructor with the provided graph.
    public BreadthFirstPathFinder(NodeGraph pGraph) : base(pGraph) { }

    //The generate method attempts to find a path from pFrom to pTo.
    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        //Check if either the start or end node is null.
        if (pFrom == null || pTo == null)
        {
            Console.WriteLine("Start or end node is null.");
            return null;
        }

        //Initialize the path, visited nodes list, queue, and parent map.
        List<Node> path = new List<Node>();
        List<Node> visited = new List<Node>();
        Queue<Node> queue = new Queue<Node>();
        Dictionary<Node, Node> parentMap = new Dictionary<Node, Node>();

        //Output debugging information if enabled
        if (_debug) Console.WriteLine("Finding shortest path using Breadth-First Search...");

        //Call the method to find the shortest path using BFS.
        return FindShortestPath(pFrom, pTo, queue, path, visited, parentMap);
    }

    //Private method that implements the BFS algorithm to find the shortest path.
    private List<Node> FindShortestPath(Node pFrom, Node pTo, Queue<Node> queue, List<Node> path, List<Node> visited, Dictionary<Node, Node> parentMap)
    {
        //Enqueue the start node, mark it as visited, and set its parent to null.
        queue.Enqueue(pFrom);
        visited.Add(pFrom);
        parentMap[pFrom] = null;

        //Continue the BFS loop until the queue is empty.
        while (queue.Count > 0)
        {
            //Dequeue the next node to process.
            Node currentNode = queue.Dequeue();

            //If the target node is reached, construct and return the path.
            if (currentNode == pTo)
            {
                if (_debug) Console.WriteLine("Target node reached. Constructing path...");
                return ConstructPath(pTo, parentMap);
            }

            //Iterate through all connected neighbors of the current node.
            foreach (Node neighbor in currentNode.connections)
            {
                //If the neighbor hasn't been visited yet.
                if (!visited.Contains(neighbor))
                {
                    if (_debug) Console.WriteLine($"Visiting neighbor: {neighbor.id}");
                    //Mark it as visited, enqueue it, and set its parent.
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    parentMap[neighbor] = currentNode;
                }
            }
        }

        //Return null if no path was found.
        if (_debug) Console.WriteLine("No path found.");
        return null;
    }

    //Private method to reconstruct the path from the parent map.
    private List<Node> ConstructPath(Node end, Dictionary<Node, Node> parentMap)
    {
        List<Node> path = new List<Node>();
        Node currentNode = end;

        //Backtrack from the end node to the start node using the parent map.
        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = parentMap[currentNode];
        }

        //Reverse the path to get the correct order from start to end.
        path.Reverse();

        if (_debug)
        {
            Console.WriteLine("Constructed path:");
            foreach (var node in path)
            {
                Console.WriteLine(node.id);
            }
        }

        return path;
    }
}


