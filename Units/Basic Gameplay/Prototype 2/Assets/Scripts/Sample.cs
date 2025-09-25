using UnityEngine;

public class Sample : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [field: SerializeField]
    private int ImageIndex { get; set; }


    public float Horizontal => Input.GetAxis(nameof(Horizontal));

    private void Update()
    {
        if (Horizontal != 0)
            Debug.Log(Horizontal);
    }
}


public class Sample2
{
    public Sample sample;

    public Sample2()
    {
        //sample.speed = 5;
        //sample.HorizontalInput = 5f;
        Debug.Log(sample.Horizontal);
    }
}