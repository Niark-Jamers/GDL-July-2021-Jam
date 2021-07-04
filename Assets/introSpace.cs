using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class introSpace : MonoBehaviour
{
    PlayableDirector pd;
    public GameObject truc;
    // Start is called before the first frame update
    void Start()
    {
        pd = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            truc.SetActive(false);
            pd.enabled = true;
        }

    }
}
