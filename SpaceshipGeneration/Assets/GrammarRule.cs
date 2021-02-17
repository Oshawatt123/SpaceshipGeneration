using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rule", menuName = "ScriptableObjects/GrammarRule", order = 1)]
public class GrammarRule : ScriptableObject
{
    public string GrammarID;
    public int priority = 1;

    [Serializable]
    public struct Rule
    {
        public string Symbol;
        public float Chance;
    }

    public Rule[] rules;

    public GameObject[] Shapes;
    
    private Dictionary<string, float> rulesDict = new Dictionary<string, float>();


    private bool terminalRule = false;

    // Awake is called before Start
    void Awake()
    {
        if (rules.Length != 0)
        {
            foreach (Rule rule in rules)
            {
                rulesDict.Add(rule.Symbol, rule.Chance);
            }
        }
        else terminalRule = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}