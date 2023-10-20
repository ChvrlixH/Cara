using Cara.Core.Entities;

namespace Cara.WebUI.ViewModels;

public class TeamViewModel
{
	public IEnumerable<Team> Teams { get; set; } = null!;
	public IEnumerable<Comment> Comments { get; set; } = null!;
}
