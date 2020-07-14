using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagueVeloz.Domain.Interfaces;
using PagueVeloz.Domain.Models;
using PagueVeloz.Domain.ViewModels;

namespace PagueVeloz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IRepository<Empresa> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public EmpresaController(
            IRepository<Empresa> repo,
            IUnitOfWork uow,
            IMapper map
        )
        {
            _repo = repo;
            _uow = uow;
            _mapper = map;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmpresaVM), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest();

            try
            {
                var response = await _repo.GetById(id);

                return response == null ?
                    NoContent() as IActionResult :
                    Ok(_mapper.Map<EmpresaVM>(response));
            }
            catch { return StatusCode(500); }
        }

        [HttpPost]
        [ProducesResponseType(typeof(EmpresaVM), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] EmpresaVM empresa)
        {
            if (string.IsNullOrWhiteSpace(empresa.CNPJ))
            {
                return BadRequest();
            }
            var exists = _repo.Query
                .Any(e => e.CNPJ.Equals(empresa.CNPJ));
            if (exists)
            {
                return StatusCode(StatusCodes.Status208AlreadyReported,
                    new
                    {
                        errorMessage = "CNPJ já cadastrado."
                    });
            }
            try
            {
                var model = _mapper.Map<Empresa>(empresa);

                _repo.Add(model);
                var result = await _uow.CommitAsync();

                var uri = new Uri(
                    baseUri: new Uri(Request.Host.Value),
                    relativeUri: $"api/Empresa/{model.Id}"
                );

                if (result)
                    return Created(uri, _mapper.Map<EmpresaVM>(model));
                else
                    return BadRequest();
            }
            catch { return StatusCode(500); }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0)
                return BadRequest();

            try
            {
                _repo.Delete(id);
                var result = await _uow.CommitAsync();
                return result ?
                    Ok() :
                    Accepted() as IActionResult;
            }
            catch { return StatusCode(500); }
        }

        [HttpPut]
        [ProducesResponseType(typeof(EmpresaVM), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update([FromBody] EmpresaVM empresa)
        {
            if (string.IsNullOrWhiteSpace(empresa.CNPJ))
            {
                return BadRequest();
            }

            try
            {
                var model = await _repo.GetById(empresa.Id);
                if (model == null)
                    return NoContent();

                if (!string.IsNullOrWhiteSpace(empresa.NomeFantasia))
                    model.NomeFantasia = empresa.NomeFantasia;
                if (!string.IsNullOrWhiteSpace(empresa.CNPJ))
                    model.CNPJ = empresa.CNPJ;
                if (!string.IsNullOrWhiteSpace(empresa.UF))
                    model.UF = empresa.UF;
                if (empresa.Fornecedor != null)
                {
                    var fornecedor = empresa.Fornecedor;
                    if (fornecedor.Remove)
                    {
                        model.Fornecedor = null;
                    }
                    else
                    {
                        const CompareOptions cOpts = CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase;
                        var forn = _mapper.Map<Fornecedor>(fornecedor);
                        if (
                            string.Compare(empresa.UF, "parana", CultureInfo.CurrentCulture, cOpts) == 0 &&
                            forn.Pessoa?.GetIdade() < 18)
                        {
                            return BadRequest(new { errorMessage = "Fornecedor precisa ser maior de idade." });
                        }
                        model.Fornecedor = _mapper.Map<Fornecedor>(fornecedor);
                    }
                }

                _repo.Update(model);
                var result = await _uow.CommitAsync();

                return result ?
                    Ok(_mapper.Map<EmpresaVM>(model)) :
                    BadRequest() as IActionResult;
            }
            catch { return StatusCode(500); }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmpresaVM>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string cnpj, [FromQuery] string nome, [FromQuery] bool semFornecedor = true,
            [FromQuery] int includeId = 0, [FromQuery] int page = 1, [FromQuery] int take = 15)
        {
            try
            {
                var query = _repo.Query;
                if (!string.IsNullOrWhiteSpace(cnpj))
                    query = query.Where(w => w.CNPJ.Contains(cnpj));
                if (!string.IsNullOrWhiteSpace(nome))
                    query = query.Where(w => EF.Functions.Like(w.NomeFantasia, $"%{nome}%"));
                if (semFornecedor)
                    query = query.Where(w => w.Fornecedor == null);
                Empresa include = null;
                if (includeId != 0 && semFornecedor)
                    include = await _repo.GetById(includeId);

                var result = query
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                if (include != null)
                    result.Add(include);

                return result.Count > 0 ?
                    Ok(_mapper.Map<IEnumerable<EmpresaVM>>(result.AsEnumerable())) :
                    NoContent() as IActionResult;
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}