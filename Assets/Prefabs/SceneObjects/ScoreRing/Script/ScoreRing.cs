using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRing : MonoBehaviour
{
    private Vector3 init_pos;
    public Material triggered_material;

    // Start is called before the first frame update
    void Start()
    {
        init_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = init_pos + new Vector3(0, Mathf.Sin(Time.time * 5) * 0.1f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<MeshRenderer>().material = triggered_material;
    }
}
