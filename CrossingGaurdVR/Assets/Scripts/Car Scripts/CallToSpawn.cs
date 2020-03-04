using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallToSpawn : MonoBehaviour {

    AudioSource audioSource;

    public GameObject displayScreen;
    public GameObject playAgainScreen;
    public GameObject mainMenuScreen;
    public GameObject WalkerSpawner;

    public GameObject[] unselectedCarsToDestory;
    public GameObject[] selectedCarsToDestory;
    public GameObject[] leftSelectedCarsToDestory;

    MenuButtonScript displayScreenScript;
    MenuButtonScript playAgainScreenScript;
    MenuButtonScript mainMenuScreenScript;

    public GameObject spawnN;
    public GameObject spawnS;
    public GameObject spawnE;
    public GameObject spawnW;

    public PathScripts PathN; 
    public PathScripts PathS;
    public PathScripts PathE;
    public PathScripts PathW;

    public PathScripts PathNE;
    public PathScripts PathNW;
    public PathScripts PathSE;
    public PathScripts PathSW;
    public PathScripts PathEN;
    public PathScripts PathES;
    public PathScripts PathWN;
    public PathScripts PathWS;

    public PathScripts PathNEnd;
    public PathScripts PathSEnd;
    public PathScripts PathEEnd;
    public PathScripts PathWEnd;

    public SpawnScript carSpawnScriptN;
    public SpawnScript carSpawnScriptS;
    public SpawnScript carSpawnScriptE;
    public SpawnScript carSpawnScriptW;

    public AudioClip carHorn;
    public AudioClip carCrashPart1;
    public AudioClip carCrashPart2;
    public AudioClip carCrashPart3;
    public AudioClip carCrashPart4;

    float spawnDelay = 15.0f;
    float honkDelay = 0.2f;
    public int crashedCars = 0;
    public int spawnsUntilWalkers = 10;
    int spawnDirection;
    int newSpawnDirection;
    int thirdSpawnDirection;
    int aboveOrBelow;

    public bool startSpawning = false;
    public bool increaseCrashedCars = false;

    public bool continuallySpawn = true;


    public bool debugTurn = false;
    bool honkTwice = true;
    bool walkersSpawned = false;
    public int debugDirection = 1;
    public bool firstSpawn = true;
    public bool lost = false;
    public bool deleteEverything = false;
    public bool wonDeleteThings = false;

    bool spawnNorth = false;
    bool spawnSouth = false;
    bool spawnEast = false;
    bool spawnWest = false;

    private void Awake()
    {
        carSpawnScriptN = spawnN.GetComponent<SpawnScript>();
        carSpawnScriptS = spawnS.GetComponent<SpawnScript>();
        carSpawnScriptE = spawnE.GetComponent<SpawnScript>();
        carSpawnScriptW = spawnW.GetComponent<SpawnScript>();

        displayScreenScript = displayScreen.GetComponent<MenuButtonScript>();
        playAgainScreenScript = playAgainScreen.GetComponent<MenuButtonScript>();
        mainMenuScreenScript = mainMenuScreen.GetComponent<MenuButtonScript>();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (debugTurn)
        {
            StartCoroutine(DebugTurnMode());
        }
    }

    // Update is called once per frame
    void Update ()
    {

        if (wonDeleteThings)
        {
            StartCoroutine(DeleteEverything());
        }

        if (startSpawning)
        {
            startSpawning = false;
            StartCoroutine(BetweenSpawns());
        }

        SpawnN();
        SpawnS();
        SpawnE();
        SpawnW();

        if (increaseCrashedCars)
        {
            crashedCars++;
            increaseCrashedCars = false;
            audioSource.PlayOneShot(carCrashPart1);
            audioSource.PlayOneShot(carCrashPart2);
            audioSource.PlayOneShot(carCrashPart3);
            audioSource.PlayOneShot(carCrashPart4);
        }

        if (crashedCars == 3)
        {
            lost = true;
            continuallySpawn = false;
            WalkerSpawner.GetComponent<SpawnWalkersScript>().stopSpawnning = true;
            if (!deleteEverything)
            {
                deleteEverything = true;
                StartCoroutine(DeleteEverything());
            }

            displayScreenScript.lost = true;
            playAgainScreenScript.lost = true;
            mainMenuScreenScript.lost = true;

            crashedCars = 0;
        }
    }

    IEnumerator DeleteEverything()
    {
        unselectedCarsToDestory = GameObject.FindGameObjectsWithTag("Car");
        for (int k = 0; k < unselectedCarsToDestory.Length; k++)
        {
            Destroy(unselectedCarsToDestory[k]);
        }

        selectedCarsToDestory = GameObject.FindGameObjectsWithTag("Person");
        for (int k = 0; k < selectedCarsToDestory.Length; k++)
        {
            Destroy(selectedCarsToDestory[k]);
        }
        yield return new WaitForSeconds(0.0f);
    }

    IEnumerator Spawnner()
    {
        RandomTwoDirections();

        spawnDirection = 0;
        newSpawnDirection = 0;

        StartCoroutine(CarHonk());

        yield return new WaitForSeconds(1.0f);

        if (continuallySpawn)
        {
            StartCoroutine(BetweenSpawns());
        }
    }

    IEnumerator BetweenSpawns()
    {
        float currentSpawnDelay;

        if (firstSpawn)
        {
            firstSpawn = false;
            currentSpawnDelay = 0.0f;
        }
        else
        {
            currentSpawnDelay = spawnDelay;
        }
        yield return new WaitForSeconds(currentSpawnDelay);

        spawnsUntilWalkers -= 1;

        if (spawnsUntilWalkers <= 0 && !walkersSpawned)
        {
            WalkerSpawner.GetComponent<SpawnWalkersScript>().startSpawning = true;
            walkersSpawned = true;
        }

        if (continuallySpawn)
        {
            StartCoroutine(Spawnner());
        }
    }

    IEnumerator DebugTurnMode()
    {
        switch (debugDirection)
        {
            case 1:
                spawnNorth = true;
                break;
            case 2:
                spawnSouth = true;
                break;
            case 3:
                spawnEast = true;
                break;
            case 4:
                spawnWest = true;
                break;
        }
        yield return new WaitForSeconds(5.0f);

        StartCoroutine(DebugTurnMode());
    }

    IEnumerator CarHonk()
    {
        audioSource.PlayOneShot(carHorn);
        yield return new WaitForSeconds(honkDelay);
        if (honkTwice)
        {
            honkTwice = false;
            StartCoroutine(CarHonk());
        }
        else
        {
            honkTwice = true;
        }
    }

    void RandomTwoDirections()
    {
        spawnDirection = Random.Range(1, 5);

        switch (spawnDirection)
        {
            case 1:
                spawnNorth = true;
                newSpawnDirection = Random.Range(2, 5);
                break;
            case 2:
                spawnSouth = true;
                aboveOrBelow = Random.Range(1, 3);
                if (aboveOrBelow == 2)
                {
                    newSpawnDirection = Random.Range(3, 5);
                }
                else
                {
                    newSpawnDirection = 1;
                }
                break;
            case 3:
                spawnEast = true;
                aboveOrBelow = Random.Range(1, 3);
                if (aboveOrBelow == 2)
                {
                    newSpawnDirection = Random.Range(1, 3);
                }
                else
                {
                    newSpawnDirection = 4;
                }
                break;
            case 4:
                spawnWest = true;
                newSpawnDirection = Random.Range(1, 4);
                break;
            case 0:
                break;
        }

        switch (newSpawnDirection)
        {
            case 1:
                spawnNorth = true;
                break;
            case 2:
                spawnSouth = true;
                break;
            case 3:
                spawnEast = true;
                break;
            case 4:
                spawnWest = true;
                break;
        }
    }

    private void SpawnW()
    {
        if (spawnWest)
        {
            spawnWest = false;
            carSpawnScriptW.targetStartSpawn = PathW.pathTransforms;
            carSpawnScriptW.targetRightSpawn = PathWN.pathTransforms;
            carSpawnScriptW.targetLeftSpawn = PathWS.pathTransforms;
            carSpawnScriptW.targetStraightEndSpawn = PathWEnd.pathTransforms;
            carSpawnScriptW.targetRightEndSpawn = PathNEnd.pathTransforms;
            carSpawnScriptW.targetLeftEndSpawn = PathSEnd.pathTransforms;
            carSpawnScriptW.spawn = true;
        }
    }

    private void SpawnE()
    {
        if (spawnEast)
        {
            spawnEast = false;
            carSpawnScriptS.targetStartSpawn = PathS.pathTransforms;
            carSpawnScriptS.targetRightSpawn = PathSW.pathTransforms;
            carSpawnScriptS.targetLeftSpawn = PathSE.pathTransforms;
            carSpawnScriptS.targetStraightEndSpawn = PathSEnd.pathTransforms;
            carSpawnScriptS.targetRightEndSpawn = PathWEnd.pathTransforms;
            carSpawnScriptS.targetLeftEndSpawn = PathEEnd.pathTransforms;
            carSpawnScriptS.spawn = true;
        }
    }

    private void SpawnS()
    {
        if (spawnSouth)
        {
            spawnSouth = false;
            carSpawnScriptE.targetStartSpawn = PathE.pathTransforms;
            carSpawnScriptE.targetRightSpawn = PathES.pathTransforms;
            carSpawnScriptE.targetLeftSpawn = PathEN.pathTransforms;
            carSpawnScriptE.targetStraightEndSpawn = PathEEnd.pathTransforms;
            carSpawnScriptE.targetRightEndSpawn = PathSEnd.pathTransforms;
            carSpawnScriptE.targetLeftEndSpawn = PathNEnd.pathTransforms;
            carSpawnScriptE.spawn = true;
        }
    }

    private void SpawnN()
    {
        if (spawnNorth)
        {
            spawnNorth = false;
            carSpawnScriptN.targetStartSpawn = PathN.pathTransforms;
            carSpawnScriptN.targetRightSpawn = PathNE.pathTransforms;
            carSpawnScriptN.targetLeftSpawn = PathNW.pathTransforms;
            carSpawnScriptN.targetStraightEndSpawn = PathNEnd.pathTransforms;
            carSpawnScriptN.targetRightEndSpawn = PathEEnd.pathTransforms;
            carSpawnScriptN.targetLeftEndSpawn = PathWEnd.pathTransforms;
            carSpawnScriptN.spawn = true;
        }
    }
}
