﻿using WAPI_GS.Dto;
using WAPI_GS.Dto.Sala;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;
using WAPI_GS.Repositorios.Salas;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Service
{
    public class SalaService(ISalaRepository repository) : ISalaService
    {
        private readonly ISalaRepository _repository = repository;

        public async Task<string> Create(DtoCreateSala dto)
        {
            try
            {
                TblSala tblSala = dto.ToEntity();
                string message = await _repository.Create(tblSala);
                return message;
            }
            catch (Exception ex)
            {

                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<string> UpdateAsync(DtoCreateSala dto, int id)
        {
            try
            {
                TblSala tblSala = await _repository.RecuperaEntidadePorIDElancaExcecaoSeNaoExiste(id);

#warning deve checar se o nome da sala ja existe;
                TblSala entity = dto.ToEntityForUpdate(tblSala.IsActive);
                tblSala = entity;
                string message = await _repository.Update(tblSala);
                return message;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }

        }


        public async Task<string> ChangeActive(int id)
        {
            try
            {
                TblSala tblSala = await _repository.RecuperaEntidadePorIDElancaExcecaoSeNaoExiste(id);
                tblSala.MudaPropriedadeIsAtivo();
                string message = await _repository.Update(tblSala);
                return message;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        public async Task DeleteAsync(int id)
        {
            try
            {
                TblSala tblSala = await _repository.RecuperaEntidadePorIDElancaExcecaoSeNaoExiste(id);
                _repository.Delete(tblSala);
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<DtoGetSala> GetByIdAsync(int id)
        {
            try
            {
                TblSala tblSala = await _repository.GetByIdAsync(id);
                var dto = tblSala.ToDto();
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<IEnumerable<DtoGetSala>> GetListAsync(FiltersParameter filtersParameter)
        {
            try
            {
                IEnumerable<TblSala> salaList = await _repository.GetListAsync(filtersParameter);
                List<DtoGetSala> dtoGetSalas = salaList.Select(e => e.ToDto()).ToList();
                return dtoGetSalas;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<List<DtoGetCombo>> GetListCombo(FiltersParameter filtersParameter)
        {
            try
            {
                var lista = await _repository.GetListAsync(filtersParameter);
                return lista.Select(e => new DtoGetCombo(e.Id.ToString(), e.Name ?? "-")).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }
    }
}
