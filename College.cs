using MediatrExample.Service;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatrExample.Models;
using MediatrExample.Interface;

namespace MediatrExample
{
    public partial class College : BaseEntity
    {
        public string Name { get; set; }

        public string Mascot { get; set; }

        public int YearStarted { get; set; }

        public string LogoUrl { get; set; }

        public string CurrentConference { get; set; }

        public string LocationCity { get; set; }

        public string LocationState { get; set; }

        public string StadiumName { get; set; }

        public int StadiumCapacity { get; set; }

        public string Sport { get; set; }
    }

    public partial interface ICollegeService : IAppService<College>
    {
        // This is a partial interface, create another one for non generic interface properties
        // that way this can always be regenerated 
    }

    [Route("api/[controller]")]
    [ApiController]
    public partial class CollegeController : ControllerBase
    {
        public readonly IMediator _mediator;

        public CollegeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<CollegeController>
        [HttpGet]
        public async Task<GetColleges.Result> GetPagedList([FromQuery] GetColleges.Query query)
        {
            return await _mediator.Send(query);
        }

        // GET api/<CollegeController>/5
        [HttpGet("{id}")]
        public async Task<GetCollege.Result> Get([FromRoute] GetCollege.Query query)
        {
            return await _mediator.Send(query);
        }

        // POST api/<CollegeController>
        [HttpPost]
        public async Task<CreateCollege.Result> Post([FromBody] CreateCollege.Query query)
        {
            return await _mediator.Send(query);
        }

        // PUT api/<CollegeController>/5
        [HttpPut("{id}")]
        public async Task<UpdateCollege.Result> Update([FromRoute] Guid id, [FromBody] UpdateCollege.Query query)
        {
            query.Id = id;
            return await _mediator.Send(query);
        }

        // DELETE api/<CollegeController>/5
        [HttpDelete("{id}")]
        public async Task<DeleteCollege.Result> Delete([FromRoute] Guid id)
        {
            return await _mediator.Send(new DeleteCollege.Query { Id = id });
        }
    }

    public partial class CollegeService : AppService<College>, ICollegeService
    {
        public CollegeService(ApplicationDbContext context) : base(context) { }
    }

    public partial class GetCollege
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public record Result(
            Guid Id,
            string Name,
            string Mascot,
            int? YearStarted,
            string LogoUrl,
            string LocationCity,
            string LocationState,
            string StadiumName,
            int? StadiumCapacity);

        public class Handler : IRequestHandler<Query, Result>
        {
            public readonly ICollegeService _collegeService;

            public Handler(ICollegeService collegeService)
            {
                _collegeService = collegeService;
            }

            public async Task<Result> Handle(Query query, CancellationToken token)
            {
                var x= await _collegeService.Get(query.Id);

                return new Result(x.Id,
                            x.Name,
                            x.Mascot,
                            x.YearStarted,
                            x.LogoUrl,
                            x.LocationCity,
                            x.LocationState,
                            x.StadiumName,
                            x.StadiumCapacity);
            }
        }
    }

    public partial class GetColleges
    {
        public class Query : PagedQuery, IRequest<Result>
        {

        }

        public class Result : PagedList<ListResult> { }

        public record ListResult(
            Guid Id, 
            string Name, 
            string Mascot, 
            int? YearStarted, 
            string LogoUrl, 
            string LocationCity, 
            string LocationState, 
            string StadiumName, 
            int? StadiumCapacity);

        public class Handler : IRequestHandler<Query, Result>
        {
            public readonly ICollegeService _collegeService;

            public Handler(ICollegeService collegeService)
            {
                _collegeService = collegeService;
            }

            public async Task<Result> Handle(Query query, CancellationToken token)
            {
                var list = await PagedList<ListResult>
                    .ToPagedList(_collegeService.GetAll(x => true)
                        .Select(x => new ListResult(
                            x.Id, 
                            x.Name, 
                            x.Mascot, 
                            x.YearStarted, 
                            x.LogoUrl,
                            x.LocationCity,
                            x.LocationState,
                            x.StadiumName,
                            x.StadiumCapacity)), query.PageNumber, query.PageSize);

                return (Result)list;
            }
        }
    }

    public partial class CreateCollege
    {
        public class Query : IRequest<Result>
        {
            public string Name { get; set; }

            public string Mascot { get; set; }

            public int YearStarted { get; set; }

            public string LogoUrl { get; set; }

            public string LocationCity { get; set; }

            public string LocationState { get; set; }

            public string StadiumName { get; set; }

            public int StadiumCapacity { get; set; }
        }

        public class Result : SuccessResponseObject
        {
            public Result(bool success, string message) : base(success, message) { }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly ICollegeService _collegeService;

            public Handler(ICollegeService collegeService)
            {
                _collegeService = collegeService;
            }

            public async Task<Result> Handle(Query query, CancellationToken token)
            {
                try
                {
                    var id = await _collegeService.Add(new College
                    {
                        Name = query.Name,
                        Mascot = query.Mascot,
                        YearStarted = query.YearStarted,
                        LogoUrl = query.LogoUrl,
                        LocationCity = query.LocationCity,
                        LocationState = query.LocationState,
                        StadiumCapacity = query.StadiumCapacity,
                        StadiumName = query.StadiumName,
                    });

                    return new Result(true, $"College created successfully. Id: {id}");
                }
                catch(Exception e1)
                {
                    return new Result(false, e1.Message);
                }
            }
        }
    }

    public partial class UpdateCollege
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string Mascot { get; set; }

            public int? YearStarted { get; set; }

            public string LogoUrl { get; set; }

            public string LocationCity { get; set; }

            public string LocationState { get; set; }

            public string StadiumName { get; set; }

            public int? StadiumCapacity { get; set; }
        }

        public class Result : SuccessResponseObject
        {
            public Result(bool success, string message) : base(success, message) { }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly ICollegeService _collegeService;

            public Handler(ICollegeService collegeService)
            {
                _collegeService = collegeService;
            }

            public async Task<Result> Handle(Query query, CancellationToken token)
            {
                try
                {
                    var college = await _collegeService.Get(query.Id);
                    if (college == null)
                    {
                        return new Result(false, "College not found");
                    }

                    college.Name = query.Name ?? college.Name;
                    college.Mascot = query.Mascot ?? college.Mascot;
                    college.YearStarted = query.YearStarted ?? college.YearStarted;
                    college.LogoUrl = query.LogoUrl ?? college.LogoUrl;
                    college.LocationCity = query.LocationCity ?? college.LocationCity;
                    college.LocationState = query.LocationState ?? college.LocationState;
                    college.StadiumName = query.StadiumName ?? college.StadiumName;
                    college.StadiumCapacity = query.StadiumCapacity ?? college.StadiumCapacity;

                    await _collegeService.Update(college);

                    return new Result(true, "College update successful");
                }
                catch(Exception e1)
                {
                    return new Result(false, e1.Message);
                }
            }
        }
    }

    public partial class DeleteCollege
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class Result : SuccessResponseObject
        {
            public Result(bool success, string message) : base(success, message) { }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly ICollegeService _collegeService;

            public Handler(ICollegeService collegeService)
            {
                _collegeService = collegeService;
            }

            public async Task<Result> Handle(Query query, CancellationToken token)
            {
                try
                {
                    var college = await _collegeService.Get(query.Id);
                    if (college == null)
                    {
                        return new Result(false, "College not found");
                    }

                    await _collegeService.Delete(college);

                    return new Result(true, "College deleted successfully");
                }
                catch(Exception e1)
                {
                    return new Result(false, e1.Message);
                }
            }
        }
    }

    public partial class ApplicationDbContext
    {
        public DbSet<College> Colleges { get; set; }
    }

    public static partial class ModelBuilderExtensions
    {
        public static ModelBuilder CollegeModel(this ModelBuilder modelBuilder)
        {
            // name of the table
            modelBuilder.Entity<College>().ToTable("Colleges");

            //other customizations here

            return modelBuilder;
        }
    }
}

