using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrudFinal.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SuperHeroController : ControllerBase
	{
		private readonly IConfiguration _config;
		public SuperHeroController(IConfiguration config)
		{
			_config= config;
		}
		[HttpGet]
		
		 public async Task<ActionResult<List<SuperHero>>> GetALlSuperHeroes()
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			IEnumerable<SuperHero> heroes = await selectallheroes (connection);
			return Ok(heroes);
		}

		private static async Task<IEnumerable<SuperHero>> selectallheroes(SqlConnection connection)
		{ 
			return await connection.QueryAsync<SuperHero>("select * from SuperHeroes");
		}

		[Route("api/[controller]/{heroId}")]
		[HttpGet]
		public async Task<ActionResult<List<SuperHero>>> GetSuperHeroesById(int heroId)
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			var heroes = await connection.QueryFirstAsync<SuperHero>("select * from SuperHeroes where Id = @Id", new {Id=heroId});
			return Ok(heroes);
		}

		[HttpPost]
		public async Task<ActionResult<List<SuperHero>>> createsuperhero(SuperHero entity)
		{
			var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			await connection.ExecuteAsync("insert into superheroes (name, firstname, lastname, place) values (@name, @firstname, @lastname, @place)", entity);
			return Ok(await selectallheroes(connection));
		}

		[HttpPut]
		public async Task<ActionResult<List<SuperHero>>> updateesuperhero(SuperHero entity)
		{
			var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			await connection.ExecuteAsync($"update superheroes set name=@name, firstname=@firstname, lastname=@lastname, place=@place where id = @id", entity);
			return Ok(await selectallheroes(connection));
		}

		[Route("api/[controller]/{heroId}")]
		[HttpDelete]
		
		public async Task<ActionResult<List<SuperHero>>> deletehero(int heroId)
		{
			var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			await connection.ExecuteAsync("Delete from superheroes where id = @Id", new {Id = heroId});
			return Ok(await selectallheroes(connection));
		}

	}
} 
