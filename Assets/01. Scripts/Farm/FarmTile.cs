using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTile : MonoBehaviour
{
    private float _humidity;
    public float humidity
    {
        get => _humidity;
        set => _humidity = Mathf.Clamp(value, 0f, 100f);
    }

    public bool isUnlock;

    public GameObject landParent;
    public GameObject previewParent;

    public List<SkinnedMeshRenderer> lands = new();
    public Color unWaterColor;
    public Color waterColor;

    public GameObject binil;
    private bool isBinilActive;

    public int harvestCount;
    public int animalProbability;

    private int lastHour;

    public GameObject animalPrefab;

    public List<GameObject> animals = new();

    public FarmLand[] childLands;

    public Animator damageAnim;

    public FarmLand centerLand;
    public CreateAutoHarvast autoHarvast;
    public Animator autoHarvastAnim;
    private bool isAutoHarvastBuild;
    private bool isAutoHarvastRunning;

    public void BuyTile()
    {
        isUnlock = true;
        GamePlayManager.Instance.activeFarmTiles.Add(this);
    }

    private void Start()
    {
        humidity = 50;

        childLands = GetComponentsInChildren<FarmLand>();

        InvokeRepeating(nameof(DecreaseHumidity), 5f, 5f);
        InvokeRepeating(nameof(AnimalAttack), 3f, 3f);
    }

    private void DecreaseHumidity()
    {
        humidity += GamePlayManager.Instance.humidity;
    }

    private void AnimalAttack()
    {
        if (animals.Count <= 0) return;
        if (isBinilActive) return;

        foreach (var land in childLands)
        {
            land.growTimer -= 1;
        }
        damageAnim.SetTrigger("Show");
    }

    public void SetBinil()
    {
        isBinilActive = !isBinilActive;
        binil.SetActive(isBinilActive);
    }

    public void AddHarvestCount()
    {
        harvestCount++;

        animalProbability = 0;

        if (harvestCount >= 3)
            animalProbability += 5;

        for (int i = 0; i < (harvestCount - 3); i++)
            animalProbability += 2;
    }

    private void Update()
    {
        foreach (SkinnedMeshRenderer land in lands)
        {
            land.materials[1].SetColor("_Color", Color.Lerp(land.materials[1].GetColor("_Color"), humidity > 50f ? waterColor : unWaterColor, Time.deltaTime * 10f));
        }

        if (isAutoHarvastBuild)
        {
            centerLand.isPlowing = false;

            if (!isAutoHarvastRunning)
            {
                foreach (FarmLand land in childLands)
                {
                    if (land.IsCanHarvast())
                    {
                        isAutoHarvastRunning = true;
                        StartCoroutine(AutoHarvast());
                        break;
                    }
                }
            }
        }

        previewParent.SetActive(!isUnlock);
        landParent.SetActive(isUnlock);

        int currentHour = GamePlayManager.Instance.GetTime()[1];
        if (lastHour != currentHour)
        {
            lastHour = currentHour;
            int randomValue = Random.Range(1, 101);
            if (randomValue <= animalProbability)
            {
                SpawnAnimal();
                harvestCount = 0;
                animalProbability = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
            SpawnAnimal();
    }

    private IEnumerator AutoHarvast()
    {
        autoHarvastAnim.SetTrigger("Active");

        yield return new WaitForSeconds(6.5f);

        foreach (FarmLand land in childLands)
        {
            if (land.IsCanHarvast() && !GamePlayManager.Instance.inventory.IsMaxItem())
            {
                GamePlayManager.Instance.inventory.AddList(land.currentCrop.cropInfo);
                land.Harvast();
            }
        }

        yield return new WaitForSeconds(3.5f);
        isAutoHarvastRunning = false;
        yield break;
    }

    private void SpawnAnimal()
    {
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            GameObject animal = Instantiate(animalPrefab);
            animal.GetComponent<AnimalMove>().center = transform.position;
            animals.Add(animal);
        }
    }

    public bool IsCanCreateAutoHarvast()
    {
        return centerLand.currentCrop == null;
    }

    public void CreateAutoHarvast()
    {
        autoHarvast.Create();
        isAutoHarvastBuild = true;
    }

    public void RemoveAnimals()
    {
        int count = 0;

        for (int i = animals.Count - 1; i >= 0; i--)
        {
            Destroy(animals[i].gameObject);
            animals.RemoveAt(i);
            count++;

            if (count >= 3)
                break;
        }
    }
}
