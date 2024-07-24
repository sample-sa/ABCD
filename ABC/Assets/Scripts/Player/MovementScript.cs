using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovementScript : MonoBehaviour
{
    [Header("Player settings")]
    public float maxGroundSpeed;
    public float maxAirSpeed;
    public float maxAccel;
    public float maxAirAccel;
    public float jumpPower;
    public float friction;
    public float gravity;

    [Space(20)]

    [Header("References")]
    public GameObject cam_;
    private GameObject player;
    private Rigidbody rb;
    public TextMeshProUGUI text;

    [Header("Variables")]
    float xAxis;
    float zAxis;
    float xMoveAxis;
    float zMoveAxis;

    float xMouse;
    float yMouse;

    private float currSpeed;
    private float addSpeed;

    bool isGrounded;
    bool canJump;

    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
        rb = gameObject.GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -gravity, 0);
        player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Debugging();
        
        Inputs();

        CameraMovements();
    }

    void FixedUpdate()
    {
    
        if(isGrounded)
        {
            rb.velocity = GroundVel(GetWishDir(player.transform,xAxis,zAxis), rb.velocity);
        }
        else
        {
            rb.velocity = AirVel(GetWishDir(player.transform, xAxis, zAxis), rb.velocity);
        }
    }

    void Inputs()
    {
        //Movement inputs
        xAxis = Input.GetAxis("Horizontal");
        zAxis = Input.GetAxis("Vertical");

        //Camera Inputs
        xMouse += Input.GetAxis("Mouse X");

        yMouse += Input.GetAxis("Mouse Y");
        yMouse = Mathf.Clamp(yMouse, -90, 90);


        if(Input.GetButton("Jump") && isGrounded && canJump)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
        canJump = false;
        Invoke(nameof(resetJump),0.2f);
    }

    void resetJump()
    {
        canJump = true;
    }

    void CameraMovements()
    {
        cam_.transform.rotation = Quaternion.Euler(-yMouse, xMouse, 0);

        player.transform.rotation = Quaternion.Euler(0, xMouse, 0);
    }

    Vector3 GroundVel(Vector3 wishDir, Vector3 vel)
    {
        vel = Friction(vel, Time.deltaTime);

        currSpeed = Vector3.Dot(vel, wishDir);

        addSpeed = clip(maxGroundSpeed - currSpeed, 0, maxAccel);

        return vel + addSpeed * wishDir;
    }

    Vector3 AirVel(Vector3 wishDir, Vector3 vel)
    {
        currSpeed = Vector3.Dot(vel, wishDir);

        addSpeed = clip(maxAirSpeed - currSpeed, 0, maxAirAccel);

        return vel + addSpeed * wishDir;
    }


    Vector3 GetWishDir(Transform _player, float xValue, float zValue)
    {
        Vector3 wishDir = (_player.forward * zValue) + (_player.right * xValue);

        Debug.DrawRay(player.transform.position, wishDir.normalized, Color.red, 1f);

        return wishDir.normalized;
    }

    Vector3 Friction(Vector3 vel, float frameTime)
    {
        //FIX FRICTION
        Vector2 flatFriction = new Vector2(vel.x, vel.z);

        flatFriction = flatFriction * Mathf.Clamp(friction,0 , 1) ;

        vel = new Vector3 (flatFriction.x, vel.y, flatFriction.y);

        return vel;
    }

    float clip(float addSpeed, float minAccel ,float maxAccel)
    {
        addSpeed = Mathf.Clamp(addSpeed, minAccel, maxAccel);

        return addSpeed;
    }



    void OnCollisionStay(Collision other)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }

    void Debugging()
    {
       

        Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //Debug.Log(flatvel.magnitude);
        
        Vector3 velocityVector = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        //Debug.DrawRay(player.transform.position, velocityVector, Color.green, 2f);

        //text.text ="vel: " + Mathf.Round(rb.velocity.magnitude);
    }
}