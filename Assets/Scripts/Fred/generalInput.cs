// Copyright (c) 2014 Augie R. Maddox, Guavaman Enterprises. All rights reserved.


using UnityEngine;
using System.Collections;
using Rewired;

[AddComponentMenu("cenas")]
public class generalInput : MonoBehaviour
{

    public int playerId = 0; // The Rewired player id of this character

    private Player player; // The Rewired Player
    // BUTTONS PRESSED
    private bool anyButtonDown;
    private bool fireDown;
	private bool changeWeaponDown;
	private bool dashDown;
	private bool bottomButtonDown;
	private bool leftButtonDown;
	private bool topButtonDown;
	private bool rightButtonDown;
	private bool parryDown;
    private bool UpActionDown;
    private bool RightActionDown;
    private bool BottomActionDown;
    private bool LeftActionDown;



    // BUTTONS DOWN
    private bool anyButton;
    private bool fire;
	private bool changeWeapon;


    // BUTTONS UP
    private bool anyButtonUp;
    private bool fireUp;
	private bool changeWeaponUp;
    private bool RightActionUp;


    private Vector2 moveVector;
	private Vector2 aimVector;


    [System.NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
    private bool initialized;

    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
        initialized = true;
    }
    public void Update()
    {
        /*	bool worked=GetInput();
            Debug.Log (worked+" "+start+" "+player.descriptiveName+" "+moveVector);*/
    }
	public int getPlayerNr(){
		return playerId+1;
	}

    public bool GetInput()
    {
        if (!ReInput.isReady) return false; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (!initialized) Initialize(); // Reinitialize after a recompile in the editor

     
		fireDown = player.GetButtonDown("Fire");
		fire = player.GetButton("Fire");
		fireUp = player.GetButtonUp("Fire");
        UpActionDown = player.GetButtonDown("UpAction");
        RightActionDown = player.GetButtonDown("RightAction");
        RightActionUp = player.GetButtonDown("RightActionUp");
        BottomActionDown = player.GetButtonDown("BottomAction");
        LeftActionDown = player.GetButtonDown("LeftAction");

        changeWeaponDown = player.GetButtonDown("Change Grenade");
		dashDown=player.GetButtonDown("Dash");
		parryDown=player.GetButtonDown("Parry");
		bottomButtonDown=player.GetButtonDown("Grenade0");
		rightButtonDown=player.GetButtonDown("Grenade1");
		topButtonDown=player.GetButtonDown("Grenade2");
		leftButtonDown=player.GetButtonDown("Grenade3");



		//changeWeapon = player.GetButton("changeWeapon");
	//	changeWeaponUp = player.GetButtonUp("changeWeaponUp");
        moveVector.x = player.GetAxis("Move Horizontal");
        moveVector.y = player.GetAxis("Move Vertical");
		aimVector.x = player.GetAxis("Aim Horizontal");
		aimVector.y = player.GetAxis("Aim Vertical");
		anyButton = player.GetAnyButton();
        anyButtonDown = player.GetAnyButtonDown();
		anyButtonUp = player.GetAnyButtonUp();
        return true;
    }
 
    public bool getAnyButtonDown()
    {
        return anyButtonDown;
    }
    public bool getFireDown()
    {
        return fireDown;
    }
    public bool getUpActionDown()
    {
        return UpActionDown;
    }
    public bool getRightActionDown()
    {
        return RightActionDown;
    }
    public bool getBottomActionDown()
    {
        return BottomActionDown;
    }
    public bool getLeftActionDown()
    {
        return LeftActionDown;
    }

    public bool getChangeWeaponDown(){
		return changeWeaponDown;
	}
	public bool getDashDown(){
		return dashDown;
	}
	public bool getParryDown(){
		return parryDown;
	}
    public bool getAnyButton()
    {
        return anyButton;
    }


    public bool getFire()
    {
        return fire;
    }

	/*public bool getChangeWeapon(){
		return changeWeapon;
	}*/
	public int getChangeGrenade(){
		if (bottomButtonDown)
			return 0;
		if (rightButtonDown)
			return 1;
		if (topButtonDown)
			return 2;
		if (leftButtonDown)
			return 3;
		return -1;
	}

   
    public bool getAnyButtonUp()
    {
        return anyButtonUp;
    }
    public bool getFireUp()
    {
        return fireUp;
    }
    public bool getRightActionUp()
    {
        return RightActionUp;
    }
	/*public bool getChangeWeaponUp(){
		return changeWeaponUp;
	}*/

    public float getHorizontalInput()
    {
        return moveVector.x;
    }

    public float getVerticalInput()
    {
        return moveVector.y;
    }

    public Vector2 getMovementInput()
    {
        return moveVector.normalized;
    }
	public Vector2 getAimInput()
	{
		return aimVector.normalized;
	}

}



