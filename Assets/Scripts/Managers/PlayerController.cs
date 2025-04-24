using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private CharacterController characterController;
    private bool isGrounded;

    [Header("Audio")]
    [SerializeField] private float stepRate = 0.5f;
    private float stepCooldown;
    private AudioSource audioSource;

    [SerializeField]
    private int health = 10;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        _UI.UpdateHealth(health);

        if (GetComponent<AudioSource>() == null)
            gameObject.AddComponent<AudioSource>();

        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        //Check if we are touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        //Gets the input from the player
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Move the player
        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move *speed * Time.deltaTime);

        //Do the jump stuff
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        //Apply the gravity to our velocity
        velocity.y += gravity;
        characterController.Move(velocity * Time.deltaTime);

        //Audio footstep stuff
        stepCooldown -= Time.deltaTime;
        if(stepCooldown < stepRate && isGrounded && (move.x != 0 || move.z != 0))
        {
            stepCooldown = stepRate;
            _AUDIO.PlayFootstep(audioSource);
        }
    }

    public void TakeDamage(int _damage)
    {
        health -= _damage;
        _UI.UpdateHealth(health);
    }
}
