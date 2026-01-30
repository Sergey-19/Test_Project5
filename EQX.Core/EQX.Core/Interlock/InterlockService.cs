namespace EQX.Core.Interlock
{
    public class InterlockService
    {
        private readonly List<IInterlockRule> _rules = new();
        private bool _isBypassAllEnabled;
        public static InterlockService Default { get; } = new();
        public event Action<string, bool>? InterlockChanged;
        public void RegisterRule(IInterlockRule rule) => _rules.Add(rule);
        public bool IsBypassAllEnabled
        {
            get => _isBypassAllEnabled;
            set
            {
                if (_isBypassAllEnabled == value)
                {
                    return;
                }

                _isBypassAllEnabled = value;
                Evaluate();
            }
        }
        public void Reevaluate() => Evaluate();
        private void Evaluate()
        {
            foreach (var rule in _rules)
            {
                bool satisfied = _isBypassAllEnabled || rule.IsSatisfied();
                InterlockChanged?.Invoke(rule.Key, satisfied);
            }
        }
    }
}
