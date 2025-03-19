using Fractura.CraftingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Topdown.Movement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class WorldEventManager : SingletonMonoBehaviour<WorldEventManager>
{
    public event Action<CraftingObject> OnDynamicOutcomeExecuted;

    [SerializeField] private List<DynamicOutcome> dynamicOutcomes;

    [Header("Objective Texts")]
    [SerializeField] private TextMeshProUGUI woodFireText;
    [SerializeField] private TextMeshProUGUI chickenText;
    [SerializeField] private TextMeshProUGUI villageObjectiveText;


    [Header("Wood Fire stuff")]
    [SerializeField] private List<Vector3Int> woodFirePositions;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileBase woodFireSprite;

    [Header("Village stuff")]
    [SerializeField] private bool isChickenMade = false;
    [SerializeField] private bool isWoodfireMade = false;
    [SerializeField] private bool villageObjectiveDone = false;
    [SerializeField] private GameObject boneWeapon;
    [SerializeField] private Vector2 boneWeaponPos;

    [Header("Graveyard Stuff")]
    [SerializeField] public int ratKillCount = 0;
    private bool isGhostRitualReady = false;
    [SerializeField] private TextMeshProUGUI miceObjectiveText;
    [SerializeField] private TextMeshProUGUI skeletonObjectiveText;
    [SerializeField] private List<GameObject> spirits;
    [SerializeField] private Sprite skeleton;
    [SerializeField] private CraftingObject skeletonRitualItem;

    [Header("Final Animation Stuff")]
    [SerializeField] private GameObject targetCamera;
    [SerializeField] private List<Transform> targetSkeletonPositions;
    [SerializeField] private AudioClip ghostClip;
    [SerializeField] private AudioClip demonClip;
    [SerializeField] private GameObject demon;
    [SerializeField] private Transform demonPos;
    [SerializeField] private Volume postProcessVolume;
    private Vignette vignette;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private TextMeshProUGUI endScreenText;
    [SerializeField] private PlayerMovementController player;
    

    private void Start()
    {
        postProcessVolume.profile.TryGet(out vignette);
    }

    public void BroadcastOutcome(CraftingObject executedObject)
    {
        Debug.Log($"WorldEventManager: Outcome executed for {executedObject.ObjectName}");
        OnDynamicOutcomeExecuted?.Invoke(executedObject);

        switch (executedObject.effectType)
        {
            case CraftingEffectType.ChickenMeal:
                Debug.Log("got them chickens");
                //NPCManager.Instance.OnChickenServed();
                CraftingUIManager.Instance.AddLog("<color=green>Chicken</color> Objective Completed.");
                chickenText.fontStyle = FontStyles.Strikethrough | FontStyles.Bold | FontStyles.Italic;
                isChickenMade = true;
                //chickenText.fontWeight = FontWeight.Bold;
                break;
            
            case CraftingEffectType.WoodFire:
                Debug.Log("Wood fire made");
                SwapAreaTiles();
                CraftingUIManager.Instance.AddLog("<color=green>Wood Fire</color> Objective Completed.");
                woodFireText.fontStyle = FontStyles.Strikethrough | FontStyles.Bold | FontStyles.Italic;
                isWoodfireMade = true;
                break;

            case CraftingEffectType.SkeletonRitual:
                foreach (GameObject spirit in spirits)
                {
                    spirit.GetComponent<SpriteRenderer>().sprite = skeleton;
                    spirit.GetComponent<NPCDialogue>().ChangeDialogue("We're ready for our master");
                }

                CraftingUIManager.Instance.AddLog("<color=green>Skeletons</color> are spawned.");
                skeletonObjectiveText.fontStyle = FontStyles.Strikethrough | FontStyles.Bold | FontStyles.Italic;

                Inventory.Instance.AddItem(skeletonRitualItem);

                break;

            case CraftingEffectType.SpawnDemon:
                StartCoroutine(FinalAnimation());
                break;

            default:
                // No dynamic effect.
                break;
        }


        // alternative for designer friendly mechanism
        //foreach (var outcome in dynamicOutcomes)
        //{
        //    if (outcome.effectType == executedObject.effectType)
        //    {
        //        outcome.OnOutcomeExecuted.Invoke();
        //    }
        //}

        if (!villageObjectiveDone)
        {
            if(isChickenMade && isWoodfireMade)
            {
                NPCManager.Instance.OnChickenServed();
                CraftingUIManager.Instance.AddLog("<color=green>You fed the village.</color>");
                villageObjectiveText.fontStyle = FontStyles.Strikethrough | FontStyles.Bold | FontStyles.Italic;
                villageObjectiveDone = true;

                CraftingUIManager.Instance.AddLog("The villagers have gifted you the bone weapon. Pick it up!");
                Instantiate(boneWeapon, boneWeaponPos, Quaternion.identity);

            }
        }

        

    }

    private void SwapAreaTiles()
    {
        foreach(Vector3Int pos in woodFirePositions)
        {
            tileMap.SetTile(pos, woodFireSprite);
        }
    }

    public void IncreaseRatKillCount(int i)
    {
        if(ratKillCount >= 3)
        {
            return;
        }

        ratKillCount += i;

        if (ratKillCount == 2)
        {
            CraftingUIManager.Instance.AddLog("<color=green>The ghosts are ready for the ritual.</color>");
            miceObjectiveText.fontStyle = FontStyles.Strikethrough | FontStyles.Bold | FontStyles.Italic;
            isWoodfireMade = true;
            isGhostRitualReady = true;
        }
    }

    // Final State Sequence
    private IEnumerator FinalAnimation()
    {
        AudioManager.Instance.PlaySinisterTrack();
        player.StopMovement();
        yield return new WaitForSeconds(2f);

        CameraControl.Instance.ChangeCamera(targetCamera);

        yield return new WaitForSeconds(1f);

        for(int i = 0; i < spirits.Count; i++)
        {
            spirits[i].transform.position = targetSkeletonPositions[i].position;
            AudioManager.Instance.PlayEventAudio(ghostClip);

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);

        Instantiate(demon, demonPos.position, Quaternion.identity);
        AudioManager.Instance.PlayEventAudio(demonClip);

        yield return new WaitForSeconds(0.5f);

        vignette.intensity.value = 0;
        vignette.active = true;

        while(vignette.intensity.value < 1f)
        {
            vignette.intensity.value += 0.3f;
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.5f);

        Image blackScreenImg = blackScreen.GetComponent<Image>();
        Color color = blackScreenImg.color;
        blackScreen.gameObject.SetActive(true);

        while(blackScreenImg.color.a < 1f)
        {
            color.a = Mathf.Clamp01(color.a + 0.3f);
            blackScreenImg.color = color;
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.5f);

        endScreenText.gameObject.SetActive(true);

        yield return null;
    }
}



[Serializable]
public class DynamicOutcome
{
    [Tooltip("The effect type that triggers this outcome")]
    public CraftingEffectType effectType;

    [Tooltip("The actions to perform when this outcome is executed.")]
    public UnityEvent OnOutcomeExecuted;
}