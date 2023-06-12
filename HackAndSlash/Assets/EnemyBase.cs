using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("References")]
    public EnemyData data;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;


    [Header("Movement")]
    private bool canMove = true;

    [Header("Health and Damage")]
    private Material defMat;
    private int knockBackDir;
    private float currentHealth;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        defMat = sr.material;
        currentHealth = data.maxHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }


    #region Health, Take Damage 

    public virtual void TakeDamage(float damageAmt)
    {
        //take damage
        currentHealth -= damageAmt;

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
            return;
        }

        sr.material = data.whiteMat;
        KnockBack();
        Invoke("ResetMat", 0.25f);


    }

    void ResetMat()
    {
        sr.material = defMat;
    }

    public void TakeDirection(float direction)
    {
        if (direction > transform.position.x)
        {
            knockBackDir = -1;
        }
        else
        {
            knockBackDir = 1;
        }
    }

    public void KnockBack()
    {
        StartCoroutine(EnumKnockBack());

        /*rb.AddForce(Vector3.right.normalized * knockBackDir * data.knockBackForce);
        rb.AddForce(Vector3.up.normalized * data.knockBackForce);*/
    }

    IEnumerator EnumKnockBack()
    {
        if (!canMove)
        {
            yield break;
        }
        canMove = false;
        rb.velocity = new Vector2(data.knockBackForce * knockBackDir, data.knockBackForce);
        yield return new WaitForSeconds(0.2f);
        canMove = true;
    }
    #endregion
}
