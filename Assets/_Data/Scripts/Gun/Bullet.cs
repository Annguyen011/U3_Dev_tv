using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;

    private Vector2 _fireDirection;

    private Rigidbody2D _rigidBody;
    private Gun gun;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
       
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    public void Init(Gun gun,Vector2 bulletSpawnPos, Vector2 mousePos)
    {
        this.gun = gun;
        _fireDirection = (mousePos - bulletSpawnPos).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Health health = other.gameObject.GetComponent<Health>();
        health?.TakeDamage(_damageAmount);
        gun.ReleaseBullet(this);
    }
}