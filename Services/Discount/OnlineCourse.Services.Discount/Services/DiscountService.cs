using OnlineCourse.Shared.Dtos;
using System.Data;
using Npgsql;
using Dapper;

namespace OnlineCourse.Discount.Services
{

    public class DiscountService:IDiscountService
    {
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _dbConnection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);

            const string query = "DELETE FROM discount WHERE id = @Id";
            var status = await _dbConnection.ExecuteAsync(query, parameters);

            return status > 0 
                ? Response<NoContent>.Success(204) 
                : Response<NoContent>.Fail("Discount not found", 404);
        }

        public async Task<Response<List<Models.Discount>>> GetAll()
        {
            const string query = "Select * from discount";

            var discounts = await _dbConnection.QueryAsync<Models.Discount>(query);

            return Response<List<Models.Discount>>.Success(200,discounts.ToList());
        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            const string query = "select * from discount where userid=@UserId and code=@Code";

            var discounts = await _dbConnection.QueryAsync<Models.Discount>(query, new { UserId = userId, Code = code });

            var hasDiscount = discounts.FirstOrDefault();

            return hasDiscount == null 
                ? Response<Models.Discount>.Fail("Discount not found", 404) 
                : Response<Models.Discount>.Success(200, hasDiscount);
        }

        public async Task<Response<Models.Discount>> GetById(int id)
        {
            const string query = "SELECT * FROM discount WHERE id = @Id";
            var parameters = new { Id = id };

            var discount = (await _dbConnection
                    .QueryAsync<Models.Discount>(query, parameters))
                .SingleOrDefault();


            return discount == null 
                ? Response<Models.Discount>.Fail("Discount not found", 404) 
                : Response<Models.Discount>.Success(200, discount);
        }

        public async Task<Response<NoContent>> Save(Models.Discount discount)
        {
            var saveStatus = await _dbConnection
                .ExecuteAsync("INSERT INTO discount (userid,rate,code) VALUES(@UserId,@Rate,@Code)", discount);

            return saveStatus > 0 
                ? Response<NoContent>.Success(204) 
                : Response<NoContent>.Fail("an error occurred while adding", 500);
        }

        public async Task<Response<NoContent>> Update(Models.Discount discount)
        {
            const string query = @" UPDATE discount SET 
                userid = @UserId,
                code = @Code,
                rate = @Rate
                WHERE id = @Id";

            var parameters = new
            {
                discount.Id,
                discount.UserId,
                discount.Code,
                discount.Rate
            };
            var status = await _dbConnection.ExecuteAsync(query,parameters);

            return status > 0 
                ? Response<NoContent>.Success(204) 
                : Response<NoContent>.Fail("Discount not found", 404);
        }
    }
}
