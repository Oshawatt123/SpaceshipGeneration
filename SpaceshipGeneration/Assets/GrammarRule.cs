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
        public string Replacement;
        public float chance;
    }

    public Rule[] rules;
    
    private Dictionary<string, float> rulesDict = new Dictionary<string, float>();
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Rule rule in rules)
        {
            rulesDict.Add(rule.Replacement, rule.chance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
