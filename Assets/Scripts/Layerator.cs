using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layerator : MonoBehaviour
{
    public GroundSlicer gs;
    public SpriteRenderer sp;

    private void Start() {
    }

    private void Update() {
        sp.sortingOrder = -gs.getMonoSlice(this.transform.position.y);
    }

}
