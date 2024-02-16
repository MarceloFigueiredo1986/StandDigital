using StandDigital.Entities;

namespace StandDigital.Models;

public record CarWithExtrasAndImagesDto(CarCompleteModel? Car, HashSet<string>? Extras, HashSet<string>? Photos,CarSellingPurposes CarSellHistory);