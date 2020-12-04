using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radianceMovement : Attack
{
    //phase1 health 50
    //phase2 health 60
    //phase3 health 40
    //phase4 health 100
    //phase5 health 1

    //public Health health;
    public ParticleSystem feather;
    private int featherCounter = 0;
    public ParticleSystem ring;
    public Transform ringT;
    private int ringCounter;
    public SpriteRenderer bigWhiteCircle;
    public Transform whiteCircleT;
    private float whiteCircleTrans = 0;
    private bool whiteCirclePlus = false;
    //public Transform player;
    public SpriteRenderer thisSR;
    public int floatCounter = 0;
    public bool floating = false;
    public bool teleport = false;
    public bool telOutRange = false;
    private int outRangeIndex = 0;
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
    private float teleportDistance = 0;
    public bool phase1 = false;
    public bool phase2 = false;
    public bool phase3 = false;
    public bool phase4 = false;
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
    public GameObject middle;
    private float middleTrans = 0;
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
    //orb variables
    private float orbUp = 0;
    private float orbDown = 0;
    private float orbLeft = 0;
    private float orbRight = 0;
    public GameObject orb;
    private int orbCount3 = 0;
    //phase2 spike
    public Transform spikeLeft;
    public Transform spikeRight;
    public Transform spikeMiddle;
    public GameObject spike;
    private bool generateSpikeLeft = true;
    List<GameObject> spikes1 = new List<GameObject>();
    List<GameObject> spikes2 = new List<GameObject>();
    private int spikeCounter = 400;
    //phase3 variables
    public int phase3ReadyCounter = 0;
    public int phase3LaunchCounter = 0;
    //phase4 variables
    public GameObject uniquePlatform;
    public GameObject phase4platforms;
    List<Transform> phase4position = new List<Transform>();
    private int uniquePlatformCounter = 0;
    private bool phase4ready = false;
    private bool phase4LongTeleport = true;
    private Animator anim;
    private int choosePlatform = 0;
    private Vector3 teleportPos2d = new Vector3(0, 0, 0);
    public bool attackStart = false;
    private int forceToStart = 0;
    private void LateUpdate()
    {
        base.Update();
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform platform in phase4platforms.transform)
        {
            phase4position.Add(platform);
        }
        choosePlatform = Random.Range(0, phase4position.Count);
        Transform platPos = phase4position[choosePlatform];
        float alterY = Random.Range(15, 20);
        float alterX = Random.Range(-10, 10);
        teleportPos2d = new Vector3(platPos.position.x + alterX, platPos.position.y + alterY, 0);
        phase4platforms.SetActive(false);
        uniquePlatform.SetActive(false);
        bigWhiteCircle.color = new Color(1, 1, 1, whiteCircleTrans);
        ring.Stop();
        anim=GetComponentInChildren<Animator>();
        SpriteRenderer middleSR = middle.GetComponent<SpriteRenderer>();
        middleSR.color = new Color(1, 1, 1, 0);
        nextLaunchIndex = Random.Range(0f, 100f);
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
        for (float i = spikeLeft.position.x; i < spikeMiddle.position.x; i += 2.8f)
        {
            GameObject newSpike = Instantiate(spike, new Vector3(i, spikeMiddle.position.y, 0), Quaternion.Euler(0, 0, 0));
            newSpike.SetActive(false);
            spikes1.Add(newSpike);
        }
        for (float i = spikeRight.position.x; i > spikeMiddle.position.x; i -= 2.8f)
        {
            GameObject newSpike = Instantiate(spike, new Vector3(i, spikeMiddle.position.y, 0), Quaternion.Euler(0, 0, 0));
            newSpike.SetActive(false);
            spikes2.Add(newSpike);
        }
    }

    void FixedUpdate()
    {
        float middleX = middle.transform.position.x;
        float middleY = middle.transform.position.y;
        if (health.Hp > 210)
        {
            phase1 = true;
            phase2 = false;
            phase3 = false;
            phase4 = false;
        }
        else if (health.Hp > 150)
        {
            phase2 = true;
            phase1 = false;
            phase3 = false;
            phase4 = false;
        }
        else if (health.Hp > 110)
        {
            phase3 = true;
            phase2 = false;
            phase1 = false;
            phase4 = false;
        }else if (health.Hp > 20)
        {
            phase4 = true;
            phase1 = false;
            phase2 = false;
            phase3 = false;
        }


        if (phase2)
        {
            spikeCounter += 1;
            if (spikeCounter > 500)
            {
                if (generateSpikeLeft == true)
                {
                    for (int i = 0; i < spikes1.Count; i++)
                    {
                        GameObject thisSpike1 = spikes2[i];
                        thisSpike1.SetActive(false);
                    }
                    for (int i = 0; i < spikes2.Count; i++)
                    {
                        GameObject thisSpike2 = spikes1[i];
                        thisSpike2.SetActive(true);
                    }
                    generateSpikeLeft = false;
                }
                else
                {
                    for (int i = 0; i < spikes2.Count; i++)
                    {
                        GameObject thisSpike2 = spikes2[i];
                        thisSpike2.SetActive(true);
                    }
                    for (int i = 0; i < spikes1.Count; i++)
                    {
                        GameObject thisSpike1 = spikes1[i];
                        thisSpike1.SetActive(false);
                    }
                    generateSpikeLeft = true;
                }
                spikeCounter = 0;
            }
        }


        if (phase1 || phase2)
        {
            if (floating)
            {
                teleportParticles();
                //launch sword burst
                if (swordBurst && !telOutRange)
                {
                    floatCounter = 0;
                    //launching skill animation
                    anim.SetBool("flying",true);
                    anim.SetBool("spin",false);

                    launchCounter += 1;
                    if (launchCounter > 15 && launchCounter < 17)
                    {
                        for (int i = 0; i < 12; i++) { 
                            float radians = (Mathf.PI / 180) * swordBurstAngle;
                            float thisX = swordBurstDistance * Mathf.Sin(radians);
                            float thisY = swordBurstDistance * Mathf.Cos(radians);
                            float newX = middleX + thisX;
                            float newY = middleY + thisY;
                            GameObject newSword =  Instantiate(sword, new Vector3(newX, newY, 0), Quaternion.Euler(0,0,-swordBurstAngle+90));
                            sword ifBurst = newSword.GetComponent<sword>();
                            ifBurst.burstRotation = -swordBurstAngle + 90;
                            ifBurst.burst = true;
                            swordBurstAngle += 30;
                        }
                    }

                    else if (launchCounter > 100 && launchCounter < 190)
                    {
                        //normal animation
                        anim.SetBool("flying",false);
                        anim.SetBool("spin",false);

                    }

                    else if (launchCounter >= 250 && launchCounter < 260)
                    {
                        //teleport animation
                        anim.SetBool("spin",true);
                        anim.SetBool("flying",false);
                    }

                    else if (launchCounter >= 260)
                    {
                        launchCounter = 0;
                        launching = false;
                        swordBurst = false;
                        teleport = true;
                        floating = false;
                    }

                }

                //launch beam burst
                if (beamBurst && !telOutRange)
                {
                    launchCounter += 1;
                    floatCounter = 0;
                    if (beamBurstCount3 < 3)
                    {
                        if (middleTrans < 1)
                        {
                            middleTrans += 0.1f;
                        }
                        SpriteRenderer middleSR = middle.GetComponent<SpriteRenderer>();
                        middleSR.color = new Color(1, 1, 1, middleTrans);
                        //lauching skill animation
                        anim.SetBool("flying",true);
                        anim.SetBool("spin",false);;

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
                                float newX = middleX;
                                float newY = middleY;
                                GameObject thisBeam = Instantiate(beam, new Vector3(newX, newY, 0), Quaternion.Euler(0, 0, beamBurstAngle));
                                beamBurstAngle += 45;
                            }
                            beamBurstStartIndex = 75;
                            beamBurstCount3 += 1;
                            launchCounter = 0;
                        }
                    }

                    else if (launchCounter > 70 && launchCounter < 80)
                    {
                        if (middleTrans > 0)
                        {
                            middleTrans -= 0.1f;
                        }
                        SpriteRenderer middleSR = middle.GetComponent<SpriteRenderer>();
                        middleSR.color = new Color(1, 1, 1, middleTrans);
                        //normal animation
                        anim.SetBool("flying",false);
                        anim.SetBool("spin",false);
                    }

                    else if (launchCounter >=150 && launchCounter < 160)
                    {
                        //teleport animation
                        anim.SetBool("spin",true);
                        anim.SetBool("flying",false);

                    }

                    else if (launchCounter >= 160)
                    {
                        resetVariables();
                        launchCounter = 0;
                        launching = false;
                        beamBurst = false;
                        teleport = true;
                        floating = false;
                    }
                }

                //launch sword rain

                else if (swordRain)
                {
                    //normal animation
                    anim.SetBool("flying",false);
                    anim.SetBool("spin",false);

                    launchCounter += 1;
                    if (launchCounter > 55 && launchCounter < 57)
                    {
                        if (count4 < 4)
                        {
                            rainLeft = leftMost.position.x - 50;
                            rainRight = rightMost.position.x + 50;
                            for (float i = rainLeft + Random.Range(-10, 10); i < rainRight; i += 7)
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
                    else if (launchCounter >= 120 && launchCounter < 130)
                    {
                        //teleport animation
                        anim.SetBool("spin",true);
                        anim.SetBool("flying",false);
                    }

                    else if (launchCounter >= 130)
                    {
                        launchCounter = 0;
                        launching = false;
                        swordRain = false;
                        teleport = true;
                        floating = false;
                    }
                }

                //launch sword wall

                else if (swordWall)
                {
                    launchCounter += 1;
                    if (launchCounter > 65 && launchCounter < 67)
                    {
                        //normal animation
                        anim.SetBool("flying",false);
                        anim.SetBool("spin",false);

                        if (count4 < 4)
                        {
                            wallLeft = leftMost.position.x - 50;
                            wallRight = rightMost.position.x + 50;
                            wallUp = upMost.position.y;
                            wallDown = downMost.position.y;
                            if (goRight)
                            {
                                for (float i = wallDown + Random.Range(-10, 10); i < wallUp; i += 7)
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
                                for (float i = wallDown + Random.Range(-10, 10); i < wallUp; i += 7)
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
                    else if (launchCounter >= 150 && launchCounter < 160)
                    {
                        //teleport animation
                        anim.SetBool("spin",true);
                        anim.SetBool("flying",false);

                    }

                    else if (launchCounter >= 160)
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
                        //normal animation
                        anim.SetBool("flying",false);
                        anim.SetBool("spin",false);

                        wallLeft = leftMost.position.x - 50;
                        wallRight = rightMost.position.x + 50;
                        GameObject player = GameObject.Find("Player");
                        float playerY = player.transform.position.y;
                        if (goRight)
                        {
                            GameObject newSword = Instantiate(lightWallGO, new Vector3(wallLeft, playerY, 0), Quaternion.Euler(0, 0, 0));
                        }
                        else
                        {
                            GameObject newSword = Instantiate(lightWallGO, new Vector3(wallRight, playerY, 0), Quaternion.Euler(0, 0, -180));
                        }
                    }

                    else if (launchCounter >= 180 && launchCounter < 190)
                    {
                        //teleport animation
                        anim.SetBool("spin",true);
                        anim.SetBool("flying",false);

                    }

                    else if (launchCounter >= 190)
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
                else if (orbAttack && !telOutRange)
                {
                    floatCounter = 0;
                    launchCounter += 1;
                    orbUp = rightMost.position.y + 5;
                    orbDown = rightMost.position.y - 5;
                    orbLeft = leftMost.position.x + 5;
                    orbRight = rightMost.position.x - 5;
                    
                    if (launchCounter > 105 && launchCounter < 107 && orbCount3 < 3)
                    {
                        //lauching skill animation
                        anim.SetBool("flying",true);
                        anim.SetBool("spin",false);
                        float orbY = Random.Range(orbUp, orbDown);
                        float orbX = Random.Range(orbLeft, orbRight);
                        GameObject thisOrb = Instantiate(orb, new Vector3(orbX, orbY, 0), Quaternion.Euler(0, 0, 0));
                        orbCount3 += 1;
                        launchCounter = 0;
                    }

                    else if (launchCounter > 170 && launchCounter < 190)
                    {
                        //normal animation
                        anim.SetBool("flying",false);
                        anim.SetBool("spin",false);
                    }

                    else if (launchCounter >= 190 && launchCounter < 200)
                    {
                        //teleport animation
                        anim.SetBool("spin",true);
                        anim.SetBool("flying",false);
                    }

                    else if (launchCounter >= 200)
                    {
                        orbCount3 = 0;
                        launchCounter = 0;
                        orbAttack = false;
                        launching = false;
                        teleport = true;
                        floating = false;
                    }
                }

                teleportDistance = Mathf.Abs(nextTeleportPosition - transform.position.x);
                if (teleportDistance < 15)
                {
                    nextTeleportPosition = Random.Range(leftMost.position.x, rightMost.position.x);
                }
                if (!launching)
                {
                    
                    if (nextLaunchIndex < 15)
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
                    else if (nextLaunchIndex < 85)
                    {
                        orbAttack = true;
                        launching = true;
                        teleportWhileLaunching = false;
                    }
                    else 
                    {
                        lightWall = true;
                        launching = true;
                        teleportWhileLaunching = true;
                    }

                    
                }else

                if (!launching || teleportWhileLaunching)
                {
                    floatCounter += 1;
                }


                if (floatCounter > nextFloatDuration - 10)
                {
                    //teleport animation
                    anim.SetBool("spin",true);
                    anim.SetBool("flying",false);
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

            if (telOutRange)
            {
                outRangeIndex += 1;
                if (outRangeIndex > 50)
                {
                    telOutRange = false;
                    outRangeIndex = 0;
                    nextFloatDuration = Random.Range(200, 250);
                    nextLaunchIndex = Random.Range(0f, 100f);
                    transform.position = new Vector3(nextTeleportPosition, transform.position.y, transform.position.z);
                    teleport = false;
                    floating = true;
                }
            }
            else
            {
                outRangeIndex = 0;
            }

            if (teleport)
            {
                ring.Play();
                ringCounter = 0;
                featherCounter = 0;
                whiteCirclePlus = true;
                whiteCircleTrans = 0;
                //normal animation
                anim.SetBool("flying",false);
                anim.SetBool("spin",false);

                resetVariables();
                nextFloatDuration = Random.Range(200, 250);
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

        //phase3
        if (phase3)
        {
            middle.SetActive(false);
            for (int i = 0; i < spikes1.Count; i++)
            {
                if (i < 6)
                {
                    spikes1[i].SetActive(true);
                }
                else
                {
                    spikes1[i].SetActive(false);
                }
            }
            for (int i = 0; i < spikes2.Count; i++)
            {
                if (i < 6)
                {
                    spikes2[i].SetActive(true);
                }
                else
                {
                    spikes2[i].SetActive(false);
                }
            }
            phase3ReadyCounter += 1;
            if (phase3ReadyCounter > 40 && phase3ReadyCounter < 50)
            {
                //teleport animation
                anim.SetBool("spin", true);
                anim.SetBool("flying", false);
            }
            else if (phase3ReadyCounter > 50 && phase3ReadyCounter < 60)
            {

                float middlePos = (leftMost.position.x + rightMost.position.x) / 2;
                nextTeleportPosition = middlePos;
                teleport = true;
            }
            else if (phase3ReadyCounter > 60)
            {
                teleport = false;
                phase3ReadyCounter = 60;
            }
            if (teleport)
            {
                ring.Play();
                ringCounter = 0;
                featherCounter = 0;
                whiteCirclePlus = true;
                whiteCircleTrans = 0;
                //launching skill animation
                anim.SetBool("flying", true);
                anim.SetBool("spin", false);
                resetVariables();
                transform.position = new Vector3(nextTeleportPosition, transform.position.y, transform.position.z);
                teleport = false;
                swordRain = true;
            }
            if (swordRain)
            {
                teleportParticles();
                phase3LaunchCounter += 1;
                if (phase3LaunchCounter > 55 && phase3LaunchCounter < 57)
                {

                    rainLeft = leftMost.position.x - 50;
                    rainRight = rightMost.position.x + 50;
                    for (float i = rainLeft+Random.Range(-10,10); i < rainRight; i += 7)
                    {
                        float ifCreate = Random.Range(0, 10);
                        if (ifCreate < 7.5)
                        {
                            GameObject newSword = Instantiate(sword, new Vector3(i, upMost.position.y-20, 0), Quaternion.Euler(0, 0, -90));
                            Animator swordAnim = newSword.GetComponent<Animator>();
                            Destroy(swordAnim);
                            sword swordScript = newSword.GetComponent<sword>();
                            swordScript.other = true;
                        }
                    }
                    phase3LaunchCounter = 0;
                }
            }
        }

        //phase4
        else if (phase4)
        {
           
           swordRain = false;
            if (forceToStart > 500)
            {
                attackStart = true;
            }
            else
            {
                forceToStart += 1;
            }
           for (int i = 0; i < spikes1.Count; i++)
            {
                GameObject thisSpike = spikes1[i];
                thisSpike.SetActive(false);
            }
            for (int i = 0; i < spikes2.Count; i++)
            {
                GameObject thisSpike = spikes2[i];
                thisSpike.SetActive(false);
            }
            //platforms ready
            if (uniquePlatformCounter < 300)
            {
                uniquePlatformCounter += 1;
                if (uniquePlatformCounter > 280)
                {
                    PolygonCollider2D uniquePC = uniquePlatform.GetComponent<PolygonCollider2D>();
                    Destroy(uniquePC);
                    SpriteRenderer uniqueSR = uniquePlatform.GetComponent<SpriteRenderer>();
                    float uniqueAlpha = uniqueSR.color.a;
                    uniqueAlpha -= 0.1f;
                    uniqueSR.color = new Color(1, 1, 1, uniqueAlpha);
                }
                if (uniquePlatformCounter > 290)
                {
                    uniquePlatform.SetActive(false);
                }
                else if (uniquePlatformCounter > 50)
                {
                    phase4platforms.SetActive(true);
                }
                {
                    uniquePlatform.SetActive(true);
                }
                if (uniquePlatformCounter > 200)
                {
                    phase4ready = true;
                }
            }
            //radiance ready
            if (!phase4ready)
            {
                if (uniquePlatformCounter < 10)
                {
                    anim.SetBool("spin", true);
                    anim.SetBool("flying", false);
                }
                else if  (uniquePlatformCounter < 11)
                {
                    ring.Play();
                    ringCounter = 0;
                    featherCounter = 0;
                    whiteCirclePlus = true;
                    whiteCircleTrans = 0;
                }
                else
                {
                    teleport = true;
                }
            }
            else
            {
                phase4LongTeleport = false;
            }

            if (floating)
            {
                teleportParticles();
                if (!phase4LongTeleport)
                {
                    //launch sword burst
                    if (swordBurst)
                    {
                        floatCounter = 0;
                        //launching skill animation
                        anim.SetBool("flying", true);
                        anim.SetBool("spin", false);

                        launchCounter += 1;
                        if (launchCounter > 15 && launchCounter < 17)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                float radians = (Mathf.PI / 180) * swordBurstAngle;
                                float thisX = swordBurstDistance * Mathf.Sin(radians);
                                float thisY = swordBurstDistance * Mathf.Cos(radians);
                                float newX = middleX + thisX;
                                float newY = middleY + thisY;
                                if (attackStart)
                                {
                                    GameObject newSword = Instantiate(sword, new Vector3(newX, newY, 0), Quaternion.Euler(0, 0, -swordBurstAngle + 90));
                                    sword ifBurst = newSword.GetComponent<sword>();
                                    ifBurst.burstRotation = -swordBurstAngle + 90;
                                    ifBurst.burst = true;
                                }
                                swordBurstAngle += 30;
                            }
                        }

                        else if (launchCounter > 100 && launchCounter < 190)
                        {
                            //normal animation
                            anim.SetBool("flying", false);
                            anim.SetBool("spin", false);

                        }

                        else if (launchCounter >= 250 && launchCounter < 260)
                        {
                            //teleport animation
                            anim.SetBool("spin", true);
                            anim.SetBool("flying", false);
                        }

                        else if (launchCounter >= 260)
                        {
                            attackStart = true;
                            launchCounter = 0;
                            launching = false;
                            swordBurst = false;
                            teleport = true;
                            floating = false;
                        }

                    }

                    //launch beam burst
                    if (beamBurst)
                    {
                        launchCounter += 1;
                        floatCounter = 0;
                        if (beamBurstCount3 < 3)
                        {
                            if (middleTrans < 1)
                            {
                                middleTrans += 0.1f;
                            }
                            SpriteRenderer middleSR = middle.GetComponent<SpriteRenderer>();
                            middleSR.color = new Color(1, 1, 1, middleTrans);
                            //lauching skill animation
                            anim.SetBool("flying", true);
                            anim.SetBool("spin", false); ;

                            if (launchCounter > beamBurstStartIndex && launchCounter < beamBurstStartIndex + 2)
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
                                    float newX = middleX;
                                    float newY = middleY;
                                    if (attackStart)
                                    {
                                        GameObject thisBeam = Instantiate(beam, new Vector3(newX, newY, 0), Quaternion.Euler(0, 0, beamBurstAngle));
                                    }
                                    beamBurstAngle += 45;
                                }
                                beamBurstStartIndex = 75;
                                beamBurstCount3 += 1;
                                launchCounter = 0;
                            }
                        }

                        else if (launchCounter > 70 && launchCounter < 80)
                        {
                            if (middleTrans > 0)
                            {
                                middleTrans -= 0.1f;
                            }
                            SpriteRenderer middleSR = middle.GetComponent<SpriteRenderer>();
                            middleSR.color = new Color(1, 1, 1, middleTrans);
                            //normal animation
                            anim.SetBool("flying", false);
                            anim.SetBool("spin", false);
                        }

                        else if (launchCounter >= 150 && launchCounter < 160)
                        {
                            //teleport animation
                            anim.SetBool("spin", true);
                            anim.SetBool("flying", false);

                        }

                        else if (launchCounter >= 160)
                        {
                            attackStart = true;
                            resetVariables();
                            launchCounter = 0;
                            launching = false;
                            beamBurst = false;
                            teleport = true;
                            floating = false;
                        }
                    }

                    //launch sword wall

                    else if (swordWall)
                    {
                        launchCounter += 1;
                        if (launchCounter > 65 && launchCounter < 67)
                        {
                            //normal animation
                            anim.SetBool("flying", false);
                            anim.SetBool("spin", false);

                            if (count4 < 4)
                            {
                                wallLeft = leftMost.position.x - 50;
                                wallRight = rightMost.position.x + 50;
                                wallUp = upMost.position.y;
                                wallDown = downMost.position.y;
                                if (goRight)
                                {
                                    for (float i = wallDown + Random.Range(-10, 10); i < wallUp; i += 7)
                                    {
                                        float ifCreate = Random.Range(0, 10);
                                        if (ifCreate < 8)
                                        {
                                            if (attackStart)
                                            {
                                                GameObject newSword = Instantiate(sword, new Vector3(wallLeft, i, 0), Quaternion.Euler(0, 0, 0));
                                                Animator swordAnim = newSword.GetComponent<Animator>();
                                                Destroy(swordAnim);
                                                sword swordScript = newSword.GetComponent<sword>();
                                                swordScript.other = true;
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    for (float i = wallDown + Random.Range(-10, 10); i < wallUp; i += 7)
                                    {
                                        float ifCreate = Random.Range(0, 10);
                                        if (ifCreate < 7.5)
                                        {
                                            if (attackStart)
                                            {
                                                GameObject newSword = Instantiate(sword, new Vector3(wallRight, i, 0), Quaternion.Euler(0, 0, -180));
                                                Animator swordAnim = newSword.GetComponent<Animator>();
                                                Destroy(swordAnim);
                                                sword swordScript = newSword.GetComponent<sword>();
                                                swordScript.other = true;
                                            }
                                        }
                                    }
                                }
                                launchCounter = 0;
                                count4 += 1;
                            }
                        }
                        else if (launchCounter >= 150 && launchCounter < 160)
                        {
                            //teleport animation
                            anim.SetBool("spin", true);
                            anim.SetBool("flying", false);

                        }

                        else if (launchCounter >= 160)
                        {
                            attackStart = true;
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
                            //normal animation
                            anim.SetBool("flying", false);
                            anim.SetBool("spin", false);

                            wallLeft = leftMost.position.x - 50;
                            wallRight = rightMost.position.x + 50;
                            float wallPos = leftMost.position.y;
                            if (goRight)
                            {
                                if (attackStart)
                                {
                                    GameObject newLight = Instantiate(lightWallGO, new Vector3(wallLeft, wallPos, 0), Quaternion.Euler(0, 0, 0));
                                    lightWallScr wallScr = newLight.GetComponent<lightWallScr>();
                                    wallScr.movingSpd = 0.4f;
                                    wallScr.phase4 = true;
                                }
                            }
                            else
                            {
                                if (attackStart)
                                {
                                    GameObject newLight = Instantiate(lightWallGO, new Vector3(wallRight, wallPos, 0), Quaternion.Euler(0, 0, -180));
                                    lightWallScr wallScr = newLight.GetComponent<lightWallScr>();
                                    wallScr.movingSpd = 0.4f;
                                    wallScr.phase4 = true;
                                }
                            }
                        }

                        else if (launchCounter >= 180 && launchCounter < 190)
                        {
                            //teleport animation
                            anim.SetBool("spin", true);
                            anim.SetBool("flying", false);

                        }

                        else if (launchCounter >= 190)
                        {
                            attackStart = true;
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
                        floatCounter = 0;
                        launchCounter += 1;
                        GameObject player = GameObject.Find("Player");
                        orbUp = player.transform.position.y + 10;
                        orbDown = player.transform.position.y + 10;
                        orbLeft = player.transform.position.x - 10;
                        orbRight = player.transform.position.x + 10;
                        bool posFound = false;
                        Vector2 pos = new Vector2(Random.Range(orbLeft, orbRight), Random.Range(orbUp, orbDown));
                        while (!posFound)
                        {
                            pos = new Vector2(Random.Range(orbLeft, orbRight), Random.Range(orbUp, orbDown));
                            Collider2D tryBox = Physics2D.OverlapBox(pos, new Vector2(5, 5), 0);
                            if (tryBox == null)
                            {
                                posFound = true;
                            }
                        }
                        
                        if (launchCounter > 105 && launchCounter < 107 && orbCount3 < 3)
                        {
                            //lauching skill animation
                            anim.SetBool("flying", true);
                            anim.SetBool("spin", false);
                            if (attackStart)
                            {
                                GameObject thisOrb = Instantiate(orb, pos, Quaternion.Euler(0, 0, 0));
                            }
                            orbCount3 += 1;
                            launchCounter = 0;
                        }

                        else if (launchCounter > 170 && launchCounter < 190)
                        {
                            //normal animation
                            anim.SetBool("flying", false);
                            anim.SetBool("spin", false);
                        }

                        else if (launchCounter >= 190 && launchCounter < 200)
                        {
                            //teleport animation
                            anim.SetBool("spin", true);
                            anim.SetBool("flying", false);
                        }

                        else if (launchCounter >= 200)
                        {
                            attackStart = true;
                            orbCount3 = 0;
                            launchCounter = 0;
                            orbAttack = false;
                            launching = false;
                            teleport = true;
                            floating = false;
                        }
                    }
                    
                    teleportDistance = Mathf.Abs(teleportPos2d.x - transform.position.x);
                    if (teleportDistance < 15)
                    {
                        Transform platPos = phase4position[choosePlatform];
                        float alterY = Random.Range(15, 20);
                        float alterX = Random.Range(-10, 10);
                        teleportPos2d = new Vector3(platPos.position.x + alterX, platPos.position.y + alterY, 0);
                    }
                    if (!launching)
                    {

                        if (nextLaunchIndex < 25)
                        {
                            beamBurst = true;
                            launching = true;
                            teleportWhileLaunching = false;
                        }
                        else if (nextLaunchIndex < 45)
                        {
                            swordBurst = true;
                            launching = true;
                            teleportWhileLaunching = false;
                        }
                        else if (nextLaunchIndex < 65)
                        {
                            swordWall = true;
                            launching = true;
                            teleportWhileLaunching = true;
                        }
                        else if (nextLaunchIndex < 85)
                        {
                            orbAttack = true;
                            launching = true;
                            teleportWhileLaunching = false;
                        }
                        else
                        {
                            lightWall = true;
                            launching = true;
                            teleportWhileLaunching = true;
                        }


                    }
                    else

                    if (!launching || teleportWhileLaunching)
                    {
                        floatCounter += 1;
                    }

                    if (floatCounter > nextFloatDuration - 10)
                    {
                        //teleport animation
                        anim.SetBool("spin", true);
                        anim.SetBool("flying", false);
                    }

                    if (floatCounter > nextFloatDuration)
                    {
                        floatCounter = 0;
                        teleport = true;
                        floating = false;
                    }

                }
                else
                {
                    teleportParticles();
                }
            }

            if (teleport)
            {
                
                if (phase4LongTeleport)
                {
                    transform.position = new Vector3(leftMost.position.x - 100, transform.position.y, 0);
                    floating = true;
                    teleport = false;
                }
                else
                {
                    ring.Play();
                    ringCounter = 0;
                    featherCounter = 0;
                    whiteCirclePlus = true;
                    whiteCircleTrans = 0;
                    anim.SetBool("flying", false);
                    anim.SetBool("spin", false);
                    resetVariables();
                    nextFloatDuration = Random.Range(200, 250);
                    nextLaunchIndex = Random.Range(0f, 100f);
                    choosePlatform = Random.Range(0, phase4position.Count);
                    transform.position = teleportPos2d;
                    teleport = false;
                    floating = true;
                }
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

    void teleportParticles()
    {
        //white circle
        whiteCircleT.position = ringT.position;
        if (whiteCirclePlus)
        {
            if (whiteCircleTrans < 0.8f)
            {
                whiteCircleTrans += 0.1f;
                bigWhiteCircle.color = new Color(1, 1, 1, whiteCircleTrans);
            }
            else
            {
                whiteCirclePlus = false;
            }
        }
        else
        {
            if (whiteCircleTrans > 0)
            {
                whiteCircleTrans -= 0.1f;
                bigWhiteCircle.color = new Color(1, 1, 1, whiteCircleTrans);
            }
        }
        //ring particle
        if (ring.isPlaying)
        {
            if (ringCounter < 20)
            {
                ringCounter += 1;
            }
            else if (ringCounter > 10)
            {
                ring.Stop();
            }
        }
        else
        {
            ringT.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        //feather particle
        if (featherCounter < 30)
        {
            featherCounter += 1;
        }
        if (featherCounter < 25)
        {
            feather.Play();
        }
        else
        {
            feather.Stop();
        }
    }



    // Update is called once per frame
    public override void Update()
    {
        
    }
}
