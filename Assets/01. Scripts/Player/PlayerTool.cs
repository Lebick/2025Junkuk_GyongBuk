using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTool : MonoBehaviour
{
    private PlayerController player;

    public Transform currentTile;
    public Transform currentLand;

    public SpawnTool spawnTool;

    public int waterUseCount;
    public int maxWaterCount;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if ((player.currentTool != null || player.currentSeed != null) && GamePlayManager.Instance.fatigue >= 100)
            {
                UIManager.Instance.SetAlert("³ª ³Ê¹« Èûµå·· ¤Ð¤Ð");
                return;
            }

            if (player.currentTool != null)
                Invoke(player.currentTool.functionName, 0f);

            if (player.currentSeed != null && currentLand != null &&
                currentLand.GetComponent<FarmLand>().currentCrop == null && currentLand.GetComponent<FarmLand>().isPlowing)
            {
                if (GamePlayManager.Instance.inventory.UseSeed(player.currentSeed))
                    GrowCrop();
                else
                    UIManager.Instance.SetAlert("¾¾¾ÑÀÌ ºÎÁ·ÇÔ ¤µ¤¡");
            }
        }
    }

    private IEnumerator SpawningToolMesh(Transform meshTransform, IEnumerator afterCoroutine)
    {
        player.isCantMove = true;
        spawnTool.SpawningTool(meshTransform);

        yield return new WaitForSeconds(2f);

        StartCoroutine(afterCoroutine);
    }

    private void Water()
    {
        if (waterUseCount >= maxWaterCount)
        {
            print("¹°ÀÌ¾øÀÝ¾Æ");
            return;
        }

        if (currentTile == null)
        {
            print("Å¸ÀÏÀÌ¾øÀÝ¾Æ");
        }
        else
        {
            GamePlayManager.Instance.fatigue += 2;

            waterUseCount++;

            StartCoroutine(SpawningToolMesh(spawnTool.water, WaterCoroutine()));
        }
    }

    private IEnumerator WaterCoroutine()
    {
        player.isCantMove = true;
        player.anim.SetTrigger("Water");
        yield return new WaitForSeconds(1.5f);
        currentTile.GetComponent<FarmTile>().humidity += 50f;
        player.isCantMove = false;

        spawnTool.RemoveTool(spawnTool.water);
    }

    private void Not()
    {
        if (currentLand == null)
        {
            print("¶¥ÀÌ¾øÀÝ¾Æ");
        }
        else
        {
            if (!currentLand.GetComponent<FarmLand>().isPlowing)
            {
                GamePlayManager.Instance.fatigue += 3;
                StartCoroutine(SpawningToolMesh(spawnTool.rake, PlowingCoroutine()));
            }

            else if (currentLand.GetComponent<FarmLand>().IsCanHarvast())
            {
                GamePlayManager.Instance.fatigue += 5;
                StartCoroutine(SpawningToolMesh(spawnTool.rake, HarvastCoroutine()));
            }
        }
    }

    //¹ç°¥±â
    private IEnumerator PlowingCoroutine()
    {
        player.anim.SetTrigger("Plowing");
        yield return new WaitForSeconds(75f / 60f);
        currentLand.GetComponent<FarmLand>().isPlowing = true;
        yield return new WaitForSeconds(75f / 60f);
        player.isCantMove = false;

        spawnTool.RemoveTool(spawnTool.rake);
    }


    //¼öÈ®
    private IEnumerator HarvastCoroutine()
    {
        player.isCantMove = true;
        player.anim.SetTrigger("Harvast");
        yield return new WaitForSeconds(1f);

        Color start = Color.black;
        Color end = Color.black;
        start.a = 0;

        FadeManager.Instance.SetFade(start, end, 1f, () =>
        {
            GamePlayManager.Instance.inventory.AddList(currentLand.GetComponent<FarmLand>().currentCrop.cropInfo);
            currentLand.GetComponent<FarmLand>().Harvast();
            FadeManager.Instance.SetFade(end, start, 1f);
        });

        yield return new WaitForSeconds(1f);



        player.isCantMove = false;
    }

    private void Auto()
    {
        print("ÀÚµ¿");
    }

    private void Animal()
    {
        if (currentLand == null)
        {
            print("¶¥ÀÌ¾øÀÝ¾Æ");
        }
        else
        {
            GamePlayManager.Instance.fatigue += 5;
            StartCoroutine(SpawningToolMesh(spawnTool.net, NetCoroutine()));
        }
    }

    private IEnumerator NetCoroutine()
    {
        ThrowNet net = spawnTool.net.GetComponent<ThrowNet>();
        net.Throw(currentTile);
        yield return new WaitForSeconds(2f);

        Color start = Color.black;
        Color end = Color.black;
        start.a = 0;

        FadeManager.Instance.SetFade(start, end, 1f, () =>
        {
            net.ResetState();
        });

        yield return new WaitForSeconds(2f);
        net.gameObject.SetActive(false);
        FadeManager.Instance.SetFade(end, start, 1f);

        player.isCantMove = false;

        yield break;
    }

    private void Binil()
    {
        player.isCantMove = true;

        Color start = Color.black;
        Color end = Color.black;
        start.a = 0;

        FadeManager.Instance.SetFade(start, end, 1f, () =>
        {
            currentTile.GetComponent<FarmTile>().SetBinil();
            player.isCantMove = false;
            FadeManager.Instance.SetFade(end, start, 1f);
        });
    }

    private void GrowCrop()
    {
        StartCoroutine(SowingCoroutine());
    }

    private IEnumerator SowingCoroutine()
    {
        player.isCantMove = true;
        player.anim.SetTrigger("Sowing");
        yield return new WaitForSeconds(0.8f);
        currentLand.GetComponent<FarmLand>().PlantCrop(player.currentSeed);
        player.isCantMove = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tile"))
        {
            currentLand = collision.transform;
            currentTile = collision.transform.parent;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tile"))
        {
            currentLand = collision.transform;
            currentTile = collision.transform.parent;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tile"))
        {
            currentLand = null;
            currentTile = null;
        }
    }
}
