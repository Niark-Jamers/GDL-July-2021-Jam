using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

// [ExecuteAlways]
public class GroundSlicer : MonoBehaviour
{

    EdgeCollider2D ec;
    public EdgeCollider2D killEc;

    public bool reset = false;

    public GameObject enemyParent;
    public Transform testPlayer;
    [HideInInspector]
    public float gUp;
    float gDown;
    [HideInInspector]
    public float gLeft;
    float gRight;

    [HideInInspector]
    public List<float> slicedGround = new List<float>();

    public float sliceInterval = 1;

    public static GroundSlicer Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

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

    public int[] getSlices(float y, int spread = 3)
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
        ReBoundGround();
    }

    void ReBoundGround()
    {
        Vector2[] tmp = ec.points;
        Vector2[] tmpKill = killEc.points;
        
        for (int i = 0; i < tmp.Length; i++)
        {
            if (tmp[i].y > -1)
            {
                //Debug.Log(tmp);
                tmp[i].y = GameManager.Instance.playerPosition.localScale.y;
            }
        }
        for (int i = 0; i < tmpKill.Length; i++)
        {
            if (tmpKill[i].y > -1)
            {
                //Debug.Log(tmpKill);
                tmpKill[i].y = GameManager.Instance.playerPosition.localScale.y + 1;
            }
        }
        ec.SetPoints(tmp.ToList());
        killEc.SetPoints(tmpKill.ToList());
        SetBounds();
        SliceGround();
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
        // 

            foreach (float f in slicedGround)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawLine(new Vector3(gLeft, f, 0), new Vector3(gRight, f, 0));
            }
            foreach (Transform ts in enemyParent.transform)
            {
                int[] tmp = getSlices(ts.position.y, ts.gameObject.GetComponent<EnemyController>().spread);
                foreach (int i in tmp)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(new Vector3(ts.position.x, sliceInterval * i - sliceInterval / 2, 0), sliceInterval / 2);
                }
            }
            int[] tmp2 = getSlices(testPlayer.position.y, testPlayer.gameObject.GetComponent<Playerator>().spread);
            foreach (int i in tmp2)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(new Vector3(testPlayer.position.x, sliceInterval * i - sliceInterval / 2, 0), sliceInterval / 2);
                }
        }
    }

