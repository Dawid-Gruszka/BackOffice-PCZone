namespace BackOffice.Services

{
    interface IService<TRead,TCreate,TUpdate>
    {
        public Task<TRead> Get(int id);
        public Task<List<TRead>> GetAllAsync();
        public Task Add(TCreate item, string imagen);
        public Task<bool> Delete(int id);
        public Task Update(int id, TUpdate item, string imagepath);
    }
}
