using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipBit : MonoBehaviour
{

    private ShipLink[] linkPoints;


    private bool changeSize = false;


    // Start is called before the first frame update

    private void Awake()
    {
        linkPoints = gameObject.GetComponentsInChildren<ShipLink>();
        if (changeSize)
        {
            float scale = Random.Range(transform.localScale.magnitude * 0.8f, transform.localScale.magnitude * 1.2f);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    void Start()
    {
        
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
        ShipLink randomLink = linkPoints[Random.Range(0, linkPoints.Length)];
        Debug.Log(randomLink.transform.name + ";" + randomLink.transform.position);

        return randomLink.transform;
    }
}
