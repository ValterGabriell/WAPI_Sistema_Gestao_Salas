using Microsoft.EntityFrameworkCore;
using WAPI_GS.Dto.UserSala;
using WAPI_GS.EM.Sala;
using WAPI_GS.EM.UserSala;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Service
{
    public class UserSalaService(AppDbContext appDbContext)
        : ICS_UserSala<DtoCreateUserSala, DtoGetUserSala>
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public DtoResponseCreate Create(DtoCreateUserSala dto)
        {
            try
            {
                if (dto.IsRepeat)
                {
                    List<string> notSavedEntityList = new();
                    for (var i = 0; i <= dto.TimeRepeat; i++)
                    {
                        bool existEntityToDayHour = IsEntityPresentForDayHour(dto);
                        if (!existEntityToDayHour)
                        {
                            TblUsersSala entity;
                            InitializeEntity(dto, out entity);
                            _appDbContext.Add(entity);
                        }
                        else
                        {
                            notSavedEntityList.Add("Dia: " + dto.Dia + " com horário inicial " + dto.HoraInicial + " e hora final " + dto.HoraFinal + " já cadastrado!");
                        }
                        dto.Dia = dto.Dia.AddDays(7);
                    }

                    return new DtoResponseCreate
                    {
                        message = "Entidade gerada! Porém alguns dias não foram salvos!",
                        errors = notSavedEntityList
                    };
                }
                else
                {
                    TblUsersSala entity;

                    bool existEntityToDayHour = IsEntityPresentForDayHour(dto);

                    if (!existEntityToDayHour)
                    {
                        InitializeEntity(dto, out entity);
                        _appDbContext.Add(entity);
                    }
                    else
                    {
                        throw new Exception("Dia " + dto.Dia + " já possui registro de horário em: " + dto.HoraInicial + " - " + dto.HoraFinal);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new DtoResponseCreate
            {
                message = "Entidade gerada!",
                errors = []
            };
        }

        private bool IsEntityPresentForDayHour(DtoCreateUserSala dto)
        {
            return _appDbContext.TblUsersSala.Any(e => e.Dia == dto.Dia && dto.HoraInicial >= e.HoraInicial && dto.HoraInicial <= e.HoraFinal);
        }

        private static void InitializeEntity(DtoCreateUserSala dto, out TblUsersSala entity)
        {
            entity = dto.ToEntity();
            var g = Guid.NewGuid();
            entity.Id = g.ToString();
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
                // Busca os registros de TblUsersSalas
                List<TblUsersSala> tblUsersSalas1 = await _appDbContext.TblUsersSala
                    .AsNoTracking()
                    .AsQueryable()
                    .ToListAsync();

                List<DtoGetUserSala> dtoGetUserSalas = new List<DtoGetUserSala>();

                // Itera sobre as TblUsersSalas
                foreach (var item in tblUsersSalas1)
                {
                    // Busca o DTO para o dia
                    var dto = dtoGetUserSalas.FirstOrDefault(d => d.Dia == item.Dia);

                    // Caso o dia não exista, cria um novo DTO
                    if (dto == null)
                    {
                        dto = new DtoGetUserSala
                        {
                            Dia = item.Dia,
                            Salas = new List<DtoGetUserSala.SalaComProfessores>()
                        };

                        // Adiciona o DTO à lista
                        dtoGetUserSalas.Add(dto);
                    }

                    // Busca a sala associada à TblUsersSala
                    TblSala tblSala = await _appDbContext.TblSalas
                        .Where(e => e.Id == item.SalaId)
                        .FirstAsync();

                    if (tblSala != null)
                    {
                        // Cria um objeto SalaComProfessores
                        DtoGetUserSala.SalaComProfessores salaComProfessores = new()
                        {
                            SalaId = tblSala.Id,
                            TblSala = tblSala,
                            HoraInit = item.HoraInicial,
                            HoraFinal = item.HoraFinal,
                            Professores = new List<TblUser>()
                        };

                        // Agora, usa foreach para procurar os professores
                        var professores = await _appDbContext.TblUsersSala
                            .Where(us => us.UserId == item.UserId)
                            .ToListAsync();

                        foreach (var professorSala in professores)
                        {
                            // Agora buscamos o professor individualmente
                            TblUser professor = await _appDbContext.TblUsers
                                .Where(u => u.Id == professorSala.UserId)
                                .FirstAsync();

                            if (professor != null)
                            {
                                // Adiciona o professor à lista de professores da sala
                                salaComProfessores.Professores.Add(professor);
                            }
                        }

                        // Adiciona a sala com os professores no DTO do dia
                        dto.Salas.Add(salaComProfessores);
                    }
                }

                return dtoGetUserSalas;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing your request.", ex);
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


