using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Node
{
    public Node(GrammarRule _rule, int _priority)
    {
        rule = _rule;
        priority = _priority;
    }

    public Node(string _name = "undefined")
    {
        name = _name;
    }
    
    private GrammarRule rule;
    private int priority;
    
    public List<Node> nodes = new List<Node>();
    public List<Node> symmetryNodes = new List<Node>();

    public bool enabled;

    public string name;
    public Color bgColor = Color.blue;

    public ShipBit parentBit = null;
    public GameObject spawnedObject = null;
    public Vector3 position;
    public Quaternion rotation;

    private bool showing = false;

    public bool killLine = false;

    public void Init()
    {
        
    }

    // grow the node
    public void Grow()
    {
        Debug.Log("#### Node Growing ####");
        // if there is nothing, don't bother
        if (rule.rules.Length == 0) return;
        
        // for each rule, attempt to grow if we hit the probability chance
        foreach (var currentRule in rule.rules)
        {
            if(Random.Range(0, 1.0f) < currentRule.Chance)
                ParseRule(currentRule);
        }
    }

    private void ParseRule(GrammarRule.Rule _rule)
    {
        if (_rule.Symbol.Length == 0) return;
        
        // Read the rule, and determine the IDs and associated probability
        Debug.Log(_rule.Symbol);
        Dictionary<string, float> ruleExpanded = new Dictionary<string, float>();
        for (int i = 0; i < _rule.Symbol.Length; i+=6)
        {
            string symbol = _rule.Symbol.Substring(i, 1);
            //Debug.Log(_rule.Symbol.Substring(i + 2, 3));
            float chance = float.Parse(_rule.Symbol.Substring(i + 2, 3));

            //Debug.Log(symbol + ":" + chance.ToString());
            
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
                try
                {
                    
                    Node newNode = new Node(SymbolLookup.instance.symbolLookUp[entry.Key], 1);
                    Debug.Log("New node created");
                    
                    AddNode(newNode);
                    Debug.Log("New node added");
                }
                catch (Exception e)
                {
                    Debug.LogError("[" + entry.Key + "] not found in lookup. Or see below");
                    Console.WriteLine(e);
                    throw;
                }

                return;
            }
        }

    }

    public void Show(Vector3 _position, Quaternion? rot = null)
    {
        Quaternion _rotation = rot ?? Quaternion.identity;

        rotation = _rotation;
        position = _position;

        Show();
    }

    public void Show()
    {

        if (!showing)
        {
            Debug.Log("## spawning object ## ");

            RaycastHit hitInfo;
            Vector3 checkPos = position + (rotation * Vector3.right);
            if (!Physics.SphereCast(position, 0.5f, Vector3.right, out hitInfo))
            {
                spawnedObject = GameObject.Instantiate(rule.Shapes[0], position, rotation);
                Debug.Log("## object spawned ## ");
                showing = true;
            }
            else
            {
                Debug.LogWarning("Object " + hitInfo.transform.name + " found at spawn location " + checkPos + ". Object will not be spawned");
                killLine = true;
            }
        }
        else
        {
            Debug.Log("Object already spawned. Skipping");
        }
    }

    public void CreateSymmetry(Node refNode, Material mat, bool showMat = true)
    {
        if (spawnedObject != null)
        {
            Debug.Log("Spawning symmetrical object");
            Vector3 relativePos = position - refNode.spawnedObject.transform.position;
            relativePos.x = -relativePos.x;

            Vector3 spawnPos = refNode.position + relativePos;
            Debug.Log(spawnPos);
            
            RaycastHit hitInfo;
            Vector3 checkPos = spawnPos;
            
            if (Mathf.Abs(relativePos.x) > 0)
            {
                Debug.Log(relativePos + "=" + position + "-" + refNode.spawnedObject.transform.position);
                Node newNode = new Node();
                newNode.position = spawnPos;
                Vector3 newRot = rotation.eulerAngles;
                newRot.z += 180;
                newNode.rotation = Quaternion.Euler(newRot);
                newNode.parentBit = spawnedObject.GetComponent<ShipBit>();
                newNode.bgColor = Color.green;
                
                GameObject temp = GameObject.Instantiate(spawnedObject, newNode.position, newNode.rotation);
                if(showMat)
                    temp.GetComponentInChildren<Renderer>().material = mat;
                
                newNode.spawnedObject = temp;
                symmetryNodes.Add(newNode);
            }
        }
        else
        {
            Debug.LogWarning("spawnedObject is null. Symmetrical object not spawned");
        }
    }

    public void UpdateNodes()
    {
        Debug.Log("Updating node with " + symmetryNodes.Count + " symmetrical nodes");
        foreach (Node node in symmetryNodes)
        {
            nodes.Add(node);
        }
    }
    
    public void Clean()
    {
        // cleaning node

        if (spawnedObject != null)
        {
            Debug.Log("destroying old objects");
            GameObject.DestroyImmediate(spawnedObject); // altough advised not to be used, this is the only way you can reliably destroy objects in edit mode. Error when using Destroy.
        }

        spawnedObject = null;
    }

    public bool hasNode(int index)
    {
        return index < nodes.Count;
    }

    public void AddNode(Node newNode, bool calculateTransform = true)
    {
        if (calculateTransform)
        {
            newNode.parentBit = spawnedObject.GetComponent<ShipBit>();
            Debug.Log(newNode.parentBit == null ? "True" : "False");
            Transform linkTransform = newNode.parentBit.GetLinkPoint();
            newNode.position = linkTransform.position;
            newNode.rotation = linkTransform.rotation;
        }
        
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

    public String GetID()
    {
        return rule.GrammarID;
    }
}
