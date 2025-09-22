using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 5f;    // 移速
    public float jumpForce = 8f;   // 跳跃数值

    float defaultJumpForce;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded;

    Vector3 checkpointPos;



    // 键位打乱
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;

    bool adSpaceShuffleEnabled = false;
    int pressCountADS = 0;

    readonly KeyCode[] poolADS = new KeyCode[] { KeyCode.A, KeyCode.D, KeyCode.Space };
    System.Random rngADS = new System.Random();

    KeyCode defaultLeft = KeyCode.A;
    KeyCode defaultRight = KeyCode.D;
    KeyCode defaultJump = KeyCode.Space;


    bool spinMode = false;
    public float spinSpeed = 240f; // 每秒旋转角度
    RigidbodyConstraints2D defaultConstraints;
    float defaultGravity;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        checkpointPos = transform.position;

        defaultJumpForce = jumpForce;

        defaultGravity = rb.gravityScale;
        defaultConstraints = rb.constraints;

    }

    // Update is called once per frame
    void Update()
    {
        // 移动
        float moveInput = 0f;
        if (Input.GetKey(leftKey)) moveInput = -1f;
        if (Input.GetKey(rightKey)) moveInput = 1f;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 朝向切换
        if (moveInput > 0) sr.flipX = true;
        if (moveInput < 0) sr.flipX = false;

        // 跳跃
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); //清垂直速度
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }


        // 打乱和统计按下次数
        if (adSpaceShuffleEnabled)
        {
            if (Input.GetKeyDown(leftKey) || Input.GetKeyDown(rightKey) || Input.GetKeyDown(jumpKey))
            {
                pressCountADS++;
                if (pressCountADS >= 4)
                {
                    ReshuffleADS();
                    pressCountADS = 0;
                }
            }
        }

        // 紫块模式
        if (spinMode)
        {
            rb.gravityScale = 0f;
            transform.Rotate(0f, 0f, -spinSpeed * Time.deltaTime);
            // AD沿自身的左右移动
            float x = 0f;
            if (Input.GetKey(leftKey)) x -= 1f;
            if (Input.GetKey(rightKey)) x += 1f;

            Vector2 rightDir = transform.right; // 自身右边方向
            rb.linearVelocity = rightDir * x * moveSpeed;

        }

    }


    // 判定地面
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = false;
    }


    public void SetCheckpoint(Vector3 pos)
    {
        checkpointPos = pos;
        Debug.Log("Save at " + pos);
    }

    public void Respawn()
    {
        transform.position = checkpointPos;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Respawn at " + checkpointPos);
    }

    public void SetJumpForce(float f)
    {
        jumpForce = f;
    }




    public void EnableShuffleADS()
    {
        adSpaceShuffleEnabled = true;
        pressCountADS = 0;
        ReshuffleADS(); 
        Debug.Log("[Blue] ADS shuffle enabled. L=" + leftKey + ", R=" + rightKey + ", J=" + jumpKey);
    }

    public void DisableShuffleADS()
    {
        adSpaceShuffleEnabled = false;
        pressCountADS = 0;

        // 恢复默认
        leftKey = defaultLeft;
        rightKey = defaultRight;
        jumpKey = defaultJump;

        Debug.Log("[Blue] ADS shuffle disabled. Back to defaults.");
    }

    void ReshuffleADS()
    {
        // 重新分配并确保三者互不相同
        var a = new System.Collections.Generic.List<KeyCode>(poolADS);
        for (int i = 0; i < a.Count; i++)
        {
            int j = rngADS.Next(i, a.Count);
            (a[i], a[j]) = (a[j], a[i]);
        }
        leftKey = a[0];
        rightKey = a[1];
        jumpKey = a[2];

        Debug.Log("[Blue] Reshuffled. L=" + leftKey + ", R=" + rightKey + ", J=" + jumpKey);
    }




    public void EnableSpinIgnoreGravity()
    {
        if (spinMode) return;
        spinMode = true;

        // 关重力与允许旋转
        defaultGravity = rb.gravityScale;
        defaultConstraints = rb.constraints;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // 清掉竖直速度
        rb.angularVelocity = 0f;
        rb.constraints = defaultConstraints & ~RigidbodyConstraints2D.FreezeRotation;

        Debug.Log("[Purple] Spin ON");
    }

    public void DisableSpinIgnoreGravity()
    {
        if (!spinMode) return;
        spinMode = false;

        // 恢复重力，角度摆正
        rb.gravityScale = defaultGravity;
        rb.angularVelocity = 0f;
        rb.constraints = defaultConstraints;
        transform.rotation = Quaternion.identity;

        Debug.Log("[Purple] Spin OFF");
    }




    public void ResetToDefaults()
    {
        // 恢复常规数值
        jumpForce = defaultJumpForce;
        DisableShuffleADS();
        DisableSpinIgnoreGravity();
    }
}
