using System;
using UnityEngine;
using Data;
using MyPrint;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Client
{
	public class ClientCommand : MonoBehaviour
	{
		[SerializeField] private CommandStatistics statistics;
		[SerializeField] private Commande command = new();

		[SerializeField] private TextMeshProUGUI commandText;

		[Header("Model")]
		[SerializeField] private GameObject[] model;
		
		[Header("UI")]
		[SerializeField] private Image _timerImage;
		public float MaxTimer;

		public Action<ClientCommand> OnGetHisCommand; 
		
		private void Start()
		{
			SetRandomModel();
			
			Command();
		}

		private void SetRandomModel()
		{
			int r = Random.Range(0, model.Length);
			
			for (int i = 0; i < model.Length; i++)
			{
				model[i].SetActive(i == r);
			}
		}

		public void Command()
		{
			// Produit
			command.produitType = ChoseProduct();
			
			// Sauce Frite
			switch (command.produitType)
			{
				case Produit.Frite:
					command.sauceFrite = ChoseSauceFrite();
					break;
				case Produit.Moule:
					command.sauceMoule = ChoseSauceMoule();
					break;
				case Produit.FriteMoule:
					command.sauceFrite = ChoseSauceFrite();
					command.sauceMoule = ChoseSauceMoule();
					break;
				default:
					Debug.LogError($"Unknown produit type: {command.produitType}");
					break;
			}
			
			// Point
			CalculatePoint();
			
			// UI
			DisplayCommand();
		}

		private Produit ChoseProduct()
		{
			float rnd = UnityEngine.Random.Range(0f, 1f);
			float p = 0;
			foreach (var stat in statistics.statisticProduit)
			{
				p += stat.probability;
				if (rnd < p)
					return stat.produitType;
			}

			return Produit.Empty;
		}
		private SauceFrite ChoseSauceFrite()
		{
			float rnd = UnityEngine.Random.Range(0f, 1f);
			float p = 0;
			foreach (var stat in statistics.statisticSauceFrite)
			{
				p += stat.probability;
				if (rnd < p)
					return stat.sauceType;
			}

			return SauceFrite.Empty;
		}
		private SauceMoule ChoseSauceMoule()
		{
			float rnd = UnityEngine.Random.Range(0f, 1f);
			float p = 0;
			foreach (var stat in statistics.statisticSauceMoule)
			{
				p += stat.probability;
				if (rnd < p)
					return stat.sauceType;
			}

			return SauceMoule.Empty;
		}
		
		private void CalculatePoint()
		{
			command.point += GetProduitPoint(command.produitType, command.sauceFrite == SauceFrite.Empty || command.sauceMoule == SauceMoule.Empty);
			command.point += GetSauceFritePoint(command.sauceFrite);
			command.point += GetSauceMoulePoint(command.sauceMoule);
		}

		private float GetProduitPoint(Produit produitType, bool bonus)
		{
			foreach (var type in statistics.statisticProduit)
				if(type.produitType == produitType)
					return bonus ? type.pointAvecSauce : type.pointSansSauce;
			return 0;
		}
		private float GetSauceFritePoint(SauceFrite sauceType)
		{
			foreach (var type in statistics.statisticSauceFrite)
				if(type.sauceType == sauceType)
					return type.point;
			return 0;
		}
		private float GetSauceMoulePoint(SauceMoule sauceType)
		{
			foreach (var type in statistics.statisticSauceMoule)
				if(type.sauceType == sauceType)
					return type.point;
			return 0;
		}

		public float CheckIfItsTheGoodCommand(Commande c)
		{
			float r = 0;
			
			if(c != null)
			{
				if (c.CheckCommand(command))
				{
					r = command.point;
				}
			
				OnGetHisCommand?.Invoke(this);
			}
			
			return r;
		}

		private void DisplayCommand()
		{
			commandText.text = $"-{command.produitType}\n";
			if(command.sauceFrite !=  SauceFrite.Empty)
				commandText.text += $"-{command.sauceFrite}\n";
			if(command.sauceMoule !=  SauceMoule.Empty)
				commandText.text += $"-{command.sauceMoule}";
		}
		
		public void ClientPartis() => Destroy(gameObject);

		public void UpdateTimer(float currentValue)
		{
			_timerImage.fillAmount = currentValue / MaxTimer;
		}
	}
}