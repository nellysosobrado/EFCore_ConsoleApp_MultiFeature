using FluentValidation;

public class InputValidator : AbstractValidator<string>
{
    public InputValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("Input is required");

        RuleFor(x => x)
            .Must(x => !x.Contains(","))
            .WithMessage("Use '.' instead of ',' for decimal numbers")
            .When(x => !string.IsNullOrEmpty(x));

        RuleFor(x => x)
            .Must(BeAValidNumber)
            .WithMessage("Please enter a valid number")
            .When(x => !string.IsNullOrEmpty(x))
;
    }
    private bool BeAValidNumber(string input)
    {
        return double.TryParse(input, System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture, out _);
    }
}
