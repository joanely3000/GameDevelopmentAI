using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetitorManager : MonoBehaviour
{
    [Header("Player Controller")]
    public GameObject PlayerModel = null;

    [Header("Game Properties")]
    public int numPlayers = 4;
    public float mapSize = 25f;

    [Header("Spawn Points")]
    public Transform[] SpawnPoints = null;
    public Material[] Materials;

    public Transform[] UIPosition = null;
    public GameObject PlayerUI;

    private List<PlayerController> playerList = new List<PlayerController>();
    // Start is called before the first frame update
    void Start()
    {
        BaseAI[] aiArray = new BaseAI[] {
            new GwendalAI(),
            new JoanAI(),
            new JoanAI(),
            new JoanAI()
        };

        for (int i = 0; i < numPlayers; i++)
        {
            GameObject player = Instantiate(PlayerModel, SpawnPoints[i].position, SpawnPoints[i].rotation);
            Debug.Log(i);
            GameObject playerUI = Instantiate(PlayerUI);
            playerUI.GetComponentInChildren<ProgressBar>().setHealthSystem(player.GetComponent<HealthSystem>());
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.viewMeshFilter.GetComponent<MeshRenderer>().material = Materials[i];
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
