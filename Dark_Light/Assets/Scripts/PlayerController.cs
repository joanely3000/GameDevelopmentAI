using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BaseAI ai = null;
    private float Yposition = 0.25f;

    private float walkSpeed = 1.0f;
    private float mapSize = 25.0f;
    private float RotationSpeed = 180.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetAI(BaseAI _ai)
    {
        ai = _ai;
        ai.Player = this;
    }

    public void StartBattle()
    {
        StartCoroutine(ai.RunAI());
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.tag);
    }

    public IEnumerator __Ahead(float distance)
    {
        int numFrames = (int)(distance / (walkSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++)
        {
            transform.Translate(new Vector3(0f, Yposition, walkSpeed * Time.fixedDeltaTime), Space.Self);
            Vector3 clampedPosition = Vector3.Max(Vector3.Min(transform.position, new Vector3(mapSize, Yposition, mapSize)), new Vector3(-mapSize, Yposition, -mapSize));
            transform.position = clampedPosition;
 
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator __Back(float distance)
    {
        int numFrames = (int)(distance / (walkSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++)
        {
            transform.Translate(new Vector3(0f, Yposition, -walkSpeed * Time.fixedDeltaTime), Space.Self);
            Vector3 clampedPosition = Vector3.Max(Vector3.Min(transform.position, new Vector3(mapSize, Yposition, mapSize)), new Vector3(-mapSize, Yposition, -mapSize));
            transform.position = clampedPosition;

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator __Left(float distance)
    {
        int numFrames = (int)(distance / (walkSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++)
        {
            transform.Translate(new Vector3(-walkSpeed * Time.fixedDeltaTime, Yposition, 0f), Space.Self);
            Vector3 clampedPosition = Vector3.Max(Vector3.Min(transform.position, new Vector3(mapSize, Yposition, mapSize)), new Vector3(-mapSize, Yposition, -mapSize));
            transform.position = clampedPosition;

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator __Right(float distance)
    {
        int numFrames = (int)(distance / (walkSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++)
        {
            transform.Translate(new Vector3(walkSpeed * Time.fixedDeltaTime, Yposition, 0f), Space.Self);
            Vector3 clampedPosition = Vector3.Max(Vector3.Min(transform.position, new Vector3(mapSize, Yposition, mapSize)), new Vector3(-mapSize, Yposition, -mapSize));
            transform.position = clampedPosition;

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator __TurnLeft(float angle)
    {
        int numFrames = (int)(angle / (RotationSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++)
        {
            transform.Rotate(0f, -RotationSpeed * Time.fixedDeltaTime, 0f);

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator __TurnRight(float angle)
    {
        int numFrames = (int)(angle / (RotationSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++)
        {
            transform.Rotate(0f, RotationSpeed * Time.fixedDeltaTime, 0f);

            yield return new WaitForFixedUpdate();
        }
    }
}
