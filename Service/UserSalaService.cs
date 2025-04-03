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
            TblDisciplina tblDisciplina = await _appDbContext.TblDisciplina.Where(e => e.Id == dto.DisciplinaId).FirstAsync() ?? throw new KeyNotFoundException("Disciplina");

            TblPtd tblPtd = InitializeEntity(dto);

            int totalAulas = tblDisciplina.TotalAulas;


            List<string> notSavedEntityList = new();
            for (var i = 0; i < totalAulas; i++)
            {
                bool existEntityToDayHour = IsEntityPresentForDayHour(dto);
                if (!existEntityToDayHour)
                {
                    TblPtd entity = InitializeEntity(dto);
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

        private bool IsEntityPresentForDayHour(DtoCreateUserSala dto)
        {
            return _appDbContext.TblUsersSala
                .AsNoTracking()
                .Any(e => e.Dia == dto.Dia && dto.HoraInicial >= e.HoraInicial && dto.HoraInicial <= e.HoraFinal);
        }

        private static TblPtd InitializeEntity(DtoCreateUserSala dto)
        {
            var entity = dto.ToEntity();
            var g = Guid.NewGuid();
            entity.Id = g.ToString();
            return entity;
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
            TblPtd tblUsersSala = await CreateQuery()
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
                //bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                //if (!requestValid)
                //{
                //    throw new Exception("000-Token Inválido");
                //}

                // Busca os registros de TblUsersSalas
                List<TblPtd> todasUserSala = await _appDbContext.TblUsersSala
                    .AsNoTracking()
                    .AsQueryable()
                    .ToListAsync();

                List<DtoGetUserSala> dtoResponse = [];

                // Itera sobre as TblUsersSalas
                foreach (var ptdAtual in todasUserSala)
                {
                    DtoGetUserSala? diaExistente = dtoResponse.FirstOrDefault(e => e.Dia == ptdAtual.Dia);

                    // Caso o dia não exista, cria um novo DTO
                    if (diaExistente == null)
                    {
                        diaExistente = new DtoGetUserSala
                        {
                            Dia = ptdAtual.Dia,
                            Salas = []
                        };
                        dtoResponse.Add(diaExistente);
                    }


                    // Busca a sala associada à TblUsersSala
                    TblSala? tblSala = await _appDbContext.TblSalas
                        .Where(e => e.Id == ptdAtual.SalaId)
                        .FirstOrDefaultAsync();


                    // Busca a disciplina associada à TblUsersSala
                    TblDisciplina? tblDisciplina = await _appDbContext.TblDisciplina
                        .Where(e => e.Id == ptdAtual.DisciplinaId)
                        .FirstOrDefaultAsync();

                    if (tblDisciplina == null) continue;

                    TblTurma? tblTurma = await _appDbContext.TblTurma
                        .Where(e => e.Id == tblDisciplina.TurmaId)
                        .FirstOrDefaultAsync();

                    // Busca o professor associada à TblUsersSala
                    TblProfessor? tblUser = await _appDbContext.TblUsers
                        .Where(e => e.Id == ptdAtual.UserId)
                        .FirstOrDefaultAsync();

                    if (tblSala != null && tblDisciplina != null && tblTurma != null && tblUser != null)
                    {
                        // Cria um objeto SalaComProfessores
                        DtoGetUserSala.SalaComProfessores salaComProfessores = new()
                        {
                            SalaId = tblSala.Id,
                            TblSala = tblSala,
                            HoraInit = ptdAtual.HoraInicial,
                            HoraFinal = ptdAtual.HoraFinal,
                            Professor = tblUser,
                            Disciplina = tblDisciplina,
                            Turma = tblTurma
                        };


                        // Adiciona a sala com os professores no DTO do dia
                        diaExistente.Salas.Add(salaComProfessores);

                    }
                }

                return dtoResponse;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

        }


        public async Task<bool> SendEmailSolicitacao(string destEmail,
            string body,
            string title,
            string fullUrl,
            int salaId,
            DateOnly dia,
            int currentUserId,
            string currentUsername,
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
                int year = dia.Year;
                int month = dia.Month;
                int day = dia.Day;
                string formattedDate = $"{day:D2}/{month:D2}/{year}";
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                using (SmtpClient client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
                {
                    client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                    client.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                    string emailBody = $@"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 20px;
                            }}
                            .container {{
                                max-width: 600px;
                                background: #ffffff;
                                padding: 20px;
                                border-radius: 10px;
                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                            }}
                            h2 {{
                                color: #333;
                            }}
                            p {{
                                font-size: 16px;
                                color: #555;
                                line-height: 1.5;
                            }}
                            .button {{
                                display: inline-block;
                                padding: 10px 20px;
                                margin: 10px 5px;
                                text-decoration: none;
                                color: white;
                                border-radius: 5px;
                                font-weight: bold;
                            }}
                            .accept {{
                                background-color: #28a745;
                            }}
                            .reject {{
                                background-color: #dc3545;
                            }}
                            .footer {{
                                margin-top: 20px;
                                font-size: 12px;
                                color: #777;
                                text-align: center;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>Confirmação de Reserva de Sala</h2>
                            <p>{body}</p>
                            <p>Caso aceite, clique em um dos botões abaixo:</p>
                            <a href='{fullUrl}/accept?salaId={salaId}&dia={formattedDate}&userId={currentUserId}&currentUsername={currentUsername}&horaInit={horaInit}&horaFinal={horaFinal}' class='button accept'>✔ Aceito</a>
                            <a href='{fullUrl}/notAccept?salaId={salaId}' class='button reject'>❌ Não Aceito</a>
                        </div>
                    </body>
                    </html>";

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpSettings["Username"], "Gestão de Salas"),
                        Subject = title,
                        Body = emailBody,
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





        public async Task<bool> Accept(int salaId, DateOnly dia, int userId, string currentUsername, int horaInit, int horaFinal)
        {
            int year = dia.Year;
            int month = dia.Month;
            int day = dia.Day;

            // Formatar como "YYYY-DD-MM"
            string formattedDate = $"{year}-{month:D2}-{day:D2}";
            var tblUsersSala = await _appDbContext.TblUsersSala
                .Where(e => e.SalaId == salaId
                    && e.UserId == userId
                    && e.Dia == DateOnly.Parse(formattedDate)
                    && e.HoraInicial == horaInit
                    && e.HoraFinal == horaFinal)
                .FirstOrDefaultAsync() ?? throw new Exception("Nenhuma reserva encontrada com os parâmetros fornecidos.");


            TblProfessor user = await _appDbContext.TblUsers.Where(e => e.Username == currentUsername).FirstAsync();
            tblUsersSala.UserId = user.Id;
            _appDbContext.Update(tblUsersSala);
            await _appDbContext.SaveChangesAsync();

            TblProfessor tblUser = await _appDbContext.TblUsers.Where(e => e.Id == user.Id).FirstAsync();
            TblSala tblSala = await _appDbContext.TblSalas.Where(e => e.Id == salaId).FirstAsync();
            await SendEmail(tblUser.Email!,
                " Solicitação para troca de sala aceita! " + tblSala.Name +
                " agora está alocada para você em " +
                tblUsersSala.Dia + " das " + tblUsersSala.HoraInicial + ":00h até as " + tblUsersSala.HoraFinal + ":00h",
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
            TblPtd tblUsersSala = await _appDbContext.TblUsersSala.Where(e => e.SalaId == salaId).FirstAsync();
            TblProfessor tblUser = await _appDbContext.TblUsers.Where(e => e.Id == tblUsersSala.UserId).FirstAsync();
            TblSala tblSala = await _appDbContext.TblSalas.Where(e => e.Id == salaId).FirstAsync();

            await SendEmail(tblUser.Email!,
                $"Solicitação para troca de sala recusada! {tblSala.Name} - O professor {tblUser.Name} não aceitou sua solicitação",
                ""
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
        private IQueryable<TblPtd> CreateQuery()
        {
            return _appDbContext.TblUsersSala
            .AsNoTracking();
        }

    }
}


