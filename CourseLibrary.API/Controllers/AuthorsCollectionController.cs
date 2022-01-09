using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
	[ApiController]
	[Route("/api/authorcollections")]
    public class AuthorsCollectionController : ControllerBase
    {
		private readonly ICourseLibraryRepository courseLibraryRepository;
		private readonly IMapper mapper;

		public AuthorsCollectionController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
		{
			this.courseLibraryRepository = courseLibraryRepository ??
				throw new ArgumentNullException(nameof(ICourseLibraryRepository));
			this.mapper = mapper ??
				throw new ArgumentNullException(nameof(IMapper));
		}

		[HttpGet("({ids})", Name = "GetAuthorsCollection")]
		public ActionResult<IEnumerable<AuthorDto>> GetAuthorsCollection([FromRoute]
			[ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
		{
			if (ids == null)
			{
				return BadRequest();
			}

			var authorsEntities = courseLibraryRepository.GetAuthors(ids);

			if (authorsEntities.Count() != ids.Count())
			{
				NotFound();
			}

			var authorsToReturn = mapper.Map<IEnumerable<AuthorDto>>(authorsEntities);

			return Ok(authorsToReturn);
		}

		[HttpPost]
		public ActionResult<IEnumerable<AuthorDto>> CreateAuthorsCollection(IEnumerable<AuthorForCreationDto> authorsCollection)
		{
			var authorsEntities = mapper.Map<IEnumerable<Author>>(authorsCollection);

			foreach(Author author in authorsEntities)
			{
				courseLibraryRepository.AddAuthor(author);
			}
			courseLibraryRepository.Save();

			var authorsCollectionToReturn = mapper.Map<IEnumerable<AuthorDto>>(authorsEntities);
			var idsToReturn = string.Join(",", authorsEntities.Select(a => a.Id));

			return CreatedAtRoute("GetAuthorsCollection", new { ids = idsToReturn }, authorsCollectionToReturn);
		}

	}
}
