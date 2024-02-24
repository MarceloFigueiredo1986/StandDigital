using System.Collections.Immutable;

namespace StandDigital.Models;

public record CarWithExtrasAndImagesDto(CarCompleteModel Car, HashSet<string>? Extras, string[]? Photos,CarSellingPurposes? CarSellHistory);