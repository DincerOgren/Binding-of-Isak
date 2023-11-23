using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Timers;
using UnityEngine;

namespace ED
{
    public class PlayerMovement : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] float defMovementSpeed = 5f;


        public float moveTowards = 5f; // 4 or 5 ideal value

        public float smoothedXInput = 0;
        public float smoothedYInput = 0;

        [SerializeField]private float currentMovementSpeed = 0f;
        public Vector2 movementDir;

        // STATIC VARIABLES
        Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            currentMovementSpeed = defMovementSpeed;
        }

        private void Update()
        {
            movementDir = HandleInputs();
        }


        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (movementDir.magnitude > 1)
            {
                movementDir.Normalize();
            }
            rb.velocity = movementDir * currentMovementSpeed;
        }

        private Vector2 HandleInputs()
        {

           float xInput = Input.GetAxisRaw("Horizontal");
           float yInput = Input.GetAxisRaw("Vertical");
            
            CalculateSmoothValues(xInput,yInput);

            return new Vector2(smoothedXInput, smoothedYInput);
        }

        private void CalculateSmoothValues(float x, float y)
        {
            if (x > 0 || x < 0)
            {
                if (x > 0 && smoothedXInput < 0)
                {
                    smoothedXInput = 0;
                }
                else if (x < 0 && smoothedXInput > 0)
                {
                    smoothedXInput = 0;
                }
                smoothedXInput = Mathf.MoveTowards(smoothedXInput, x, moveTowards * Time.deltaTime);
            }
            else
            {
                smoothedXInput = Mathf.MoveTowards(smoothedXInput, 0, moveTowards * Time.deltaTime);
            }


            if (y > 0 || y < 0)
            {
                if (y > 0 && smoothedYInput < 0)
                {
                    smoothedYInput = 0;
                }
                else if (y < 0 && smoothedYInput > 0)
                {
                    smoothedYInput = 0;
                }
                smoothedYInput = Mathf.MoveTowards(smoothedYInput, y, moveTowards * Time.deltaTime);
            }
            else
            {
                smoothedYInput = Mathf.MoveTowards(smoothedYInput, 0, moveTowards * Time.deltaTime);
            }
        }
    }
}
