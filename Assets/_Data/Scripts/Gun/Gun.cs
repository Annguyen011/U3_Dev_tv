using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;

    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float gunFireCD = .5f;

    private ObjectPool<Bullet> bulletPool;

    private float lastFireTime = 0f;
    private Vector2 mousePos;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        CreateBulletPool();
    }

    private void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += FireAnimation;
        OnShoot += ResetLastFireTime;
    }

    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= FireAnimation;
        OnShoot -= ResetLastFireTime;
    }

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
        if (Input.GetMouseButtonDown(0) && Time.time >= lastFireTime) {

            OnShoot?.Invoke();
        }
    }

    private void FireAnimation()
    {
        _animator.Play("Fire", 0,0);
    }

    private void ResetLastFireTime()
    {
        lastFireTime = Time.time + gunFireCD;
    }

    private void ShootProjectile()
    {
        Bullet newBullet = bulletPool.Get();

        newBullet.Init(this, _bulletSpawnPoint.position, mousePos);
    }

    #region Pool
    private void CreateBulletPool()
    {
        bulletPool = new ObjectPool<Bullet>(
            () =>
            {
                return Instantiate(_bulletPrefab);
            },
            bullet =>
            {
                bullet.gameObject.SetActive(true);
            },
            bullet =>
            {
                bullet.gameObject.SetActive(false);
            },
            bullet =>
            {
                Destroy(bullet);
            }
            );
    }

    public void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet);
    }

    #endregion
}
