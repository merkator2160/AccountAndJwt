﻿using AccountAndJwt.AuthorizationService.Database.Interfaces;
using AccountAndJwt.AuthorizationService.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.AuthorizationService.Database.Mappings
{
    internal class UserRoleMap : IEntityMap<UserRoleDb>
    {
        public void Configure(EntityTypeBuilder<UserRoleDb> entityBuilder)
        {
            entityBuilder
                .ToTable("UserRoles")
                .HasKey(p => new { p.RoleId, p.UserId });

            entityBuilder
                .HasOne(p => p.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.RoleId);

            entityBuilder
                .HasOne(p => p.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.UserId);
        }
    }
}