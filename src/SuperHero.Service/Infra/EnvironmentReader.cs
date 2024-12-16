namespace SuperHero.Service.Infra;

public static class EnvironmentReader
{
    public static T Read<T>(string varName, T defaultValue = default, string varEmptyError = null)
        where T : IComparable, IConvertible
    {
        if (string.IsNullOrEmpty(varName))
            throw new ArgumentNullException(nameof(varName));
        bool hasDefault = !EqualityComparer<T>.Default.Equals(defaultValue, default);
        var value = Environment.GetEnvironmentVariable(varName);
        if (string.IsNullOrEmpty(value))
            if (hasDefault)
                return defaultValue;
            else
                throw new NullReferenceException(varEmptyError);
        var type = typeof(T);
        if (type.IsEnum)
        {
            if (Enum.TryParse(type, value.ToString(), out object ret))
                return (T)ret;
            return defaultValue;
        }
        T result = (T)Convert.ChangeType(value, typeof(T));
        if (!EqualityComparer<T>.Default.Equals(result, defaultValue))
            return result;
        return defaultValue;
    }
}