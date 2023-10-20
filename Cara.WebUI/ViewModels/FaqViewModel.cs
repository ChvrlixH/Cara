using Cara.Core.Entities;

namespace Cara.WebUI.ViewModels;

public class FaqViewModel
{
	public IEnumerable<Faq> Faqs { get; set; } = null!;
}
