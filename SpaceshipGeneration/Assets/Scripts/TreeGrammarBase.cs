using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrammarBase : MonoBehaviour
{

    [SerializeField] public Tree grammarTree;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tree GetTree()
    {
        return grammarTree;
    }
}
