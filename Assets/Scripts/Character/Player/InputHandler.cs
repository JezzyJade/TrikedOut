﻿using UnityEngine;
using XboxCtrlrInput;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour
{
    /*
     * THis class is required to be a component of all player objects. 
     * It will send messages to the player's move class, which will move the player
     * The Move class will not move the player until the Update function is called, which the Player will do
     **/
    private Player player;
    private Move move;
    private string prefix;
    private XboxController playerNumber;
    private bool useController;
    private Dictionary<string, XboxAxis> xba;
    private Dictionary<string, XboxButton> xbb;
    private Dictionary<string, KeyCode> kbc;

	// Use this for initialization
	void Start ()
    {
        player = gameObject.GetComponent<Player>();
        move = gameObject.GetComponent<Move>();
        prefix = player.prefix;

        switch (prefix)
        {
            case "P1":
                playerNumber = XboxController.First;
                break;
            case "P2":
                playerNumber = XboxController.Second;
                break;
            case "P3":
                playerNumber = XboxController.Third;
                break;
            case "P4":
                playerNumber = XboxController.Fourth;
                break;
        }
    }

    public void ReceiveDefinitions(bool uc, Dictionary<string, XboxAxis> xa, Dictionary<string, XboxButton> xb, Dictionary<string, KeyCode> kb)
    {
        useController = uc;
        xba = xa;
        xbb = xb;
        kbc = kb;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!StateManager.instance.isPaused)
        {
            int leftPedal;
            int rightPedal;
            float turnFactor;
            bool fire;
            bool ceaseFire;

            if (useController)
            {
                leftPedal = (XCI.GetAxis(xba["leftPedal"], playerNumber) == 1) ? 1 : 0;
                rightPedal = (XCI.GetAxis(xba["rightPedal"], playerNumber) == 1) ? 2 : 0;
                turnFactor = XCI.GetAxis(xba["steerX"], playerNumber);
                fire = XCI.GetButtonDown(xbb["activateWeapon"], playerNumber);
                ceaseFire = XCI.GetButtonUp(xbb["activateWeapon"], playerNumber);
            }
            else
            {
                leftPedal = (Input.GetKeyDown(kbc["leftPedal"]) == true) ? 1 : 0;
                rightPedal = (Input.GetKeyDown(kbc["rightPedal"]) == true) ? 2 : 0;
                turnFactor = (Input.GetKey(kbc["steerX"]) == true) ? -1 : (Input.GetKey(kbc["steerY"]) == true) ? 1 : 0;
                fire = Input.GetKeyDown(kbc["activateWeapon"]);
                ceaseFire = Input.GetKeyUp(kbc["activateWeapon"]);
            }

            //Debug.Log("Player: " + prefix + " LeftPedal: " + leftPedal + " RightPedal: " + rightPedal + " TurnFactor: " + turnFactor);

            move.SetFactors(leftPedal, rightPedal, turnFactor);
            if (fire) player.FireWeapon();
            else if (ceaseFire) player.CeaseFire();
        }
	}
}
