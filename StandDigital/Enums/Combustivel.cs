using System.ComponentModel;

namespace StandDigital.Enums;

public enum Combustivel : ushort
{
    [Description("Gasóleo")]
    Gasoleo = 0,
    [Description("Gasolina")]
    Gasolina = 1,
    [Description("Híbrido Gasóleo")]
    HibridoGasoleo = 2,
    [Description("Eléctrico")]
    Electrico = 3,
    [Description("GPL")]
    GPL = 4,
    [Description("Hidrogénio")]
    Hidrogenio = 5,
    [Description("Híbrido Gasolina")]
    HibridoGasolina = 6,
    [Description("GNC")]
    GNC
}
