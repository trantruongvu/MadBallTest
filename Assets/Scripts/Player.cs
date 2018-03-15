using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    private Rigidbody rigidbd;

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    public void Setup ()
    {
        rigidbd = GetComponent<Rigidbody>();
        rigidbd.useGravity = true;
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefault();
    }

    // Update
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        // Suicide
        if (Input.GetKey(KeyCode.K))
        {
            RpcTakeDamage(9999);
        }
    }

    private void SetDefault()
    {
        isDead = false;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = true;
        }

        currentHealth = maxHealth;
    }

    [ClientRpc]
    public void RpcTakeDamage (int _damage)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= _damage;
        Debug.Log(transform.name + " now has " + currentHealth + " HP.");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die ()
    {
        isDead = true;
        rigidbd.useGravity = false;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }

        Debug.Log(transform.name + " is dead.");

        StartCoroutine(ReSpawn());

    }

    private IEnumerator ReSpawn ()
    {
        yield return new WaitForSeconds(GameController.instance.matchSetting.RespawnTime);

        SetDefault();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
    }
}
