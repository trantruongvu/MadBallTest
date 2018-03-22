using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private Behaviour[] disableOndeath;
    private bool[] wasEnabled;

    [SyncVar]
    private int currentHealth;

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    //void Update()
    //{
    //    if (!isLocalPlayer)
    //        return;

    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        RpcTakeDamage(99999);
    //    }
    //}

    public void SetupPlayer()
    {
        wasEnabled = new bool[disableOndeath.Length];

        for (int i = 0; i < wasEnabled.Length; i++)
            wasEnabled[i] = disableOndeath[i].enabled;

        SetDefault();
    }

    public void SetDefault()
    {
        isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < disableOndeath.Length; i++)
            disableOndeath[i].enabled = wasEnabled[i];

        Collider _collider = GetComponent<Collider>();
        if (_collider != null)
            _collider.enabled = true;
    }

    [ClientRpc]
    public void RpcTakeDamage(int _damage)
    {
        if (_isDead)
            return;

        currentHealth -= _damage;
        Debug.Log(transform.name + " now has " + currentHealth + " HP.");

        if (currentHealth <= 0)
            Die();
    }
	
    private void Die ()
    {
        isDead = true;
        Debug.Log(transform.name + " iz DED.");

        // Disable components
        for (int i = 0; i < disableOndeath.Length; i++)
            disableOndeath[i].enabled = false;

        Collider _collider = GetComponent<Collider>();
        if (_collider != null)
            _collider.enabled = false;

        // Respawn
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn ()
    {
        yield return new WaitForSeconds(GameManager.instance.setting.respawnTime);
        SetDefault();

        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        Debug.Log(transform.name + " iz RES.");
    }
}
