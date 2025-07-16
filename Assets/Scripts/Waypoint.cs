using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Waypoint[] nextWaypoint;

    public Waypoint[] NextWaypoint { get => nextWaypoint; set => nextWaypoint = value; }
}
