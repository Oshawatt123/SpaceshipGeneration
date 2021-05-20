using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class TreeGrammarV2 : TreeGrammarBase
{
    
    // GLOBAL CONSTANTS
    private const float COCKPITMAX_X = 2f;
    private const float COCKPITMAX_Y = 2f;
    
    
    [Serializable]
    private class BoundingBox
    {
        public BoundingBox()
        {
            location = Vector3.zero;
            size = Vector3.zero;
            init = true;
        }
        
        public Vector3 location;
        public Vector3 size;
        public bool init = false;

        public BoundingBox GetFrontFaceBB()
        {
            BoundingBox FrontFaceBB = new BoundingBox();
            // get size
            FrontFaceBB.size = new Vector3(size.x, size.y, 1f);
            
            // get location
            FrontFaceBB.location = new Vector3(location.x, location.y, location.z + (size.z/2) + (FrontFaceBB.size.z/2));

            return FrontFaceBB;
        }
        
        public BoundingBox GetBackFaceBB()
        {
            BoundingBox BackFaceBB = new BoundingBox();
            // get size
            BackFaceBB.size = new Vector3(size.x, size.y, 1f);
            
            // get location
            BackFaceBB.location = new Vector3(location.x, location.y, location.z - (size.z/2) - (BackFaceBB.size.z/2));

            return BackFaceBB;
        }
    }

    [SerializeField] private BoundingBox rootBB = new BoundingBox();
    [SerializeField] private BoundingBox hullBB = new BoundingBox();
    private BoundingBox engineBB = new BoundingBox();
    [SerializeField] private BoundingBox cockpitBB = new BoundingBox();

    // root BB variables
    [SerializeField] private Vector3 shipSize;
    
    // hull BB variables
    [SerializeField] [Range(0.5f, 0.8f)] private float hullSize;
    
    
    [SerializeField] [Range(0.2f, 1.0f)] private float engineSize;
    [SerializeField] [Range(0.2f, 2.0f)] private float cockpitSize;
    
    
    // Objects
    [SerializeField] private GrammarRule hullRule;


    public bool showSymmetryBlocks = true;
    [SerializeField] private Material mat;



    // Debug
    private Color[] colors = {Color.black, Color.black, Color.yellow, Color.magenta, Color.cyan, Color.green, Color.blue, Color.red, Color.white};
    
    // Start is called before the first frame update
    void Start()
    {
        rootBB = new BoundingBox();
        hullBB = new BoundingBox();
        engineBB = new BoundingBox();
        cockpitBB = new BoundingBox();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Cleanup()
    {
        grammarTree.Clean();
    }

    public void BuildShip()
    {

        try
        {
            if (!SymbolLookup.instance.init)
            {
                SymbolLookup.instance.Init();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Symbol lookup not initialized. Please initialize");
            throw;
        }

        grammarTree.Clean();
        
        // build root BB
        Node rootNode = new Node("root");
        rootNode.bgColor = Color.blue;
        grammarTree = new Tree(rootNode);
        rootBB = new BoundingBox();
        rootBB.location = transform.position;
        rootBB.size = new Vector3(shipSize.x, shipSize.y, shipSize.z);

        // build hull BB
        Node hullNode = new Node("hull");
        hullNode.bgColor = Color.gray;
        rootNode.AddNode(hullNode, false);
        hullBB = new BoundingBox();
        hullBB.size = rootBB.size * hullSize;
        hullBB.location = rootBB.location + getComfyOffsetInCuboid(hullBB.size, rootBB.size);

        var watch = System.Diagnostics.Stopwatch.StartNew();
        BuildHull(hullNode);
        AddSymmetry(hullNode, null);
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        int treeSize = grammarTree.CalculateSize();
        Debug.LogWarning("##### " + elapsedMs + " / " + treeSize + " => " + elapsedMs/(float)treeSize + " #####");

        // build engine BB
        Node engineNode = new Node("engine");
        engineNode.bgColor = Color.red;
        rootNode.AddNode(engineNode, false);
        engineBB = hullBB.GetBackFaceBB();
        engineBB.size.x = engineBB.size.x * engineSize;
        engineBB.size.y = engineBB.size.y * engineSize;
        
        

        // build cockpit BB
        Node cockpitNode = new Node("cockpit");
        cockpitNode.bgColor = Color.yellow;
        rootNode.AddNode(cockpitNode, false);
        cockpitBB = hullBB.GetFrontFaceBB();
        cockpitBB.size.x = Mathf.Min(cockpitBB.size.x, COCKPITMAX_X) * cockpitSize;
        cockpitBB.size.y = Mathf.Min(cockpitBB.size.y, COCKPITMAX_Y) * cockpitSize;

        
        Debug.Log(grammarTree);

        SceneView.RepaintAll();
    }

    private void BuildHull(Node hullNode)
    {
        Debug.Log("Building hull");
        
        int z = 0;
        Node newNode = new Node();

        // building the spine
        do
        {
            //Debug.Log("newNode z " + newNode.spawnedObject.transform.position.z + " < " + hullBB.location.z + hullBB.size.z);
            
            newNode = new Node(hullRule, 0);

            Vector3 spawnPosition = new Vector3(hullBB.location.x, hullBB.location.y,
                (hullBB.location.z - (hullBB.size.z / 2)) + z);
            
            
            newNode.Show(spawnPosition, transform.rotation);

            hullNode.AddNode(newNode, false);
            z++;
        } while (newNode.position.z < hullBB.location.z + (hullBB.size.z/2));

        Debug.Log("######## Starting recursive hull build ########");
        foreach (Node node in hullNode.nodes)
        {
            RecurseBuildNode(node, 0);
        }
    }

    // Recursive
    private void AddSymmetry(Node refNode, Node _node)
    {
        Debug.Log("Starting some symmetry");
        // check node is not null
        // null is passed in first call for the refernce node
        if (_node != null)
        {
            _node.CreateSymmetry(refNode.nodes[0], mat, showSymmetryBlocks);
        }
        else
        {
            _node = refNode;
        }

        foreach (Node node in _node.nodes)
        {
            AddSymmetry(refNode, node);
        }
        
        _node.UpdateNodes();
    }
    
    // Recursive
    private void RecurseBuildNode(Node node, int level)
    {
        
        //Debug.Log("Level " + level.ToString() + ": " + node.ToString());
        
        node.Show();
        if(!node.killLine)
            node.Grow();

        if (!node.hasNode(0))
            return;

        foreach (Node currentNode in node.nodes)
        {
            RecurseBuildNode(currentNode, level+1);
        }
    }


    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, shipSize);
        
        
        if (rootBB.init)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(rootBB.location, rootBB.size);
        }

        if (hullBB.init)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawWireCube(hullBB.location, hullBB.size);
        }

        if (engineBB.init)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(engineBB.location, engineBB.size);
        }
        
        if (cockpitBB.init)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(cockpitBB.location, cockpitBB.size);
        }

        DrawTransformGizmos(grammarTree.root, 0);
    }

    private void DrawTransformGizmos(Node node, int level)
    {
        if (level < colors.Length)
        {
            Gizmos.color = colors[level];
        }

        Gizmos.DrawLine(node.position, node.position + (node.rotation*Vector3.right));
        Gizmos.DrawWireSphere(node.position + (node.rotation * Vector3.right), 0.1f);

        if (!node.hasNode(0))
            return;

        foreach (Node currentNode in node.nodes)
        {
            DrawTransformGizmos(currentNode, level+1);
        }
    }
    


    /// <summary>
    /// gets a position within a space so that cubeToFit will not collide with cubeToFitInto
    /// but instead sit comfortable inside 
    /// </summary>
    /// <param name="cubeToFit"></param>
    /// <param name="cubeToFitInto"></param>
    /// <returns>Vector3 position for cubeToFit</returns>
    private Vector3 getComfyOffsetInCuboid(Vector3 cubeToFit, Vector3 cubeToFitInto)
    {
        Vector3 position = Vector3.zero;
        
        // for each axis:
        // get wiggle room
        // get random number in wiggle room
        
        // x axis
        float xWiggle = (cubeToFitInto.x - cubeToFit.x) / 2;
        float xOffset = UnityEngine.Random.Range(-xWiggle, xWiggle);
        
        // y axis
        float yWiggle = (cubeToFitInto.y - cubeToFit.y) / 2;
        float yOffset = UnityEngine.Random.Range(-yWiggle, yWiggle);
        
        // x axis
        float zWiggle = (cubeToFitInto.z - cubeToFit.z) / 2;
        float zOffset = UnityEngine.Random.Range(-zWiggle, zWiggle);

        position.x = xOffset;
        position.y = yOffset;
        position.z = zOffset;
        
        return position;
    }
}
