using Data;

namespace Item.Interfaces
{
    public enum AlimentCuisson
    {
        PasCoupé,
        Crut,
        Cuit,
        Brulé
    }
    
    public interface IAliments
    {
        public AlimentCuisson Cuisson { get; set; }
        public void SetNewCuissonState(AlimentCuisson cuisson);
    }
}