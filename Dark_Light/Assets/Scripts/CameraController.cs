using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject mainCamera;
    public List<GameObject> cameras = new List<GameObject>();

    private int activeCamera = 0;

    // Start is called before the first frame update
    void Start()
    {
        //cameras.Add(mainCamera);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchCamera(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(cameras.Count > 1)
            {
                SwitchCamera(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (cameras.Count > 2)
            {
                SwitchCamera(2);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (cameras.Count > 3)
            {
                SwitchCamera(3);
            }
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
