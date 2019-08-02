using ShortStoryBOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IStoryDb
    {
        IEnumerable<Story> GetAll();

        IEnumerable<Story> GetAll(bool IsApproved);

        Story GetById(int SSid);

        void Create(Story story);

        void Update(Story story);

        void Delete(int SSid);

        void Approve(int SSid);
    }

    public class StoryDb : IStoryDb
    {
        SSDbContext dbContext;

        public StoryDb (SSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Approve(int SSid)
        {
            var story = dbContext.Stories.Find(SSid);
            story.IsApproved = true;
            dbContext.Update<Story>(story);
            dbContext.SaveChanges();
        }

        public void Create(Story story)
        {
            dbContext.Add<Story>(story);
            dbContext.SaveChanges();
        }

        public void Delete(int SSid)
        {
            var story = dbContext.Stories.Find(SSid);
            dbContext.Remove<Story>(story);
            dbContext.SaveChanges();
        }

        public IEnumerable<Story> GetAll()
        {
            return dbContext.Stories.ToList();
        }

        public IEnumerable<Story> GetAll(bool IsApproved)
        {
            return dbContext.Stories.Where(x=>x.IsApproved==IsApproved).ToList();

        }

        public Story GetById(int SSid)
        {
            return dbContext.Stories.Find(SSid);
        }

        public void Update(Story story)
        {
            dbContext.Update<Story>(story);
            dbContext.SaveChanges();
        }
    }
}
