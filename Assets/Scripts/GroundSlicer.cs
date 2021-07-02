using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteAlways]
public class GroundSlicer : MonoBehaviour
{

    EdgeCollider2D ec;

    public bool reset = false;

    public bool showChar = false;

    [Serializable]
    public struct testChar
    {
        public Transform testTransform;
        public int testSpread;
        public int[] testStorage;
    }

    [SerializeField]
    public List<testChar> TestList = new List<testChar>();

    float gUp;
    float gDown;
    float gLeft;
    float gRight;

    [HideInInspector]
    public List<float> slicedGround = new List<float>();

    public float sliceInterval = 1;

    void Start()
    {
        SetBounds();
        SliceGround();
    }

    //#region Slice
    void SetBounds()
    {
        ec = GetComponent<EdgeCollider2D>();
        float[] tmp = FindUpDownY();
        gUp = tmp[0];
        gDown = tmp[1];
        tmp = FindLeftRightX();
        gLeft = tmp[0];
        gRight = tmp[1];
    }

    float[] FindLeftRightX()
    {
        float[] tmp = new float[2] { 0, 0 };
        foreach (Vector2 p in ec.points)
        {
            tmp[0] = (tmp[0] > p.x) ? p.x : tmp[0];
            tmp[1] = (tmp[1] < p.x) ? p.x : tmp[1];
        }
        return tmp;
    }

    float[] FindUpDownY()
    {
        float[] tmp = new float[2] { 0, 0 };
        foreach (Vector2 p in ec.points)
        {
            tmp[0] = (tmp[0] < p.y) ? p.y : tmp[0];
            tmp[1] = (tmp[1] > p.y) ? p.y : tmp[1];
        }
        return tmp;
    }

    void SliceGround()
    {
        slicedGround.Clear();
        for (float i = gUp; i > gDown; i -= sliceInterval)
        {
            slicedGround.Add(i);
        }
    }

    //#region
    public int getMonoSlice(float y)
    {
        return (int)(y / sliceInterval);
    }

    public int[] getSlice(float y, int spread)
    {
        int[] tmp = new int[spread];
        int i = (int)(y / sliceInterval);
        i -= spread / 2;
        for (int j = 0; j < spread; j++)
        {
            tmp[j] = i + j;
            //Debug.Log(tmp[j]);
        }
        return tmp;
    }

    public bool CompareSlices(int[] s1, int[] s2)
    {
        foreach (int i in s1)
        {
            foreach (int j in s2)
            {
                if (i == j)
                    return true;
            }
        }
        return false;
    }

    void Update()
    {
        if (reset)
        {
            reset = false;
            SetBounds();
            SliceGround();
        }
        if (showChar)
        {
            for (int i = 0; i < TestList.Count; i++)
            {
                testChar tmp = TestList[i];
                tmp.testStorage = getSlice(tmp.testTransform.position.y, tmp.testSpread);
                TestList[i] = tmp;
            }
            // if (CompareSlices(TestList[0].testStorage, TestList[1].testStorage))
            //     Debug.Log("ca touche");
            // else
            //     Debug.Log("ca touche pas");
        }
    }

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawSphere(new Vector3(transform.position.x, gUp, 0), 1);
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(new Vector3(transform.position.x, gDown, 0), 1);
        // Gizmos.color = Color.green;
        // Gizmos.DrawSphere(new Vector3(gLeft, gDown / 2, 0), 1);
        // Gizmos.color = Color.blue;
        // Gizmos.DrawSphere(new Vector3(gRight, gDown / 2, 0), 1);
        foreach (float f in slicedGround)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(new Vector3(gLeft, f, 0), new Vector3(gRight, f, 0));
        }
        if (showChar)
        {
            foreach (testChar ts in TestList)
            {
                for (int i = 0; i < ts.testStorage.Length; i++)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(new Vector3(ts.testTransform.position.x, sliceInterval * ts.testStorage[i], 0), sliceInterval / 2);
                }
            }
        }
    }
}
