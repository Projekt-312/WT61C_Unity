using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    private GameData game_data;
    public Material check_point_triggered;

    // Start is called before the first frame update
    void Start()
    {
        game_data = GameObject.Find("GameData").GetComponent<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        game_data.Player_last_checkpoint = transform;
        GetComponent<MeshRenderer>().material = check_point_triggered;
    }
}
