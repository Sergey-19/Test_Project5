namespace EQX.Core.Interlock
{
    public class LambdaInterlockRule : IInterlockRule
    {
        private readonly Func<bool> _predicate;
        public LambdaInterlockRule(string key, Func<bool> predicate)
        {
            Key = key;
            _predicate = predicate;
        }
        public string Key { get; }
        public bool IsSatisfied() => _predicate();
    }
}
