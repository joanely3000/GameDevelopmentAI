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
    public GameObject playerCamera;

    private List<PlayerController> playerList = new List<PlayerController>();
    private string[] names = { "Gwendal", "Joan", "Jesus", "Random" };
    private CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = GetComponent<CameraController>();

        BaseAI[] aiArray = new BaseAI[] {
            new GwendalAI(),
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
            playerUI.transform.SetParent(HealthUIPlace.transform);
            playerUI.GetComponentInChildren<ProgressBar>().setHealthSystem(player.GetComponent<HealthSystem>());
            playerUI.GetComponentInChildren<ProgressBar>().Name.text = names[i];

            //Set Players cameras
            GameObject cam = Instantiate(playerCamera, player.transform.position, player.transform.rotation);

            cameraController.AddCamera(cam);

            FollowPlayer fp = cam.GetComponent<FollowPlayer>();
            if(fp != null){ 
                fp.SetPlayer(player); }

            cam.GetComponent<AudioListener>().enabled = false;
            cam.SetActive(false);

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
