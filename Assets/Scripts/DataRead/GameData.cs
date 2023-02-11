using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

    private Transform player_last_checkpoint;
    public Transform Player_last_checkpoint
    {
        get { return player_last_checkpoint; }
        set { player_last_checkpoint = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
