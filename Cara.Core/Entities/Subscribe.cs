using Cara.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Cara.Core.Entities;

public class Subscribe : IEntity
{
	public int Id { get; set; }
	[MaxLength(256),DataType(DataType.EmailAddress)]
	public string? Email { get; set; }
}
