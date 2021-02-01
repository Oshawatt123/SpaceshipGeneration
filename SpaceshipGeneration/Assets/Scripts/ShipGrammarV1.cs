using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShipGrammarV1 : MonoBehaviour
{

    public static ShipGrammarV1 instance;
    
    private bool cockpitPlaced = false;


    public GrammarRule[] SymbolLookup;

    private Tree grammarTree;

    public Dictionary<string, GrammarRule> symbolLookUp = new Dictionary<string, GrammarRule>();

    private void Awake()
    {
        instance = this;
        Debug.Log("Instance set");
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        BuildDictionary();
     
        // create tree
        grammarTree = new Tree(new Node(symbolLookUp["H"], 0));
        
        CreateGrammar(grammarTree.root);
        
        ParseGrammar(grammarTree.root);
    }

    private void BuildDictionary()
    {
        Debug.Log("Building dictionary");
        int index = 0;
        foreach (GrammarRule grammarRule in SymbolLookup)
        {
            symbolLookUp.Add(grammarRule.GrammarID, grammarRule);
            index++;
        }

        Debug.Log("Added " + index + " entries to dictionary");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateGrammar(Node node, int level = 0)
    {
        Debug.Log("Level " + level + ": " + node.ToString());
        
        node.Grow();

        if (!node.hasNode(0))
            return;

        foreach (Node currentNode in node.nodes)
        {
            CreateGrammar(currentNode, level+1);
        }
    }

    private void ParseGrammar(Node node, int level = 0)
    {
        Debug.Log("Level " + level + ": " + node.ToString());
        
        node.Grow();

        if (!node.hasNode(0))
            return;

        foreach (Node currentNode in node.nodes)
        {
            // Instantiate a prefab
            Instantiate(currentNode.GetModel(), transform);

            ParseGrammar(currentNode, level+1);
        }
    }
}