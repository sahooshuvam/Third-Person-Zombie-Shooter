using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    [SerializeField] Transform camTransform;
    [SerializeField] float playerSpeed;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float turnSmoothVelocity;

    [SerializeField] private Text healthValue;

    int health = 100;
    int maxHealth = 100;
    int maxMedkitHealth = 50;

  


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        healthValue.text = health.ToString();
    }
  

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal,0f,vertical);

        if (direction.magnitude >0f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;//
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);//
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
            characterController.Move(moveDir.normalized *playerSpeed *Time.deltaTime);
            animator.SetFloat("Speed",direction.magnitude);
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
