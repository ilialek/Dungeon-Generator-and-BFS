using System;
using System.Collections.Generic;
using GXPEngine;

//OnGraphWayPointAgent class inherits from NodeGraphAgent
class OnGraphWayPointAgent : NodeGraphAgent
{
    //Current node Morc is on
    private Node _current = null;

    //Queue to store the path of nodes Morc will traverse
    private Queue<Node> _nodePath = new Queue<Node>();

    //Constructor for OnGraphWayPointAgent
    public OnGraphWayPointAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
    {
        //Set the origin of the agent
        SetOrigin(width / 2, height / 2);

        //Position ourselves on a random node if available
        if (pNodeGraph.nodes.Count > 0)
        {
            _nodePath.Clear();
            _current = pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)];
            jumpToNode(_current);
        }

        //Listen to node clicks
        pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
    }

    //Event handler for node clicks
    protected virtual void onNodeClickHandler(Node pNode)
    {
        //Check if the clicked node is a neighbour of the node Morc is on and add it to the queue
        if (_current.connections.Contains(pNode))
        {
            _nodePath.Enqueue(pNode);
            Console.Write("Queued node " + pNode.id + " ");
            _current = pNode;
        }
        else
        {
            Console.WriteLine("Node isn't a neighbour of the node Morc is on. Click on a valid node.");
        }
    }

    //Update method to handle Morc's movement along the nodes
    protected override void Update()
    {
        //If no target node, don't walk
        if (_nodePath.Count == 0) return;

        //Move towards the next node in the path
        if (moveTowardsNode(_nodePath.Peek()))
        {
            Console.WriteLine("Reached node " + _nodePath.Peek().id);
            _nodePath.Dequeue();
        }
    }
}
