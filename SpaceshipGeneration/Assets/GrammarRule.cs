using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rule", menuName = "ScriptableObjects/GrammarRule", order = 1)]
public class GrammarRule : ScriptableObject
{
    public string GrammarID;

    [Serializable]
    public struct Rule
    {
        public string Symbol;
        public float Chance;
    }

    public Rule[] rules;

    public GameObject[] Shapes;
    
    private Dictionary<string, float> rulesDict = new Dictionary<string, float>();
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Rule rule in rules)
        {
            rulesDict.Add(rule.Symbol, rule.Chance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
