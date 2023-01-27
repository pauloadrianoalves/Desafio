using Desafio.Persistence.Contexts;

namespace Desafio.Persistence
{
    /// <summary>
    /// Operador de crud
    /// </summary>
    public interface IBaseRepository
    {
        public void Add<T>(T entity) where T : class;
        public void Update<T>(T entity) where T : class;
        public void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
    }

    public class BaseRepository : IBaseRepository
    {
        private readonly DesafioContext _context;

        public BaseRepository(DesafioContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
