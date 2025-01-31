using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using WAPI_GS.Dto.UserSala;
using WAPI_GS.EM.Sala;
using WAPI_GS.EM.UserSala;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Service
{
    public class UserSalaService(AppDbContext appDbContext, IConfiguration configuration)
        : ICS_UserSala<DtoCreateUserSala, DtoGetUserSala>
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly IConfiguration _configuration = configuration;

        public async Task<DtoResponseCreate> Create(DtoCreateUserSala dto, string requestKey)
        {
            if (dto.IsRepeat)
            {
                List<string> notSavedEntityList = new();
                for (var i = 0; i < dto.TimeRepeat; i++)
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
                    message = "Entidade gerada!",
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
            return new DtoResponseCreate
            {
                message = "Entidade gerada!",
                errors = []
            };
        }

        private bool IsEntityPresentForDayHour(DtoCreateUserSala dto)
        {
            return _appDbContext.TblUsersSala
                .AsNoTracking()
                .Any(e => e.Dia == dto.Dia && dto.HoraInicial >= e.HoraInicial && dto.HoraInicial <= e.HoraFinal);
        }

        private static void InitializeEntity(DtoCreateUserSala dto, out TblUsersSala entity)
        {
            entity = dto.ToEntity();
            var g = Guid.NewGuid();
            entity.Id = g.ToString();
        }

        public async Task<string> Update(DtoUpdateSalaUser dto, int oldUserId, int SalaId, string requestKey)
        {

            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
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

        public async Task Delete(int userId, int salaId, string requestKey)
        {

            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
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


        public async Task<List<DtoGetUserSala>> GetList(int? salaId, int? profId, string requestKey)
        {

            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
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
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> SendEmailSolicitacao(string destEmail,
            string body,
            string title,
            string fullUrl,
            int salaId,
            DateOnly dia,
            int currentUserId,
            int newUserId,
            int horaInit,
            int horaFinal
            , string requestKey)
        {

            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                using (SmtpClient client = new(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
                {
                    client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                    client.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpSettings["Username"], "Gestão de Salas"),
                        Subject = title,
                        Body = body + "\n\n" +
                            "Caso aceite, clique no link abaixo referente ao aceite ou não:\n\n" +
                            "✔ Aceito: " + fullUrl + "/accept?salaId=" + salaId +
                            "&dia=" + dia +
                            "&userId=" + currentUserId +
                            "&newUserId=" + newUserId +
                            "&horaInit=" + horaInit +
                            "&horaFinal=" + horaFinal + "\n\n" +
                            "❌ Não aceito: " + fullUrl + "/notAccept?salaId=" + salaId + " \n",
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(destEmail);

                    await client.SendMailAsync(mailMessage);
                }

                // Enviar mensagem no WhatsApp após o e-mail
                await SendWhatsAppMessage("+5591985253357", $"Sua solicitação de troca de sala foi enviada! Para aceitar ou recusar, acesse: {fullUrl}");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





        public async Task<bool> Accept(int salaId, DateOnly dia, int userId, int newUserId, int horaInit, int horaFinal)
        {
            int year = dia.Year;
            int month = dia.Month;
            int day = dia.Day;

            // Formatar como "YYYY-DD-MM"
            string formattedDate = $"{year}-{day:D2}-{month:D2}";
            var tblUsersSala = await _appDbContext.TblUsersSala
                .Where(e => e.SalaId == salaId
                    && e.UserId == userId
                    && e.Dia == DateOnly.Parse(formattedDate)
                    && e.HoraInicial == horaInit
                    && e.HoraFinal == horaFinal)
                .FirstOrDefaultAsync() ?? throw new Exception("Nenhuma reserva encontrada com os parâmetros fornecidos.");

            tblUsersSala.UserId = newUserId;
            await _appDbContext.SaveChangesAsync();

            TblUser tblUser = await _appDbContext.TblUsers.Where(e => e.Id == newUserId).FirstAsync();
            TblSala tblSala = await _appDbContext.TblSalas.Where(e => e.Id == salaId).FirstAsync();
            await SendEmail(tblUser.Email!,
                "Solicitação para troca de sala aceita! " + tblSala.Name +
                " agora está alocada para você em " +
                tblUsersSala.Dia + " " + tblUsersSala.HoraInicial + ":" + tblUsersSala.HoraFinal,
                tblSala.Name + "Solicitação de troca de sala aceita!"
                );


            return true;
        }

        public async Task<bool> SendEmail(string destEmail, string body, string title)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                using (SmtpClient client = new(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
                {
                    client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                    client.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpSettings["Username"], "Gestão de Salas"),
                        Subject = title,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(destEmail);

                    await client.SendMailAsync(mailMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> NotAccept(int salaId)
        {
            TblUsersSala tblUsersSala = await _appDbContext.TblUsersSala.Where(e => e.SalaId == salaId).FirstAsync();
            TblUser tblUser = await _appDbContext.TblUsers.Where(e => e.Id == tblUsersSala.UserId).FirstAsync();
            TblSala tblSala = await _appDbContext.TblSalas.Where(e => e.Id == salaId).FirstAsync();
            await SendEmail(tblUser.Email!,
                "Solicitação para troca de sala recusada! " + tblSala.Name +
                "O professor referente " + tblUser.Name + " não aceitou sua solicitação",
                tblSala.Name + "Solicitação de troca de sala recusada!"
                );
            return false;
        }


        private async Task SendWhatsAppMessage(string phoneNumber, string message)
        {
            var whatsappApiUrl = "https://graph.facebook.com/v17.0/YOUR_WHATSAPP_NUMBER_ID/messages";
            var accessToken = "YOUR_ACCESS_TOKEN"; // Obtido na conta do Meta for Developers

            var payload = new
            {
                messaging_product = "whatsapp",
                to = phoneNumber,
                type = "text",
                text = new { body = message }
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(whatsappApiUrl, content);
                response.EnsureSuccessStatusCode();
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


