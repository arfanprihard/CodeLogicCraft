using System.Collections;
using UnityEngine;

public class MovementCharacter : MonoBehaviour
{
    public float speed = 2f; // Kecepatan gerak
    public float rotationSpeed = 200f; // Kecepatan rotasi

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isMoving = false; // Status gerakan

    private Vector3 posisiAwal;
    private Quaternion rotasiAwal;

    void Start()
    {
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
        }
    }

    // Mengecek apakah karakter sedang bergerak
    public bool IsMoving()
    {
        return isMoving;
    }

    // Reset posisi ke posisi awal
    public void ResetPosisi()
    {
        transform.position = posisiAwal;
        transform.rotation = rotasiAwal;
        isMoving = false;
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
}
