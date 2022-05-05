using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    [SerializeField] Transform camTransform;
    [SerializeField] float playerSpeed;
    [SerializeField] float backSpeed;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float turnSmoothVelocity;

    [SerializeField] private Text healthValue;
    [SerializeField] private float rotateXspeed = 4.0f;

    int health = 100;
    int maxHealth = 100;
    int maxMedkitHealth = 50;

    public GameObject loadingPanel;
    float time = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        healthValue.text = health.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;
        if (time >=3f)
        {
            loadingPanel.SetActive(false);
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        var mouseHorizontal = Input.GetAxis("Mouse X");
        Vector3 direction = new Vector3(horizontal,0f,vertical);
        animator.SetFloat("Speed", direction.magnitude);
        this.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + mouseHorizontal * rotateXspeed, transform.localEulerAngles.z);

        if (direction.magnitude >=0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;//
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);//
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
            float moveSpeed = (vertical > 0) ? playerSpeed : backSpeed;
            characterController.SimpleMove(moveDir * moveSpeed);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MedBox")
        {
            int healthNeeded = maxHealth - health;
            if (maxMedkitHealth >= healthNeeded)
                health = health + healthNeeded;
            else
                health = health + maxMedkitHealth;

            healthValue.text = health.ToString();
            Debug.Log("Health:" + health);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "SpawningZombie")
        {
            ObjectPooling.Instance.AddToPool(10);
        }

        if ((other.gameObject.tag == "FinishPoint") && (this.gameObject.GetComponent<ScoreManager>().score >= 100))
        {
                SceneManager.LoadScene("GameOver");
        }
    }
    public void TakeHit(int value)
    {
        if (health >0)
        {
            health = Mathf.Clamp(health - value, 0,maxHealth);
            healthValue.text = health.ToString();
        }
    }
}
