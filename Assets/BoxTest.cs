using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTest : MonoBehaviour
{
    public GameObject smoke;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            this.gameObject.SetActive(false);
            Instantiate(smoke, transform.position += new Vector3(0, 1f, 0), smoke.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
