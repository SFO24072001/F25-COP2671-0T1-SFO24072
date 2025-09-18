using Unity.VisualScripting;
using UnityEngine;

public class Mushroom : Interactable
{
    protected override void UseInteractable(PlayerController player)
    {   
        player.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
        player.AddComponent<FirePower>();



        base.UseInteractable(player);
    }
}
