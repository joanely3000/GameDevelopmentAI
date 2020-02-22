using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetitorManager : MonoBehaviour
{

    public GameObject PlayerModel = null;
    public Transform[] SpawnPoints = null;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject playerModel = Instantiate(PlayerModel, SpawnPoints[i].position, SpawnPoints[i].rotation);
        //    PirateShipController pirateShipController = pirateShip.GetComponent<PirateShipController>();
         //   pirateShipController.SetAI(aiArray[i]);
          //  pirateShips.Add(pirateShipController);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
