using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radianceMovement : MonoBehaviour
{
    //phase1 health 50
    //phase2 health 60
    //phase3 health 40
    //phase4 health 100
    //phase5 health 1

    public Health health;

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

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponentInChildren<Animator>();

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

        if (health.Hp > 210)
        {
            phase1 = true;
            phase2 = false;
        }
        else if (health.Hp > 150)
        {
            phase2 = true;
            phase1 = false;
            phase3 = false;
        }
        else if (health.Hp > 110)
        {
            phase3 = true;
            phase2 = false;
            phase1 = false;
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
                //launch sword burst
                if (swordBurst && !telOutRange)
                {
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
                            float newX = transform.position.x + thisX;
                            float newY = transform.position.y + thisY;
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

                    else if (launchCounter >= 190 && launchCounter < 200)
                    {
                        //teleport animation
                        anim.SetBool("spin",true);
                        anim.SetBool("flying",false);
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
                if (beamBurst && !telOutRange)
                {
                    launchCounter += 1;
                    if (beamBurstCount3 < 3)
                    {
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
                                float newX = transform.position.x;
                                float newY = transform.position.y;
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
                        //normal animation
                        anim.SetBool("flying",false);
                        anim.SetBool("spin",false);
                    }

                    else if (launchCounter >=80 && launchCounter < 90)
                    {
                        //teleport animation
                        anim.SetBool("spin",true);
                        anim.SetBool("flying",false);

                    }

                    else if (launchCounter >= 100)
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
                    else if (launchCounter >= 90 && launchCounter < 100)
                    {
                        //teleport animation
                        anim.SetBool("spin",true);
                        anim.SetBool("flying",false);
                    }

                    else if (launchCounter >= 100)
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
                            Debug.Log(count4);
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
                    else if (launchCounter >= 90 && launchCounter < 100)
                    {
                        //teleport animation
                        anim.SetBool("spin",true);
                        anim.SetBool("flying",false);

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
                        //normal animation
                        anim.SetBool("flying",false);
                        anim.SetBool("spin",false);

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

                    else if (launchCounter >= 100 && launchCounter < 110)
                    {
                        //teleport animation
                        anim.SetBool("spin",true);
                        anim.SetBool("flying",false);

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
                else if (orbAttack && !telOutRange)
                {
                    launchCounter += 1;
                    orbUp = rightMost.position.y + 5;
                    orbDown = rightMost.position.y - 5;
                    orbLeft = leftMost.position.x + 5;
                    orbRight = rightMost.position.x - 5;
                    if (launchCounter > 165 && launchCounter < 167 && orbCount3 < 3)
                    {
                        //lauching skill animation
                        anim.SetBool("flying",true);
                        anim.SetBool("spin",false);

                        Debug.Log("inst");
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
                    nextFloatDuration = Random.Range(150, 200);
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
                //normal animation
                anim.SetBool("flying",false);
                anim.SetBool("spin",false);

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

        //phase3
        if (phase3)
        {
            //launching skill animation
                    anim.SetBool("flying",true);
                    anim.SetBool("spin",false);
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
            if (phase3ReadyCounter > 50)
            {
                float middlePos = (leftMost.position.x + rightMost.position.x) / 2;
                nextTeleportPosition = middlePos;
                teleport = true;
            }
            if (teleport)
            {
                resetVariables();
                transform.position = new Vector3(nextTeleportPosition, transform.position.y, transform.position.z);
                teleport = false;
                swordRain = true;
            }
            if (swordRain)
            {
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
                            GameObject newSword = Instantiate(sword, new Vector3(i, upMost.position.y, 0), Quaternion.Euler(0, 0, -90));
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
