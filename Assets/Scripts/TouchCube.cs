using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCube : MonoBehaviour
{



    private void OnTriggerEnter(Collider other)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material.color = Color.green;
        }
    }
    // Start is called before the first frame update



}
