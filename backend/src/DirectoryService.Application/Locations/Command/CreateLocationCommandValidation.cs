using DirectoryService.Application.Extations;
using DirectoryService.Domain.LocationValueObjects;
using FluentValidation;

namespace DirectoryService.Application.Locations.Command;

public class CreateLocationCommandValidation : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationCommandValidation()
    {
        RuleFor(x => x.LocationName).MustBeValueObject(x => LocationName.Create(x));
        RuleFor(x => x.AddressDto).MustBeValueObject(x =>
            Address.Create(x.City, x.Street, x.HouseNumber, x.Number));
        RuleFor(x => x.TimeZone).MustBeValueObject(x => Timezone.Create(x));
    }
}