using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ViewerController : MonoBehaviour
{
    float nextMessageTime = 0.5f;
    [field: SerializeField] int testCounter;
    public float viewerCounter;
    [field: SerializeField] public TextMeshProUGUI ViewerCounterLabel { get; private set; }
    // Start is called before the first frame update

    void Awake()
    {
        StartCoroutine(Loop());
    }
    void Start()
    {
        viewerCounter = 1;
        testCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        // if (Time.time > nextMessageTime)
        // {
        //     testCounter += 1;
        //     if (CheckProgress())
        //     {
        //         viewerCounter += Random.Range(4f, 11f);
        //     }
        //     if (CheckHype())
        //     {
        //         viewerCounter *= 1.05f;
        //     }
        //     if (CheckFall())
        //     {
        //         viewerCounter *= 1.5f;
        //     }
        //     ViewerCounterLabel.text = "<color=\"red\">" + (int)viewerCounter;
        // }
        // nextMessageTime = Time.time + 2f;
    }

    public bool CheckProgress()
    {
        return true;
    }

    public bool CheckHype()
    {
        return false;
    }

    public bool CheckFall()
    {
        return false;
    }

    IEnumerator Loop(){
        while(true){
            yield return new WaitForSecondsRealtime(1);
            testCounter += 1;
            if (CheckProgress())
            {
                viewerCounter += Random.Range(4f, 11f);
            }
            if (CheckHype())
            {
                viewerCounter *= 1.05f;
            }
            if (CheckFall())
            {
                viewerCounter *= 1.5f;
            }
            ViewerCounterLabel.text = "<color=\"red\">" + (int)viewerCounter;
        }
    }
}
