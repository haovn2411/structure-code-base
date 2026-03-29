using MediatR;
using Microsoft.AspNetCore.Mvc;
using StructureCodeSolution.API.Abstractions;
using StructureCodeSolution.Application.Usecases.V1.Commands.Courses.Abstracts;
using StructureCodeSolution.Application.Usecases.V1.Queries.Courses.Abstracts;

namespace StructureCodeSolution.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CoursesController : ApiController
    {
        public CoursesController(ISender sender) : base(sender)
        {
        }

        /// <summary>
        /// Create a new course
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCourse([FromBody] Command.CreateCourseCommand command)
        {
            var result = await Sender.Send(command);
            
            if (result.IsFailure)
            {
                return HandlerFailure(result);
            }

            return CreatedAtAction(
                nameof(GetCourseById), 
                new { courseId = result.Value }, 
                result.Value);
        }

        /// <summary>
        /// Get course by ID
        /// </summary>
        [HttpGet("{courseId:guid}")]
        [ProducesResponseType(typeof(Response.CourseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseById(Guid courseId)
        {
            var result = await Sender.Send(new Query.GetCourseByIdQuery(courseId));
            
            if (result.IsFailure)
            {
                return HandlerFailure(result);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get all courses with pagination and filters
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response.CourseListResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCourses(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] int? levelId = null)
        {
            var query = new Query.GetAllCoursesQuery(pageIndex, pageSize, searchTerm, categoryId, levelId);
            var result = await Sender.Send(query);
            
            if (result.IsFailure)
            {
                return HandlerFailure(result);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Update course
        /// </summary>
        [HttpPut("{courseId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCourse(
            Guid courseId, 
            [FromBody] Command.UpdateCourseCommand command)
        {
            if (courseId != command.CourseId)
            {
                return BadRequest("Course ID mismatch");
            }

            var result = await Sender.Send(command);
            
            if (result.IsFailure)
            {
                return HandlerFailure(result);
            }

            return Ok();
        }

        /// <summary>
        /// Delete course
        /// </summary>
        [HttpDelete("{courseId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var result = await Sender.Send(new Command.DeleteCourseCommand(courseId));
            
            if (result.IsFailure)
            {
                return HandlerFailure(result);
            }

            return Ok();
        }

        /// <summary>
        /// Add video to course
        /// </summary>
        [HttpPost("{courseId:guid}/videos")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddVideo(
            Guid courseId,
            [FromBody] Command.AddVideoToCourseCommand command)
        {
            if (courseId != command.CourseId)
            {
                return BadRequest("Course ID mismatch");
            }

            var result = await Sender.Send(command);
            
            if (result.IsFailure)
            {
                return HandlerFailure(result);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get course videos
        /// </summary>
        [HttpGet("{courseId:guid}/videos")]
        [ProducesResponseType(typeof(Response.VideoListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseVideos(Guid courseId)
        {
            var result = await Sender.Send(new Query.GetCourseVideosQuery(courseId));
            
            if (result.IsFailure)
            {
                return HandlerFailure(result);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Publish course
        /// </summary>
        [HttpPost("{courseId:guid}/publish")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PublishCourse(Guid courseId)
        {
            var result = await Sender.Send(new Command.PublishCourseCommand(courseId));
            
            if (result.IsFailure)
            {
                return HandlerFailure(result);
            }

            return Ok();
        }

        /// <summary>
        /// Rate course
        /// </summary>
        [HttpPost("{courseId:guid}/rate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RateCourse(
            Guid courseId,
            [FromBody] Command.RateCourseCommand command)
        {
            if (courseId != command.CourseId)
            {
                return BadRequest("Course ID mismatch");
            }

            var result = await Sender.Send(command);
            
            if (result.IsFailure)
            {
                return HandlerFailure(result);
            }

            return Ok();
        }

        /// <summary>
        /// Enroll student to course
        /// </summary>
        [HttpPost("{courseId:guid}/enroll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EnrollStudent(Guid courseId)
        {
            var result = await Sender.Send(new Command.EnrollStudentCommand(courseId));
            
            if (result.IsFailure)
            {
                return HandlerFailure(result);
            }

            return Ok();
        }
    }
}
