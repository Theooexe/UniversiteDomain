using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;

public class NoteConverter : ITypeConverter
{
    public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0d; // Si la cellule est vide ou nulle, retourner 0 par défaut (double)
        }

        // Essayer de convertir en double (ou float)
        if (double.TryParse(text, out double result))
        {
            return result;
        }
        throw new Exception("Cannot convert \"" + text + "\" to double");
    }

    public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        return value?.ToString() ?? string.Empty;
    }
}