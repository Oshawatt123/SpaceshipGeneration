using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class ShipBit : MonoBehaviour
{

    private ShipLink[] linkPoints;


    private bool changeSize = false;
    private bool init = false;


    // Start is called before the first frame update

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        Init();
    }

    private void Init()
    {
        if (!init)
        {
            linkPoints = gameObject.GetComponentsInChildren<ShipLink>();
            if (changeSize)
            {
                float scale = Random.Range(transform.localScale.magnitude * 0.8f,
                    transform.localScale.magnitude * 1.2f);
                transform.localScale = new Vector3(scale, scale, scale);
            }

            init = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Link(Transform orientation)
    {
        Debug.Log("Linking " + gameObject.name + " at " + transform.position + ";" + transform.rotation + " to " + orientation.name + " at " + orientation.position + ";" + orientation.rotation);
        
        transform.position = orientation.position;
        transform.rotation = orientation.rotation;
    }

    public Transform GetLinkPoint()
    {
        if (!init)
            Init();
        
        ShipLink randomLink = linkPoints[Random.Range(0, linkPoints.Length)];
        Debug.Log(randomLink.transform.name + ";" + randomLink.transform.position);

        return randomLink.transform;
    }
}
