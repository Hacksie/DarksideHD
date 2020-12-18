using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] public bool doDrop = true;
        [SerializeField] public float drop = -5.0f;

        [SerializeField] private float startingDistance = 30.0f;
        [SerializeField] private float endingDistance = 15.0f;
        [SerializeField] private float dropTime = 1.0f;
        private float minHeight = 0;

        private Vector3 startingPosition;
        private Vector3 dropPosition;
        private bool started = false;
        private bool finished = false;
        private float timer = 0;

        void Start()
        {
            startingPosition = this.transform.position;
            if (doDrop)
            {
                dropPosition = startingPosition;
                dropPosition.y += drop;
                minHeight = dropPosition.y;
                this.transform.position = dropPosition;
            }
        }

        void Update()
        {
            var distance = (GameManager.Instance.Player.transform.position - this.transform.position).magnitude;

            if (!finished)
            {
                if (!started && distance <= startingDistance)
                {
                    timer = Time.time;
                    started = true;
                }

                if (started)
                {
                    this.transform.position = Vector3.Lerp(dropPosition, startingPosition, (Time.time - timer) / dropTime);
                    if (Time.time - timer > dropTime)
                    {
                        finished = true;
                    }
                }
            }


            // if (distance <= startingDistance && distance > endingDistance)
            // {
            //     var perc = (distance - endingDistance) / (startingDistance - endingDistance);
            //     var dropPosition = startingPosition;
            //     dropPosition.y = Mathf.Max(dropPosition.y + (perc * drop), minHeight);
            //     minHeight = dropPosition.y;
            //     this.transform.position = dropPosition;
            // }
            // else if (distance <= endingDistance)
            // {
            //     this.transform.position = startingPosition;
            // }
        }
    }
}