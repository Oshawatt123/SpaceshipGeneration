using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentPoint : MonoBehaviour
{
    [SerializeField] private List<GameObject> attachments;

    public void SpawnAttachment()
    {
        // pick an attachment
        int rando = Random.Range(0, attachments.Count);

        // spawn new part
        GameObject newPart = Instantiate(attachments[rando], transform.position, transform.rotation);

        // if new part is an end symbol, do nothing
        if (newPart.GetComponent<RootPart>().NullSymbol) return;

        // grow off of the new part
        newPart.GetComponent<RootPart>().Grow();
    }
}