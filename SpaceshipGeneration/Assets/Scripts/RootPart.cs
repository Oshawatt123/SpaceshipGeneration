using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RootPart : MonoBehaviour
{
    public bool NullSymbol;
    [SerializeField] private AttachmentPoint[] attachmentPoints;

    public void Grow()
    {
        attachmentPoints = gameObject.GetComponentsInChildren<AttachmentPoint>();

        if (attachmentPoints.Length <= 0) return;
        
        foreach (AttachmentPoint attachmentPoint in attachmentPoints)
        {
            attachmentPoint.SpawnAttachment();
        }
    }
    
}
