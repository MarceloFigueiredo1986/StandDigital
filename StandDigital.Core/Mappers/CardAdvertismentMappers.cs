using StandDigital.Core.Entities;
using StandDigital.Core.RequestHandlers.CarAdvertisement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandDigital.Core.Mappers;

public static class CardAdvertismentMappers
{
    public static CarEntity ToCardEntity(this CarAdvertisementCrudModel car)
        => new CarEntity()
        {
            Ano = Convert.ToInt16(car.Ano!.Value),
            Bateria = car.Bateria,
            Caixa = car.Caixa,
            Cilindrada = car.Cilindrada,
            Combustivel = car.Combustivel!,
            Comentarios = car.Comentarios,
            Cv = car.Cv!.Value,
            Danos = car.Danos,
            Kms = car.Kms!.Value,
            Lotacao = car.Lotacao!.Value,
            Marca = car.Marca,
            Matricula = car.Matricula!,
            TipoNegocioId = car.TipoNegocioId!.Value,
            MesRegisto = car.MesRegisto!.Value,
            Modelo = car.Modelo!,
            Nacional = car.Nacional!.Value,
            Portas = car.Portas!.Value,
            PrecoBase = car.PrecoBase!.Value,
            PrimeiroDono = car.PrimeiroDono!.Value,
            ProblemasMotor = car.ProblemasMotor!,
            TipoCarregamento = car.TipoCarregamento!
        };
}
