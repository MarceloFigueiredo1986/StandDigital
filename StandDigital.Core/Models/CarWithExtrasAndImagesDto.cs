namespace StandDigital.Core.Models;

public record CarWithExtrasAndImagesDto(
    CarCompleteModel Car,
    HashSet<string>? Extras, 
    string[]? Photos,
    CarSellingPurposes? CarSellHistory);