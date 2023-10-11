﻿using System;
using Message_Broker.Models;
using Microsoft.EntityFrameworkCore;


namespace Message_Broker.Data
{
	public class AppDbContext :DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Topic> Topics => Set<Topic>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<Message> Messages => Set<Message>();
    }
}

