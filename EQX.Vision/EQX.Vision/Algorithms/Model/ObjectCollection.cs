using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.Vision.Algorithms;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace EQX.Vision.Algorithms
{
    public class ObjectCollection : ObservableObject, IObjectCollection
    {
        #region Properties
        [IndexerName("Item")]
        public object? this[string key]
        {
            get
            {
                return GetObject(key);
            }
            set
            {
                SetObject(key, value);
                OnPropertyChanged("Item[]");
            }
        }

        public ObservableCollection<KeyType>? Keys
        {
            get
            {
                return _keys;
            }
            set
            {
                _keys = value;
                OnPropertyChanged(nameof(Keys));
            }
        }
        #endregion

        #region Constructor(s)
        public ObjectCollection(IEnumerable<KeyType>? keys = null)
        {
            Keys = new ObservableCollection<KeyType>();
            _objects = new Dictionary<string, object?>();

            KeyRegistration(keys);
        }
        #endregion

        #region Private method(s)
        private void KeyRegistration(IEnumerable<KeyType>? keys)
        {
            if (keys == null) return;

            foreach (var key in keys)
            {
                KeyRegistration(key);
            }
        }

        private void KeyRegistration(KeyType key)
        {
            Keys?.Add(key);
            _objects.Add(key.Key, null);
        }

        private object? GetObject(string key)
        {
            return _objects[key];
        }

        private void SetObject(string key, object? output)
        {
            if (Keys?.Count(t => t.Key == key) <= 0)
            {
                throw new NotSupportedException($"Key \"{key}\" is not supported");
            }

            var foundKey = Keys?.First(t => t.Key == key);

            if (output == null)
            {
                _objects[key] = output;
                return;
            }

            if ((output.IsNumericType() && foundKey!.Type.IsNumericType()) == false)
            if (output.GetType() != foundKey!.Type)
            {
                throw new NotSupportedException($"Type {output.GetType()} is not match for Key \"{key}\"");
            }

            _objects[key] = output;
        }
        #endregion

        #region Private fields
        [JsonProperty]
        private readonly Dictionary<string, object?> _objects;
        ObservableCollection<KeyType>? _keys;
        #endregion
    }
}
