using System;
using System.Collections.Generic;

//RecursivePathFinder class inherits from PathFinder
class RecursivePathFinder : PathFinder
{
    //Flag for debugging purposes
    private bool _debug = true;

    //Constructor for RecursivePathFinder
    public RecursivePathFinder(NodeGraph pGraph) : base(pGraph)
    {

    }

    //Override the generate method to find the shortest path between two nodes
    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        //Initialize lists to store the path and visited nodes
        List<Node> path = new List<Node>();
        List<Node> visitedPath = new List<Node>();

        //Output debugging information if enabled
        if (_debug) Console.WriteLine("Finding shortest path to the node...");

        //Call the recursive method to find the shortest route
        return FindShortestRoute(pFrom, pTo, path, visitedPath);
    }

    //Recursive method to find the shortest route between two nodes
    private List<Node> FindShortestRoute(Node pFrom, Node pTo, List<Node> path, List<Node> currentPath)
    {
        //Add the current node to the current path
        if (_debug) Console.WriteLine("Adding node " + pFrom.id + " to path");
        currentPath.Add(pFrom);

        //Check if the end node is reached
        if (_debug) Console.WriteLine("Is the end node reached?");
        if (pFrom == pTo)
        {
            //If the end node is reached, check if the current path is shorter than the shortest path
            if (_debug) Console.WriteLine("It is. Check if the current path is shorter than the shortest path...");
            if (path.Count == 0 || currentPath.Count < path.Count)
            {
                if (_debug) Console.WriteLine("Change shortest path to current path.");
                path = currentPath;
            }

            //Return the shortest path
            if (_debug) Console.WriteLine("Return shortest path.");
            return path;
        }

        //Iterate through every node and its neighbors to find a path
        foreach (Node node in pFrom.connections)
        {
            if (!currentPath.Contains(node))
            {
                path = FindShortestRoute(node, pTo, path, new List<Node>(currentPath));
            }
        }

        //Return the path
        if (_debug) Console.WriteLine("Returns a path.");
        return path;
    }
}

