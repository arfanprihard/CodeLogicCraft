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
    private bool isTouchingItem = false;

    private Stack<GameObject> itemStack = new Stack<GameObject>();
    private GameObject itemTake;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Simpan posisi awal
        posisiAwal = transform.position;
        // Simpan rotasi awal
        rotasiAwal = transform.rotation;
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
            animator.SetBool("isMoving", true);
        }else{
            animator.SetBool("isMoving", false);
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
        EnableAllItems();
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
    }

    // Hadap kanan (rotasi +90 derajat)
    public void HadapKanan()
    {
        StartCoroutine(RotateToAngle(90));
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
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Finish"))
        {
            isTouchingFinish = false;
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
        return isTouchingItem;
    }

    public void TakeItem(){
        animator.SetTrigger("triggerTakeItem");
        // Mulai Coroutine untuk menunggu selama 2 detik
        StartCoroutine(WaitForTimeAndTakeItem(2f)); // 2 detik jeda
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
    }

    public bool CekFinish()
    {
        return isTouchingFinish;
        
    }

    public bool CekFallArea(){
        if (transform.position.y < -2f) // Periksa apakah karakter berada di bawah sumbu Y -2
        {
            isTouchingFinish = false;
            Debug.Log("Karakter jatuuuuhh");
            return true; // Karakter jatuh
        }else{
            return false;
        }
    }
}
