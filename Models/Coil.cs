namespace MyAPI.Models;

public class Coil
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double Weight { get; set; }

    public double Thickness { get; set; }

    public double Length { get; set; }

    public double Width { get; set; }

    private double _tolerance = 5;
    private double _density = 0.00785;

    public int CheckWeight ()
    {
        switch (Weight)
        { 
            case var value when value > GetMaxWeightTolerated():
                return 1; //"Peso acima do limite tolerado. Excedendo " + (GetMaxWeightTolerated()-Weight).ToString() + " Kg.";

            case var value when value < GetMinWeightTolerated():
                return -1; //"Peso abaixo do limite tolerado. Faltando " + (GetMinWeightTolerated()-Weight).ToString() + " Kg.";

            default:
                return 0; //"Peso dentro dos limites tolerados.";
        
        }
    } 

    public double GetMaxWeightTolerated(){
        return Length * Width * Thickness * _density * (1 + _tolerance/100);
    }

    public double GetMinWeightTolerated(){
        return Length * Width * Thickness * _density * (1 - _tolerance/100);
    }

    public double GetWeightCalculated(){ 
        return Length * Width * Thickness * _density;
    }
    
}