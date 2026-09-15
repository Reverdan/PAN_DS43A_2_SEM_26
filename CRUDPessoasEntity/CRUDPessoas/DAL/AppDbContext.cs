using System;
using System.Collections.Generic;
using System.Text;
using CRUDPessoas.modelo;
using Microsoft.EntityFrameworkCore;


//Microsoft.EntityFrameworkCore
//Microsoft.EntityFrameworkCore.SqlServer
//Microsoft.EntityFrameworkCore.Tools

/*
 * No Visual Studio, vá no menu superior em Ferramentas (Tools) > Gerenciador de Pacotes NuGet > Console do Gerenciador de Pacotes.
 * 
 *    Add-Migration Inicial
 *    Update-Database
 *    
 */


namespace CRUDPessoas.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=REVER-NOTE\SQLEXPRESS;
            Initial Catalog=ds34a;User ID=sa;
            Password=rever;Encrypt=False");
        }
    }
}
