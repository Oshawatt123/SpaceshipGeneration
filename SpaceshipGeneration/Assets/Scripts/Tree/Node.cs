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
            ParseRule(currentRule);
        }
    }

    private void ParseRule(GrammarRule.Rule _rule)
    {
        if (_rule.Symbol.Length == 0) return;
        
        Debug.Log(_rule.Symbol);
        Dictionary<string, float> ruleExpanded = new Dictionary<string, float>();
        for (int i = 0; i < _rule.Symbol.Length; i+=6)
        {
            string symbol = _rule.Symbol.Substring(i, 1);
            Debug.Log(_rule.Symbol.Substring(i + 2, 3));
            float chance = float.Parse(_rule.Symbol.Substring(i + 2, 3));

            Debug.Log(symbol + ":" + chance.ToString());
            
            ruleExpanded.Add(symbol, chance);
        }

        if (ruleExpanded.Count == 0) return;

        float rando = Random.Range(0f, 1f);

        float ruleChance = 0.0f;
        foreach(KeyValuePair<string, float> entry in ruleExpanded)
        {
            ruleChance = Mathf.Min(ruleChance + entry.Value, 1f);
            // do something with entry.Value or entry.Key
            if (rando < ruleChance)
            {
                nodes.Add(new Node(ShipGrammarV1.instance.symbolLookUp[entry.Key], 1));
                return;
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
