using StandDigital.Entities;
using StandDigital.Services.Repositories.CarExtras;

namespace StandDigital.Models;

public record CarWithExtrasAndImagesDto(CarCompleteModel? Car, HashSet<string>? Extras, HashSet<string>? Photos);