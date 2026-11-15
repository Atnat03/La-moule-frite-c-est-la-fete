using Item;
using UnityEngine;

public class StockDeNourriture : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _alimentPrefab;
    [SerializeField] private Transform _alimentPos;
    
    public void StartInteract()
    {
        Instantiate(_alimentPrefab, _alimentPos.position, _alimentPos.rotation);
    }

    public void StopInteract() { }
}
