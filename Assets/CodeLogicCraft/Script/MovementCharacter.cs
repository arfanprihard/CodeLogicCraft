using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class MovementCharacter : MonoBehaviour
{
    public float speed = 2f; // Kecepatan gerak
    public float rotationSpeed = 200f; // Kecepatan rotasi

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isMoving = false; // Status gerakan

    private Vector3 posisiAwal;
    private Quaternion rotasiAwal;

    private bool isTouchingFinish = false;
    private bool isTouchingPercabangan = false;
    private bool isTouchingItem = false;

    private Stack<GameObject> itemStack = new Stack<GameObject>();
    private GameObject itemTake;
    private Animator animator;

    [Header("Footstep Sounds")]
    public AudioClip walkSounds;
    public float footstepInterval = 0.35f;
    private float footstepTimer = 0f;

    [Header("Action Sounds")]
    public AudioClip turnLeftRightSound;
    public AudioClip takeItemSound;
    public AudioClip itemTaked;

    public AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Simpan posisi awal
        posisiAwal = transform.position;
        // Simpan rotasi awal
        rotasiAwal = transform.rotation;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
            animator.SetBool("isMoving", true);
            if (footstepTimer == 0f)
            {
                PlayFootstep();
            }
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                PlayFootstep();
                footstepTimer = 0f;
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
            footstepTimer = 0f;
        }
    }

    private void PlayFootstep()
    {
        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(walkSounds);
        }
    }

    // Mengecek apakah karakter sedang bergerak
    public bool IsMoving()
    {
        return isMoving;
    }

    public bool IsOnStartPosition()
    {
        return transform.position == posisiAwal && transform.rotation == rotasiAwal;
    }

    // Reset posisi ke posisi awal
    public void ResetPosisi()
    {
        transform.position = posisiAwal;
        transform.rotation = rotasiAwal;
        isMoving = false;
        isTouchingItem = false;
        isTouchingFinish = false;
        isTouchingPercabangan = false;
        EnableAllItems();
        animator.Rebind();
        animator.Update(0f);
    }
    void EnableAllItems()
    {
        // Cari semua objek dengan tag "Item"
        GameObject[] allItems = itemStack.ToArray();

        // Iterasi melalui semua item dan mencetak nama-nama item
        foreach (GameObject item in allItems)
        {
            item.SetActive(true);
        }
        itemStack.Clear();
    }
    public void StopAllActions()
    {
        StopAllCoroutines(); // Hentikan semua Coroutine di MovementCharacter
        isMoving = false;    // Pastikan status bergerak menjadi false
    }


    // Metode untuk melangkah maju
    public void Langkah()
    {
        if (!isMoving)
        {
            targetPosition = transform.position + transform.forward;
            isMoving = true;
        }
    }

    // Gerakkan karakter ke targetPosition
    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }

    // Hadap kiri (rotasi -90 derajat)
    public void HadapKiri()
    {
        StartCoroutine(RotateToAngle(-90));
        PlayTurnLeftRightSound();
    }

    // Hadap kanan (rotasi +90 derajat)
    public void HadapKanan()
    {
        StartCoroutine(RotateToAngle(90));
        PlayTurnLeftRightSound();
    }
    private void PlayTurnLeftRightSound()
    {
        if (turnLeftRightSound != null)
        {
            audioSource.PlayOneShot(turnLeftRightSound, 0.5f);
        }
    }

    // Coroutine untuk rotasi ke sudut tertentu
    private IEnumerator RotateToAngle(float angle)
    {
        float targetY = transform.eulerAngles.y + angle;
        targetRotation = Quaternion.Euler(0, targetY, 0);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Finish"))
        {
            isTouchingFinish = true;
        }
        if (collision.collider.CompareTag("Percabangan"))
        {
            isTouchingPercabangan = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Finish"))
        {
            isTouchingFinish = false;
        }
        if (collision.collider.CompareTag("Percabangan"))
        {
            isTouchingPercabangan = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            itemTake = other.gameObject;
            isTouchingItem = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            isTouchingItem = false;
        }
    }

    public bool CekTakeItem()
    {
        if (isTouchingItem)
        {
            isTouchingItem = false;
            return true;
        }
        else
        {
            return false;
        }

    }

    public void TakeItem()
    {
        animator.SetTrigger("triggerTakeItem");
        PlayTakeItemSound();
        StartCoroutine(WaitForTimeAndTakeItem(2f));
    }

    private void PlayTakeItemSound()
    {
        if (takeItemSound != null)
        {
            audioSource.PlayOneShot(takeItemSound, 0.7f);
        }
    }

    // Coroutine untuk menunggu waktu tertentu (misalnya 2 detik)
    IEnumerator WaitForTimeAndTakeItem(float delayTime)
    {
        // Tunggu selama waktu tertentu (misalnya 2 detik)
        yield return new WaitForSeconds(delayTime);

        // Setelah 2 detik, baru lanjutkan aksi selanjutnya
        // Menambahkan item ke stack dan menonaktifkan item
        itemStack.Push(itemTake);
        itemStack.Peek().SetActive(false);
        PlayItemTakedSound();
    }
    private void PlayItemTakedSound()
    {
        if (itemTaked != null)
        {
            audioSource.PlayOneShot(itemTaked, 0.5f);
        }
    }

    public bool CekFinish()
    {
        if (isTouchingFinish)
        {
            isTouchingFinish = false;
            return true;
        }
        else
        {
            return false;
        }

    }
    public bool CekPercabangan()
    {
        if (isTouchingPercabangan)
        {
            // isTouchingPercabangan = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CekFallArea()
    {
        if (transform.position.y < -2f) // Periksa apakah karakter berada di bawah sumbu Y -2
        {
            isTouchingFinish = false;
            Debug.Log("Karakter jatuuuuhh");
            return true; // Karakter jatuh
        }
        else
        {
            return false;
        }
    }
}
