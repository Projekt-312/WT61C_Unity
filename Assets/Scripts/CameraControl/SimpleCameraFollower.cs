using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraFollower : MonoBehaviour
{

    public Transform target;

    private Vector3 delta_pos;

    // Start is called before the first frame update
    void Start()
    {
        delta_pos = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(target.position - delta_pos, transform.position, Time.deltaTime);
    }
}
