using Microsoft.EntityFrameworkCore;
using WAPI_GS.Dto.UserSala;
using WAPI_GS.EM.UserSala;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Service
{
    public class UserSalaService(AppDbContext appDbContext)
        : ICS_UserSala<DtoCreateUserSala, DtoGetUserSala>
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public string Create(DtoCreateUserSala dto)
        {
            try
            {
                if (dto.IsRepeat)
                {
                    for (var i = 0; i <= dto.TimeRepeat; i++)
                    {
                        TblUsersSala entity = dto.ToEntity();
                        Guid g = Guid.NewGuid();
                        entity.Id = g.ToString();
                        _appDbContext.Add(entity);
                        dto.Dia = dto.Dia.AddDays(7);
                    }
                }
                else
                {
                    TblUsersSala entity = dto.ToEntity();
                    Guid g = Guid.NewGuid();
                    entity.Id = g.ToString();
                    _appDbContext.Add(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return "Entidade gerada!";
        }

        public async Task<string> Update(DtoUpdateSalaUser dto, int oldUserId, int SalaId)
        {
            //A entidade existe, se nao solta um erro e nem passa dessa linha
            TblUsersSala tblUsersSala = await CreateQuery()
                .Where(e => e.Dia == DateOnly.Parse(dto.DiaCorrente))
                .Where(e => e.SalaId == SalaId)
                .Where(e => e.UserId == oldUserId)
                .FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Entidade nao encontrda!");

            _appDbContext.Remove(tblUsersSala);
            await _appDbContext.SaveChangesAsync();


            tblUsersSala.UserId = dto.UserId;
            tblUsersSala.HoraInicial = dto.HoraInicial;
            tblUsersSala.HoraFinal = dto.HoraFinal;

            try
            {
                _appDbContext.Add(tblUsersSala);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return tblUsersSala.SalaId.ToString() + "-> User: " + tblUsersSala.UserId;
        }

        public async Task Delete(int userId, int salaId)
        {
            var entity = CreateQuery()
                .Where(e => e.UserId == userId)
                .Where(e => e.SalaId == salaId);
            try
            {
                _appDbContext.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<DtoGetUserSala>> GetList(int? salaId, int? profId)
        {
            try
            {
                IQueryable<TblUsersSala> completeQuery = CreateQuery()
           .Include(e => e.TblUser)
           .Include(e => e.TblSala)
           .AsQueryable();

                if (salaId != null)
                {
                    completeQuery = completeQuery.Where(e => e.SalaId == salaId);
                }

                if (profId != null)
                {
                    completeQuery = completeQuery.Where(e => e.UserId == profId);
                }

                // Obtendo a lista de todos os dados necessários
                var lista = await completeQuery.ToListAsync();

                // Agrupando os dados por Dia
                var groupedByDate = lista
                    .GroupBy(e => e.Dia)
                    .Select(dateGroup => new DtoGetUserSala
                    {
                        Dia = dateGroup.Key,
                        HoraInit = dateGroup.Min(e => e.HoraInicial),
                        HoraFinal = dateGroup.Max(e => e.HoraFinal),
                        Salas = dateGroup
                            .GroupBy(e => e.SalaId)
                            // Agrupando por SalaId dentro de cada dia
                            .Select(salaGroup => new DtoGetUserSala.SalaComProfessores
                            {
                                SalaId = salaGroup.Key,  // Id da sala
                                TblSala = salaGroup.First().TblSala,  // Detalhes da sala
                                Professores = salaGroup.Select(e => e.TblUser).ToList()  // Lista de professores alocados na sala
                            }).ToList()  // Lista de salas para aquela data
                    }).ToList();

                return groupedByDate;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing your request.");
            }
        }



        //PRIVATE
        private IQueryable<TblUsersSala> CreateQuery()
        {
            return _appDbContext.TblUsersSala
            .AsNoTracking();
        }

    }
}


