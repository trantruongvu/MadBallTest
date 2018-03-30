using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {

    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private Behaviour[] disableOndeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectOndeath;

    [SerializeField]
    private GameObject deathEffect;
    [SerializeField]
    private GameObject spawnEffect;

    [SyncVar]
    private int currentHealth;

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(99999);
        }
    }

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

        // Enable components
        for (int i = 0; i < disableOndeath.Length; i++)
            disableOndeath[i].enabled = wasEnabled[i];

        // Enable Game Object
        for (int i = 0; i < disableGameObjectOndeath.Length; i++)
            disableGameObjectOndeath[i].SetActive(true);

        // Enable collider
        Collider _collider = GetComponent<Collider>();
        if (_collider != null)
            _collider.enabled = true;

        // Switch camera 
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        // Spawn effect
        GameObject _effectIns = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_effectIns, 3f);
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

        // Disable Game Object
        for (int i = 0; i < disableGameObjectOndeath.Length; i++)
            disableGameObjectOndeath[i].SetActive(false);

        // Disable Collider
        Collider _collider = GetComponent<Collider>();
        if (_collider != null)
            _collider.enabled = false;

        // Death effect
        GameObject _effectIns = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_effectIns, 3f);

        // Switch camera 
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        // Respawn
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn ()
    {
        yield return new WaitForSeconds(GameManager.instance.setting.respawnTime);

        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        SetDefault();
        Debug.Log(transform.name + " iz RES.");
    }
}
