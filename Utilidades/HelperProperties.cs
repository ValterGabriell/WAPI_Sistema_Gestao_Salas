namespace WAPI_GS.Utilidades
{
    public static class HelperProperties
    {
        public static DateTime? ConverteStringVaziaParaDataNula(this string? date)
        {
            if (string.IsNullOrEmpty(date))
                return null;

            return DateTime.TryParse(date, out DateTime result) ? result : (DateTime?)null;
        }


        /// <summary>
        /// Percorre todas as propriedades de um objeto e converte valores padrão para nulo.
        /// - `string?`: Converte strings nuláveis vazias (`""`) para `null`, mas apenas se a propriedade for nullable.
        /// - `int?`: Converte `0` nuláveis para `null`.
        /// - `long?`: Converte `0` nuláveis para `null` (Havia um erro, pois tentava converter para `decimal?` em vez de `long?`, agora corrigido).
        /// </summary>
        /// <typeparam name="T">O tipo do objeto a ser processado.</typeparam>
        /// <param name="obj">O objeto cujas propriedades serão verificadas e possivelmente modificadas.</param>
        public static void ConverteValoresPadraoParaNulo<T>(this T obj)
        {
            if (obj == null) return;

            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.CanWrite)
                {
                    Type propertyType = property.PropertyType;
                    bool isNullable = !propertyType.IsValueType || Nullable.GetUnderlyingType(propertyType) != null;

                    if (propertyType == typeof(string) && isNullable)
                    {
                        var value = property.GetValue(obj) as string;
                        if (value == string.Empty)
                        {
                            property.SetValue(obj, null);
                        }
                    }
                    if (propertyType == typeof(int?))
                    {
                        var value = (int?)property.GetValue(obj);
                        if (value == 0)
                        {
                            property.SetValue(obj, null);
                        }
                    }

                    if (propertyType == typeof(long?))
                    {
                        var value = (long?)property.GetValue(obj);
                        if (value == 0)
                        {
                            property.SetValue(obj, null);
                        }
                    }
                }
            }
        }
    }
}
