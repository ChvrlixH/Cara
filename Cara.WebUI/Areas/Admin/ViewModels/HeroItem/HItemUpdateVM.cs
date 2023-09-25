﻿using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.HeroItem;

public class HItemUpdateVM
{
	public int Id { get; set; }
	public IFormFile? Photo { get; set; }
	[Required, MaxLength(70)]
	public string? Name { get; set; }
	[Required, MaxLength(70)]
	public string? Title { get; set; }
	[MaxLength(70)]
	public string? SubTitle { get; set; }
	[MaxLength(160)]
	public string? Description { get; set; }
	public bool isActive { get; set; }
	public string? ImagePath { get; set; }
}
