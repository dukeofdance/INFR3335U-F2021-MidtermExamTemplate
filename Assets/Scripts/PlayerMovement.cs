using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    public CharacterController control;
    public Transform cam;
    public float moveSpeed = 10f;

    float turnSmoothing = 0.1f;
    float turnSmoothVel;

    int coinCount = 0;
    
    public Text coinText;

    bool isWalking;

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<CharacterController>();
        coinText.text = coinCount.ToString("0");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1)
        {
            isWalking = true;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothing);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            control.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        else
        {
            isWalking = false;
        }

        if (!isWalking)
        {
            GetComponent<Animator>().SetInteger("AnimatorState", 0);
        }
        else
        {
            GetComponent<Animator>().SetInteger("AnimatorState", 1);
        }
        Recount();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Coin")
        {
            Destroy(col.gameObject);
            coinCount++;        

        }
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    void Recount()
    {
        coinText.text = coinCount.ToString("");
    }
}
