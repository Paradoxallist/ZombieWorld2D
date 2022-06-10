using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed;
    public float nextWayPointDistance;

    Path path;
    int currentWaypoint = 0;
    //bool reachEndOfPath = false; 

    Seeker seeker;
    Rigidbody2D rb;
    //MapController controller;

    void Start()
    {
        seeker = GetComponent<Seeker>();   
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("PathUpdate", 0f, 0.5f);
        
    }

    void PathUpdate()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null) return;
        
        if(Vector2.Distance(rb.position,target.position) < 0.05f)
        {
            rb.velocity = new Vector2(0,0);
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            //reachEndOfPath = true;
            return;
        }
        else
        {
            //reachEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }
    }
}
