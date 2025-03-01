using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : SingletonMonoBehaviour<NPCManager>
{
    [SerializeField] private List<NPCDialogue> npcs;
    [SerializeField] private List<string> chickenDialogues;

    public void OnChickenServed()
    {
        foreach(NPCDialogue npc in npcs)
        {
            int dialogueIndex = Random.Range(0, chickenDialogues.Count);
            npc.ChangeDialogue(chickenDialogues[dialogueIndex]);
        }
    }
}
