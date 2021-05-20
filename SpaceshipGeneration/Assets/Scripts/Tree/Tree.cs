using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tree
{

    public Tree(Node root)
    {
        this.root = root;
    }

    private int treeSize;
    
    public Node root;

    public void Clean()
    {
        clean(root);
    }

    // Recursive
    private void clean(Node toClean)
    {
        // base case
        // if this is a leaf, clean it and return
        if (toClean.nodes.Count == 0)
        {
            toClean.Clean();
            return;
        }
        
        foreach(Node node in toClean.nodes)
        {
            clean(node);
        }

        toClean.Clean();
        
        // reset list to let garbage cleaning take care of the rest
        root.nodes = new List<Node>();
    }

    public void Print()
    {
        
    }
    
    // Recursive
    public int CalculateSize()
    {
        treeSize = 0;
        
        calculateSize(root, 0);

        return treeSize;
    }

    private void calculateSize(Node _node, int depth)
    {
        treeSize++;
        
        if (_node.nodes.Count == 0)
            return;
        
        foreach (Node node in _node.nodes)
        {
            calculateSize(node, depth + 1);
        }
    }
}
