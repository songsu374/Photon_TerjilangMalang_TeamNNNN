using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHeightWeight : MonoBehaviour
{
    BoxCollider col;
    Renderer render;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
        render = GetComponent<Renderer>();

        print(col.bounds.size);
        print(render.bounds.size);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
