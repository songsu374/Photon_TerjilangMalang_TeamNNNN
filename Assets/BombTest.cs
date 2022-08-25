using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTest : MonoBehaviour
{
    public GameObject explosionPrefeb;
    public LayerMask levelMask;

    public int explong = 2;
    bool exploded = false;

    public Transform bombPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Explode()
    {
        Instantiate(explosionPrefeb, bombPos.position, Quaternion.identity);

        StartCoroutine(CreateExplosion(Vector3.forward));
        StartCoroutine(CreateExplosion(Vector3.right));
        StartCoroutine(CreateExplosion(Vector3.left));
        StartCoroutine(CreateExplosion(Vector3.back));

        GetComponent<MeshRenderer>().enabled = false;
        exploded = true;
        Destroy(gameObject, .3f);
    }

    IEnumerator CreateExplosion(Vector3 direction)
    {
        for (int i = 1; i < explong; i++)
        {
            RaycastHit hit;

            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit, i, levelMask);

            if (!hit.collider)
            {
                Instantiate(explosionPrefeb, transform.position + ((i * 0.7f) * direction), explosionPrefeb.transform.rotation);

            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Explosion"))
        {
            CancelInvoke("Explode");
            Explode();
        }
        if (exploded && other.gameObject.CompareTag("box"))
        {
            print("start");
        }
    }

    // Update is called once per frame
    void Update()
    {
            Invoke("Explode", 0.5f);
    }
}
