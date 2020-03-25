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
    public GameObject HealthUIPlace;

    private List<PlayerController> playerList = new List<PlayerController>();
    private string[] names = { "Gwendal", "Joan", "Jesus", "Random" };

    // Start is called before the first frame update
    void Start()
    {
        BaseAI[] aiArray = new BaseAI[] {
            new JoanAI(),
            new JoanAI(),
            new JoanAI(),
            new JoanAI()
        };

        for (int i = 0; i < numPlayers; i++)
        {
            //Instantiate player
            GameObject player = Instantiate(PlayerModel, SpawnPoints[i].position, SpawnPoints[i].rotation);
            player.name = names[i];

            //Instantiate Player UI
            GameObject playerUI = Instantiate(PlayerUI);
            playerUI.transform.parent = HealthUIPlace.transform;
            playerUI.GetComponentInChildren<ProgressBar>().setHealthSystem(player.GetComponent<HealthSystem>());


            //Set the color for the light of the player
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.viewMeshFilter.GetComponent<MeshRenderer>().material = Materials[i];

            //Set the player AI
            playerController.SetAI(aiArray[i]);

            //Add to the player list
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
