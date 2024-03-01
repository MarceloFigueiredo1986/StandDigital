using StandDigital.Constants;
using System.ComponentModel;

namespace StandDigital.Enums;

public enum Combustivel : ushort
{
    [Description(Multiton.Gasoleo)]
    Gasoleo = 0,
    [Description(Multiton.Gasolina)]
    Gasolina = 1,
    [Description(Multiton.HibridoGasoleo)]
    HibridoGasoleo = 2,
    [Description(Multiton.Eletrico)]
    Electrico = 3,
    [Description(Multiton.GPL)]
    GPL = 4,
    [Description(Multiton.Hidrogenio)]
    Hidrogenio = 5,
    [Description(Multiton.HibridoGasolina)]
    HibridoGasolina = 6,
    [Description(Multiton.GNC)]
    GNC
}
