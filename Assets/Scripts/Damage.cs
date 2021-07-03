using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Damage : MonoBehaviour
{
    [System.Serializable]
    public struct Profile
    {
        public float dmg;
        public float hitStun;
        public float knockback;
        [HideInInspector]
        public Vector2 dir;
    }
}


