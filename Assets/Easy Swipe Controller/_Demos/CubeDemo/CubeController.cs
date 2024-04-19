using UnityEngine;
using System.Collections;

namespace MOSoft.SwipeController
{
    public class CubeController : MonoBehaviour
    {
        //Speed of the player
        public float m_playerSpeed = 0.5f;

        //Size of the player
        public float m_playerSize = 1f;

        //Move the cube only when last movement is finished/stopped
        private bool m_isMoving = false;

        //Start Y axis positon
        private float m_startY = 0f;

        //Child object name from cube
        private const string TARGET_POINT = "TargetPoint";

        void Update()
        {
            //Control methods
            if (!SwipeController.getInstance.m_isPlaymakerSupported)
            {
                if (SwipeController.getInstance.isUp())
                {
                    moveUp();
                }
                if (SwipeController.getInstance.isDown())
                {
                    moveDown();
                }
                if (SwipeController.getInstance.isLeft())
                {
                    moveLeft();
                }
                if (SwipeController.getInstance.isRight())
                {
                    moveRight();
                }
            }
        }

        // Special movement method for rolling over the edges of the cube
        private IEnumerator DoRoll(Vector3 point, Vector3 axis, float angle)
        {
            float steps = Mathf.Ceil(m_playerSpeed * 30.0f);
            float angleStep = angle / steps;

            // Rotate the cube by the point, axis and angle  
            for (int i = 1; i <= steps; i++)
            {
                transform.RotateAround(point, axis, angleStep);
                yield return new WaitForSeconds(0.0033333f);
            }

            // move the TARGET_POINT to the center of the cube   
            transform.Find(TARGET_POINT).position = transform.position;

            // Make sure the y position is correct 
            Vector3 pos = transform.position;
            pos.y = m_startY;
            transform.position = pos;

            // Make sure the angles are snaping to 90 degrees.       
            Vector3 vec = transform.eulerAngles;
            vec.x = Mathf.Round(vec.x / 90) * 90;
            vec.y = Mathf.Round(vec.y / 90) * 90;
            vec.z = Mathf.Round(vec.z / 90) * 90;
            transform.eulerAngles = vec;

            // The cube is stoped  
            m_isMoving = false;
        }

        //### Player movement methods ###
        private void moveUp()
        {
            if (m_isMoving)
            {
                return;
            }
            m_isMoving = true;
            transform.Find(TARGET_POINT).Translate(0, -m_playerSize / 2, m_playerSize / 2, Space.Self);
            StartCoroutine(DoRoll(transform.Find(TARGET_POINT).position, Vector3.right, 90.0f));
        }

        private void moveDown()
        {
            if (m_isMoving)
            {
                return;
            }
            m_isMoving = true;
            transform.Find(TARGET_POINT).Translate(0, -m_playerSize / 2, -m_playerSize / 2, Space.Self);
            StartCoroutine(DoRoll(transform.Find(TARGET_POINT).position, -Vector3.right, 90.0f));
        }

        private void moveLeft()
        {
            if (m_isMoving)
            {
                return;
            }
            m_isMoving = true;
            transform.Find(TARGET_POINT).Translate(-m_playerSize / 2, -m_playerSize / 2, 0, Space.Self);
            StartCoroutine(DoRoll(transform.Find(TARGET_POINT).position, Vector3.forward, 90.0f));
        }

        private void moveRight()
        {
            if (m_isMoving)
            {
                return;
            }
            m_isMoving = true;
            transform.Find(TARGET_POINT).Translate(m_playerSize / 2, -m_playerSize / 2, 0, Space.Self);
            StartCoroutine(DoRoll(transform.Find(TARGET_POINT).position, -Vector3.forward, 90.0f));
        }
    }
}