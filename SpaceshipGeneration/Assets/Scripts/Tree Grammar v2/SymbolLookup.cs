using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SymbolLookup : MonoBehaviour
{
    public static SymbolLookup instance;
    
    private bool cockpitPlaced = false;


    [Tooltip("Index 0 will be the root of the Grammar")]
    public GrammarRule[] Lookup;

    public Dictionary<string, GrammarRule> symbolLookUp = new Dictionary<string, GrammarRule>();
    [SerializeField] private List<string> connectedSymbols = new List<string>();
    

    [HideInInspector] public bool init = false;
    
    private void Awake()
    {
        SetInstance();
        Debug.Log("Instance set");
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        SetInstance();

        BuildDictionary();
    }

    private void SetInstance()
    {
        if (!instance)
        {
            instance = this;
            Debug.Log("Instance set");
        }
    }

    private void BuildDictionary()
    {
        Debug.Log("Building dictionary");
        symbolLookUp.Clear();
        connectedSymbols.Clear();
        
        int index = 0;
        if (Lookup.Length > 0)
        {
            foreach (GrammarRule grammarRule in Lookup)
            {
                symbolLookUp.Add(grammarRule.GrammarID, grammarRule);
                connectedSymbols.Add(grammarRule.GrammarID);
                index++;
            }

            Debug.Log("Added " + index + " entries to dictionary");
            init = true;
        }
        else
        {
            Debug.LogWarning("Symbol Lookup has no values to lookup");
        }
    }
}
