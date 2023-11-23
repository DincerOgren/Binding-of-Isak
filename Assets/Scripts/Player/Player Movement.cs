using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Timers;
using UnityEngine;

namespace ED {
    public class PlayerMovement : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] float defMovementSpeed = 5f;


        public bool isForce=false;
        public bool velocitySub = false;
        public float moveTowards=.2f;
        
          public  float xInput = 0;

            public float yInput = 0;
        public float smoothedX=0;
        public float smoothedY=0;
        private float currentMovementSpeed = 0f;
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
                rb.velocity = movementDir * defMovementSpeed;
        }


        private Vector2 HandleInputs()
        {
            
                xInput = Input.GetAxisRaw("Horizontal");
                yInput = Input.GetAxisRaw("Vertical");


            if (xInput > 0 || xInput < 0)
            {
                if (xInput>0 && smoothedX<0)
                {
                    smoothedX = 0;
                }
                else if (xInput<0 && smoothedX>0)
                {
                    smoothedX = 0;
                }
                smoothedX = Mathf.MoveTowards(smoothedX, xInput, moveTowards * Time.deltaTime);
                print("Sa X");
            }
            else {
                smoothedX = Mathf.MoveTowards(smoothedX, 0, moveTowards * Time.deltaTime);
                print("AS x");
                    }



            if (yInput > 0 || yInput < 0)
            {
                smoothedY=Mathf.MoveTowards(smoothedY, yInput, moveTowards );
            }
            else
                smoothedY = Mathf.MoveTowards(smoothedY, 0, moveTowards );
            //}
            //else
            //{
            //     xInput = Input.GetAxis("Horizontal");
            //     yInput = Input.GetAxis("Vertical");
            //}
            return new Vector2(smoothedX, smoothedY);
        }

    }
}
