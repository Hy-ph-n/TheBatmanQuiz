using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    // Determines camera movement speed
    public float speedFactor = 0.1f;

    // Defines the anchor that the camera travels to
    public Transform anchorToMoveTo;

    // Transition audio source
    public AudioSource slide;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Moves the camera
        transform.position = Vector3.Lerp(transform.position, anchorToMoveTo.position, speedFactor);

        // Rotates the camera
        transform.rotation = Quaternion.Slerp(transform.rotation, anchorToMoveTo.rotation, speedFactor);
    }

    // A method that receives values the camera needs to move to
    public void setAnchor(Transform anchor)
    {
        // Plays transitional audio
        slide.Play();

        // Adjusts the anchor that the camera is moving toard
        anchorToMoveTo = anchor;
    }
}
