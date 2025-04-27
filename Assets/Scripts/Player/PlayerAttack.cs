using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Transform attack1;
    [SerializeField]
    private Transform attack2;
    private SpriteRenderer sprite1;
    private SpriteRenderer sprite2;

    private bool attacking = false;
    private float lerp = 0f;
    private float attackDistance = 1f;

    private bool fading = false;

    // Start is called before the first frame update
    void Start()
    {
        sprite1 = attack1.GetComponent<SpriteRenderer>();
        sprite2 = attack2.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // TEMP
        if (Input.GetKeyDown(KeyCode.A))
        {
            Attack(transform.position);
        }

        if (attacking)
        {
            lerp += Time.deltaTime * 3f;
            attack1.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * attackDistance, lerp);
            attack2.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * attackDistance, lerp - 0.2f);
            if (attack2.localScale.z >= 1f * attackDistance)
            {
                lerp = 0f;
                fading = true;
                attacking = false;
            }
        }
        else if (fading)
        {
            lerp += Time.deltaTime * 1.5f;
            sprite1.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, lerp));
            sprite2.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, lerp));
            if (sprite1.color.a == 0f)
            {
                attack1.gameObject.SetActive(false);
                attack2.gameObject.SetActive(false);
                fading = false;
            }
        }
    }

    public void Attack(Vector3 position)
    {
        attack1.position = position;
        attack2.position = position;
        attack1.gameObject.SetActive(true);
        attack2.gameObject.SetActive(true);
        lerp = 0f;
        attack1.localScale = Vector3.zero;
        attack2.localScale = Vector3.zero;
        sprite1.color = Color.white;
        sprite2.color = Color.white;
        attacking = true;
    }
}
