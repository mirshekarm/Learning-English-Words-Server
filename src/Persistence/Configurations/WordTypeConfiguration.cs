﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Persistence.Configurations
{
	internal class WordTypeConfiguration : IEntityTypeConfiguration<WordType>
	{
		public WordTypeConfiguration() : base()
		{
		}


		public void Configure(EntityTypeBuilder<WordType> builder)
		{
			builder.Property
				(current => current.Type)
				.IsRequired();

			builder.HasMany(c => c.Words)
				.WithOne(c => c.WordType)
				.HasForeignKey(c => c.WordTypeId)
				.IsRequired();

			builder.HasData(new List<WordType>()
			{
				new WordType()
				{
					Id = 1,
					Type = "Noun"
				},
				new WordType()
				{
					Id = 2,
					Type = "Letters"
				},
				new WordType()
				{
					Id = 3,
					Type = "Pronoun"
				},
				new WordType()
				{
					Id = 4,
					Type = "Adverb"
				},
				new WordType()
				{
					Id = 5,
					Type = "Verb"
				},
				new WordType()
				{
					Id = 6,
					Type = "Adjective"
				}
			});
		}
	}
}
