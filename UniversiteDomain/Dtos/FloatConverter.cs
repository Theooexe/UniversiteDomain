using CsvHelper;
using CsvHelper.TypeConversion;
using System;
using CsvHelper.Configuration;

public class FloatConverter : ITypeConverter
{
    public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return 0f;  // Retourner 0 si le texte est vide ou seulement des espaces
        }

        if (float.TryParse(text, out var result))
        {
            return result;  // Convertir en float si c'est une valeur valide
        }

        return 0f;  // Retourner 0 si la conversion échoue (valeur invalide)
    }

    public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        return value.ToString();  // Conversion inverse, selon votre besoin
    }
}