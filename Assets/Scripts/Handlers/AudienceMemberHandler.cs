using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceMemberHandler : MonoBehaviour
{
    public List<Sprite> _sprites;
    [SerializeField] SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        SetDirection();
        PickRandomSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PickRandomSprite()
    {
        int Index = Random.Range(0, _sprites.Count);
        _spriteRenderer.sprite = _sprites[Index];
    }
    public void SetDirection()
    {
        bool flipped = transform.position.x > 0;
        _spriteRenderer.flipX = flipped;
    }
}
