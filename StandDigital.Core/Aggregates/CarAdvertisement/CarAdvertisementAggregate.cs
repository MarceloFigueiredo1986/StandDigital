using StandDigital.Core.CarAdvertisement;
using StandDigital.Core.Entities;
using StandDigital.Core.Models;

namespace StandDigital.Core.Aggregates.CarAdvertisement;

public class CarAdvertisementAggregate(CarAdvertisementCrudModel car, IEnumerable<byte[]> photos, HashSet<string> extras,
        CarMetadataEntity carMetadataEntity, CarSelledOffer? buyingProposal)
{
    public CarAdvertisementCrudModel Car { get; private set; } = car;
    public IEnumerable<byte[]> Photos { get; private set; } = photos;
    public HashSet<string> Extras { get; private set; } = extras;
    public CarMetadataEntity CarMetadataEntity { get; private set; } = carMetadataEntity;
    public CarSelledOffer? BuyingProposal { get; private set; } = buyingProposal;
}