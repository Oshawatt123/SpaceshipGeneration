using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class RootPart : MonoBehaviour
{
    public bool MirrorPart;
    public bool NullSymbol;
    [SerializeField] private AttachmentPoint[] attachmentPoints;

    private void Start()
    {
        
    }

    public void Grow(bool slow = false)
    {
        attachmentPoints = gameObject.GetComponentsInChildren<AttachmentPoint>();

        if (attachmentPoints.Length <= 0) return;
        
        foreach (AttachmentPoint attachmentPoint in attachmentPoints)
        {
            if (slow)
                Thread.Sleep(100);

            attachmentPoint.SpawnAttachment(slow);
        }
    }

}
