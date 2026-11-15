using System;
using Item;
using Unity.VisualScripting;
using UnityEngine;

public class Friteuse : MonoBehaviour
{
	[SerializeField] private Collider colliderFriteuse;
	[SerializeField] private Transform patateTargetTransform;
	[SerializeField] private float expulsionForce = 2f;
	[SerializeField] private CookingTimerBar cookingTimerBar;
	
	private Rigidbody rbOther;
	private bool canFry = true;

	private void OnEnable()
	{
		Patate.OnCooked += OnCooked;
	}

	private void OnDisable()
	{
		Patate.OnCooked -= OnCooked;
	}

	public void Deactivate()
	{
		canFry = false;
	}

	public void Activate()
	{
		canFry = true;
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent(out IFriable friable) && canFry)
		{
			if (other.gameObject.TryGetComponent(out Rigidbody rb))
			{
				rbOther = rb;
				rb.isKinematic = true;
				other.gameObject.transform.position = patateTargetTransform.position; // Je sais ce qu'ils veulent donc pour l'instant c'est ça
			}
			friable.Frying(cookingTimerBar);
			Deactivate();
		}
		else
		{
			if (other.gameObject.TryGetComponent(out Rigidbody rb))
				rb.AddForce((other.gameObject.transform.position - gameObject.transform.position).normalized * expulsionForce, ForceMode.Impulse);
		}
	}
	
	private void OnCooked()
	{
		Activate();
		if (rbOther)
		{
			rbOther.isKinematic = false;
			rbOther.AddForce(gameObject.transform.up * expulsionForce, ForceMode.Impulse);
		}
	}
}
