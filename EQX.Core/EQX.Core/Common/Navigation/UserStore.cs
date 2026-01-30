using EQX.Core.Common.Navigation;

namespace EQX.Core.Common
{
    public class UserStore
    {
        public event Action UserChanged;
        private EPermission permission = EPermission.Operator;

        public EPermission Permission
        {
            get => permission;
            set
            {
                permission = value;
                UserChanged?.Invoke();
            }
        }
    }

}
