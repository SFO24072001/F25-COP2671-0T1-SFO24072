using System.Collections;
using UnityEngine;

public class FirePower : MonoBehaviour
{
    [SerializeField]
    private float timerEnd = 5f;
    private void Start()
    {
        StartCoroutine(countDownTimer());
    }
    IEnumerator countDownTimer()
    {
        yield return new WaitForSeconds(timerEnd);

        gameObject.transform.localScale = Vector3.one;

        Destroy(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Debug.Log("I shot a fireball");
        }
    }
}
