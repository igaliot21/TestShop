using MyShop.Core;
using MyShop.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockContext<T> : IRepository<T> where T:BaseEntity
    {
        List<T> items;
        string className;
        public MockContext()
        {
            items = new List<T>();
        }
        public void Commit()
        {
            return;
        }
        public void Insert(T t)
        {
            items.Add(t);
        }
        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.ID == i.ID);

            if (tToUpdate == null)
            {
                throw new Exception(className + " not found");
            }
            else
            {
                tToUpdate = t;
            }
        }
        public T Find(string ID)
        {
            T t = items.Find(i => i.ID == ID);

            if (t == null)
            {
                throw new Exception(className + " not found");
            }
            else
            {
                return t;
            }
        }
        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }
        public void Delete(string ID)
        {
            T tToDelete = items.Find(i => i.ID == ID);

            if (tToDelete == null)
            {
                throw new Exception(className + " not found");
            }
            else
            {
                items.Remove(tToDelete);
            }
        }
    }
}
