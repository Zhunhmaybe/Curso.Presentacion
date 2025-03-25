using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Curso.Entidades;

namespace Curso.Data;
    public class Dbcontext : Microsoft.EntityFrameworkCore.DbContext
{
        public Dbcontext (DbContextOptions<Dbcontext> options)
            : base(options)
        {
        }

        public DbSet<Curso.Entidades.Usuarios> Usuarios { get; set; } = default!;
        public DbSet<ListasTareas> ListasTareas { get; set; }
        public DbSet<Tareas> Tareas { get; set; }
        public DbSet<Recordatorios> Recordatorios { get; set; }
    }
