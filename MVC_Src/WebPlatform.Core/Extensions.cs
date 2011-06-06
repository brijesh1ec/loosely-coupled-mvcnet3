using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Contracts.Entities;

namespace WebPlatform.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Casts all objects inside an IQueryable<T> to their corresponding interface type
        /// </summary>
        /// <typeparam name="TClass">Type of the class</typeparam>
        /// <typeparam name="TInterface">Interface to which to cast to</typeparam>
        /// <param name="source">Source collection</param>
        /// <returns></returns>
        public static IEnumerable<TInterface> AsInterfaceEnumerable<TClass, TInterface>(this IQueryable<TClass> source)
            where TClass : class, TInterface, new()
            where TInterface : IEntity
        {
            foreach (var item in source)
                yield return (TInterface)item;
        }
    }
}
