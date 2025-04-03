namespace WAPI_GS.Utilidades
{
    public static class HelperExceptions
    {

        public static string CreateExceptionMessage(Exception ex)
        {
            var msg = ex.Message;
            if (ex.InnerException != null)
            {
                msg = ex.InnerException.Message;
            }
            return msg;
        }
    }
}
