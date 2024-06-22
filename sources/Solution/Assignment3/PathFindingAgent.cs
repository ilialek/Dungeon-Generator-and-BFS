using System;
using System.Collections.Generic;
using GXPEngine;

//PathFindingAgent class inherits from NodeGraphAgent and adds pathfinding capabilities.
class PathFindingAgent : NodeGraphAgent
{
    // Fields to track the current node, path, and pathfinding logic.
    private Node _current = null;
    private Queue<Node> _nodePath = new Queue<Node>();
    private PathFinder _pathFinder;
    private List<Node> _path = new List<Node>();

    //Constructor initializes the agent with a node graph and a pathfinder.
    public PathFindingAgent(NodeGraph pNodeGraph, PathFinder pathFinder) : base(pNodeGraph)
    {
        //Set the origin of the agent's sprite to its center.
        SetOrigin(width / 2, height / 2);
        _pathFinder = pathFinder;

        //If the node graph has nodes, initialize the path and set the current node randomly.
        if (pNodeGraph.nodes.Count > 0)
        {
            _nodePath.Clear();
            _path.Clear();
            _current = pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)];
            jumpToNode(_current);
        }

        //Subscribe to node left-click events.
        pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
    }

    //Event handler for node click events to generate a path.
    protected virtual void onNodeClickHandler(Node pNode)
    {
        Console.Write("Is Morc currently following a path? ");
        //If the agent is already following a path, do nothing.
        if (_nodePath.Count != 0) return;

        //Generate a path from the current node to the clicked node.
        Console.WriteLine("No. Generate path.");
        _path = _pathFinder.Generate(_current, pNode);

        //Enqueue the generated path nodes.
        Console.WriteLine("Add the path to the queue.");
        foreach (Node node in _path)
        {
            _nodePath.Enqueue(node);
        }
    }

    //Update method is called every frame to move the agent.
    protected override void Update()
    {
        //If there are no nodes in the path, do nothing.
        if (_nodePath.Count == 0) return;

        //Get the next node in the path if the agent is not already at the current node.
        if (_nodePath.Count > 0)
        {
            if (_current != _nodePath.Peek()) Console.WriteLine("Get next node: " + _nodePath.Peek());
            _current = _nodePath.Peek();
        }

        //Move towards the current node. If the agent reaches it, dequeue it from the path.
        if (moveTowardsNode(_current))
        {
            _nodePath.Dequeue();
        }
    }
}

