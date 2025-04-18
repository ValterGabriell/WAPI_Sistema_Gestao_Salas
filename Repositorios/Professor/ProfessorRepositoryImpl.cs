﻿using Microsoft.EntityFrameworkCore;
using WAPI_GS.Dto.User;
using WAPI_GS.Infra.Professor;
using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Repositorios.Professor
{
    public class ProfessorRepositoryImpl(AppDbContext appDbContext) : IProfessorRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<string> Create(TblProfessor entity)
        {
            _appDbContext.Add(entity);
            await _appDbContext.SaveChangesAsync();
            return HelperMessages.PROFESSOR_SALVO_SUCESSO;
        }

        public async Task<string> Update(TblProfessor entity)
        {
            _appDbContext.Update(entity);
            await _appDbContext.SaveChangesAsync();
            return HelperMessages.PROFESSOR_SALVO_SUCESSO;
        }

        public async Task<string> ChangeActive(TblProfessor entity)
        {
            try
            {
                entity = entity.ChangeActivePropriedade();
                _appDbContext.Update(entity);
                await _appDbContext.SaveChangesAsync();
                return entity.Id.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Delete(TblProfessor entity)
        {
            try
            {
                _appDbContext.Remove(entity);
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TblProfessor> GetByIdAsync(int id)
        {
            try
            {
                TblProfessor? professorEncontrado = await RecuperaProfessorPorIDELancaExcecaoSeNaoExistir(id);
                return professorEncontrado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(IEnumerable<TblProfessor>, int)> GetListAsync(FiltersParameter filtersParameter)
        {
            try
            {
                IQueryable<TblProfessor> query = from professor in _appDbContext.TblUsers
                                                 select professor;

                if (filtersParameter.IsActive is not null) query = query.Where(e => e.IsActive == filtersParameter.IsActive);
                if (filtersParameter.Search is not null) query = query.Where(e => e.Name == filtersParameter.Search);
                if (filtersParameter.Search is not null) query = query.Where(e => e.Email == filtersParameter.Search);

                query = query.PaginacaoNoBanco(filtersParameter.PageNumber, filtersParameter.PageSize);
                int count = query.GetCountTotal();

                query.AsNoTracking();

                return (await query.ToListAsync(), count);
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        public async Task<TblProfessor?> RecuperaProfessorPorEmailOuUsername(DtoCreateUpdateUser dto)
        {
            TblProfessor? professorEncontrado = await (from professor in _appDbContext.TblUsers
                                                       where dto.Username == professor.Username || dto.Email == professor.Email
                                                       select professor).FirstOrDefaultAsync();
            return professorEncontrado;
        }

        public async Task<TblProfessor> RecuperaProfessorPorIDELancaExcecaoSeNaoExistir(int id)
        {
            TblProfessor? professorEncontrado = await (from professor in _appDbContext.TblUsers
                                                       where professor.Id == id
                                                       select professor).FirstOrDefaultAsync();
            if (professorEncontrado is null) throw new KeyNotFoundException(HelperMessages.PROFESSOR_NAO_ENCONTRADO);
            return professorEncontrado;

        }
    }
}
