using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree
{
    /* ==================== Fields ==================== */

    private Node _head = null;
    private Stack<Node> _prevNodes = new Stack<Node>();



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// Add Selector node. It returns Success when a child returns Success.
    /// </summary>
    public BehaviourTree Selector()
    {
        Node node = new SelectorNode();
        if(_head == null)
        {
            _head = node;
        }
        else
        {
            _prevNodes.Peek().AddChild(node);
        }
        _prevNodes.Push(node);
        return this;
    }


    /// <summary>
    /// Add Sequence node. It returns Success when all children returns Success.
    /// </summary>
    public BehaviourTree Sequence()
    {
        Node node = new SequenceNode();
        if (_head == null)
        {
            _head = node;
        }
        else
        {
            _prevNodes.Peek().AddChild(node);
        }
        _prevNodes.Push(node);
        return this;
    }


    /// <summary>
    /// Add Sequence node. It runs till one of chilren returns Failure and always returns fixed value.
    /// </summary>
    public BehaviourTree SubSequence(byte fixedReturn)
    {
        Node node = new SubSequenceNode(fixedReturn);
        if (_head == null)
        {
            _head = node;
        }
        else
        {
            _prevNodes.Peek().AddChild(node);
        }
        _prevNodes.Push(node);
        return this;
    }


    /// <summary>
    /// Add Action node as a child
    /// </summary>
    public BehaviourTree Action(BehaviourDelegate action)
    {
#if UNITY_EDITOR
        if (_prevNodes.Count == 0)
        {
            Debug.LogError("BehaviourTree: There's no parent node to add child.");
            return this;
        }
#endif
        _prevNodes.Peek().AddChild(new ActionNode(action));
        return this;
    }


    /// <summary>
    /// Back to parant node.
    /// </summary>
    public BehaviourTree Back()
    {
#if UNITY_EDITOR
        if (_prevNodes.Count == 0)
        {
            Debug.LogError("BehaviourTree: No node left in stack.");
            return this;
        }
#endif
        _prevNodes.Pop();
        return this;
    }


    /// <summary>
    /// End building behaviour tree
    /// </summary>
    public void End()
    {
        _prevNodes = null;
    }


    /// <summary>
    /// Run behaviour tree.
    /// </summary>
    public void Execute()
    {
        _head.Execute();
    }
    


    /* ==================== Class ==================== */

    private abstract class Node
    {
        protected List<Node> Children;


        public abstract byte Execute();


        public Node()
        {
            Children = new List<Node>();
        }


        public void AddChild(Node child)
        {
            Children.Add(child);
        }
    }


    private class SelectorNode : Node
    {
        public override byte Execute()
        {
            foreach (Node child in Children)
            {
                switch (child.Execute())
                {
                    case Constants.SUCCESS:
                        return Constants.SUCCESS;

                    default:
                        break;
                }
            }

            return Constants.FAILURE;
        }
    }



    private class SequenceNode : Node
    {
        public override byte Execute()
        {
            foreach (Node child in Children)
            {
                switch (child.Execute())
                {
                    case Constants.FAILURE:
                        return Constants.FAILURE;

                    default:
                        break;
                }
            }

            return Constants.SUCCESS;
        }
    }



    private class SubSequenceNode : Node
    {
        private byte _fixedReturn = Constants.SUCCESS;


        public SubSequenceNode(byte fixedReturn)
            :base()
        {
            _fixedReturn = fixedReturn;
        }


        public override byte Execute()
        {
            foreach (Node child in Children)
            {
                switch (child.Execute())
                {
                    case Constants.FAILURE:
                        return _fixedReturn;

                    default:
                        break;
                }
            }

            return _fixedReturn;
        }
    }



    private class ActionNode : Node
    {
        private BehaviourDelegate _action = null;


        public ActionNode(BehaviourDelegate action)
        {
            _action = action;
        }


        public override byte Execute()
        {
#if UNITY_EDITOR
            if (null == _action)
            {
                Debug.LogError("BehaviourTree: Action node is null");
                return Constants.FAILURE;
            }
#endif
            return _action.Invoke();
        }
    }
}
