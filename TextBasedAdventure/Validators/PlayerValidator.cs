using FluentValidation;
using TextBasedAdventure.Classes;

namespace TextBasedAdventure.Validators
{
    public class PlayerValidator : AbstractValidator<Player>
    {
        public PlayerValidator()
        {
            RuleFor(player => player.Name)
                .NotEmpty().WithMessage("Player name cannot be empty.")
                .Length(3, 20).WithMessage("Player name must be between 3 and 20 characters.");

            RuleFor(player => player.CurrentClass)
                .IsInEnum().WithMessage("Invalid player class. Choose Mage, Archer, or Warrior.");
        }
    }
}
