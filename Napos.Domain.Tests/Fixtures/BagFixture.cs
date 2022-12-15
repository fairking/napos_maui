using System.Collections.Generic;

namespace Napos.Domain.Tests.Fixtures
{
    public class BagFixture
    {
        private readonly IDictionary<string, object> _bag;

        public BagFixture()
        {
            _bag = new Dictionary<string, object>();
        }

        public T GetValue<T>(string key, bool remove = false)
        {
            object value;

            _bag.TryGetValue(key, out value);

            if (remove)
                _bag.Remove(key);

            if (value != null)
                return (T)value;
            else
                return default;
        }

        public void SetValue<T>(string key, T value)
        {
            _bag[key] = value;
        }
    }
}
