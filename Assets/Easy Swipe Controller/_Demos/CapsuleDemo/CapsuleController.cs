using UnityEngine;
using System.Collections;

namespace MOSoft.SwipeController
{
    public class CapsuleController : MonoBehaviour
    {
        //Speed of the player
        public float m_playerSpeed = 5f;

        //Size of the player
        public float m_playerSize = 1f;

        void Update()
        {
            //Control methods
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
            if (SwipeController.getInstance.isUpLeft())
            {
                moveUpLeft();
            }
            if (SwipeController.getInstance.isUpRight())
            {
                moveUpRight();
            }
            if (SwipeController.getInstance.isDownLeft())
            {
                moveDownLeft();
            }
            if (SwipeController.getInstance.isDownRight())
            {
                moveDownRight();
            }
        }

        //### Player movement methods ###
        private void moveUp()
        {
            transform.Translate(0, 0, m_playerSize / 2 * getFactor(), Space.Self);
        }

        private void moveDown()
        {
            transform.Translate(0, 0, -m_playerSize / 2 * getFactor(), Space.Self);
        }

        private void moveLeft()
        {
            transform.Translate(-m_playerSize / 2 * getFactor(), 0, 0, Space.Self);
        }

        private void moveRight()
        {
            transform.Translate(m_playerSize / 2 * getFactor(), 0, 0, Space.Self);
        }

        private void moveUpLeft()
        {
            transform.Translate(-m_playerSize / 2 * getFactor(), 0, m_playerSize / 2 * getFactor(), Space.Self);
        }

        private void moveUpRight()
        {
            transform.Translate(m_playerSize / 2 * getFactor(), 0, m_playerSize / 2 * getFactor(), Space.Self);
        }

        private void moveDownLeft()
        {
            transform.Translate(-m_playerSize / 2 * getFactor(), 0, -m_playerSize / 2 * getFactor(), Space.Self);
        }

        private void moveDownRight()
        {
            transform.Translate(m_playerSize / 2 * getFactor(), 0, -m_playerSize / 2 * getFactor(), Space.Self);
        }

        private float getFactor()
        {
            if (SwipeController.getInstance.isContinuousMovment())
            {
                return m_playerSpeed * Time.deltaTime;
            }
            return 1;
        }
    }
}