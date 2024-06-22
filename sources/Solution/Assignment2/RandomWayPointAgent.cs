using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;

//RandomWayPointAgent class inherits from NodeGraphAgent
class RandomWayPointAgent : NodeGraphAgent
{
    //Current node Morc is on
    private Node _current = null;

    //Previous node Morc was on
    private Node _previousNode = null;

    //Queue to store the path of nodes Morc will traverse
    private Queue<Node> _nodePath = new Queue<Node>();

    //Flag indicating if Morc is moving randomly
    private bool _isMovingRandomly = false;

    //Random target node for Morc's movement
    private Node _randomTarget = null;

    //Constructor for RandomWayPointAgent
    public RandomWayPointAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
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
        //Check if Morc is not already moving randomly and the node path is empty
        if (!_isMovingRandomly && _nodePath.Count == 0)
        {
            //Add the clicked node to the queue
            _nodePath.Enqueue(pNode);
            Console.WriteLine("Queued node " + pNode.id);
            _isMovingRandomly = true;
        }
    }

    //Update method to handle Morc's movement along the nodes
    protected override void Update()
    {
        //If no target node, don't move
        if (_nodePath.Count == 0) return;

        //If reached the random target node or no random target set yet
        if (_randomTarget == null || moveTowardsNode(_randomTarget))
        {
            //Update current and previous nodes after reaching the target
            if (_randomTarget != null)
            {
                Console.WriteLine("Reached node " + _randomTarget.id);
                _previousNode = _current;
                _current = _randomTarget;
            }

            //If current node is the target node
            if (_current == _nodePath.Peek())
            {
                Console.WriteLine("Reached target node");
                _nodePath.Dequeue();
                _isMovingRandomly = false;
                _randomTarget = null;
                return;
            }

            //Move randomly towards the queued node
            MoveRandomlyTowardsTarget(_nodePath.Peek());
        }
    }

    //Method to move Morc randomly towards the target node
    private void MoveRandomlyTowardsTarget(Node targetNode)
    {
        //Get the neighbors of the current node excluding the previous node if it's not the only connection
        List<Node> neighbors = _current.connections.Where(n => n != _previousNode || _current.connections.Count == 1).ToList();

        //If the target node is a neighbor, move directly towards it
        if (neighbors.Contains(targetNode))
        {
            _randomTarget = targetNode;
            Console.WriteLine("Moving directly to queued node " + _randomTarget.id);
        }
        //If there are other neighbors, choose one randomly to move towards
        else if (neighbors.Count > 0)
        {
            _randomTarget = neighbors[Utils.Random(0, neighbors.Count)];
            Console.WriteLine("Moving randomly to node " + _randomTarget.id);
        }
    }
}