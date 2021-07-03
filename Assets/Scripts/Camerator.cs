using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteAlways]
public class Camerator : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera vCam;
    public EdgeCollider2D ec;

    float updatedCamAtPos;

    float ratio;
    float XStart;
    float XEnd;
    float YStart = 0;
    float YEnd = 0;

    Vector2 startPos;
    Vector2 camPos;

    Vector2 rayDir;
    private RaycastHit2D[] hit;
    LayerMask mask;

    float targetSize;
    float speed = 3f;

    public float baseCamSize = 5;
    // Start is called before the first frame update
    void Start()
    {
        ratio = Camera.main.aspect;
        targetSize = baseCamSize;
        // Debug.Log(ratio);
        setX();
        YStart = GetLowYAtStart();
        YEnd = GetLowYAtEnd();
        startPos = new Vector2(XStart, YStart);
        camPos = new Vector2(this.transform.position.x, this.transform.position.y);
        rayDir = Vector2.down + Vector2.left * ratio;
        mask = LayerMask.GetMask("Wall");
    }

    void setX()
    {
        foreach (Vector2 p in ec.points)
        {
            XStart = (XStart > p.x) ? p.x : XStart;
            XEnd = (XEnd < p.x) ? p.x : XEnd;
        }
    }

    float GetLowYAtStart()
    {
        float tmp = ec.points[0].y;
        foreach (Vector2 v in ec.points)
        {
            if (v.x > XStart + 5 || v.x < XStart - 5)
                continue;
            tmp = (tmp > v.y) ? v.y : tmp;
        }
        return tmp;
    }

    float GetLowYAtEnd()
    {
        float tmp = ec.points[0].y;
        foreach (Vector2 v in ec.points)
        {
            if (v.x > XEnd + 5 || v.x < XEnd - 5)
                continue;
            tmp = (tmp > v.y) ? v.y : tmp;
        }
        return tmp;
    }

    void UpdateCamSize()
    {
        if (Mathf.Abs(this.transform.position.x - updatedCamAtPos) < 0.05)
            return;
        updatedCamAtPos = this.transform.position.x;
        camPos.x = this.transform.position.x;

        Debug.DrawRay(camPos, rayDir * 100, Color.red, Time.deltaTime);
        hit = Physics2D.RaycastAll(camPos, rayDir, 1000, mask);
        for (int i = 0; i < hit.Length; i++)
        {
            //Debug.Log("ca touche : " + hit[i].transform.tag);
            if (Vector2.Distance(camPos, hit[i].point) > 1)
                targetSize = -hit[i].point.y;
        }

        // Vector2 hypo = new Vector2(XEnd, YEnd) - new Vector2(XStart, YStart);
        // Vector2 adj = camPos - new Vector2(XStart, YStart);
        // float hyangle = Vector2.Angle(adj, hypo) * Mathf.Deg2Rad;
        // Vector2 op = Vector2.down * (Mathf.Tan(hyangle) * adj.magnitude);
        // Vector2 adjSub = adj - Vector2.right * (ratio * (baseCamSize + op.magnitude));
        // float size = op.magnitude / (adj.magnitude / adjSub.magnitude);
        // Debug.DrawLine(startPos, startPos + hypo, Color.red, Time.deltaTime);
        // Debug.DrawLine(startPos, startPos + adj, Color.yellow, Time.deltaTime);
        // Debug.DrawLine(camPos, camPos + op, Color.green, Time.deltaTime);
        // Debug.DrawLine(camPos, camPos - adj.normalized * (ratio * (op.magnitude)), Color.magenta, Time.deltaTime);
        // Debug.DrawLine(startPos, startPos + adjSub, Color.cyan, Time.deltaTime);
        // Debug.DrawLine(startPos + adjSub, startPos + adjSub + Vector2.down * size, Color.cyan, Time.deltaTime);
        // vCam.m_Lens.OrthographicSize = baseCamSize + size;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamSize();
        vCam.m_Lens.OrthographicSize = Mathf.Lerp(vCam.m_Lens.OrthographicSize, targetSize, Time.deltaTime * speed);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3(XStart, YStart, 0), 1);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(XEnd, YEnd, 0), 1);
    }
}
