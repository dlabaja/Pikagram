namespace Pikagram.ViewModels.ProfileViewModel;

public class ProfileViewModel
{
    public ChangeBackgroundViewModel BackgroundForm { get; set; } = new();
    public ChangeAvatarViewModel AvatarForm { get; set; } = new();
    public ChangeBioViewModel BioForm { get; set; } = new();
}