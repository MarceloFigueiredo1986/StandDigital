namespace StandDigital.Core.Entities;

public record PaymentEntity(int PagamentoId,int CarroId, string TokenPagamento,DateTimeOffset DataCriacao)
{
    public PaymentEntity() : this(default,default,default,default)
    {
        
    }
}
