using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	[CreateAssetMenu(fileName = "CommandStatistics", menuName = "ScriptableObjects/CommandStatistics", order = 1)]
	public class CommandStatistics : ScriptableObject
	{
		public List<StatisticProduit> statisticProduit = new();
		public List<StatisticSauceFrite> statisticSauceFrite = new();
		public List<StatisticSauceMoule> statisticSauceMoule = new();

		[Serializable]
		public class StatisticProduit
		{
			public Produit produitType;
			public float probability;
			public float pointSansSauce;
			public float pointAvecSauce;
		}
		[Serializable]
		public class StatisticSauceFrite
		{
			public SauceFrite sauceType;
			public float probability;
			public float point;
		}
		
		[Serializable]
		public class StatisticSauceMoule
		{
			public SauceMoule sauceType;
			public float probability;
			public float point;
		}
	}
}