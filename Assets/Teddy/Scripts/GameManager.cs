using System;
using System.Runtime.InteropServices;
using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        public Vector3 startPosBall;
        public Vector3 startPosCannon;
        public Quaternion startAngleCannon;
        [SerializeField]
        public GameObject ballGameObject;
        [SerializeField]
        public GameObject cannonGameObject;
        public Camera camera;
        public float force;
        public float rotSpeed;
        public GameObject dummiesGameObject;
        float horiRotation;
        float vertRotation;
        public Cannon c;
        public UnityEngine.UI.Text txtScore;
        public UnityEngine.UI.Text txtLevel;
        public GameObject panelMenu;
        
        public GameObject[] levels;
        
        private void Start()
        {
            c = cannonGameObject.GetComponent<Cannon>();
            //startPosBall = new Vector3(13f, 1f, -11f);
            startPosBall = c.frontTransform.position;
            
            startPosCannon = new Vector3(9f, 1f, -11f);
            showMenu(true);
        }

        private void Update()
        {
            //only get input if menu is not active
            if (!panelMenu.activeSelf)
            {
                GetInputFire1();
                MoveCannon();
                UpdateUI();
            }
            //ball falls outside boundary, reset
            if (ballGameObject.transform.position.y < -20)
            {
                ResetCanon();
                ResetBall();
            }
        }

        //updates the score on the UI
        public void UpdateUI()
        {
            txtScore.text = getTotalScore(dummiesGameObject).ToString();
        }

        //loops through the group of dummies and add the score
        public float getTotalScore(GameObject dummies)
        {
            float totalScore = 0;
            foreach (var d in dummies.GetComponentsInChildren<RagdollScoring>())
            {
                totalScore += d.currentScore;
            }

            return totalScore;
        }

        //resets the position and aim of the canon
        public void ResetCanon()
        {
            //reset cannon
            cannonGameObject.transform.SetPositionAndRotation(startPosCannon, Quaternion.Euler(0,0,0));
            
        }

        //resets the position of the ball
        public void ResetBall()
        {
            ballGameObject.GetComponent<Rigidbody>().isKinematic = true; //stops the physics rotation
            ballGameObject.transform.SetPositionAndRotation(startPosBall, Quaternion.Euler(0,0,0));
            ballGameObject.GetComponent<Rigidbody>().isKinematic = false; //allows physics again
        }
        
        //shoots the ball
        public void Shoot(Vector3 direction, float force)
        {  
            //reset ball to be at the front of cannon
            ResetBall();
            ballGameObject.GetComponent<Rigidbody>().AddForce(direction * force);
        }

        public void GetInputFire1()
        {
            if (Input.GetButtonDown("Fire1") )
            {
                Vector3 dir = (c.frontTransform.position - c.backTransform.position).normalized;
                Shoot(dir, force);
                // Debug.Log($"shooting to {dir}");
            }
        }

        //initialise all the positions ready for a new game
        public void startNewGame()
        {
            ResetBall();
            ResetCanon();
            ResetScore();
            showMenu(false);

        }
        
        public void ResetScore()
        {
            txtScore.text = "0";
        }

        //aim the canon according to user input
        public void MoveCannon()
        {
            //get keyboard direction
            horiRotation = Input.GetAxis("Horizontal");
            vertRotation = Input.GetAxis("Vertical");
            
            cannonGameObject.transform.rotation = Quaternion.Euler(cannonGameObject.transform.rotation.eulerAngles +
                                                                   new Vector3(0, horiRotation * rotSpeed,
                                                                       vertRotation * rotSpeed));
        }

        public void toggleMenu()
        {
            bool current = panelMenu.activeSelf;
            showMenu(!current);
        }

        public void showMenu(bool show)
        {
            panelMenu.SetActive(show);
        }

        /// <summary>
        /// level selection triggered by the dropdown
        /// </summary>
        /// <param name="choice"></param>
        public void setLevel(int choice)
        {
            Debug.Log($"choice is {choice}");
            var levelPos = new Vector3(3.97f, 0, 0);
            var otherlevel = (choice + 1) % 2;
            levels[choice].transform.position = levelPos;
            levels[choice].SetActive(true);
            levels[otherlevel].SetActive(false);
        }
    }