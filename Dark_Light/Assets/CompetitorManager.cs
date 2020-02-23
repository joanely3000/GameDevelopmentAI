using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetitorManager : MonoBehaviour
{

    public GameObject PlayerModel = null;
    public Transform[] SpawnPoints = null;

    private List<PlayerController> playerList = new List<PlayerController>();
    // Start is called before the first frame update
    void Start()
    {
        BaseAI[] aiArray = new BaseAI[] {
            new GwendalAI(),
            new GwendalAI(),
            new GwendalAI(),
            new GwendalAI()
        };

        for (int i = 0; i < 4; i++)
        {
            GameObject player = Instantiate(PlayerModel, SpawnPoints[i].position, SpawnPoints[i].rotation);
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.SetAI(aiArray[i]);
            playerList.Add(playerController);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var player in playerList)
            {
                player.StartBattle();
            }
        }
    }
}
