using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryMovement : MonoBehaviour
{
    public float speed;

    private Rigidbody rb;
    private Camera cm;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cm = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = cm.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cm.transform.position.y));
        transform.LookAt(mousePos + Vector3.up * transform.position.y);
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * speed;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
