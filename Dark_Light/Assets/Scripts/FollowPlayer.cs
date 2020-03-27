using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 offsetPosition;

    private AudioListener audioListener;

    // Start is called before the first frame update
    void Start()
    {
        audioListener = GetComponent<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offsetPosition;
        Vector3 rotation = Quaternion.AngleAxis(30, player.transform.right) * player.transform.forward;
        transform.rotation = Quaternion.LookRotation(rotation);
    }

    public void SetAudioListenerActive(bool value)
    {
        audioListener.enabled = value;
    }

    public void SetPlayer(GameObject target)
    {
        player = target;
    }
}
