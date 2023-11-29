using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //very basic movement, needs improving with things like accelleration and deccelleration.
    private Rigidbody2D rb;
    [SerializeField] private Vector2 move;

    [Header("Stats")]
    [SerializeField] private float speed;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        if (DialogueManager.Instance.DialogueIsPlaying) return; //needs a better way but works for now
        Move();
    }

    private void GetInput()
    {
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    private void Move()
    {
        rb.velocity = move * speed;
    }
}
