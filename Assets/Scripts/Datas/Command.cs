using System;
using MyPrint;

namespace Data
{
	[Serializable]
	public class Commande
	{
		public float point = 0;

		public Produit produitType = Produit.Empty;
		public SauceFrite sauceFrite = SauceFrite.Empty;
		public SauceMoule sauceMoule = SauceMoule.Empty;

		public bool CheckCommand(Commande other)
		{
			ABPrint.PrintBool(produitType == other.produitType, "produitType == other.produitType");
			ABPrint.PrintBool(sauceFrite == other.sauceFrite, "sauceFrite == other.sauceFrite");
			ABPrint.PrintBool(sauceMoule == other.sauceMoule, "sauceMoule == other.sauceMoule");
			
			return produitType == other.produitType && sauceFrite == other.sauceFrite && sauceMoule == other.sauceMoule;
		}
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
		Moutarde
	}
	
	public enum SauceMoule
	{
		Empty,
		CremeFraiche,
		Oignon,
		Mariniaire
	}
}