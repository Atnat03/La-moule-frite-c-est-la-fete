using System;

namespace Data
{
	[Serializable]
	public class Commande
	{
		public float point = 0;

		public Produit produitType = Produit.Empty;
		public SauceFrite sauceFrite = SauceFrite.Empty;
		public SauceMoule sauceMoule = SauceMoule.Empty;
	}

	public enum Produit
	{
		Empty,
		
		Frite,
		Moule,
		FriteMoule
	}
	
	public enum SauceFrite
	{
		Empty,
		Ketchup,
		Mayonnaise,
		Moutarde,
		Samourai,
		Algerienne
	}
	
	public enum SauceMoule
	{
		Empty,
		CremeFraiche,
		Oignon,
		Diable,
		Mariniaire,
		Provencale
	}
}