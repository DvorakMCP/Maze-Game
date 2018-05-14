using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    float MOVE_SPEED = 6f;

    CharacterController myCharacter;

    void Start() {
        myCharacter = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update () {
        Movement();
	}

    void Movement() {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDir.Normalize();
        moveDir *= MOVE_SPEED;

        myCharacter.Move(moveDir * Time.deltaTime);
    }
}
