using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float gunFireCD = .5f;

    private float lastFireTime = 0f;
    private Vector2 mousePos;
    
    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void RotateGun()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 direction = mousePos - (Vector2)PlayerController.Instance.transform.position;
        Vector2 direction = PlayerController.Instance.transform.InverseTransformVector(mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0,0,angle);
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastFireTime + gunFireCD) {

            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        lastFireTime = Time.time;
        Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.Init(_bulletSpawnPoint.position, mousePos);
    }
}
