using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

[RequireComponent(/*typeof(Animator), */typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class UnityChan2DController : MonoBehaviour
{
	[HideInInspector]
	public Image weaponIcon;


    [SerializeField]
    private WeaponController weaponController;

	public Slider hpSlider;
	public GameObject destroyEffect;

	public float hp = 10;
	public int playerId;
    public Camera camera;
    public float maxSpeed = 10f;
    public float jumpPower = 1000f;
    public Vector2 backwardForce = new Vector2(-4.5f, 5.4f);
    public LayerMask whatIsGround;

    public NetworkPlayerManager networkPlayerManager;
    public NetworkTransform networkTransform;

    private Animator m_animator;
	private BoxCollider2D m_boxcollier2D;
    private Rigidbody2D m_rigidbody2D;
    private bool m_isGround;
    //private const float m_centerY = 1.5f;
	private const float m_centerY = 0.2f;
    private State m_state = State.Normal;

	private bool isDirectionRight;
	float maxHp;


    void Reset()
    {
        Awake();

        // UnityChan2DController
        maxSpeed = 10f;
        jumpPower = 1000;
        backwardForce = new Vector2(-4.5f, 5.4f);
        whatIsGround = 1 << LayerMask.NameToLayer("Ground");

        // Transform
        transform.localScale = new Vector3(10, 10, 1);

        // Rigidbody2D
        m_rigidbody2D.gravityScale = 3.5f;
        m_rigidbody2D.fixedAngle = true;

        // BoxCollider2D
        m_boxcollier2D.size = new Vector2(1, 2.5f);
        m_boxcollier2D.offset = new Vector2(0, -0.25f);

        // Animator
        m_animator.applyRootMotion = false;
    }

    void Awake()
    {
        m_animator = GetComponent<Animator>();
		m_boxcollier2D = GetComponent<BoxCollider2D>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
		maxHp = hp;
		//m_centerY = (m_boxcollier2D.size.y / 2) + 0.25f;
    }

    void Start(){
        if (networkTransform.isLocalPlayer) {
            camera = GameObject.Find("MainCamera").GetComponent<Camera>();
			weaponIcon = GameObject.Find("Canvas/WeaponIcon").GetComponent<Image>();
        }
    }


    void Update()
    {
        if(camera != null){
            camera.transform.position = new Vector3(transform.position.x, transform.position.y, -20f);
        }

        if (m_state != State.Damaged)
        {
            if (networkTransform.isLocalPlayer) {
				float x = Input.GetAxis("Horizontal");
				bool jump = Input.GetButtonDown("Jump");
				Dig (Input.GetButtonDown("Fire2"));
				Move(x, jump);
				weaponController.Shot (Input.GetButton("Fire1"));
				weaponController.ChangeBullet (Input.GetAxis ("Mouse ScrollWheel"));
			} else {

			}
        }
    }

	void Dig(bool isDig){
		if(isDig){
			float digPointX;
            if (transform.localScale.x == 1) {
				digPointX = transform.position.x - 2.0f;
			} else {
				digPointX = transform.position.x + 2.0f;
			}
            networkPlayerManager.dungeonController.CreateDigCircle(new Vector2(digPointX, transform.position.y));
            networkPlayerManager.CmdProvideDigToServer(new Vector2(digPointX, transform.position.y), 1);
		}
	}

    void Move(float move, bool jump)
    {
		if (weaponController.gameObject.transform.rotation.eulerAngles.z > 180f && !isDirectionRight) {
			isDirectionRight = true;
			transform.localScale = new Vector3 (-1, transform.localScale.y, transform.localScale.z);
			networkPlayerManager.CmdProvideScaleToServer(transform.localScale);
		} else if(weaponController.gameObject.transform.rotation.eulerAngles.z < 180f && isDirectionRight){
			isDirectionRight = false;
			transform.localScale = new Vector3 (1, transform.localScale.y, transform.localScale.z);
			networkPlayerManager.CmdProvideScaleToServer(transform.localScale);
		}

        if (Mathf.Abs(move) > 0)
        {
			//transform.localScale = new Vector3 ((Mathf.Sign(move) == 1 ? 1 : -1)*transform.localScale.x, transform.localScale.y, 1);
            //networkPlayerManager.CmdProvideScaleToServer(transform.localScale);
        }

        m_rigidbody2D.velocity = new Vector2(move * maxSpeed, m_rigidbody2D.velocity.y);

        m_animator.SetFloat("Horizontal", move);
        m_animator.SetFloat("Vertical", m_rigidbody2D.velocity.y);
        m_animator.SetBool("isGround", m_isGround);

        if (jump && m_isGround)
        {
            m_animator.SetTrigger("Jump");
            SendMessage("Jump", SendMessageOptions.DontRequireReceiver);
            m_rigidbody2D.AddForce(Vector2.up * jumpPower);
        }
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        Vector2 groundCheck = new Vector2(pos.x, pos.y - (m_centerY * transform.localScale.y));
		Vector2 groundArea = new Vector2(m_boxcollier2D.size.x * 0.49f, 0.1f);

        m_isGround = Physics2D.OverlapArea(groundCheck + groundArea, groundCheck - groundArea, whatIsGround);
        m_animator.SetBool("isGround", m_isGround);
    }

    IEnumerator INTERNAL_OnDamage()
    {
        m_animator.Play(m_isGround ? "Damage" : "AirDamage");
        m_animator.Play("Idle");

        SendMessage("OnDamage", SendMessageOptions.DontRequireReceiver);

        m_rigidbody2D.velocity = new Vector2(transform.right.x * backwardForce.x, transform.up.y * backwardForce.y);

        yield return new WaitForSeconds(.2f);

        while (m_isGround == false)
        {
            yield return new WaitForFixedUpdate();
        }
        m_animator.SetTrigger("Invincible Mode");
        m_state = State.Invincible;
    }

    void OnFinishedInvincibleMode()
    {
        m_state = State.Normal;
    }

    enum State
    {
        Normal,
        Damaged,
        Invincible,
    }

	void OnCollisionEnter2D (Collision2D c){
        if(c.gameObject.CompareTag ("bullet")/* && !networkPlayerManager.isLocalPlayer*/){
            Destroy (c.gameObject);
        } else if(c.gameObject.CompareTag ("enemy_bullet")){
            Destroy (c.gameObject);
		} else if(c.gameObject.CompareTag ("item") && networkTransform.isLocalPlayer){
			ItemController itemController = c.gameObject.GetComponent<ItemController> ();
			networkPlayerManager.CmdProvideGetItemToServer (networkPlayerManager.netId, itemController.itemId, itemController.itemCount, itemController.itemPopId);
		} else if(c.gameObject.CompareTag ("my_home_area") && transform.gameObject.CompareTag("my_player_character") && networkTransform.isLocalPlayer){
			var gemDatas = networkPlayerManager.syncListholdItems.Where (r => r.itemId == 1).ToList();
			if(gemDatas.Count > 0){
				foreach(var gemData in gemDatas){
					networkPlayerManager.CmdDeliverGemToServer (networkPlayerManager.netId, gemData.itemCount);
					//networkPlayerManager.syncListholdItems.Remove(gemData);
				}
			}
		} else if(c.gameObject.CompareTag ("other_home_area") && transform.gameObject.CompareTag("other_player_character") && networkTransform.isLocalPlayer){
			var gemDatas = networkPlayerManager.syncListholdItems.Where (r => r.itemId == 1).ToList();
			if(gemDatas.Count > 0){
				foreach(var gemData in gemDatas){
					networkPlayerManager.CmdDeliverGemToServer (networkPlayerManager.netId, gemData.itemCount);
					networkPlayerManager.syncListholdItems.Remove(gemData);
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D c){
		if(c.gameObject.CompareTag ("item") && networkTransform.isLocalPlayer){
			ItemController itemController = c.gameObject.GetComponent<ItemController> ();
			networkPlayerManager.CmdProvideGetItemToServer (networkPlayerManager.netId, itemController.itemId, itemController.itemCount, itemController.itemPopId);
		}
	}

    public void DoDamageAction(){
		hpSlider.value =  hp / maxHp;

		if (hp <= 0) {
			var effect = Instantiate (destroyEffect, transform.position, Quaternion.identity);
			m_boxcollier2D.enabled = false; //ポップしたアイテムを自分で取得してしまうため当たり判定を消す

			if(networkPlayerManager.isServer){
				foreach(var holdItem in networkPlayerManager.syncListholdItems){
					GameStatusManager.Instance.myNetworkManager.gameStageManager.CmdPopDeathItem (holdItem.itemId, holdItem.itemCount, transform.position);
				}
			}
			this.gameObject.SetActive(false);
		} else {
			StartCoroutine(INTERNAL_OnDamage());
		}
    }
}
