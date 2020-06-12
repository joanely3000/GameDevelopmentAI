using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject mainCamera;
    public List<GameObject> cameras = new List<GameObject>();
    private int activeCamera = 0;
    private GameObject lastPlayer;
    // Start is called before the first frame update
    void Start()
    {
        //cameras.Add(mainCamera);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length == 1)
        {
            if (lastPlayer == null)
            {
                lastPlayer = GameObject.FindGameObjectWithTag("Player");
                SwitchCamera(1);
                GetComponent<CompetitorManager>().winnerText.text = lastPlayer.name + " wins!";
            }
            cameras[1].transform.position = new Vector3(lastPlayer.transform.position.x, cameras[1].transform.position.y, lastPlayer.transform.position.z);

        }
        
    }

    private void SwitchCamera(int index)
    {
        if (activeCamera != index)
        {
            cameras[activeCamera].GetComponent<AudioListener>().enabled = false;
            cameras[activeCamera].SetActive(false);

            cameras[index].SetActive(true);
            cameras[index].GetComponent<AudioListener>().enabled = true;

            activeCamera = index;
        }
    }

    public void AddCamera(GameObject cam)
    {
        cameras.Add(cam);
    }
}
