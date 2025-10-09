using UnityEngine;

public class TriggerItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obscuringItemFader = collision.gameObject.GetComponentsInChildren<ObjectFader>();

        if (obscuringItemFader.Length > 0)
        {
            for (int i = 0; i < obscuringItemFader.Length; i++)
            {
                obscuringItemFader[i].FadeOut();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var obscuringItemFader = collision.gameObject.GetComponentsInChildren<ObjectFader>();

        if (obscuringItemFader.Length > 0)
        {
            for (int i = 0; i < obscuringItemFader.Length; i++)
            {
                obscuringItemFader[i].FadeIn();
            }
        }
    }
}
