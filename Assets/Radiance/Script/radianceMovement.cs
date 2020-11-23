﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radianceMovement : MonoBehaviour
{
    //public Transform player;
    public SpriteRenderer thisSR;
    public int floatCounter = 0;
    public bool floating = false;
    public bool teleport = false;
    public bool telOutRange = false;
    public bool casting = false;
    public float nextTeleportPosition = 0;
    public int nextFloatDuration = 0;
    public Transform leftMost;
    public Transform rightMost;
    public Transform upMost;
    public Transform downMost;
    public float nextLaunchIndex = 0;
    public bool launching = false;
    public bool teleportWhileLaunching = false;
    public bool phase1 = false;
    public bool phase2 = false;
    //phase1 && phase2 attacks
    public bool beamBurst = false;
    public bool swordBurst = false;
    public bool swordRain = false;
    public bool swordWall = false;
    public bool lightWall = false;
    public bool orbAttack = false;
    public int launchCounter = 0;
    //sword burst variables
    private int swordBurstAngle = 0;
    private int swordBurstDistance = 10;
    public GameObject sword;
    //beam burst variables
    public float beamBurstAngle = 0;
    private int beamBurstCount3 = 0;
    public GameObject beam;
    private int beamBurstStartIndex = 30;
    private float beamBurstUsedAngle = 0;
    //sword rain variables
    private float rainLeft = 0;
    private float rainRight = 0;
    private int count4 = 0;
    //sword wall variables
    private float wallLeft = 0;
    private float wallRight = 0;
    private float wallUp = 0;
    private float wallDown = 0;
    private bool goRight = false;
    //light wall variables
    public GameObject lightWallGO;
    //phase2 spike
    public bool spikes1 = false;
    public bool spikes2 = false;
    // Start is called before the first frame update
    void Start()
    {
        nextLaunchIndex = Random.Range(0, 100);
        beamBurstAngle = Random.Range(0, 90);
        floating = true;
        nextTeleportPosition = (leftMost.position.x+rightMost.position.x)/2;
        nextFloatDuration = Random.Range(50, 60);
        phase1 = true;
        float wallDir = Random.Range(0, 10);
        if (wallDir < 5)
        {
            goRight = true;
        }
        else
        {
            goRight = false;
        }
    }

    void FixedUpdate()
    {
        if (phase1 || phase2)
        {
            if (floating)
            {
                //launch sword burst
                if (swordBurst)
                {
                    launchCounter += 1;
                    if (launchCounter > 15 && launchCounter < 17)
                    {
                        for (int i = 0; i < 12; i++) { 
                            float radians = (Mathf.PI / 180) * swordBurstAngle;
                            float thisX = swordBurstDistance * Mathf.Sin(radians);
                            float thisY = swordBurstDistance * Mathf.Cos(radians);
                            float newX = transform.position.x + thisX;
                            float newY = transform.position.y + thisY;
                            GameObject newSword =  Instantiate(sword, new Vector3(newX, newY, 0), Quaternion.Euler(0,0,-swordBurstAngle+90));
                            sword ifBurst = newSword.GetComponent<sword>();
                            ifBurst.burstRotation = -swordBurstAngle + 90;
                            ifBurst.burst = true;
                            swordBurstAngle += 30;
                        }
                    }
                    else if (launchCounter >= 200)
                    {
                        launchCounter = 0;
                        launching = false;
                        swordBurst = false;
                        teleport = true;
                        floating = false;
                    }

                }

                //launch beam burst
                else if (beamBurst)
                {
                    launchCounter += 1;
                    if (beamBurstCount3 < 3)
                    {
                        if (launchCounter > beamBurstStartIndex && launchCounter < beamBurstStartIndex+2)
                        {
                            if (beamBurstCount3 == 0)
                            {
                                beamBurstUsedAngle = beamBurstAngle;
                            }
                            else
                            {
                                beamBurstAngle = beamBurstUsedAngle + Random.Range(10, 30);
                                beamBurstUsedAngle = beamBurstAngle;
                            }
                            for (int i = 0; i < 8; i++)
                            {
                                float newX = transform.position.x;
                                float newY = transform.position.y;
                                GameObject thisBeam = Instantiate(beam, new Vector3(newX, newY, 0), Quaternion.Euler(0, 0, beamBurstAngle));
                                beamBurstAngle += 45;
                            }
                            beamBurstStartIndex = 65;
                            beamBurstCount3 += 1;
                            launchCounter = 0;
                        }
                    }
                    else if (launchCounter >= 100)
                    {
                        beamBurst = false;
                        launching = false;
                        launchCounter = 0;
                        teleport = true;
                        floating = false;
                    }
                }

                //launch sword rain

                else if (swordRain)
                {
                    launchCounter += 1;
                    if (launchCounter > 55 && launchCounter < 57)
                    {
                        if (count4 < 4)
                        {
                            rainLeft = leftMost.position.x - 50;
                            rainRight = rightMost.position.x + 50;
                            for (float i = rainLeft; i < rainRight; i += 7)
                            {
                                float ifCreate = Random.Range(0, 10);
                                if (ifCreate < 7.5)
                                {
                                    GameObject newSword = Instantiate(sword, new Vector3(i, upMost.position.y, 0), Quaternion.Euler(0, 0, -90));
                                    Animator swordAnim = newSword.GetComponent<Animator>();
                                    Destroy(swordAnim);
                                    sword swordScript = newSword.GetComponent<sword>();
                                    swordScript.other = true;
                                }
                            }
                            launchCounter = 0;
                            count4 += 1;
                        }
                    }
                    else if (launchCounter >= 100)
                    {
                        launchCounter = 0;
                        swordRain = false;
                        launching = false;
                        teleport = true;
                        floating = false;
                        count4 = 0;
                    }
                }

                //launch sword wall

                else if (swordWall)
                {
                    launchCounter += 1;
                    if (launchCounter > 65 && launchCounter < 67)
                    {
                        if (count4 < 4)
                        {
                            Debug.Log(count4);
                            wallLeft = leftMost.position.x - 50;
                            wallRight = rightMost.position.x + 50;
                            wallUp = upMost.position.y;
                            wallDown = downMost.position.y;
                            if (goRight)
                            {
                                for (float i = wallDown; i < wallUp; i += 10)
                                {
                                    float ifCreate = Random.Range(0, 10);
                                    if (ifCreate < 8)
                                    {
                                        GameObject newSword = Instantiate(sword, new Vector3(wallLeft,i, 0), Quaternion.Euler(0, 0, 0));
                                        Animator swordAnim = newSword.GetComponent<Animator>();
                                        Destroy(swordAnim);
                                        sword swordScript = newSword.GetComponent<sword>();
                                        swordScript.other = true;
                                    }
                                }
                            }

                            else
                            {
                                for (float i = wallDown; i < wallUp; i += 7)
                                {
                                    float ifCreate = Random.Range(0, 10);
                                    if (ifCreate < 7.5)
                                    {
                                        GameObject newSword = Instantiate(sword, new Vector3(wallRight, i, 0), Quaternion.Euler(0, 0, -180));
                                        Animator swordAnim = newSword.GetComponent<Animator>();
                                        Destroy(swordAnim);
                                        sword swordScript = newSword.GetComponent<sword>();
                                        swordScript.other = true;
                                    }
                                }
                            }
                            launchCounter = 0;
                            count4 += 1;
                        }
                    }
                    else if (launchCounter >= 100)
                    {
                        launchCounter = 0;
                        swordWall = false;
                        launching = false;
                        teleport = true;
                        floating = false;
                        count4 = 0;
                        float wallDir = Random.Range(0, 10);
                        if (wallDir < 5)
                        {
                            goRight = true;
                        }
                        else
                        {
                            goRight = false;
                        }
                    }
                }
                else if (lightWall)
                {
                    launchCounter += 1;
                    if (launchCounter > 105 && launchCounter < 107)
                    {
                        wallLeft = leftMost.position.x - 50;
                        wallRight = rightMost.position.x + 50;
                        float wallPos = leftMost.position.y;
                        if (goRight)
                        {
                            GameObject newSword = Instantiate(lightWallGO, new Vector3(wallLeft, wallPos, 0), Quaternion.Euler(0, 0, 0));
                        }
                        else
                        {
                            GameObject newSword = Instantiate(lightWallGO, new Vector3(wallRight, wallPos, 0), Quaternion.Euler(0, 0, -180));
                        }
                    }
                    else if (launchCounter >= 110)
                    {
                        launchCounter = 0;
                        lightWall = false;
                        launching = false;
                        teleport = true;
                        floating = false;
                        float wallDir = Random.Range(0, 10);
                        if (wallDir < 5)
                        {
                            goRight = true;
                        }
                        else
                        {
                            goRight = false;
                        }
                    }
                }
                else if (orbAttack)
                {
                    launchCounter += 1;
                    if (launchCounter > 15 && launchCounter < 100)
                    {
                        thisSR.color = new Color(1, 1, 1);
                    }
                    else if (launchCounter >= 100)
                    {
                        launchCounter = 0;
                        orbAttack = false;
                        launching = false;
                        teleport = true;
                        floating = false;
                    }
                }

                nextTeleportPosition = Random.Range(leftMost.position.x, rightMost.position.x);
                if (!launching)
                {
                    
                    if (nextLaunchIndex < 10)
                    {
                        beamBurst = true;
                        launching = true;
                        teleportWhileLaunching = false;
                    }
                    else if (nextLaunchIndex < 25)
                    {
                        swordBurst = true;
                        launching = true;
                        teleportWhileLaunching = false;
                    }
                    else if (nextLaunchIndex < 45)
                    {
                        swordRain = true;
                        launching = true;
                        teleportWhileLaunching = true;
                    }
                    else if (nextLaunchIndex < 65)
                    {
                        swordWall = true;
                        launching = true;
                        teleportWhileLaunching = true;
                    }
                    else if (nextLaunchIndex < 75)
                    {
                        orbAttack = true;
                        launching = true;
                        teleportWhileLaunching = false;
                    }
                    else if (nextLaunchIndex < 100)
                    {
                        lightWall = true;
                        launching = true;
                        teleportWhileLaunching = true;
                    }
                    else
                    {
                        launching = false;
                        thisSR.color = new Color(0, 0, 0);
                    }
                    
                }else

                if (!launching || teleportWhileLaunching)
                {
                    floatCounter += 1;
                }


                if (floatCounter > nextFloatDuration)
                {
                    float isOut = Random.Range(1f, 10f);
                    if (isOut > 9)
                    {
                        telOutRange = true;
                    }
                    else
                    {
                        telOutRange = false;
                    }
                    floatCounter = 0;
                    teleport = true;
                    floating = false;
                }
            }
            if (teleport)
            {
                resetVariables();
                nextFloatDuration = Random.Range(150, 200);
                nextLaunchIndex = Random.Range(0f, 100f);
                if (!telOutRange)
                {
                    transform.position = new Vector3(nextTeleportPosition, transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(leftMost.position.x - 100, transform.position.y, transform.position.z);
                }
                teleport = false;
                floating = true;
            }
        }
    }

    void resetVariables()
    {
        launchCounter = 0;
        //sword burst variables
        swordBurstAngle = 0;
        swordBurstDistance = 10;
        //beam burst variables
        beamBurstAngle = Random.Range(0, 90);
        beamBurstCount3 = 0;
        beamBurstStartIndex = 30;
        beamBurstUsedAngle = 0;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}