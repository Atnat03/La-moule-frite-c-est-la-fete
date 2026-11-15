using System;
using Item;
using MyPrint;
using UnityEngine;

public class Couteau : BasePickableItem
{
    [SerializeField] private Transform _rayStart;

    Vector3 startPosition;
    Vector3 direction;
    
    protected override void Update()
    {
        base.Update();
        
        if (_isInHand)
        {
            startPosition = _rayStart.position;
            direction = _rayStart.forward;
            
            Debug.DrawLine(startPosition, direction.normalized * 5f, Color.red);
            
            if (Physics.Raycast(startPosition, direction, out RaycastHit hit))
            {
                ABPrint.Print(hit.collider.gameObject.name);
                
                if (hit.transform.TryGetComponent(out Patate patate))
                {
                    patate.CutCutPatate();
                }
            }
        }
    }
}
