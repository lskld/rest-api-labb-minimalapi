using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rest_api_labb_minimalapi.Data;
using rest_api_labb_minimalapi.Models.DTOs;
using rest_api_labb_minimalapi.Models.Entities;

namespace rest_api_labb_minimalapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<RestLabbDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DB_CONNECTION_STRING")));

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // 1. Get all people in the system
            app.MapGet("/people", async (RestLabbDbContext context) =>
            {
                return await context.People.Select(p => new PersonReadDto
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    PhoneNumber = p.PhoneNumber
                }).ToListAsync();
            });

            // 2. Get all interest connected to a specific person
            app.MapGet("/people/{id}/interests", async (RestLabbDbContext context, int id) =>
            {
                var interests = await context.People
                    .Where(p => p.PersonId == id)
                    .SelectMany(p => p.Interests)
                    .Select(i => new
                    {
                        i.InterestId,
                        i.InterestName,
                        i.InterestDescription
                    }).ToListAsync();

                if (interests == null)
                    return Results.NotFound("Either person with that id does not exist, or they have no connected interests");

                return Results.Ok(interests);
            });

            // 3. Get all links connected to a specific person
            app.Map("/people/{id}/links", async (RestLabbDbContext context, int id) =>
            {
                var person = await context.People
                    .Where(p => p.PersonId == id)
                    .SelectMany(p => p.Links)
                    .Select(l => new
                    {
                        l.LinkId,
                        l.LinkUrl
                    })
                    .ToListAsync();

                if (person == null)
                    return Results.NotFound("Person with that id was not found");

                return Results.Ok(person);
            });

            // 4. Connect a person to a new interest
            app.MapPost("/people/{personId}/connect/interests/{interestId}", async (RestLabbDbContext context, int personId, int interestId) =>
            {
                var person = await context.People
                    .Include(p => p.Interests)
                    .FirstOrDefaultAsync(p => p.PersonId == personId);

                var interest = await context.Interests.FirstOrDefaultAsync(i => i.InterestId == interestId);

                if (person is null || interest is null)
                    return Results.NotFound("Person or interest was not found");

                person.Interests.Add(interest);
                await context.SaveChangesAsync();
                return Results.Ok($"{person.FirstName} {person.LastName} is now conntected to interest: {interest.InterestName}");
            });

            // 5. Add new link for specific person & interest
            app.MapPost("/people/{personId}/interests/{interestId}/addlink", async (RestLabbDbContext context, int personId, int interestId, [FromBody] LinkCreateDto dto) => 
            {
                var person = await context.People.FirstOrDefaultAsync(p => p.PersonId == personId);
                var interest = await context.Interests.FirstOrDefaultAsync(i => i.InterestId == interestId);

                if (person is null || interest is null)
                    return Results.NotFound("Person or interest was not found");

                var newLink = new Link
                {
                    LinkUrl = dto.LinkUrl,
                    PersonId = personId,
                    InterestId = interestId
                };

                context.Links.Add(newLink);
                await context.SaveChangesAsync();

                return Results.Ok("Link added successfully");
            });

            app.Run();
        }
    }
}
