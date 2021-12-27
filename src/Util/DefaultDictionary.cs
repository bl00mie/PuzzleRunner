namespace PuzzleRunner.Util
{
    public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue?> where TKey : notnull
    {
        public Func<TValue?> def = () => default;
        private bool _storeOnMissingLookup;

        public DefaultDictionary() : base() { }
        
        public DefaultDictionary(Func<TValue?> def, bool storeOnMissingLookup = false) : base()
        {
            this.def = def;
            _storeOnMissingLookup = storeOnMissingLookup;
        }

        public new TValue? this[TKey key]
        {
            get
            {
                if (!ContainsKey(key))
                    if (!_storeOnMissingLookup)
                        return def();
                    Add(key, def());
                return base[key];
            }
        }
    }
}
