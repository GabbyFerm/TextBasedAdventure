using FluentValidation;
using TextBasedAdventure.Classes;

namespace TextBasedAdventure.Validators
{
    public class MenuChoiceValidator : AbstractValidator<string>
    {
        public MenuChoiceValidator(int minOption, int maxOption)
        {
            RuleFor(choice => choice)
                .NotEmpty().WithMessage("Menu choice cannot be empty.")
                .Must(choice => int.TryParse(choice, out int result) && result >= minOption && result <= maxOption)
                .WithMessage($"Please choose a valid option between {minOption} and {maxOption}.");
        }
    }
}