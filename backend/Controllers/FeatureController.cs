using BasarStajApp.DTOs;
using BasarStajApp.Entity;
using BasarStajApp.Resources;
using BasarStajApp.Resources.BasarStajApp.Resources;
using BasarStajApp.Services;
using BasarStajApp.Validators;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Collections.Generic;
using System.Linq;

namespace BasarStajApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureService _pointService;

        public FeatureController(IFeatureService pointService)
        {
            _pointService = pointService;
        }

        [HttpPost]
        public ActionResult<ApiResponse<FeatureDTO>> Add(FeatureDTO dto)
        {
            var validation = FeatureValidator.Validate(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>(false, validation.ErrorMessage, null));

            var addedPoint = _pointService.Add(dto);
            var resultDto = MapEntityToDto(addedPoint);

            return Ok(new ApiResponse<FeatureDTO>(true, Messages.PointAdded, resultDto));
        }

        [HttpPost]
        public ActionResult<ApiResponse<List<FeatureDTO>>> AddRange(List<FeatureDTO> dtos)
        {
            var validation = FeatureValidator.Validate(dtos);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>(false, validation.ErrorMessage, null));

            var addedPoints = _pointService.AddRange(dtos);
            var resultDtos = addedPoints.Select(MapEntityToDto).ToList();

            return Ok(new ApiResponse<List<FeatureDTO>>(true, Messages.PointAdded, resultDtos, new { Count = resultDtos.Count }));
        }

        [HttpPut("{id}")]
        public ActionResult<ApiResponse<FeatureDTO>> Update(int id, FeatureDTO dto)
        {
            var validation = FeatureValidator.Validate(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>(false, validation.ErrorMessage, null));

            var updatedPoint = _pointService.Update(id, dto);
            if (updatedPoint == null)
                return NotFound(new ApiResponse<object>(false, Messages.PointNotFound, null));

            var resultDto = MapEntityToDto(updatedPoint);
            return Ok(new ApiResponse<FeatureDTO>(true, Messages.PointUpdated, resultDto));
        }

        [HttpGet]
        public ActionResult<ApiResponse<List<FeatureDTO>>> GetAll(int page = 1, int pageSize = 10)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var points = _pointService.GetAll().OrderBy(p => p.Id).ToList();
            var totalCount = points.Count;

            var pagedPoints = points
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = pagedPoints.Select(MapEntityToDto).ToList();

            return Ok(new ApiResponse<List<FeatureDTO>>(
                true,
                "",
                dtos,
                new
                {
                    Count = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                }
            ));
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResponse<FeatureDTO>> GetByID(int id)
        {
            var point = _pointService.GetByID(id);
            if (point == null)
                return NotFound(new ApiResponse<object>(false, Messages.PointNotFound, null));

            var resultDto = MapEntityToDto(point);
            return Ok(new ApiResponse<FeatureDTO>(true, "", resultDto));
        }

        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<object>> Delete(int id)
        {
            var deleted = _pointService.Delete(id);
            if (!deleted)
                return NotFound(new ApiResponse<object>(false, Messages.PointNotFound, null));

            return Ok(new ApiResponse<object>(true, Messages.PointDeleted, null));
        }

        private FeatureDTO MapEntityToDto(Feature feature)
        {
            if (feature == null) return null;

            var writer = new WKTWriter();
            return new FeatureDTO
            {
                
                Name = feature.Name,
                WKT = feature.Location != null ? writer.Write(feature.Location) : null
            };
        }
    }
}
