using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float fuelRegenSpeed = 0.3f;
    [SerializeField] float fuelBurnSpeed = 1f;
    [SerializeField] Camera cam;
    public AudioListener getAudioListener() { return cam.GetComponent<AudioListener>(); }

    [SerializeField] Transform playerBody;
    [SerializeField] GameObject playerBooster;

    private float fuelAmount = 1f;
    private Rigidbody rigidbd;
    private Quaternion targetRotation;
    private float rotationSpeed = 2000f;

    void Start()
    {
        rigidbd = GetComponent<Rigidbody>();
    }

    void Update()
    {
        InputKeyBoard();
        InputMouse();
    }

    void InputKeyBoard()
    {
        Vector3 _velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 _veloRotate = new Vector3(Input.GetAxisRaw("Horizontal"), 0, -Input.GetAxisRaw("Vertical"));
        playerBody.Rotate(_veloRotate *= 10f);

        // Apply movement
        if (Input.GetButton("Run") && fuelAmount > 0f)
        {
            // Burn fuel
            fuelAmount -= fuelBurnSpeed * Time.deltaTime;
            fuelAmount = Mathf.Clamp(fuelAmount, 0f, 1f);

            if (fuelAmount >= 0.01f)
                rigidbd.velocity = _velocity *= runSpeed;

            // Bật hiệu ứng chạy
            if (!playerBooster.activeSelf)
                DisplayBooster(true);
        }
        else
        {
            // Regen fuel
            fuelAmount += fuelRegenSpeed * Time.deltaTime;
            fuelAmount = Mathf.Clamp(fuelAmount, 0f, 1f);

            rigidbd.velocity = _velocity *= walkSpeed;

            // Tắt hiệu ứng chạy
            if (playerBooster.activeSelf)
                DisplayBooster(false);
        }
    }

    public void DisplayBooster(bool status)
    {
        playerBooster.SetActive(status);
    }

    void InputMouse()
    {
        // Player Rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));

        Vector3 currentPos = mousePos - new Vector3(transform.position.x, 0f, transform.position.z);
        targetRotation = Quaternion.LookRotation(currentPos);
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
    }

    // Get fuel amount
    public float GetFuelAmount()
    {
        return fuelAmount;
    }
}
