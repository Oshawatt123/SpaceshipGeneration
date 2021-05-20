using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShipGrammarV1 : TreeGrammarBase
{

    public static ShipGrammarV1 instance;
    
    private bool cockpitPlaced = false;


    [Tooltip("Index 0 will be the root of the Grammar")]
    public GrammarRule[] SymbolLookup;

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
        grammarTree = new Tree(new Node(SymbolLookup[0], 0));

        Debug.Log("##### STARTING GRAMMAR GROWTH #####");
        CreateGrammar(grammarTree.root);
        
        Debug.Log("##### STARTING GRAMMAR PARSE ######");
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

    private void ParseGrammar(Node node, int level = 0, Transform parent = null)
    {
        Debug.Log("Level " + level + ": " + node.ToString());
        
        // this is where I need to do context sensitive stuff for how to place each bit of the grammar

        Vector3 worldPos = transform.position;
        Quaternion worldRot = transform.rotation;
        Transform linkPoint = null;

        if (parent != null)
        {
            if (node.GetID() == "W")
            {
                worldPos = parent.position;
                worldRot = parent.rotation;
            }
            linkPoint = parent.GetComponent<ShipBit>().GetLinkPoint();
            worldPos = linkPoint.position;
            worldRot = linkPoint.rotation;
        }

        GameObject newObj = Instantiate(node.GetModel(), worldPos, worldRot, transform);
        ShipBit newBit = newObj.GetComponent<ShipBit>();
        
        if (linkPoint != null)
            newBit.Link(linkPoint.transform);
        else
        {
            Debug.LogWarning("Parent: " + parent + ";" + "NewBit" + newBit);

            Debug.Log(newBit.transform.name);
        }

        if (!node.hasNode(0))
            return;

        foreach (Node currentNode in node.nodes)
        {
            ParseGrammar(currentNode, level+1, newBit.transform);
        }
    }

}