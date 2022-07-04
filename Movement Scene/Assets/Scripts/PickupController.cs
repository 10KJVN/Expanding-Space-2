using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public shooting gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, WeaponHolder, CameraHolder;

    [Header("Pick-up")]
    public float pickupRange;

    [Header("Drop")]
    public float dropForwardForce;
    public float dropUpwardForce;

    [Header("Keybinds")]
    [SerializeField] KeyCode equipKey = KeyCode.E;
    [SerializeField] KeyCode dropKey = KeyCode.Z;

    [Header("On/Off State")]
    public bool equipped;
    public static bool slotFull;

    public float posX; 
    public float posY;
    public float posZ;

    private void Start()
    {
        //Setup
        if (!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    private void Update()
    {
        //Checks if player is in range and "E" is pressed
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickupRange && Input.GetKey(equipKey) && !slotFull) pickUp();

        //Drop if equipped and "Z" is pressed
        if (equipped && Input.GetKey(dropKey)) Drop();
    }
    private void pickUp()
    {
        equipped = true;
        slotFull = true;

        //Make a weapon the child of the camera and move it to a default position
        transform.SetParent(WeaponHolder);
        // transform.localPosition = (Vector3.zero); //Vector3(1, -0.75, 1);
        transform.localRotation = Quaternion.Euler(-90, 0, -90); //.(Vector3.zero);
        //transform.localScale = Vector3.one;

        //Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        //Enables GunScript
        gunScript.enabled = true;

    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        //Set parent to null
        transform.SetParent(null);

        //Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = false;
        coll.isTrigger = false;

        //Gun carries momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        //AddForce
        rb.AddForce(CameraHolder.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(CameraHolder.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random)*10);

        //Disables GunScript
        gunScript.enabled = false;


    }
    
}
