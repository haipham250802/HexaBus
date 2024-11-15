using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
public class HexBroken : MonoBehaviour
{
    [SerializeField] List<Transform> _listFragment = new();
    [SerializeField] List<MeshRenderer> _listMeshInside = new();
    [SerializeField] List<MeshRenderer> _listMeshBorder = new();
    [SerializeField] private float forceMin = 300f;
    [SerializeField] private float forceMax = 500f;
    private void OnEnable()
    {
        Explode();
    }
    public void InitMaterial(Material matInside, Material matBorder)
    {
        InitMeshInside(matInside);
        InitMeshBorder(matBorder);
    }
    public void InitMeshInside(Material mat)
    {
        foreach (var item in _listMeshInside)
        {
            item.material = mat;
        }
    }
    public void InitMeshBorder(Material mat)
    {
        foreach (var item in _listMeshBorder)
        {
            item.material = mat;
        }
    }
    [Button]
    void Explode()
    {
        foreach (Transform fragment in _listFragment)
        {
            Rigidbody rb = fragment.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 randomDirection = Random.insideUnitSphere;
                float randomForce = Random.Range(forceMin, forceMax);
                rb.AddForce(randomDirection * randomForce);
                //   rb.AddForce(Vector3.down * upwardForce * 1.5f);
            }
        }
    }
}
