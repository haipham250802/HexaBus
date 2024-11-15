using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class BrokenElement : MonoBehaviour
{
    [SerializeField] MeshRenderer _mesh;
    [SerializeField] Rigidbody _rb;

    public void SetMaterial(Material mat)
    {
        _mesh.material = mat;
    }
    private void Update()
    {
        HexaFallWithVelocity();
    }
    private void HexaFallWithVelocity()
    {
        _rb.velocity += Camera.main.transform.up * Physics.gravity.y * 1f * Time.deltaTime;
    }
    [Button]
    private void Init()
    {
        _mesh = GetComponent<MeshRenderer>();
        _rb = GetComponent<Rigidbody>();
    }

}
