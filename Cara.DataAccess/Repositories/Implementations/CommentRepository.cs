﻿using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;

namespace Cara.DataAccess.Repositories.Implementations;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
	public CommentRepository(AppDbContext context) : base(context)
	{
	}
}
