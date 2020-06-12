using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject mainCamera;
    public List<GameObject> cameras = new List<GameObject>();
    public CompetitorManager competitorManager;
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
