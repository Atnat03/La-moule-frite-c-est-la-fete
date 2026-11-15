using System;
using System.Collections;
using Item;
using Item.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class Friteuse : MonoBehaviour, IInteractable
{
	[SerializeField] private Collider colliderFriteuse;
	[SerializeField] private Transform patateTargetTransform;
	[SerializeField] private float expulsionForce = 2f;
	[SerializeField] private CookingTimerBar cookingTimerBar;
	
	[SerializeField] private float cookingTimeToBurn = 5;
	[SerializeField] private float cookingTime = 3;
	
	private Rigidbody rbOther;
	private bool canFry = true;
	private bool frying;
	private Patate patate;
	private Coroutine cookingCoroutine;

	void OnEnable()
	{
		ExtincteurPourLesPompiers.fireOff += OnFireOff;
	}

	void OnDisable()
	{
		ExtincteurPourLesPompiers.fireOff -= OnFireOff;
	}

	public void Fire()
	{
		if (patate != null)
		{
			if (cookingCoroutine != null) StopCoroutine(cookingCoroutine);
			cookingTimerBar.gameObject.SetActive(false);
			Burned();
		}
		Deactivate();
	}

	private void OnFireOff()
	{
		Activate();
	}

	private void Deactivate()
	{
		canFry = false;
	}

	private void Activate()
	{
		canFry = true;
	}

	public void StartInteract()
	{
		if (!frying)
			return;
		
		if (cookingTimerBar.goodTime)
			patate.SetNewCuissonState(AlimentCuisson.Cuit);
		else
			patate.SetNewCuissonState(AlimentCuisson.Brulé);
		
		OnCooked();
		StopCoroutine(cookingCoroutine);
		cookingTimerBar.gameObject.SetActive(false);
	}

	public void StopInteract() { }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent(out Patate actualPatate) && canFry)
		{
			if (other.gameObject.TryGetComponent(out Rigidbody rb))
			{
				rbOther = rb;
				rb.isKinematic = true;
				other.gameObject.transform.position = patateTargetTransform.position; // Je sais ce qu'ils veulent donc pour l'instant c'est ça
			}

			patate = actualPatate;
			
			Deactivate();
			Frying();
		}
		else if (other.gameObject.TryGetComponent(out BouteilleAlcool alcool))
		{
			Fire();
			Destroy(other.gameObject);
		}
		else
		{
			Debug.Log(other.gameObject.name);
			if (other.gameObject.TryGetComponent(out Rigidbody rb))
				rb.AddForce((other.gameObject.transform.position - gameObject.transform.position).normalized * expulsionForce, ForceMode.Impulse);
		}
	}
	
	public void Frying()
	{
		frying = true;
		
		if (patate.Cuisson == AlimentCuisson.Crut && patate.Cuisson != AlimentCuisson.Brulé)
		{
			cookingCoroutine = StartCoroutine(TimerPatate());
			cookingTimerBar.gameObject.SetActive(true);
			cookingTimerBar.InitializeTimerBar(cookingTimeToBurn - cookingTime, cookingTime, cookingTimeToBurn);
		}
		else
			Burned();
	}

	IEnumerator TimerPatate()
	{
		bool notBurned = true;
		float timeElasped = 0;
		while (notBurned)
		{
			timeElasped += Time.deltaTime;
			cookingTimerBar.HandleTimer(timeElasped, cookingTimeToBurn - cookingTime, cookingTime);

			if (timeElasped >= cookingTimeToBurn)
				notBurned = false;
			yield return null;
		}

		cookingTimerBar.gameObject.SetActive(false);
		Burned();
	}

	private void Burned()
	{
		patate.SetNewCuissonState(AlimentCuisson.Brulé);
		OnCooked();
		Debug.Log("Burned");
	}
	
	private void OnCooked()
	{
		frying = false;
		Activate();
		if (rbOther)
		{
			rbOther.isKinematic = false;
			rbOther.AddForce(gameObject.transform.up * expulsionForce, ForceMode.Impulse);
			patate = null;
		}
	}

}
