using System;
using System.Collections.Generic;
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
    public class FornecedorController : ControllerBase
    {
        private readonly IRepository<Fornecedor> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public FornecedorController(
            IRepository<Fornecedor> repo,
            IUnitOfWork uow,
            IMapper map
        )
        {
            _mapper = map;
            _repo = repo;
            _uow = uow;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FornecedorVM), 200)]
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
                    Ok(_mapper.Map<FornecedorVM>(response));
            }
            catch { return StatusCode(500); }
        }

        [HttpPost]
        [ProducesResponseType(typeof(FornecedorVM), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] FornecedorVM fornecedor)
        {
            if (
                ( !fornecedor.IsPessoaFisica &&
                    string.IsNullOrWhiteSpace(fornecedor.CNPJ)) ||
                (fornecedor.IsPessoaFisica &&
                    string.IsNullOrWhiteSpace(fornecedor.Pessoa.CPF))
            )
            {
                return BadRequest();
            }

            if (
                !string.IsNullOrWhiteSpace(fornecedor.CNPJ) &&
                _repo.Query.Any(a => a.CNPJ.Equals(fornecedor.CNPJ))
            )
            {
                return StatusCode(StatusCodes.Status208AlreadyReported,
                new
                {
                    errorMessage = "CNPJ já cadastrado."
                });
            }

            if (
                !string.IsNullOrWhiteSpace(fornecedor.Pessoa.CPF) &&
                _repo.Query.Any(a => a.Pessoa.CPF.Equals(fornecedor.Pessoa.CPF))
            )
            {
                return StatusCode(StatusCodes.Status208AlreadyReported,
                new
                {
                    errorMessage = "CPF já cadastrado."
                });
            }

            try
            {
                var model = _mapper.Map<Fornecedor>(fornecedor);
                _repo.Add(model);
                var result = await _uow.CommitAsync();

                var uri = new Uri(
                    baseUri: new Uri(Request.Host.Value),
                    relativeUri: $"api/fornecedor/{model.Id}"
                );

                if (result)
                    return Created(uri, _mapper.Map<FornecedorVM>(model));
                else
                    return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
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
        [ProducesResponseType(typeof(FornecedorVM), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update([FromBody] FornecedorVM fornecedor)
        {
            if (
                fornecedor.Id == 0 ||
                ( !fornecedor.IsPessoaFisica &&
                    string.IsNullOrWhiteSpace(fornecedor.CNPJ)) ||
                (fornecedor.IsPessoaFisica &&
                    string.IsNullOrWhiteSpace(fornecedor.Pessoa.CPF))
            )
            {
                return BadRequest();
            }

            try
            {
                var model = await _repo.GetById(fornecedor.Id);
                if (model == null)
                    return NoContent();

                if (!string.IsNullOrWhiteSpace(fornecedor.Nome))
                    model.Nome = fornecedor.Nome;
                if (fornecedor.EmpresaId != 0)
                    model.EmpresaId = fornecedor.EmpresaId;
                if (fornecedor.Fones.Any())
                {
                    foreach (var fone in fornecedor.Fones)
                    {
                        if (fone.New)
                        {
                            model.Fones.Add(_mapper.Map<Fone>(fone));
                        }
                        else if (fone.Remove)
                        {
                            var removing = model.Fones.FirstOrDefault(f => f.Id.Equals(fone.Id));
                            model.Fones.Remove(removing);
                        }
                        else
                        {
                            if (fone.Id != 0)
                            {
                                var existingFone = model.Fones.FirstOrDefault(f => f.Id.Equals(fone.Id));
                                model.Fones.Remove(existingFone);
                                existingFone.Numero = fone.Numero;
                                model.Fones.Add(existingFone);
                            }
                        }
                    }
                }
                if (model.Pessoa != null)
                {
                    if (!string.IsNullOrWhiteSpace(fornecedor.Pessoa.CPF))
                        model.Pessoa.CPF = fornecedor.Pessoa.CPF;
                    if (!string.IsNullOrWhiteSpace(fornecedor.Pessoa.RG))
                        model.Pessoa.RG = fornecedor.Pessoa.RG;
                    if (fornecedor.Pessoa.Nascimento != default)
                        model.Pessoa.Nascimento = fornecedor.Pessoa.Nascimento;
                }


                _repo.Update(model);
                var result = await _uow.CommitAsync();

                return result ?
                    Ok(_mapper.Map<FornecedorVM>(model)) :
                    BadRequest() as IActionResult;
            }
            catch { return StatusCode(500); }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FornecedorVM>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public IActionResult GetFiltered(
            [FromQuery] DateTime createdAt,
            [FromQuery] string cpf, [FromQuery] string cnpj, [FromQuery] string nome,
            [FromQuery] int page = 1, [FromQuery] int take = 15)
        {
            try
            {
                var query = _repo.Query;
                if (!string.IsNullOrWhiteSpace(cnpj))
                    query = query.Where(w => w.CNPJ.Contains(cnpj));
                if (!string.IsNullOrWhiteSpace(nome))
                    query = query.Where(w => EF.Functions.Like(w.Nome, $"%{nome}%"));
                if (!string.IsNullOrWhiteSpace(cpf))
                    query = query.Where(w => w.Pessoa.CPF.Contains(cpf));
                if (createdAt != default)
                    query = query.Where(w => w.CreatedAt.Date == createdAt.Date);

                var result = query
                    .Skip((page - 1) * take)
                    .Take(take);

                return result.Count() > 0 ?
                    Ok(_mapper.Map<IEnumerable<FornecedorVM>>(result.AsEnumerable())) :
                    NoContent() as IActionResult;
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}