using DirectoryService.Application.Extations;
using DirectoryService.Domain.LocationValueObjects;
using FluentValidation;

namespace DirectoryService.Application.Locations.Command;

public class CreateLocationCommandValidation : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationCommandValidation()
    {
        RuleFor(x => x.Request.LocationName).MustBeValueObject(x =>
            LocationName.Create(x));

        RuleFor(x => x.Request).MustBeValueObject(x =>
            Address.Create(x.City, x.Street, x.HouseNumber, x.Number));

        RuleFor(x => x.Request.TimeZone).MustBeValueObject(x => Timezone.Create(x));
    }
}