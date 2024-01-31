﻿using BusinessLogic.Core.Entities;
using BusinessLogic.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Infrastructure.Repositories;

public abstract class RepositoryBase<T> where T : EntityBase
{
    public RepositoryBase(DbSet<T> values)
    {
        DbSet = values;
    }

    protected DbSet<T> DbSet { get; private set; }

    /// <summary>
    /// Применить спецификацию
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    protected IQueryable<T> ApplySpecification(
        SpecificationBase<T> specification)
    {
        return SpecificationEvaluator.GetQuery(
           DbSet,
           specification);
    }
}
