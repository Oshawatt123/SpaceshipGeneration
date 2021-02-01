using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Node
{
    public Node(GrammarRule _rule, int _priority)
    {
        rule = _rule;
        priority = _priority;
    }

    private GrammarRule rule;
    private int priority;
    
    public List<Node> nodes = new List<Node>();

    public bool enabled;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // grow the node
    public void Grow()
    {
        // if there is nothing, don't bother
        if (rule.rules.Length == 0) return;
        
        // for each rule, attempt to grow
        foreach (var currentRule in rule.rules)
        {
            if (Random.Range(0f, 1f) > currentRule.Chance)
            {
                //Dictionary<String, GrammarRule> wow = ShipGrammarV1.instance.symbolLookUp;
                // if the rule passes, create a new node in this one of the new type
                nodes.Add(new Node(ShipGrammarV1.instance.symbolLookUp[currentRule.Symbol], 1));
            }
        }
    }

    public bool hasNode(int index)
    {
        return index < nodes.Count;
    }

    public void AddNode(Node newNode)
    {
        nodes.Add(newNode);
    }

    public override string ToString()
    {
        return rule.GrammarID;
    }

    public GameObject GetModel()
    {
        return rule.Shapes[Random.Range(0, rule.Shapes.Length)];
    }
}
