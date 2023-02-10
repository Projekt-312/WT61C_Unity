using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{

    private GameData game_data;

    // Start is called before the first frame update
    void Start()
    {
        game_data = GameObject.Find("GameData").GetComponent<GameData>();

        game_data.Player_last_checkpoint = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
