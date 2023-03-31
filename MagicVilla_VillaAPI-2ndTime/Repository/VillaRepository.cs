using MagicVilla_VillaAPI_2ndTime.Data;
using MagicVilla_VillaAPI_2ndTime.Model;
using MagicVilla_VillaAPI_2ndTime.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MagicVilla_VillaAPI_2ndTime.Repository
{
    public class VillaRepository :Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext db):base    (db)
        {
            _db = db;   
        }

        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
           _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

       
    }
    }

