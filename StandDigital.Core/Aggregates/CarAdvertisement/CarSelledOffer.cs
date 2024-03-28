namespace StandDigital.Core.Aggregates.CarAdvertisement;

public record CarSelledOffer(string Buyer,string Telefone,bool Paid,decimal Offer)
{
    public CarSelledOffer() : this(default,default,default,default){}
}
