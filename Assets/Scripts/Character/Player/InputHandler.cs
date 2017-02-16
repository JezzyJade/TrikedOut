﻿using UnityEngine;
//using XboxCtrlrInput;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour
{
    /*
     * THis class is required to be a component of all player objects. 
     * It will send messages to the player's move class, which will move the player
     * The Move class will not move the player until the Update function is called, which the Player will do
     **/
    public float turnAnimSensitivity = 0.4f;
    private GameObject weapon;
    private GameObject ulti;
    private GameObject minimap;
    private NewMove move;
    private string prefix;
    private bool useController = true;
    private Dictionary<string, KeyCode> kbc;
    private Dictionary<string, string> ucv;
    private float currentTime;
    private float delay = 0.2f;
    private Animator anim;

    public void ReceiveDefinitions(bool uc, Dictionary<string, KeyCode> kb, Dictionary<string, string> uv)
    {
        useController = uc;
        ucv = uv;
    }

    public void SetAnimator(Animator a)
    {
        anim = a;
    }

    private void Start()
    {
        minimap = GameObject.Find("MinimapCam");
        prefix = name;//player.prefix;
        ulti = GameObject.Find(prefix + "_Ulti");
        move = gameObject.GetComponent<NewMove>();
        weapon = GameObject.Find(prefix + "_Weapon");
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (move == null)
        {
        }

        bool leftPedal;
        bool rightPedal;
        float cameraFactor;
        bool fire;
        bool ceaseFire;
        bool activateUlti;
        bool openMinimap;

        leftPedal = (Input.GetAxis(prefix + "_LeftTrigger") == 1) ? true : false;
        rightPedal = (Input.GetAxis(prefix + "_RightTrigger") == 1) ? true : false;
        cameraFactor = Input.GetAxis(prefix + "_RightStickX");
        fire = Input.GetButtonDown(prefix + "_A");
        ceaseFire = Input.GetButtonUp(prefix + "_A");
        activateUlti = Input.GetButtonDown(prefix + "_Y");
        openMinimap = Input.GetButtonDown(prefix + "_B");

        move.SetFactors(leftPedal, rightPedal);
        if (fire) FireWeapon();
        else if (ceaseFire) CeaseFire();
        if (activateUlti) ActivateUlti();
        if (openMinimap) minimap.SetActive(!minimap.activeInHierarchy);

        if (anim != null)
        {
            anim.SetBool("Pedalling", move.IsPedalling());
            anim.SetBool("Turning", Mathf.Abs(Input.GetAxis(prefix + "_LeftStickX")) > turnAnimSensitivity);
            anim.SetBool("Right", Input.GetAxis(prefix + "_LeftStickX") > 0);
            anim.SetBool("Taunting", Input.GetButtonDown(prefix + "_X"));
        }
    }

    void FireWeapon()
    {
        weapon.GetComponent<Item>().Activate();

        // Not implemented yet
        //SoundManager.instance.PlayShootSound();
    }

    void CeaseFire()
    {
        weapon.GetComponent<Item>().Deactivate();
    }

    void ActivateUlti()
    {
        //ulti.SetActive(true);
        ulti.GetComponent<Item>().Activate();
    }
}