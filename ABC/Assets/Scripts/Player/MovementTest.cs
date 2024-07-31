using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    [Header("settings")]
    public float maxSpeed;
    public float maxAccel;
    public float grip;


    float currentSpeed;
    GameObject player;

    Rigidbody rbPlayer;

    Vector3 groundNormal;
    float xMoveAxis;
    float zMoveAxis;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        rbPlayer = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    void FixedUpdate()
    {
        ApplyVelocity();
    }

    void Inputs()
    {
        //Movement inputs
        xMoveAxis = Input.GetAxis("Horizontal");
        zMoveAxis = Input.GetAxis("Vertical");
    }

    void ApplyVelocity()
    {
        Vector3 wishDir = GetWishDir(player.transform);

        float currentSpeed = rbPlayer.velocity.magnitude;
        float acceleration = Mathf.Clamp(maxSpeed - currentSpeed, 0, maxAccel);

        Debug.Log(wishDir + "        " + rbPlayer.velocity + "             " + (rbPlayer.velocity.normalized - wishDir));

        rbPlayer.velocity +=  wishDir * acceleration;
    }
    Vector3 GetWishDir(Transform player)
    {
        Vector3 wishDir = (player.forward * zMoveAxis) + (player.right * xMoveAxis);
        
        Debug.DrawRay(player.transform.position, wishDir.normalized, Color.red, 1f);

        return wishDir.normalized;
    }
}
