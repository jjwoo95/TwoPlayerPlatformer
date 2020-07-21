using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(Controller2D))]
public class PlayerOne : MonoBehaviour {

    // bullet and player
    public GameObject bullet;
    private SpriteRenderer redPlayer;

    // movement variables
    public float jumpHeight = 4;
    public float timeToJumpApex = 0.4f;
    float moveSpeed = 25;
    float accelationTimeAirborne = 0.2f;
    float accelerationTimeGrounded = 0.1f;

    // movement variable adjusters
    float gravity;
    Vector3 velocity;
    float jumpVelocity;
    float velocityXSmoothing;

    // movement and action controls
    Vector2 input;
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode jump;
    public KeyCode shoot;
    public KeyCode sprint;
    public KeyCode useSword;
    public KeyCode shootTopRight;
    public KeyCode shootBottomRight;
    public KeyCode shootBottomLeft;
    public KeyCode shootTopLeft;

    // player controller
    Controller2D controller;

    // bullet direction
    public Quaternion bulletRotation = Quaternion.identity;
    private Rigidbody2D bulletDirection;
    public bool bulletLeft, bulletRight;
    public bool bulletTopRight, bulletTopLeft, bulletBottomRight, bulletBottomLeft;
    public bool playerTwo;

    // rate of fire
    private float fireRate = 0.35F;
    private float nextFire = 0.0F;

    // sword direction
    public GameObject sword;
    public Quaternion swordRotation = Quaternion.identity;
    private float swordDirection;

    // player one stamina
    PlayerStamina playerOneStamina;
    private float sprintSpeed = 50;

    // wall slide when wall jumping
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = 0.25f;

    // wall jump variables
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    // Use this for initialization
    void Start () {

        // get component for controller, sprite and stamina
        controller = GetComponent<Controller2D>();
        redPlayer = GetComponent<SpriteRenderer>();
        playerOneStamina = GetComponent<PlayerStamina>();

        // adjust for gravity
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        // bullet facing right
        bulletRotation.eulerAngles = new Vector3(0, 0, -90);
        bulletRight = true;
        bulletLeft = false;

        // rotate sword 
        swordRotation.eulerAngles = new Vector3(0, 0, 0);
        swordDirection = 1f;

        // flip player direction 
        redPlayer.flipX = true;
    }
	
	// Update is called once per frame
	void Update () {

        // get player input for direction and which direction they are colliding inot the wall
        Vector2 input = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        // currently not sliding on wall
        bool wallSliding = false;

        // check if sliding on wall
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            // currently colliding into wall
            wallSliding = true;

            // slide down wall slowly
            if(velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }
        }

        // stop when hitting object from above or below
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        // player wants to move left
        if (Input.GetKey(moveLeft))
        {
            // check if player can sprint
            if(playerOneStamina.currentStamina > 0 && Input.GetKey(sprint) && playerOneStamina.fiftyStamina)
            {
                // move player faster to sprint and decrease stamina
                input.x = -sprintSpeed;
                playerOneStamina.decreaseStamina();
            }

            // do not sprint
            else
            {
                // move player at normal speed
                input.x = -moveSpeed;

                // increase player stamina when not sprinting
                if(playerOneStamina.currentStamina < playerOneStamina.maxStamina)
                    playerOneStamina.increaseStamina();
            }

            // face bullet to the left
            bulletRotation.eulerAngles = new Vector3(0, 0, 90);
            resetBulletDirection();
            bulletLeft = true;

            // face sowrd to the left
            swordDirection = -1f;
            swordRotation.eulerAngles = new Vector3(0, 0, 90);

            // face player to the left
            redPlayer.flipX = false;
        }

        // player want to move right
        else if(Input.GetKey(moveRight))
        {
            // check if player can sprint
            if (playerOneStamina.currentStamina > 0 && Input.GetKey(sprint) && playerOneStamina.fiftyStamina)
            {
                // move player faster to sprint and decrease stamina
                input.x = sprintSpeed;
                playerOneStamina.decreaseStamina();
            }

            // do not sprint
            else
            {
                // move player at normal speed
                input.x = moveSpeed;

                // increase player stamina when not sprinting
                if (playerOneStamina.currentStamina < playerOneStamina.maxStamina)
                    playerOneStamina.increaseStamina();
            }

            // face bullet to the right
            bulletRotation.eulerAngles = new Vector3(0, 0, -90);
            resetBulletDirection();
            bulletRight = true;

            // face sword to the right
            swordDirection = 1f;
            swordRotation.eulerAngles = new Vector3(0, 0, 0);

            // face player to the right
            redPlayer.flipX = true;
        }

        // player does not want to move
        else
        {
            // do not move player
            input.x = 0;

            // increase player stamina
            if (playerOneStamina.currentStamina < playerOneStamina.maxStamina)
                playerOneStamina.increaseStamina();
        }

        // player wants to shoot diaginally up and right
        if (Input.GetKey(shootTopRight) && Time.time > nextFire)
        {
            // face bullet up and right
            bulletRotation.eulerAngles = new Vector3(0, 0, -45);
            resetBulletDirection();
            bulletTopRight = true;

            // create three different bullets
            Instantiate(bullet, transform.position, bulletRotation);
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), bulletRotation);
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), bulletRotation);

            // add cooldown to fire rate
            nextFire = Time.time + fireRate;
        }

        // player wants to shoot diaginally down and right
        else if (Input.GetKey(shootBottomRight) && Time.time > nextFire)
        {
            // face bullet down and right
            bulletRotation.eulerAngles = new Vector3(0, 0, -135);
            resetBulletDirection();
            bulletBottomRight = true;

            // create three different bullets
            Instantiate(bullet, transform.position, bulletRotation);
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), bulletRotation);
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), bulletRotation);

            // add cooldown to fire rate
            nextFire = Time.time + fireRate;
        }

        // player wants to shoot diaginally up and left
        else if (Input.GetKey(shootTopLeft) && Time.time > nextFire)
        {
            // face bullet up and left
            bulletRotation.eulerAngles = new Vector3(0, 0, 45);
            resetBulletDirection();
            bulletTopLeft = true;

            // create three different bullets
            Instantiate(bullet, transform.position, bulletRotation);
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), bulletRotation);
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), bulletRotation);

            // add cooldown to fire rate
            nextFire = Time.time + fireRate;
        }

        // player wants to shoot diaginally down and left
        else if (Input.GetKey(shootBottomLeft) && Time.time > nextFire)
        {
            // face bullet down and left
            bulletRotation.eulerAngles = new Vector3(0, 0, 135);
            resetBulletDirection();
            bulletBottomLeft = true;

            // create three different bullets
            Instantiate(bullet, transform.position, bulletRotation);
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), bulletRotation);
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), bulletRotation);

            // add cooldown to fire rate
            nextFire = Time.time + fireRate;
        }

        // player wants to jump
        if (Input.GetKey(jump))
        {
            // check if sliding on wall
            if(wallSliding)
            {
                // player jumps to same wall they are sliding on
                if(wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }

                // player jumps off wall
                else if(input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }

                // player jumps off wall and goes in opposite direction of wall
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }

            // be able to jump when on ground
            if(controller.collisions.below)
            {
                velocity.y = jumpVelocity;
            }
        }

        // player wants to shoot forward
        if (Input.GetKey(shoot) && Time.time > nextFire)
        {
            // create three different bullets
            Instantiate(bullet, transform.position, bulletRotation);
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), bulletRotation);
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), bulletRotation);

            // add cooldown to fire rate
            nextFire = Time.time + fireRate;
        }

        // player wants to use sword
        if (Input.GetKey(useSword) && Time.time > nextFire)
        {
            // create sword
            Instantiate(sword, new Vector3(transform.position.x + swordDirection, transform.position.y, transform.position.z), swordRotation);

            // add cooldown to fire rate
            nextFire = Time.time + fireRate;
        }
     
        // move player
        float targetVelocityX = input.x * moveSpeed * Time.deltaTime;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)? accelerationTimeGrounded: accelationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
	}

    void resetBulletDirection()
    {
        // reset all directions for bullet
        bulletLeft = false;
        bulletRight = false;
        bulletTopRight = false;
        bulletTopLeft = false;
        bulletBottomRight = false;
        bulletBottomLeft = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // tag of object colliding into
        string name = other.gameObject.tag;

        // player runs into death zone
        if (name == "DeathLayer")
        {
            // destroy player and restart game
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
