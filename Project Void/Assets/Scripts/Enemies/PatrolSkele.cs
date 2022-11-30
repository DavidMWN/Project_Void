using UnityEngine;
using UnityEngine.SceneManagement;

public class PatrolSkele : MonoBehaviour
{
    Animator anim;
    SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    
    public float speed = 3.0f;

    public bool isDead = false;
    public Sprite deadSprite;

    public SceneInfo sceneInfo;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        isDead = sceneInfo.skele1Dead;

        transform.position = sceneInfo.skele1Point;

        if (isDead)
        {
            anim.enabled = false;
            spriteRenderer.sprite = deadSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDead)
        {
            if (other.tag == "Player")
            {
                sceneInfo.playerPoint = other.transform.position;
                sceneInfo.skele1Point = transform.position;
                SceneManager.LoadScene("Battle");
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
            {
                currentWaypointIndex++;

                if (currentWaypointIndex >= waypoints.Length)
                    currentWaypointIndex = 0;
            }

            if ((waypoints[currentWaypointIndex].transform.position.x) < transform.position.x)
                anim.SetFloat("Direction X", 0);
            else if ((waypoints[currentWaypointIndex].transform.position.x) > transform.position.x)
                anim.SetFloat("Direction X", 1);

            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
        }
    }

    public void SetDead()
    {
        isDead = true;
    }
}
