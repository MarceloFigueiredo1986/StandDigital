using StandDigital.Core.CarAdvertisement;
using StandDigital.Core.Entities;

namespace StandDigital.Core.Aggregates.CarAdvertisement;

public class CarAdvertisementAggregate(CarAdvertisementCrudModel car, (byte[], bool)[] photos, HashSet<string> extras,
        CarMetadataEntity carMetadataEntity)
{
    public CarAdvertisementCrudModel Car { get; private set; } = car;
    public (byte[] photoStream,bool isPrimary)[] Photos { get; private set; } = photos;
    public HashSet<string> Extras { get; private set; } = extras;
    public CarMetadataEntity CarMetadataEntity { get; private set; } = carMetadataEntity;
    public CarSelledOffer? BuyingProposal { get; private set; }
}