using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public int startPoint;
    public Transform[] points;
    public float speed = 1f;
    public float delay = 1f;
    public bool automatic = true;
    private int i;
    private bool reverse = false;
    public bool moving = true;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startPoint].position;
        i = startPoint;
        if(automatic)
        {
            Invoke(nameof(StartMoving), delay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (automatic || moving)
        {
            Move();
        }
    }

    private void Move()
    {
        if(Vector3.Distance(transform.position, points[i].position) < 0.01f) {
            moving = false;

            if (automatic)
            {
                Invoke(nameof(StartMoving), delay);

            }

            if (i == points.Length - 1)
            {
                reverse = true;
                i--;
                return;
            } else if (i == 0 )
            {
                reverse = false;
                i++;
                return;
            }

            if(reverse)
            {
                i--;
            } else
            {
                i++;
            }
            
            
        }

        if(moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
    }

    public void StartMoving()
    {
        moving = true;
    }

    public void ToggleAutomatic()
    {
        automatic = !automatic;
    }
}
