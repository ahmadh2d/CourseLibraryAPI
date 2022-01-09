using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace CourseLibrary.API.Controllers
{
	[ApiController]
	[Route("/api/authors")]
	public class AuthorsController : ControllerBase
	{
		private readonly ICourseLibraryRepository courseLibraryRepository;
		private readonly IMapper mapper;

		public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
		{
			this.courseLibraryRepository = courseLibraryRepository ??
				throw new ArgumentNullException(nameof(ICourseLibraryRepository));
			this.mapper = mapper ??
				throw new ArgumentNullException(nameof(IMapper));
		}

		[HttpGet]
		[HttpHead]
		public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] AuthorResourceParameters authorResourceParameters)
		{
			var authorsFromRepo = courseLibraryRepository.GetAuthors(authorResourceParameters);
			return Ok(mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
		}

		[HttpGet("{authorId}", Name = "GetAuthor")]
		public ActionResult<AuthorDto> GetAuthor(Guid authorId)
		{
			var authorFromRepo = courseLibraryRepository.GetAuthor(authorId);

			if (authorFromRepo == null)
			{
				return NotFound();
			}

			return Ok(mapper.Map<AuthorDto>(authorFromRepo));
		}

		public IActionResult CreateAuthor(AuthorForCreationDto author)
		{
			var authorEntity = mapper.Map<Author>(author);
			courseLibraryRepository.AddAuthor(authorEntity);
			courseLibraryRepository.Save();

			var authorToReturn = mapper.Map<AuthorDto>(authorEntity);
			return CreatedAtRoute("GetAuthor", new { authorId = authorToReturn.Id }, authorToReturn);
		}

		[HttpOptions]
		public IActionResult GetAuthorsOptions()
		{
			Response.Headers.Add("Allow", "GET, POST, OPTIONS");
			return Ok();
		}

		[HttpDelete("{authorId}")]
		public ActionResult DeleteAuthor(Guid authorId)
		{
			var authorToDelete = courseLibraryRepository.GetAuthor(authorId);

			if (authorToDelete == null)
			{
				return NotFound();
			}

			courseLibraryRepository.DeleteAuthor(authorToDelete);
			courseLibraryRepository.Save();

			return NoContent();
		}
	}
}
